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
    class BlobDetector
    {
        Bitmap img;
        Color white = Color.White;
        Color black = Color.Black;
        int curlab;
        int[,] labels;
        System.Drawing.Point[] points;
        System.Drawing.Point p;

        public BlobDetector(Bitmap img)
        {
            this.img = img;
            labels = new int[img.Width, img.Height];
        }

        /*public bool checkForObject(int x, int y)
        {
            if (img.GetPixel(x, y).ToArgb() == white.ToArgb())
                return true;
            else
                return false;
        }


        public bool compareColor(Color col1, Color col2)
        {
            if (col1.ToArgb() == col2.ToArgb())
                return true;
            else
                return false;
        }

        public void label(int x, int y)
        {
            labels[x, y]++;
        }

        public int getLabel(int x, int y)
        {
            return labels[x, y];
        }

        public bool border(int x, int y)
        {
            if (x < 0 || x > img.Width || y < 0 || y > img.Height)
                return true;
            else
                return false;
        }

        public void check()
        {
            points = new System.Drawing.Point[img.Height * img.Width];
            for (int x = 0; x < img.Width; x++)
            {
                for (int y = 0; y < img.Height; y++)
                {
                    if ((checkForObject(x, y) == true) && (getLabel(x, y) == 0) && (border(x, y) == false))
                    {
                        p = new System.Drawing.Point(x, y);
                        points[curlab] = p;
                        curlab++;
                        label(x, y);
                        img.SetPixel(x, y, Color.Cyan);
                    }
                }
            }
        }

        public void printBlob()
        {
            for (int i = 0; i < points.Length; i++)
            {
                if (points[i] != new System.Drawing.Point(0, 0))
                {
                    Console.WriteLine(points[i]);
                }
            }
        }

        /*public void detect()
        {
            bool[,] painted = new bool[img.Width, img.Height];

            for (int x = 0; x < img.Height - 0; x++)
            {
                for (int y = 0; y < img.Width - 0; y++)
                {
                    Color color = img.GetPixel(y, x);
                    if (compareColor(color, black) && !painted[y, x])
                    {
                        int pixelCount = 0;

                        Queue<System.Drawing.Point> queue = new Queue<System.Drawing.Point>();
                        queue.Enqueue(new System.Drawing.Point(x, y));
                        while (queue.Count > 0)
                        {
                            System.Drawing.Point p = queue.Dequeue();

                            if ((p.X >= 0) && (p.X < img.Width) && (p.Y >= 0) && (p.Y < img.Height))
                            {
                                Color col = img.GetPixel(p.X, p.Y);
                                if (!painted[p.X, p.Y] && compareColor(col, white) == true)
                                {
                                    painted[p.X, p.Y] = true;
                                    img.SetPixel(p.X, p.Y, Color.Cyan);
                                    pixelCount++;
                                    queue.Enqueue(new System.Drawing.Point(p.X + 1, p.Y));
                                    queue.Enqueue(new System.Drawing.Point(p.X - 1, p.Y));
                                    queue.Enqueue(new System.Drawing.Point(p.X, p.Y + 1));
                                    queue.Enqueue(new System.Drawing.Point(p.X, p.Y - 1));
                                    queue.Enqueue(new System.Drawing.Point(p.X + 1, p.Y + 1));
                                    queue.Enqueue(new System.Drawing.Point(p.X - 1, p.Y + 1));
                                    queue.Enqueue(new System.Drawing.Point(p.X - 1, p.Y + 1));
                                    queue.Enqueue(new System.Drawing.Point(p.X - 1, p.Y - 1));
                                }
                            }
                        }
                        if (pixelCount > 100)
                            Console.WriteLine("blob detected: " + pixelCount + " pixels");
                    }
                }
            }
        }
    }*/



        public Bitmap biggestShape()
        {   
            ExtractBiggestBlob filter = new ExtractBiggestBlob();
            img = filter.Apply(img);
            return img;
            //blob = filter.BlobPosition;
        }
    }
}
