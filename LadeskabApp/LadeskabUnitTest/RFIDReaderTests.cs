using NUnit.Framework;
using NSubstitute;
using System;
using System.IO;

using LadeskabLib;

namespace Ladeskab.Unit.Test
{
    [TestFixture]
    public class RFIDReaderTests
    {
        private RFIDReader _uut;

        [SetUp]
        public void Setup()
        {
            _uut = new RFIDReader();
        }

        [Test]
        public void SimulateRFIDDetection_EventRaised()
        {
            // Arrange
            bool eventRaised = false;
            RFIDEventArgs receivedArgs = null;
            
            _uut.RFIDDetected += (sender, args) => 
            {
                eventRaised = true;
                receivedArgs = args;
            };

            // Act
            _uut.SimulateRFIDDetection(123);

            // Assert
            Assert.That(eventRaised, Is.True);
            Assert.That(receivedArgs.RFID, Is.EqualTo(123));
        }
    }

}