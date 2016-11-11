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
        public int shape;

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
            Console.WriteLine(perimeter);
        }

        public void drawShape()
        {
            int pixels = 0;
            bool[,] check = new bool[img.Width, img.Height];
            for (int x = 0; x < img.Width; x++)
            {
                for (int y = 0; y < img.Height; y++)
                {
                    Color color = img.GetPixel(x, y);
                    while (compareColor(color, white) == true && check[x, y] == false)
                    {
                        pixels++;
                        check[x, y] = true;
                    }
                }
            }
            int a = (img.Width * img.Height) - pixels;
            Console.WriteLine(img.Width * img.Height);
            Console.WriteLine("pixels: " + a);

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
                triangleOrSquare();
        }

        public void triangleOrSquare()
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

        public int whichShape()
        {
            perimeterDetection();
            drawShape();
            areaDetection();
            circularity();
            if (circle)
                shape = 1;
            else if (triangle)
                shape = 2;
            else if (square)
                shape = 3;
            else if (rect)
                shape = 4;
            else
                shape = 5;
            return shape;
        }
    }
}
