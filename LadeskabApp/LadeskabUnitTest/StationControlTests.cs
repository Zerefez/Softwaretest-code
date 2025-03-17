using NUnit.Framework;
using NSubstitute;
using System;
using System.IO;

using LadeskabLib;

namespace Ladeskab.Unit.Test
{
    [TestFixture]
    public class StationControlTests
    {
        private StationControl _uut;
        private IDoor _fakeDoor;
        private IDisplay _fakeDisplay;
        private IRFIDReader _fakeRfidReader;
        private IChargeControl _fakeChargeControl;
        private ILogger _fakeLogger;

        [SetUp]
        public void Setup()
        {
            _fakeDoor = Substitute.For<IDoor>();
            _fakeDisplay = Substitute.For<IDisplay>();
            _fakeRfidReader = Substitute.For<IRFIDReader>();
            _fakeChargeControl = Substitute.For<IChargeControl>();
            _fakeLogger = Substitute.For<ILogger>();

            _uut = new StationControl(
                _fakeDoor,
                _fakeDisplay,
                _fakeRfidReader,
                _fakeChargeControl,
                _fakeLogger);
        }

        [Test]
        public void DoorOpened_AvailableState_DisplaysConnectMessage()
        {
            // Act
            _fakeDoor.DoorStatusChanged += Raise.EventWith(new DoorEventArgs { DoorOpen = true });

            // Assert
            _fakeDisplay.Received(1).ShowUserMessage(Arg.Is<string>(s => s.Contains("Tilslut")));
        }
        [Test]
        public void DoorClosed_DoorOpenState_DisplaysScanMessage()
        {
            // Arrange - First set state to DoorOpen
            _fakeDoor.DoorStatusChanged += Raise.EventWith(new DoorEventArgs { DoorOpen = true });

            // Act
            _fakeDoor.DoorStatusChanged += Raise.EventWith(new DoorEventArgs { DoorOpen = false });

            // Assert
            _fakeDisplay.Received(1).ShowUserMessage(Arg.Is<string>(s => s.Contains("Scan")));
        }

        [Test]
        public void RFIDDetected_AvailableState_PhoneConnected_LocksDoor()
        {
            // Arrange
            _fakeChargeControl.IsConnected().Returns(true);

            // Act
            _fakeRfidReader.RFIDDetected += Raise.EventWith(new RFIDEventArgs { RFID = 123 });

            // Assert
            _fakeDoor.Received(1).LockDoor();
            _fakeChargeControl.Received(1).StartCharge();
            _fakeLogger.Received(1).LogDoorLocked(123);
            _fakeDisplay.Received(1).ShowUserMessage(Arg.Is<string>(s => s.Contains("låst")));
        }

        [Test]
        public void RFIDDetected_AvailableState_PhoneNotConnected_DisplaysError()
        {
            // Arrange
            _fakeChargeControl.IsConnected().Returns(false);

            // Act
            _fakeRfidReader.RFIDDetected += Raise.EventWith(new RFIDEventArgs { RFID = 123 });

            // Assert
            _fakeDoor.DidNotReceive().LockDoor();
            _fakeChargeControl.DidNotReceive().StartCharge();
            _fakeDisplay.Received(1).ShowUserMessage(Arg.Is<string>(s => s.Contains("ikke korrekt tilsluttet")));
        }

        [Test]
        public void RFIDDetected_DoorOpenState_DisplaysCloseMessage()
        {
            // Arrange - Set state to DoorOpen
            _fakeDoor.DoorStatusChanged += Raise.EventWith(new DoorEventArgs { DoorOpen = true });

            // Act
            _fakeRfidReader.RFIDDetected += Raise.EventWith(new RFIDEventArgs { RFID = 123 });

            // Assert
            _fakeDisplay.Received(1).ShowUserMessage(Arg.Is<string>(s => s.Contains("Luk døren")));
        }

        [Test]
        public void RFIDDetected_LockedState_CorrectRFID_UnlocksDoor()
        {
            // Arrange - Set state to Locked
            _fakeChargeControl.IsConnected().Returns(true);
            _fakeRfidReader.RFIDDetected += Raise.EventWith(new RFIDEventArgs { RFID = 123 });

            // Act - Use same RFID to unlock
            _fakeRfidReader.RFIDDetected += Raise.EventWith(new RFIDEventArgs { RFID = 123 });

            // Assert
            _fakeDoor.Received(1).UnlockDoor();
            _fakeChargeControl.Received(1).StopCharge();
            _fakeLogger.Received(1).LogDoorUnlocked(123);
            _fakeDisplay.Received(1).ShowUserMessage(Arg.Is<string>(s => s.Contains("Tag din telefon")));
        }

        [Test]
        public void RFIDDetected_LockedState_IncorrectRFID_DisplaysError()
        {
            // Arrange - Set state to Locked
            _fakeChargeControl.IsConnected().Returns(true);
            _fakeRfidReader.RFIDDetected += Raise.EventWith(new RFIDEventArgs { RFID = 123 });

            // Act - Use different RFID to unlock
            _fakeRfidReader.RFIDDetected += Raise.EventWith(new RFIDEventArgs { RFID = 456 });

            // Assert
            _fakeDoor.DidNotReceive().UnlockDoor();
            _fakeChargeControl.DidNotReceive().StopCharge();
            _fakeDisplay.Received(1).ShowUserMessage(Arg.Is<string>(s => s.Contains("Forkert RFID")));
        }
    }
}