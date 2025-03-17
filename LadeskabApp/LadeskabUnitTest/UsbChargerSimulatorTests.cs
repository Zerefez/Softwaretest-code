
using NUnit.Framework;
using NSubstitute;
using System;
using System.IO;

using LadeskabLib;

namespace Ladeskab.Unit.Test
{
    [TestFixture]
    public class UsbChargerSimulatorTests
    {
        private UsbChargerSimulator _uut;

        [SetUp]
        public void Setup()
        {
            _uut = new UsbChargerSimulator();
        }

        [Test]
        public void Constructor_SetsInitialValues()
        {
            // Assert
            Assert.That(_uut.CurrentValue, Is.EqualTo(0.0));
            Assert.That(_uut.Connected, Is.True);
            Assert.That(_uut.Overload, Is.False);
        }

        [Test]
        public void StartCharge_WhenConnected_StartsCharging()
        {
            // Arrange
            bool eventFired = false;
            CurrentEventArgs receivedArgs = null;
            
            _uut.CurrentValueEvent += (sender, args) => 
            {
                eventFired = true;
                receivedArgs = args;
            };

            // Act
            _uut.StartCharge();
            
            // Wait for at least one timer tick
            Thread.Sleep(300);

            // Assert
            Assert.That(eventFired, Is.True);
            Assert.That(receivedArgs.Current, Is.GreaterThan(0));
        }


        [Test]
        public void StartCharge_WhenOverload_DoesNotStartCharging()
        {
            // Arrange
            _uut.SimulateOverload(true);
            bool eventFired = false;
            
            _uut.CurrentValueEvent += (sender, args) => 
            {
                eventFired = true;
            };

            // Act
            _uut.StartCharge();
            
            // Wait for potential timer ticks
            Thread.Sleep(300);

            // Assert
            Assert.That(eventFired, Is.False);
            Assert.That(_uut.CurrentValue, Is.EqualTo(750.0));
        }

