using System;
using System.Drawing;
using System.Drawing.Imaging;
using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;

namespace EdgeDetectionApp
{
    class Shapecheck
    {
        Bitmap img;
        double perimeter, area; //obvod
        Color white = Color.White;

        public bool circle, rect, square, triangle;
        
        public Shapecheck(Bitmap img)
        {
            this.img = img;
        }

        private double countPixels() //we add all of the white pixel to the area variable, now that it has been filled with colour
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

        private void floodFill(int x, int y) //bucket tool, fills it with paint, AForge, stops when it hits the edges
        {
            PointedColorFloodFill filter = new PointedColorFloodFill();
            filter.FillColor = white;
            filter.StartingPoint = new IntPoint(x, y);
            filter.ApplyInPlace(img);
        }

        private void circularity(double area, double perimeter) //checks if it is cicular or not (function on the internet)
        {
            double c = perimeter / (2 * Math.Sqrt(Math.PI * area));
            if (c < 1.092)
                circle = true;
            Console.WriteLine(c);
        }

        private void triangleOrSquare(int boxHeight, int boxWidth)
        {
            double boxArea = (double)boxHeight * (double)boxWidth; //bbArea - bounding box area, now doesn't work
            if (area / boxArea < 0.703)
                triangle = true;


            double boxRatio;
            if (boxWidth > boxHeight)
                    boxRatio = (double)boxHeight / (double)boxWidth;
            else
                boxRatio = (double)boxWidth / (double)boxHeight;

            if (boxRatio > 0.95 && triangle == false)
                square = true;
            else
                rect = true;
        }

        public void whichShape(int boxHeight, int boxWidth, int minX, int minY, int maxX, int maxY)
        {
            perimeter = 1.10850404074*countPixels();
            floodFill(minX + boxWidth / 2, minY + boxHeight / 2);
            area = 0.9952575298*countPixels();
            circularity(area, perimeter);
            if(circle)
                triangleOrSquare(boxHeight, boxWidth);
        }
    }
}
