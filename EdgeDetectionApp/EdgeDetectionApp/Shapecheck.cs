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
        double perimeter, area; //obvod
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

        public void perimeterDetection() //checks every white pixel and adds it to the perimeter
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

        public void drawShape() //bucket tool, fills it with paint, AForge, stops when it hits the edges
        {
            PointedColorFloodFill filter = new PointedColorFloodFill();
            filter.FillColor = Color.White;
            filter.StartingPoint = new IntPoint(img.Width / 2, img.Width / 2);
            filter.ApplyInPlace(img);
        }

        public void areaDetection() //we add all of the white pixel to the area variable, now that it has been filled with colour
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
        }

        public void circularity() //checks if it is cicular or not (function on the internet)
        {
            double c = (4 * Math.PI * area) / Math.Pow(perimeter, 2);
            if (c > 0.95)
                circle = true;
            else
                triangleOrSquare();
        }

        public void triangleOrSquare(int BBheight, int BBwidth)
        {
            double bbArea = BBheight * BBwidth; //bbArea - bounding box area, now doesn't work
            if (area / bbArea < 0.55)
                triangle = true;

            double bbRatio;
            if (BBwidth > BBheight)
                bbRatio = BBheight / BBwidth;
            else
                bbRatio = BBwidth / BBheight;

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
