using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TeilorClass;

namespace DrawingApp
{
    public class DrawingForm : Form
    {
        private TextBox x1TextBox, y1TextBox, x2TextBox, y2TextBox, errorTextBox, termsTextBox;
        private Label labelCurrentEr;
        private Button drawButton;
        private List<Point> pointsToDraw = new List<Point>();
        Panel controlPanel;
        private Panel drawingPanel;

        public DrawingForm()
        {
            this.Text = "Drawing Application";
            this.ClientSize = new Size(800, 600);
            this.DoubleBuffered = true;

            controlPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = Color.LightGray
            };

            x1TextBox = new TextBox { Width = 50, Left = 30, Top = 10, Height = 20 };
            y1TextBox = new TextBox { Width = 50, Left = 110, Top = 10, Height = 20 };
            x2TextBox = new TextBox { Width = 50, Left = 190, Top = 10, Height = 20 };
            y2TextBox = new TextBox { Width = 50, Left = 270, Top = 10, Height = 20 };

            drawButton = new Button
            {
                Text = "Draw",
                Left = 340,
                Top = 10,
                Width = 80
            };
            drawButton.Click += DrawButton_Click;

            controlPanel.Controls.Add(new Label { Text = "X1:", Left = x1TextBox.Left - 25, Top = 10, Width = 25, Height = 15 });
            controlPanel.Controls.Add(x1TextBox);
            controlPanel.Controls.Add(new Label { Text = "Y1:", Left = y1TextBox.Left - 25, Top = 10, Width = 25, Height = 15 });
            controlPanel.Controls.Add(y1TextBox);
            controlPanel.Controls.Add(new Label { Text = "X2:", Left = x2TextBox.Left - 25, Top = 10, Width = 25, Height = 15 });
            controlPanel.Controls.Add(x2TextBox);
            controlPanel.Controls.Add(new Label { Text = "Y2:", Left = y2TextBox.Left - 25, Top = 10, Width = 25, Height = 15 });
            controlPanel.Controls.Add(y2TextBox);
            controlPanel.Controls.Add(drawButton);

            controlPanel.Controls.Add(new Label { Text = "Error:", Left = drawButton.Right + 10, Top = 10, Width = 40, Height = 15 });
            errorTextBox = new TextBox { Width = 50, Left = drawButton.Right + 60, Top = 10, Height = 20 };
            controlPanel.Controls.Add(errorTextBox);

            controlPanel.Controls.Add(new Label { Text = "Number terms:", Left = errorTextBox.Right + 10, Top = 10, Width = 90, Height = 15 });
            termsTextBox = new TextBox { Width = 50, Left = errorTextBox.Right + 110, Top = 10, Height = 20 };
            controlPanel.Controls.Add(termsTextBox);

            controlPanel.Controls.Add(new Label { Text = "Current error:", Left = termsTextBox.Right + 10, Top = 10, Width = 100, Height = 15 });
            labelCurrentEr = new Label { Text = "-", Left = termsTextBox.Right + 105, Top = 10, Width = 40, Height = 15 };
            controlPanel.Controls.Add(labelCurrentEr);

            drawingPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };
            drawingPanel.Paint += DrawingPanel_Paint;

            this.Controls.Add(drawingPanel);
            this.Controls.Add(controlPanel);
        }

        private void DrawButton_Click(object sender, EventArgs e)
        {
            if (int.TryParse(x1TextBox.Text, out int x1) &&
                int.TryParse(y1TextBox.Text, out int y1) &&
                int.TryParse(x2TextBox.Text, out int x2) &&
                int.TryParse(y2TextBox.Text, out int y2) &&
                int.TryParse(errorTextBox.Text, out int error) &&
                int.TryParse(termsTextBox.Text, out int terms))
            {
                Point p1 = new Point(x1, y1);
                Point p2 = new Point(x2, y2);

                pointsToDraw = PointGenerator.GeneratePointsWithAngle(p1, p2, 3, error, terms);
                pointsToDraw.Add(p1);
                pointsToDraw.Add(p2);

                drawingPanel.Invalidate();
            }
            else
            {
                MessageBox.Show("Please enter valid integer coordinates.");
            }
        }

        private void DrawingPanel_Paint(object sender, PaintEventArgs e)
        {
            if (pointsToDraw.Count != 0)
            {
                Graphics g = e.Graphics;
                g.ScaleTransform(0.3f, 0.3f);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                int numPoint = pointsToDraw.Count;
                if (numPoint > 0)
                {
                    Point[] pointsArray = pointsToDraw.Take(numPoint - 2).ToArray();
                    g.DrawLines(Pens.Black, pointsArray);
                }

                if (pointsToDraw.Count >= 2)
                {
                    g.FillRectangle(Brushes.Green,
                        pointsToDraw[numPoint - 2].X - 4, pointsToDraw[numPoint - 2].Y - 4, 8, 8);

                    g.FillRectangle(Brushes.Blue,
                        pointsToDraw[numPoint - 1].X - 4,
                        pointsToDraw[numPoint - 1].Y - 4, 8, 8);
                }

                Point lastPoint = pointsToDraw[numPoint - 1];

                int.TryParse(errorTextBox.Text, out int circleRadius);
                int circleDiameter = circleRadius * 2;

                g.DrawEllipse(Pens.Red,
                              lastPoint.X - circleRadius,
                              lastPoint.Y - circleRadius,
                              circleDiameter,
                              circleDiameter);
            }
        }
    }
}