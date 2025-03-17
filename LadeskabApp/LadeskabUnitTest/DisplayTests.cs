using NUnit.Framework;
using NSubstitute;
using System;
using System.IO;

using LadeskabLib;

namespace Ladeskab.Unit.Test
{
    [TestFixture]
    public class DisplayTests
    {
        private Display _uut;
        private StringWriter _stringWriter;

        [SetUp]
        public void Setup()
        {
            _uut = new Display();
            _stringWriter = new StringWriter();
            Console.SetOut(_stringWriter);
        }

        [TearDown]
        public void TearDown()
        {
            _stringWriter.Dispose();
        }

        [Test]
        public void ShowUserMessage_WriteToConsole()
        {
            // Arrange
            string message = "Test message";

            // Act
            _uut.ShowUserMessage(message);

            // Assert
            Assert.That(_stringWriter.ToString(), Does.Contain("Bruger Message: Test message"));
        }

        [Test]
        public void ShowChargingMessage_WriteToConsole()
        {
            // Arrange
            string message = "Test charging message";

            // Act
            _uut.ShowChargingMessage(message);

            // Assert
            Assert.That(_stringWriter.ToString(), Does.Contain("Opladning Status: Test charging message"));
        }
    }
}