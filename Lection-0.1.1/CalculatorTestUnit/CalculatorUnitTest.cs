
using NUnit.Framework;
using System;
using Calculator;


namespace CalculatorTestUnit;

internal class Tests
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
        // Arrange
        // Act
        uut.Add(5, 3);
        // Assert
        Assert.That(uut.Add(5, 3), Is.EqualTo(8));
    }

    [Test]
    public void TestSubtract()
    {
        // Arrange
        // Act
        uut.Subtract(5, 3);
        // Assert
        Assert.That(uut.Subtract(5, 3), Is.EqualTo(2));
    }

    [Test]
    public void TestMultiply()
    {
        // Arrange
        // Act
        uut.Multiply(5, 3);
        // Assert
        Assert.That(uut.Multiply(5, 3), Is.EqualTo(15));
    }

    [Test]
    public void TestPower()
    {
        // Arrange
        // Act
        uut.Power(5, 3);
        // Assert
        Assert.That(uut.Power(6, 3), Is.EqualTo(2));
    }
}
