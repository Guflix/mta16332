using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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
        List<List<IntPoint>> blobs;

        public List<IntPoint> biggestBlob;
        public int boxWidth, boxHeight, minX, minY, maxX, maxY;

        public BlobDetector(Bitmap img)
        {
            this.img = img;
        }

        public Bitmap blobDetection()
        {
            blobs = new List<List<IntPoint>>();  //list of blobs which contains the list of coordinates for blobs (pixels)
            bool[,] burned = new bool[img.Width, img.Height]; // twodimensional array of booleans
            for (int x = 0; x < img.Width; x++) // goes through every pixel,
            {
                for (int y = 0; y < img.Height; y++)
                {
                    Color color = img.GetPixel(x, y);
                    if (color.ToArgb() == white.ToArgb() && !burned[x, y]) // compares the colour of every pixel, if it is not black and we haven't checked it already
                    {
                        Queue<IntPoint> q = new Queue<IntPoint>(); //then it makes a new queue for every pixel and adds it there
                        q.Enqueue(new IntPoint(x, y));

                        List<IntPoint> blob = new List<IntPoint>(); //initiates new blob
                        while (q.Count > 0) //while the queue is not empty, the dequeue it and take a point p out
                        {
                            IntPoint p = q.Dequeue(); //duqueue means the 1st pixel in the queue
                            if (p.X >= 0 && p.X < img.Width && p.Y >= 0 && p.Y < img.Height) //it checks that it is within the boundaries of the image
                            {
                                Color col = img.GetPixel(p.X, p.Y); // new colour at the position of the point p
                                if (!burned[p.X, p.Y] && col.ToArgb() == white.ToArgb()) // checks if point p has not been checked and it's not black
                                {
                                    burned[p.X, p.Y] = true; // here you check/burn the pixel
                                    blob.Add(p);

                                    q.Enqueue(new IntPoint(p.X + 1, p.Y)); //you connect all the pixels around it to the queue
                                    q.Enqueue(new IntPoint(p.X, p.Y + 1));
                                    q.Enqueue(new IntPoint(p.X - 1, p.Y));
                                    q.Enqueue(new IntPoint(p.X, p.Y - 1));
                                    q.Enqueue(new IntPoint(p.X + 1, p.Y + 1));
                                    q.Enqueue(new IntPoint(p.X - 1, p.Y - 1));
                                    q.Enqueue(new IntPoint(p.X + 1, p.Y - 1));
                                    q.Enqueue(new IntPoint(p.X - 1, p.Y + 1));
                                }
                            }
                        }
                        blobs.Add(blob);
                    }
                }
            }
            sort();
            biggestBlob = blobs[1];

            List<IntPoint> hull = convexHull(biggestBlob);
            Bitmap image = new Bitmap(img.Width, img.Height, PixelFormat.Format24bppRgb); //new bitmap, same size as the picture
            BitmapData shapeImg = image.LockBits(new Rectangle(0, 0, img.Width, img.Height), ImageLockMode.ReadWrite, image.PixelFormat);
            Drawing.Polygon(shapeImg, hull, Color.White);
            image.UnlockBits(shapeImg);
            
            return image;
        }

        private void sort()
        {
            for (int i = 0; i < blobs.Count - 1; i++)
            {
                int j = i + 1;
                while (j > 0)
                {
                    if (blobs[j - 1].Count < blobs[j].Count)
                    {
                        List<IntPoint> temp = blobs[j - 1];
                        blobs[j - 1] = blobs[j];
                        blobs[j] = temp;
                    }
                    j--;
                }
            }
        }

        private List<IntPoint> maxBlob(List<List<IntPoint>> list) //searches the list and makes every bigger blob than the previous one the biggest blob
        {
            List<IntPoint> biggestBlob = new List<IntPoint>();
            foreach (List<IntPoint> blob in list)
            {
                if (blob.Count > biggestBlob.Count)
                {
                    biggestBlob = blob;
                }
            }
            return biggestBlob;
        }

        private List<IntPoint> convexHull(List<IntPoint> blob)
        {
            IConvexHullAlgorithm hullFinder = new GrahamConvexHull();
            blob = hullFinder.FindHull(blob);
            return blob;
        }

        private int MinX()
        {
            minX = img.Width;
            foreach (IntPoint p in biggestBlob)
            {
                if (p.X < minX)
                {
                    minX = p.X;
                }
            }
            return minX;
        }

        private int MinY()
        {
            minY = img.Height;
            foreach (IntPoint p in biggestBlob)
            {
                if (p.Y < minY)
                {
                    minY = p.Y;
                }
            }
            return minY;
        }

        private int MaxX()
        {
            maxX = 0;
            foreach (IntPoint p in biggestBlob)
            {
                if (p.X > maxX)
                {
                    maxX = p.X;
                }
            }
            return maxX;
        }

        private int MaxY()
        {
            maxY = 0;
            foreach (IntPoint p in biggestBlob)
            {
                if (p.Y > maxY)
                {
                    maxY = p.Y;
                }
            }
            return maxY;
        }

        public void boxSize()
        {
            boxWidth = MaxX() - MinX();
            boxHeight = MaxY() - MinY();
        }
    }
}
