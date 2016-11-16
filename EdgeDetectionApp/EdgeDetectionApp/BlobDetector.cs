using System;
using System.Collections;
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
        Color black = Color.Black;
        List<List<System.Drawing.Point>> blobs;
        public List<System.Drawing.Point> biggestBlob;
        public int BBwidth, BBheight;

        public BlobDetector(Bitmap img)
        {
            this.img = img;
        }

        public bool compareColors(Color col1, Color col2) //comares two colours, returns true if they are the same, returns false if they are not
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

        public List<System.Drawing.Point> maxBlob(List<List<System.Drawing.Point>> list) //searches the list and makes every bigger blob than the previous one the biggest blob
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
        public int SmallestX(List<System.Drawing.Point> blob)
        {
            int smallest_x = img.Width;
            foreach (System.Drawing.Point p in blob)
            {
                if (p.X < smallest_x)
                {
                    smallest_x = p.X;
                }
            }
            return smallest_x;
        }
        public int SmallestY(List<System.Drawing.Point> blob)
        {
            int smallest_y = img.Height;
            foreach (System.Drawing.Point p in blob)
            {
                if (p.Y < smallest_y)
                {
                    smallest_y = p.Y;
                }
            }
            return smallest_y;
        }
        public int BiggestX(List<System.Drawing.Point> blob)
        {
            int biggest_x = 0;
            foreach (System.Drawing.Point p in blob)
            {
                if (p.X > biggest_x)
                {
                    biggest_x = p.X;
                }
            }
            return biggest_x;
        }
        public int BiggestY(List<System.Drawing.Point> blob)
        {
            int biggest_y = 0;
            foreach (System.Drawing.Point p in blob)
            {
                if (p.Y > biggest_y)
                {
                    biggest_y = p.Y;
                }
            }
            return biggest_y;
        }
        public void BBsize()
        {
            BBwidth = BiggestX(biggestBlob) - SmallestX(biggestBlob);
            BBheight = BiggestY(biggestBlob) - SmallestY(biggestBlob);
        }

    }
}
