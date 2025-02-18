﻿using System;

namespace Calculator
{
    public class CalculatorApp
    {
        // 01.2 OPGAVE
        public double Accumulator {get; private set; }

        public double Add(double addend)
        {
            Accumulator += addend;
            return Accumulator;
            
        }
        public double Subtract(double subtractor)
        {
            Accumulator -= subtractor;
            return Accumulator;
        }
        public double Multiply(double multiplier)
        {
            Accumulator *= multiplier;
            return Accumulator;
        }
        public double Power(double exponent)
        {
            Accumulator = Math.Pow(Accumulator, exponent);
            return Accumulator;
        }

        public double Divide(double divisor)
        {
            if (divisor == 0)
            {
                throw new DivideByZeroException();
            }
            Accumulator /= divisor;
            return Accumulator;
        }

        public double Divide(double dividend, double divisor) {
            if (divisor == 0) {
                throw new DivideByZeroException();
            }
            Accumulator = dividend / divisor;
            return Accumulator;
        }

        public void Clear(){
            Accumulator = 0;
        }

        // 01.1 OPGAVE
        public double Add(double a, double b)
        {
            return a + b;
            
        }
        public double Subtract(double a, double b)
        {
            return a - b;

        }
        public double Multiply(double a, double b)
        {
            return a * b;

        }
        public double Power(double x, double exp)
        {
            return Math.Pow(x, exp);
        }

    }
}
