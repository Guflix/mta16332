using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using Emgu.CV;
using Emgu.CV.Structure;
using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Math.Geometry;

namespace EdgeDetectionApp
{
    class Shapecheck
    {
        Bitmap img;
        double perimeter, area;
        bool circle, rect, square, triangle;
        Color white = Color.White;
        Color black = Color.Black;

        public Shapecheck(Bitmap img)
        {
            this.img = img;
        }

        public bool compareColor(Color col1, Color col2)
        {
            if (col1.ToArgb() == col2.ToArgb())
                return true;
            else
                return false;
        }

        public void perimeterDetection()
        {
            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    Color color = img.GetPixel(i, j);
                    if (compareColor(color, white) == true)
                        perimeter++;
                }
            }
        }

        public void drawShape()
        {
            /*int pixels = 0;
            bool[,] check = new bool[img.Width, img.Height];
            for (int x = 0; x < img.Width; x++)
            {
                for (int y = 0; y < img.Height; y++)
                {
                    Color color = img.GetPixel(x, y);
                    while (compareColor(color, black) == true && check[x, y] == false)
                    {
                        pixels++;
                    }
                }
            }
            int a = img.Width * img.Height - pixels;
            Console.WriteLine(a);*/

            PointedColorFloodFill filter = new PointedColorFloodFill();
            filter.FillColor = Color.White;
            filter.StartingPoint = new IntPoint(img.Width / 2, img.Width / 2);
            filter.ApplyInPlace(img);
        }

        public void areaDetection()
        {
            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    Color color = img.GetPixel(i, j);
                    if (compareColor(color, white) == true)
                        area++;
                }
            }
            Console.WriteLine(area);
        }

        public void circularity()
        {
            double c = (4 * Math.PI * area) / Math.Pow(perimeter, 2);
            if (c > 0.95)
                circle = true;
            else
                shape();
        }

        public void shape()
        {
            double bbArea = img.Height * img.Width;
            if (area / bbArea < 0.55)
                triangle = true;

            double bbRatio;
            if (img.Width > img.Height)
                bbRatio = img.Height / img.Width;
            else
                bbRatio = img.Width / img.Height;

            if (bbRatio > 0.95 && triangle == false)
                square = true;
            else
                rect = true;
        }

        public string whichShape()
        {
            perimeterDetection();
            drawShape();
            areaDetection();
            circularity();
            if (circle)
                return "circle";
            else if (triangle)
                return "triangle";
            else if (square)
                return "square";
            else if (rect)
                return "rect";
            else
                return "what the fuck";
        }
    }
}
