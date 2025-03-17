

using NUnit.Framework;
using NSubstitute;
using System;
using System.IO;
using LadeskabLib;

namespace Ladeskab.Unit.Test
{
    [TestFixture]
    public class DoorTests
    {
        private Door _uut;

        [SetUp]
        public void Setup()
        {
            _uut = new Door();
        }
    

        [Test]
        public void Constructor_DoorInitiallyClosedAndUnlocked()
        {
            Assert.That(_uut.Closed, Is.True);
            Assert.That(_uut.Locked, Is.False);
        }

        [Test]
        public void OpenDoor_WhenDoorIsUnlocked_DoorOpens()
        {
            // Act
            _uut.OpenDoor();

            // Assert
            Assert.That(_uut.Closed, Is.False);
        }

        [Test]
        public void OpenDoor_WhenDoorIsLocked_DoorRemainsLocked()
        {
            // Arrange
            _uut.LockDoor();

            // Act
            _uut.OpenDoor();

            // Assert
            Assert.That(_uut.Closed, Is.True);
        }

        [Test]
        public void CloseDoor_WhenDoorIsOpen_DoorCloses()
        {
            // Arrange
            _uut.OpenDoor();

            // Act
            _uut.CloseDoor();

            // Assert
            Assert.That(_uut.Closed, Is.True);
        }

        [Test]
        public void LockDoor_WhenDoorIsClosed_DoorLocks()
        {
            // Act
            _uut.LockDoor();

            // Assert
            Assert.That(_uut.Locked, Is.True);
        }

        [Test]
        public void UnlockDoor_WhenDoorIsLocked_DoorUnlocks()
        {
            // Arrange
            _uut.LockDoor();

            // Act
            _uut.UnlockDoor();

            // Assert
            Assert.That(_uut.Locked, Is.False);
        }

        [Test]
        public void OpenDoor_RaisesEvent()
        {
            // Arrange
            bool eventRaised = false;
            DoorEventArgs receivedArgs = null;
            
            _uut.DoorStatusChanged += (sender, args) => 
            {
                eventRaised = true;
                receivedArgs = args;
            };

            // Act
            _uut.OpenDoor();

            // Assert
            Assert.That(eventRaised, Is.True);
            Assert.That(receivedArgs.DoorOpen, Is.True);
        }

        [Test]
        public void CloseDoor_RaisesEvent()
        {
            // Arrange
            _uut.OpenDoor();

            bool eventRaised = false;
            DoorEventArgs receivedArgs = null;
            
            _uut.DoorStatusChanged += (sender, args) => 
            {
                eventRaised = true;
                receivedArgs = args;
            };

            // Act
            _uut.CloseDoor();

            // Assert
            Assert.That(eventRaised, Is.True);
            Assert.That(receivedArgs.DoorOpen, Is.False);
        }
    }
}