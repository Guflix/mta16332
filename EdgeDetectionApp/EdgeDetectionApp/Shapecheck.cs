using System;
using System.Drawing;
using System.Drawing.Imaging;
using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;

namespace EdgeDetectionApp
{
    public class Shapecheck
    {
        Bitmap img;
        double perimeter, area;
        Color white = Color.White;

        public bool circle, rect, square, triangle;

        public Shapecheck(Bitmap img)
        {
            this.img = img;
        }

        private double countPixels()
        {
            double pixels = 0;
            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    Color color = img.GetPixel(i, j);
                    if (color.ToArgb() == white.ToArgb())
                        pixels++;
                }
            }
            return pixels;
        }

        private void floodFill(int x, int y)
        {
            PointedColorFloodFill filter = new PointedColorFloodFill();
            filter.FillColor = white;
            filter.StartingPoint = new IntPoint(x, y);
            filter.ApplyInPlace(img);
        }

        public void whichShape(int boxHeight, int boxWidth, int minX, int minY, int maxX, int maxY)
        {
            perimeter = 1.10850404074 * countPixels();
            floodFill(minX + boxWidth / 2, minY + boxHeight / 2);
            area = 0.9952575298 * countPixels();

            double c = perimeter / (2 * Math.Sqrt(Math.PI * area));
            if (c < 1.092)
                circle = true;
            else
            {
                double boxArea = (double)boxHeight * (double)boxWidth;
                if (area / boxArea < 0.684)
                    triangle = true;
                else
                {
                    double boxRatio;
                    if (boxWidth > boxHeight)
                        boxRatio = (double)boxHeight / (double)boxWidth;
                    else
                        boxRatio = (double)boxWidth / (double)boxHeight;

                    if (boxRatio > 0.893)
                        square = true;
                    else
                        rect = true;
                }
            }
        }
    }
}