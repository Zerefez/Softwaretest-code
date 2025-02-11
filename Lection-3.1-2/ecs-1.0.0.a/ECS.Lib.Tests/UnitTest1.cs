using NUnit.Framework;
using ECS.Lib;
using ECS.Lib.MockData;
using System;
using System.IO;

namespace ECS.Lib.Tests
{
    public class EcsControllerTests
    {
        private FakeHeater _fakeHeater;
        private FakeTempSensor _fakeTempSensor;
        private EcsController _controller;
        private StringWriter _consoleOutput;

        [SetUp]
        public void Setup()
        {
            _fakeHeater = new FakeHeater();
            _fakeTempSensor = new FakeTempSensor();
            _controller = new EcsController(23, _fakeTempSensor, _fakeHeater);
            _consoleOutput = new StringWriter();
            Console.SetOut(_consoleOutput);
        }

        [TearDown]
        public void Teardown()
        {
            _consoleOutput.Dispose();
            Console.SetOut(Console.Out);
        }

        [Test]
        public void Regulate_MultipleRandomTemperatures_HeaterBehaviorIsCorrect()
        {
            // Arrange
            int threshold = 23;
            _controller.SetThreshold(threshold);
            int iterations = 100;
            bool heaterTurnedOn = false;
            bool heaterTurnedOff = false;

            // Act
            for (int i = 0; i < iterations; i++)
            {
                _controller.Regulate();
                string output = _consoleOutput.ToString();
                _consoleOutput.GetStringBuilder().Clear();

                if (output.Contains("Heater is on"))
                {
                    heaterTurnedOn = true;
                }
                else if (output.Contains("Heater is off"))
                {
                    heaterTurnedOff = true;
                }

                // If both conditions are met, exit early
                if (heaterTurnedOn && heaterTurnedOff)
                {
                    break;
                }
            }

            // Assert
            Assert.IsTrue(heaterTurnedOn, "Heater was never turned on during the test.");
            Assert.IsTrue(heaterTurnedOff, "Heater was never turned off during the test.");
        }
    }
}
