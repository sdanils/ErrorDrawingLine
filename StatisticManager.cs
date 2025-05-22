using DrawingApp;
using ScottPlot;

namespace StatisticManagerNM
{
    public class StatisticManager
    {
        public static void CreateStatistic(Point start, Point end)
        {
            using (StreamWriter writer = File.AppendText("result/data.txt"))
            {
                for (int term = 1; term < 10; term++)
                {
                    List<Point> pointsToDraw = PointGenerator.GeneratePointsWithAngle(start, end, 3, term);
                    double err = PointGenerator.GetError(pointsToDraw, end);

                    string data = $"{start.X} {start.Y} {end.X} {end.Y} {term} {err}";
                    writer.WriteLine(data);
                }
            }
        }

        public static void CreatePlotFromData(Point start, Point end)
        {
            try
            {
                string[] lines = File.ReadAllLines("result/data.txt");

                List<double> xValues = new List<double>();
                List<double> yValues = new List<double>();

                foreach (string line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    string[] values = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (values.Length >= 6)
                    {
                        if (double.TryParse(values[4], out double x) && double.TryParse(values[5], out double y))
                        {
                            xValues.Add(x);
                            yValues.Add(y);
                        }
                    }
                }

                var plot = new ScottPlot.Plot();

                var line1 = plot.Add.Scatter(xValues, yValues);
                line1.LegendText = $"Зависимость ошибки от точности разложения";
                line1.Color = Colors.Blue;

                plot.ShowLegend();
                plot.Title($"Старт: {start.X}:{start.Y} Конец: {end.X}:{end.Y}");
                plot.YLabel($"Ошибка");
                plot.XLabel($"Точность: n");

                plot.SavePng("result/plot.png", 1000, 800);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }
}