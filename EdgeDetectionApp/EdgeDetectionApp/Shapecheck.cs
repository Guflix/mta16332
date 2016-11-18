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
        int perimeter, area; //obvod
        Color white = Color.White;
        Color black = Color.Black;

        public bool circle, rect, square, triangle;
        
        public Shapecheck(Bitmap img)
        {
            this.img = img;
        }

        private bool compareColor(Color col1, Color col2)
        {
            if (col1.ToArgb() == col2.ToArgb())
                return true;
            else
                return false;
        }

        private int countPixels() //we add all of the white pixel to the area variable, now that it has been filled with colour
        {
            int pixels = 0;
            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    Color color = img.GetPixel(i, j);
                    if (compareColor(color, white) == true)
                        pixels++;
                }
            }
            return pixels;
        }

        private void drawShape() //bucket tool, fills it with paint, AForge, stops when it hits the edges
        {
            PointedColorFloodFill filter = new PointedColorFloodFill();
            filter.FillColor = Color.White;
            filter.StartingPoint = new IntPoint(img.Width / 2, img.Width / 2);
            filter.ApplyInPlace(img);
        }

        private void circularity(int area, int perimeter) //checks if it is cicular or not (function on the internet)
        {
            double c = (4 * Math.PI * area) / Math.Pow(perimeter, 2);
            if (c > 0.95)
                circle = true;
        }

        private void triangleOrSquare(int BBheight, int BBwidth)
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

        public void whichShape(int BBheight, int BBwidth)
        {
            perimeter = countPixels();
            drawShape();
            area = countPixels();
            circularity(area, perimeter);
            if(!circle)
                triangleOrSquare(BBheight, BBwidth);
        }
    }
}
