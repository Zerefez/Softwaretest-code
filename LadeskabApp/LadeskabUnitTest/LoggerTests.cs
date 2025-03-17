using NUnit.Framework;
using NSubstitute;
using System;
using System.IO;

using LadeskabLib;

namespace Ladeskab.Unit.Test
{
      [TestFixture]
    public class LoggerTests
    {
        private Logger _uut;
        private string _testLogFile;

        [SetUp]
        public void Setup()
        {
            _testLogFile = "testlog.txt";
            if (File.Exists(_testLogFile))
                File.Delete(_testLogFile);
                
            _uut = new Logger(_testLogFile);
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(_testLogFile))
                File.Delete(_testLogFile);
        }

        [Test]
        public void LogDoorLocked_WritesToFile()
        {
            // Act
            _uut.LogDoorLocked(123);

            // Assert
            Assert.That(File.Exists(_testLogFile), Is.True);
            string content = File.ReadAllText(_testLogFile);
            Assert.That(content, Does.Contain("Skab låst med RFID: 123"));
        }

        [Test]
        public void LogDoorUnlocked_WritesToFile()
        {
            // Act
            _uut.LogDoorUnlocked(456);

            // Assert
            Assert.That(File.Exists(_testLogFile), Is.True);
            string content = File.ReadAllText(_testLogFile);
            Assert.That(content, Does.Contain("Skab låst op med RFID: 456"));
        }
    }
}