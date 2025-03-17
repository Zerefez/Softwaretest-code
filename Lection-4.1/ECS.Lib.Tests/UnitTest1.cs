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
         private FakeWindow _fakeWindow;
        private EcsController _controller;
        private StringWriter _consoleOutput;

        [SetUp]
        public void Setup()
        {
            _fakeHeater = new FakeHeater();
            _fakeTempSensor = new FakeTempSensor();
            _fakeWindow = new FakeWindow();
            _controller = new EcsController(23,30, _fakeTempSensor, _fakeHeater,_fakeWindow);
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
            int iterations = 100;
            int heaterOnCount = 0;
            int heaterOffCount = 0;
            int windowOpenCount = 0;
            int windowCloseCount = 0;

            // Act
            for (int i = 0; i < iterations; i++)
            {
                _controller.Regulate();
                string output = _consoleOutput.ToString();
                _consoleOutput.GetStringBuilder().Clear();

                if (output.Contains("Heater is on"))
                    heaterOnCount++;
                else if (output.Contains("Heater is off"))
                    heaterOffCount++;

                if (output.Contains("Window is open"))
                    windowOpenCount++;
                else if (output.Contains("Window is closed"))
                    windowCloseCount++;
            }

            // Assert
            Assert.Greater(heaterOnCount, 0, "Heater was never turned on during the test.");
            Assert.Greater(heaterOffCount, 0, "Heater was never turned off during the test.");
            Assert.Greater(windowOpenCount, 0, "Window was never opened during the test.");
            Assert.Greater(windowCloseCount, 0, "Window was never closed during the test.");
        }
    }
}