        [Test]
        public void SimulateOverload_WhileCharging_StopsCharging()
        {
            // Arrange
            _uut.StartCharge();
            Thread.Sleep(300); // Let charging start
            
            bool eventFired = false;
            CurrentEventArgs receivedArgs = null;
            
            _uut.CurrentValueEvent += (sender, args) => 
            {
                eventFired = true;
                receivedArgs = args;
            };

            // Act
            _uut.SimulateOverload(true);
            
            // Clear event tracking
            eventFired = false;
            
            // Simulate another timer tick using reflection
            var timerElapsedMethod = typeof(UsbChargerSimulator).GetMethod("OnTimerElapsed", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            timerElapsedMethod.Invoke(_uut, new object[] { null, null });
            
            // Assert
            Assert.That(_uut.Overload, Is.True);
            Assert.That(eventFired, Is.False); // No new events should fire as charging has stopped
        }

        [Test]
        public void NullEvent_DoesNotThrowException()
        {
            // Arrange - create a new instance without attaching any event handlers
            var uut = new UsbChargerSimulator();
            
            // Act & Assert - Should not throw exceptions
            Assert.DoesNotThrow(() => {
                uut.StartCharge();
                Thread.Sleep(300); // Allow time for event to potentially fire
                uut.StopCharge();
                uut.SimulateOverload(true);
            });
        }

        [Test]
        public void StopCharge_StopsChargingAndResetsCurrentValue()
        {
            // Arrange
            _uut.StartCharge();
            Thread.Sleep(300); // Let charging start
            
            bool eventFired = false;
            CurrentEventArgs receivedArgs = null;
            
            _uut.CurrentValueEvent += (sender, args) => 
            {
                eventFired = true;
                receivedArgs = args;
            };

            // Act
            _uut.StopCharge();

            // Assert
            Assert.That(eventFired, Is.True);
            Assert.That(receivedArgs.Current, Is.EqualTo(0.0));
        }

        [Test]
        public void SimulateConnected_SetsConnectedStatus()
        {
            // Act
            _uut.SimulateConnected(false);

            // Assert
            Assert.That(_uut.Connected, Is.False);
        }

        [Test]
        public void SimulateOverload_SetsOverloadStatusAndValue()
        {
            // Arrange
            bool eventFired = false;
            CurrentEventArgs receivedArgs = null;
            
            _uut.CurrentValueEvent += (sender, args) => 
            {
                eventFired = true;
                receivedArgs = args;
            };

            // Act
            _uut.SimulateOverload(true);

            // Assert
            Assert.That(_uut.Overload, Is.True);
            Assert.That(_uut.CurrentValue, Is.EqualTo(750)); // Overload current value
            Assert.That(eventFired, Is.True);
            Assert.That(receivedArgs.Current, Is.EqualTo(750));
        }

        [Test]
        public void ChargeComplete_CurrentValueReducesToFullyChargedLevel()
        {
            // Arrange
            _uut.StartCharge();
            
            bool reachedFullyCharged = false;
            double finalCurrent = 0;
            
            _uut.CurrentValueEvent += (sender, args) => 
            {
                // If current is at fully charged level (5.0)
                if (Math.Abs(args.Current - 5.0) < 0.1)
                {
                    reachedFullyCharged = true;
                    finalCurrent = args.Current;
                }
            };
            
            // Act - let the timer run for the full charge time
            // This is a test hack - would normally need to wait 20 minutes
            var fieldInfo = typeof(UsbChargerSimulator).GetField("_ticksSinceStart", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            // Set ticks to very high value to force completion
            fieldInfo.SetValue(_uut, int.MaxValue / 2);
            
            // Trigger another timer tick
            var timerElapsedMethod = typeof(UsbChargerSimulator).GetMethod("OnTimerElapsed", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            timerElapsedMethod.Invoke(_uut, new object[] { null, null });
            
            // Assert
            Assert.That(reachedFullyCharged, Is.True);
            Assert.That(finalCurrent, Is.EqualTo(5.0).Within(0.1));
        }
        
        [Test]
        public void SimulateOverload_WhenFalse_DoesNotChangeCurrentValue()
        {
            // Arrange
            _uut.StartCharge();
            Thread.Sleep(300); // Let charging start
            double initialCurrent = _uut.CurrentValue;
            
            // Act
            _uut.SimulateOverload(false);
            
            // Assert
            Assert.That(_uut.Overload, Is.False);
            Assert.That(_uut.CurrentValue, Is.EqualTo(initialCurrent));
        }

        [Test]
        public void SimulateConnected_WhenChargingAndDisconnected_StopsCharging()
        {
            // Arrange
            _uut.StartCharge();
            Thread.Sleep(300); // Let charging start
            
            bool eventFired = false;
            CurrentEventArgs receivedArgs = null;
            
            _uut.CurrentValueEvent += (sender, args) => 
            {
                eventFired = true;
                receivedArgs = args;
            };

            // Act
            _uut.SimulateConnected(false);
            
            // Assert
            Assert.That(_uut.Connected, Is.False);
            
            // Try to start charging again
            eventFired = false;
            _uut.StartCharge();
            
            // Wait for potential timer ticks
            Thread.Sleep(300);
            
            // Verify that it didn't start charging
            Assert.That(eventFired, Is.False);
        }

        [Test]
        public void StartCharge_WhenDisconnected_DoesNotStartCharging()
        {
            // Arrange
            _uut.SimulateConnected(false);
            bool eventFired = false;
            
            _uut.CurrentValueEvent += (sender, args) => 
            {
                eventFired = true;
            };

            // Act
            _uut.StartCharge();
            
            // Wait for potential timer ticks
            Thread.Sleep(300);

            // Assert
            Assert.That(eventFired, Is.False);
            Assert.That(_uut.CurrentValue, Is.EqualTo(0.0));
        }



        [Test]
        public void OnTimerElapsed_WhenNotConnected_CurrentValueRemains()
        {
            // Arrange
            _uut.StartCharge();
            Thread.Sleep(300); // Let charging start
            
            // Act
            _uut.SimulateConnected(false);
            
            // Set event handler after disconnecting
            bool eventFired = false;
            
            _uut.CurrentValueEvent += (sender, args) => 
            {
                eventFired = true;
            };
            
            // Simulate a timer tick using reflection
            var timerElapsedMethod = typeof(UsbChargerSimulator).GetMethod("OnTimerElapsed", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            timerElapsedMethod.Invoke(_uut, new object[] { null, null });
            
            // Assert
            Assert.That(eventFired, Is.False);
        }

        [Test]
        public void OnTimerElapsed_WhenNotCharging_CurrentValueRemains()
        {
            // Arrange - don't start charging
            bool eventFired = false;
            
            _uut.CurrentValueEvent += (sender, args) => 
            {
                eventFired = true;
            };
            
            // Act - simulate a timer tick using reflection
            var timerElapsedMethod = typeof(UsbChargerSimulator).GetMethod("OnTimerElapsed", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            timerElapsedMethod.Invoke(_uut, new object[] { null, null });
            
            // Assert
            Assert.That(eventFired, Is.False);
            Assert.That(_uut.CurrentValue, Is.EqualTo(0.0));
        }
    }
}
