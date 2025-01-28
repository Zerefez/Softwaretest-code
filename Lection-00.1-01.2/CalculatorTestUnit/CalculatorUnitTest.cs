using NUnit.Framework;
using System;
using Calculator;

namespace Calculator.Test.Unit
{
    internal class CalculatorTests
    {
        private CalculatorApp uut;

        [SetUp]
        public void Setup()
        {
            uut = new CalculatorApp();
        }

        [Test]
        public void TestAdd()
        {
            // Act
            var result = uut.Add(5, 3);
            // Assert
            Assert.That(result, Is.EqualTo(8));
        }

        [Test]
        public void TestSubtract()
        {
            // Act
            var result = uut.Subtract(5, 3);
            // Assert
            Assert.That(result, Is.EqualTo(2));
        }

        [Test]
        public void TestMultiply()
        {
            // Act
            var result = uut.Multiply(5, 3);
            // Assert
            Assert.That(result, Is.EqualTo(15));
        }

        [Test]
        public void TestPower()
        {
            // Act
            var result = uut.Power(5, 3);
            // Assert
            Assert.That(result, Is.EqualTo(125));
        }

        [Test]
        public void TestDivide()
        {
            // Act
            var result = uut.Divide(6, 3);
            // Assert
            Assert.That(result, Is.EqualTo(2));
        }

        [Test]
        public void TestDivideByZero()
        {
            // Act & Assert
            Assert.Throws<DivideByZeroException>(() => uut.Divide(6, 0));
        }

        [Test]
        public void TestAccumulatorInitialValue()
        {
            // Assert
            Assert.That(uut.Accumulator, Is.EqualTo(0));
        }

        [Test]
        public void TestAccumulatorAfterAdd()
        {
            // Act
            uut.Add(5, 3);
            // Assert
            Assert.That(uut.Accumulator, Is.EqualTo(8));
        }

        [Test]
        public void TestAccumulatorAfterSubtract()
        {
            // Act
            uut.Subtract(5, 3);
            // Assert
            Assert.That(uut.Accumulator, Is.EqualTo(2));
        }

        [Test]
        public void TestAccumulatorAfterMultiply()
        {
            // Act
            uut.Multiply(5, 3);
            // Assert
            Assert.That(uut.Accumulator, Is.EqualTo(15));
        }

        [Test]
        public void TestAccumulatorAfterPower()
        {
            // Act
            uut.Power(5, 3);
            // Assert
            Assert.That(uut.Accumulator, Is.EqualTo(125));
        }

        [Test]
        public void TestAccumulatorAfterDivide()
        {
            // Act
            uut.Divide(6, 3);
            // Assert
            Assert.That(uut.Accumulator, Is.EqualTo(2));
        }

        [Test]
        public void TestAccumulatorAfterDivideByZero()
        {
            // Act & Assert
            Assert.Throws<DivideByZeroException>(() => uut.Divide(6, 0));
            // Assert
            Assert.That(uut.Accumulator, Is.EqualTo(0));
        }
    }
}