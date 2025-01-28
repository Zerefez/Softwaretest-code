using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    public class Calculator
    {
        
        private static double Add(double a, double b)
        {
            return a + b;
            
        }
        private static double Subtract(double a, double b)
        {
            return a - b;

        }
        private static double Multiply(double a, double b)
        {
            return a + b;

        }
        private static double Power(double x, double exp)
        {
            return Math.Pow(x, exp);
        }
    }
}
