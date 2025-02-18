using NUnit.Framework;
using System;
using Calculator;

namespace Calculator.Test.Unit
{
    internal class CalculatorTests
    {
        // 01.2 OPGAVE
        private CalculatorApp _uut;

        [SetUp]
        public void Setup()
        {
            _uut = new CalculatorApp();
        }

        // Add Tests
        [TestCase(5, 10)] 
        [TestCase(-5, 3)] 
        [TestCase(0, 0)] 
        [TestCase(-2, -3)] // Edge case: Negative numbers
        [TestCase(0.1, 0.2)] // Edge case: Decimal numbers
        public void Add_AddTwoDoubles_SumIsCorrect(double a, double b)
        {
            // Act
            double result = _uut.Add(a, b);
            // Assert
            Assert.That(result, Is.EqualTo(a + b).Within(0.005));
        }

        // Subtract Tests
        [TestCase(10, 3)]  
        [TestCase(5, -5)] 
        [TestCase(0, 0)]   
        [TestCase(-5, -3)] // Edge case: Negative numbers
        [TestCase(2.5, 1.5)] // Edge case: Decimal numbers
        public void Subtract_SubtractTwoDoubles_DifferenceIsCorrect(double a, double b)
        {
            // Act
            double result = _uut.Subtract(a, b);
            // Assert
            Assert.That(result, Is.EqualTo(a - b).Within(0.005));
        }

        // Multiply Tests
        [TestCase(2, 3)]   
        [TestCase(-2, 4)]  
        [TestCase(0, 7)]   
        [TestCase(-2, -3)] // Edge case: Negative numbers
        [TestCase(0.5, 2)] // Edge case: Decimal numbers
        public void Multiply_MultiplyTwoDoubles_ProductIsCorrect(double a, double b)
        {
            // Act
            double result = _uut.Multiply(a, b);
            // Assert
            Assert.That(result, Is.EqualTo(a * b).Within(0.005));
        }

        // Power Tests
        [TestCase(2, 3)]  
        [TestCase(4, 0.5)] 
        [TestCase(5, 1)]  
        [TestCase(-2, 3)] // Edge case: Negative base with odd exponent
        [TestCase(2, -3)] // Edge case: Negative exponent
        public void Power_PowerTwoDoubles_ResultIsCorrect(double a, double b)
        {
            // Act
            double result = _uut.Power(a, b);
            // Assert
            Assert.That(result, Is.EqualTo(Math.Pow(a, b)).Within(0.0001));
        }

        // Divide Tests
        [TestCase(10, 2)] 
        [TestCase(-10, 5)]
        [TestCase(1, 1)]  
        [TestCase(-10, -2)] // Edge case: Negative numbers
        [TestCase(10, 3)]  // Edge case: Non-integer result
        public void Divide_DivideTwoDoubles_QuotientIsCorrect(double a, double b)
        {
            // Act
            double result = _uut.Divide(a, b);
            // Assert
            Assert.That(result, Is.EqualTo(a / b).Within(0.005));
        }

        [Test]
        public void Divide_ByZero_ThrowsException()
        {
            _uut.Add(5);
            Assert.Throws<DivideByZeroException>(() => _uut.Divide(0));
        }

        // Clear Tests
        [Test]
        public void Clear_AccumulatorIsResetToZero()
        {
            _uut.Add(5);
            _uut.Clear();
            Assert.That(_uut.Accumulator, Is.EqualTo(0.0).Within(0.005));
        }

        // Accumulator Chaining Tests

        // Add
        [TestCase(5, 10)] 
        [TestCase(-5, 3)] 
        [TestCase(0, 0)] 
        [TestCase(-2, -3)] // Edge case: Negative numbers
        [TestCase(0.1, 0.2)] // Edge case: Decimal numbers
        public void Add_AccumulatorChaining_ReturnsCorrectResult(double a, double b)
        {
            _uut.Add(a);
            Assert.That(_uut.Add(b), Is.EqualTo(a + b));
        }

        // Subtract
        [TestCase(10, 3)]  
        [TestCase(5, -5)] 
        [TestCase(0, 0)]   
        [TestCase(-5, -3)] // Edge case: Negative numbers
        [TestCase(2.5, 1.5)] // Edge case: Decimal numbers
        public void Subtract_AccumulatorChaining_ReturnsCorrectResult(double a, double b)
        {
            _uut.Add(a);
            Assert.That(_uut.Subtract(b), Is.EqualTo(a - b));
        }

        // Multiply
        [TestCase(2, 3)]   
        [TestCase(-2, 4)]  
        [TestCase(0, 7)]   
        [TestCase(-2, -3)] // Edge case: Negative numbers
        [TestCase(0.5, 2)] // Edge case: Decimal numbers
        public void Multiply_AccumulatorChaining_ReturnsCorrectResult(double a, double b)
        {
            _uut.Add(a);
            Assert.That(_uut.Multiply(b), Is.EqualTo(a * b));
        }

        // Divide
        [TestCase(10, 2)] 
        [TestCase(-10, 5)]
        [TestCase(1, 1)]  
        [TestCase(-10, -2)] // Edge case: Negative numbers
        [TestCase(10, 3)]  // Edge case: Non-integer result
        public void Divide_AccumulatorChaining_ReturnsCorrectResult(double a, double b)
        {
            _uut.Add(a);
            Assert.That(_uut.Divide(b), Is.EqualTo(a / b));
        }

        [Test]
        public void Divide_ByZero_ThrowsExceptionInAccumulatorChaining()
        {
            _uut.Add(5);
            Assert.Throws<DivideByZeroException>(() => _uut.Divide(0));
        }

        // Power
        [TestCase(2, 3)]  
        [TestCase(4, 0.5)] 
        [TestCase(5, 1)]  
        [TestCase(-2, 3)] // Edge case: Negative base with odd exponent
        [TestCase(2, -3)] // Edge case: Negative exponent
        public void Power_AccumulatorChaining_ReturnsCorrectResult(double a, double b)
        {
            _uut.Add(a);
            Assert.That(_uut.Power(b), Is.EqualTo(Math.Pow(a, b)).Within(0.0001));
        }
    }
}