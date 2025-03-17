using NUnit.Framework;
using NSubstitute;
using System;
using System.IO;

using LadeskabLib;

namespace Ladeskab.Unit.Test
{
    [TestFixture]
    public class ChargeControlTests
    {
        private ChargeControl _uut;
        private IUsbCharger _fakeUsbCharger;
        private IDisplay _fakeDisplay;

        [SetUp]
        public void Setup()
        {
            _fakeUsbCharger = Substitute.For<IUsbCharger>();
            _fakeDisplay = Substitute.For<IDisplay>();
            _uut = new ChargeControl(_fakeUsbCharger, _fakeDisplay);
        }

        [Test]
        public void Constructor_InitializesCorrectly()
        {
            // Assert
            Assert.That(_uut.IsCharging, Is.False);
        }

        [Test]
        public void Constructor_SubscribesToCurrentValueEvent()
        {
            // Act - Trigger the event
            _fakeUsbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = 250 });
            
            // Assert - The display should receive a message indicating charging in progress
            _fakeDisplay.Received(1).ShowChargingMessage(Arg.Is<string>(s => s.Contains("Opladning i gang")));
        }

        [Test]
        public void IsConnected_ReturnsUsbChargerConnected()
        {
            // Arrange
            _fakeUsbCharger.Connected.Returns(true);

            // Act
            bool result = _uut.IsConnected();

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsConnected_WhenNotConnected_ReturnsFalse()
        {
            // Arrange
            _fakeUsbCharger.Connected.Returns(false);

            // Act
            bool result = _uut.IsConnected();

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void StartCharge_WhenConnected_StartsCharging()
        {
            // Arrange
            _fakeUsbCharger.Connected.Returns(true);

            // Act
            _uut.StartCharge();

            // Assert
            _fakeUsbCharger.Received(1).StartCharge();
            _fakeDisplay.Received(1).ShowChargingMessage(Arg.Is<string>(s => s.Contains("startet")));
            Assert.That(_uut.IsCharging, Is.True);
        }

        [Test]
        public void StartCharge_WhenConnected_RaisesChargingStateChangedEvent()
        {
            // Arrange
            _fakeUsbCharger.Connected.Returns(true);
            bool eventRaised = false;
            ChargeEventArgs receivedArgs = null;
            
            _uut.ChargingStateChanged += (sender, args) => 
            {
                eventRaised = true;
                receivedArgs = args;
            };
            
            // Act
            _uut.StartCharge();
            
            // Assert
            Assert.That(eventRaised, Is.True);
            Assert.That(receivedArgs.Connected, Is.True);
            Assert.That(receivedArgs.Message, Does.Contain("startet"));
        }

        [Test]
        public void StartCharge_WhenNotConnected_ShowsError()
        {
            // Arrange
            _fakeUsbCharger.Connected.Returns(false);

            // Act
            _uut.StartCharge();

            // Assert
            _fakeUsbCharger.DidNotReceive().StartCharge();
            _fakeDisplay.Received(1).ShowChargingMessage(Arg.Is<string>(s => s.Contains("Ingen telefon tilsluttet")));
            Assert.That(_uut.IsCharging, Is.False);
        }

        [Test]
        public void StopCharge_StopsUsbCharging()
        {
            // Arrange
            _fakeUsbCharger.Connected.Returns(true);
            _uut.StartCharge();

            // Act
            _uut.StopCharge();

            // Assert
            _fakeUsbCharger.Received(1).StopCharge();
            _fakeDisplay.Received(1).ShowChargingMessage(Arg.Is<string>(s => s.Contains("stoppet")));
            Assert.That(_uut.IsCharging, Is.False);
        }

        [Test]
        public void StopCharge_RaisesChargingStateChangedEvent()
        {
            // Arrange
            bool eventRaised = false;
            ChargeEventArgs receivedArgs = null;
            
            _uut.ChargingStateChanged += (sender, args) => 
            {
                eventRaised = true;
                receivedArgs = args;
            };
            
            // Act
            _uut.StopCharge();
            
            // Assert
            Assert.That(eventRaised, Is.True);
            Assert.That(receivedArgs.Current, Is.EqualTo(0.0));
            Assert.That(receivedArgs.Message, Does.Contain("stoppet"));
        }

        [Test]
        public void HandleCurrentValueChanged_ZeroCurrent_WhenCharging_ShowsNotConnected()
        {
            // Arrange
            _fakeUsbCharger.Connected.Returns(true);
            _uut.StartCharge(); // Set IsCharging to true

            // Act
            _fakeUsbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = 0 });

            // Assert
            _fakeDisplay.Received(1).ShowChargingMessage(Arg.Is<string>(s => s.Contains("Ingen telefon tilsluttet")));
        }

        [Test]
        public void HandleCurrentValueChanged_ZeroCurrent_WhenNotCharging_DoesNotDisplayMessage()
        {
            // Act - Note: IsCharging is false by default
            _fakeUsbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = 0 });
            
            // Assert - We should not see any messages since IsCharging is false
            _fakeDisplay.DidNotReceive().ShowChargingMessage(Arg.Any<string>());
        }

        [Test]
        public void HandleCurrentValueChanged_SmallCurrent_ShowsFullyCharged()
        {
            // Act
            _fakeUsbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = 5 });

            // Assert
            _fakeDisplay.Received(1).ShowChargingMessage(Arg.Is<string>(s => s.Contains("Telefon fuldt opladet")));
            Assert.That(_uut.IsCharging, Is.False);
        }

        [Test]
        public void HandleCurrentValueChanged_FullyCharged_RaisesChargingStateChangedEvent()
        {
            // Arrange
            bool eventRaised = false;
            ChargeEventArgs receivedArgs = null;
            
            _uut.ChargingStateChanged += (sender, args) => 
            {
                eventRaised = true;
                receivedArgs = args;
            };
            
            // Act
            _fakeUsbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = 5 });
            
            // Assert
            Assert.That(eventRaised, Is.True);
            Assert.That(receivedArgs.Connected, Is.True);
            Assert.That(receivedArgs.Current, Is.EqualTo(5));
            Assert.That(receivedArgs.Message, Does.Contain("Fuldt opladet"));
        }

        [Test]
        public void HandleCurrentValueChanged_NormalCurrent_ShowsCharging()
        {
            // Act
            _fakeUsbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = 250 });

            // Assert
            _fakeDisplay.Received(1).ShowChargingMessage(Arg.Is<string>(s => s.Contains("Opladning i gang")));
        }

        [Test]
        public void HandleCurrentValueChanged_HighCurrent_ShowsError()
        {
            // Act
            _fakeUsbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = 750 });

            // Assert
            _fakeDisplay.Received(1).ShowChargingMessage(Arg.Is<string>(s => s.Contains("FEJL")));
            _fakeUsbCharger.Received(1).StopCharge();
        }

        [Test]
        public void HandleCurrentValueChanged_Overload_RaisesChargingStateChangedEvent()
        {
            // Arrange
            bool eventRaised = false;
            ChargeEventArgs receivedArgs = null;
            
            _uut.ChargingStateChanged += (sender, args) => 
            {
                eventRaised = true;
                receivedArgs = args;
            };
            
            // Act
            _fakeUsbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = 750 });
            
            // Assert
            Assert.That(eventRaised, Is.True);
            Assert.That(receivedArgs.Connected, Is.True);
            Assert.That(receivedArgs.Current, Is.EqualTo(750));
            Assert.That(receivedArgs.Message, Does.Contain("Fejl: Overbelastning"));
        }

        [Test]
        public void IsCharging_AfterFullyCharged_BecomesFalse()
        {
            // Arrange
            _fakeUsbCharger.Connected.Returns(true);
            _uut.StartCharge();
            
            // Act
            _fakeUsbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = 5 });
            
            // Assert
            Assert.That(_uut.IsCharging, Is.False);
        }

        [Test]
        public void IsCharging_AfterOverload_BecomesFalse()
        {
            // Arrange
            _fakeUsbCharger.Connected.Returns(true);
            _uut.StartCharge();
            
            // Act
            _fakeUsbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = 750 });
            
            // Assert
            Assert.That(_uut.IsCharging, Is.False);
        }

        [Test]
        public void HandleCurrentValueChanged_BetweenFiveAndFiveHundred_NoStateChange()
        {
            // Arrange
            _fakeUsbCharger.Connected.Returns(true);
            _uut.StartCharge(); // IsCharging becomes true
            
            // Act
            _fakeUsbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs { Current = 250 });
            
            // Assert
            Assert.That(_uut.IsCharging, Is.True); // IsCharging should remain true
        }
    }
}