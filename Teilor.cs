using System;
using System.Collections.Generic;

namespace TeilorClass
{
    public class Teilor
    {
        static private double Factorial(int n)
        {
            if (n <= 1) return 1;
            double result = 1;
            for (int i = 2; i <= n; i++)
            {
                result *= i;
            }
            return result;
        }

        static public double Sin(double x, int terms)
        {
            double result = 0;
            for (int n = 0; n < terms; n++)
            {
                double term = Math.Pow(-1, n) * Math.Pow(x, 2 * n + 1) / Factorial(2 * n + 1);
                result += term;
            }
            return result;
        }

         static public double Cos(double x, int terms)
        {
            double result = 0;
            for (int n = 0; n < terms; n++)
            {
                double term = Math.Pow(-1, n) * Math.Pow(x, 2 * n) / Factorial(2 * n);
                result += term;
            }
            return result;
        }
    }
}