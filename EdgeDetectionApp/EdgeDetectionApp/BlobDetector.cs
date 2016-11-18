using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;

namespace EdgeDetectionApp
{
    class BlobDetector
    {
        Bitmap img;
        Color black = Color.Black;
        List<List<System.Drawing.Point>> blobs;

        public List<System.Drawing.Point> biggestBlob;
        public int BBwidth, BBheight;

        public BlobDetector(Bitmap img)
        {
            this.img = img;
        }

        private bool compareColors(Color col1, Color col2) //comares two colours, returns true if they are the same, returns false if they are not
        {
            if (col1.ToArgb() == col2.ToArgb())
                return true;
            else
                return false;
        }

        public Bitmap blobDetection()
        {
            blobs = new List<List<System.Drawing.Point>>();  //list of blobs which contains the list of coordinates for blobs (pixels)
            bool[,] burned = new bool[img.Width, img.Height]; // twodimensional array of booleans
            for (int x = 0; x < img.Width; x++) // goes through every pixel,
            {
                for (int y = 0; y < img.Height; y++)
                {
                    Color color = img.GetPixel(x, y);
                    if (!compareColors(color, black) && !burned[x, y]) // compares the colour of every pixel, if it is not black and we haven't checked it already
                    {
                        Queue<System.Drawing.Point> q = new Queue<System.Drawing.Point>(); //then it makes a new queue for every pixel and adds it there
                        q.Enqueue(new System.Drawing.Point(x, y));

                        List<System.Drawing.Point> blob = new List<System.Drawing.Point>(); //initiates new blob
                        while (q.Count > 0) //while the queue is not empty, the dequeue it and take a point p out
                        {
                            System.Drawing.Point p = q.Dequeue(); //duqueue means the 1st pixel in the queue
                            if (p.X >= 0 && p.X < img.Width && p.Y >= 0 && p.Y < img.Height) //it checks that it is within the boundaries of the image
                            {
                                Color col = img.GetPixel(p.X, p.Y); // new colour at the position of the point p
                                if (!burned[p.X, p.Y] && !compareColors(col, black)) // checks if point p has not been checked and it's not black
                                {
                                    burned[p.X, p.Y] = true; // here you check/burn the pixel
                                    blob.Add(p);

                                    q.Enqueue(new System.Drawing.Point(p.X + 1, p.Y)); //you connect all the pixels around it to the queue
                                    q.Enqueue(new System.Drawing.Point(p.X, p.Y + 1));
                                    q.Enqueue(new System.Drawing.Point(p.X - 1, p.Y));
                                    q.Enqueue(new System.Drawing.Point(p.X, p.Y - 1));
                                    q.Enqueue(new System.Drawing.Point(p.X + 1, p.Y + 1));
                                    q.Enqueue(new System.Drawing.Point(p.X - 1, p.Y - 1));
                                    q.Enqueue(new System.Drawing.Point(p.X + 1, p.Y - 1));
                                    q.Enqueue(new System.Drawing.Point(p.X - 1, p.Y + 1));
                                }
                            }
                        }
                        blobs.Add(blob);
                    }
                }
            }
            biggestBlob = maxBlob(blobs); // finds the biggest blob, chcecks for every single blob in the list of blobs
            Bitmap shapeImg = new Bitmap(img.Width, img.Height, PixelFormat.Format24bppRgb); //new bitmap, same size as the picture
            foreach (System.Drawing.Point coor in biggestBlob) //just draws the blob which is the biggest one 
            {
                shapeImg.SetPixel(coor.X, coor.Y, Color.White);
            }
            return shapeImg;
        }

        private List<System.Drawing.Point> maxBlob(List<List<System.Drawing.Point>> list) //searches the list and makes every bigger blob than the previous one the biggest blob
        {
            List<System.Drawing.Point> biggestBlob = new List<System.Drawing.Point>();
            foreach (List<System.Drawing.Point> blob in list)
            {
                if (blob.Count > biggestBlob.Count)
                {
                    biggestBlob = blob;
                }
            }
            return biggestBlob;
        }

        private int MinX()
        {
            int minX = img.Width;
            foreach (System.Drawing.Point p in biggestBlob)
            {
                if (p.X < minX)
                {
                    minX = p.X;
                }
            }
            return minX;
        }

        public int MinY()
        {
            int minY = img.Height;
            foreach (System.Drawing.Point p in biggestBlob)
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
            int maxX = 0;
            foreach (System.Drawing.Point p in biggestBlob)
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
            int maxY = 0;
            foreach (System.Drawing.Point p in biggestBlob)
            {
                if (p.Y > maxY)
                {
                    maxY = p.Y;
                }
            }
            return maxY;
        }

        public void BBsize()
        {
            BBwidth = MaxX() - MinX();
            BBheight = MaxY() - MinY();
        }
    }
}
