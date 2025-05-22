using System;
using System.Collections.Generic;
using TeilorClass;

namespace DrawingApp
{
    public class PointGenerator
    {
        public static List<Point> GeneratePointsWithAngle(Point start, Point end, double stepSize, double r, int numTerms)
        {
            List<Point> points = new List<Point>();

            int x1 = start.X, y1 = start.Y;
            int x2 = end.X, y2 = end.Y;

            double angle = Math.Atan2(y2 - y1, x2 - x1);

            double totalDistance = DistanceBetween(x1, y1, x2, y2);
            double coveredDistance = 0;
            double x = x1;
            double y = y1;


            //Ряды Тейлора для тригонометрических функций наиболее точны при малых значениях аргумента (|x| < π/2). Чем дальше x от нуля (например, при x ≈ π), тем больше членов ряда нужно для точности
            while (totalDistance > coveredDistance)
            {
                points.Add(new Point((int)Math.Round(x), (int)Math.Round(y)));

                double xNew = x + stepSize * Teilor.Cos(angle, numTerms);
                double yNew = y + stepSize * Teilor.Sin(angle, numTerms);

                coveredDistance += DistanceBetween(x, y, xNew, yNew);

                x = xNew;
                y = yNew;
            }

            return points;
        }

        private static double DistanceBetween(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }
    }
}