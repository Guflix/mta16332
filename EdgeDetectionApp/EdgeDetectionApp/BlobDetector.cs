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

        public BlobDetector(Bitmap img)
        {
            this.img = img;
        }

        public bool compareColors(Color col1, Color col2)
        {
            if (col1.ToArgb() == col2.ToArgb())
                return true;
            else
                return false;
        }

        public Bitmap blobDetection()
        {
            blobs = new List<List<System.Drawing.Point>>();
            bool[,] burned = new bool[img.Width, img.Height];
            for (int x = 0; x < img.Width; x++)
            {
                for (int y = 0; y < img.Height; y++)
                {
                    Color color = img.GetPixel(x, y);
                    if (!compareColors(color, black) && !burned[x, y])
                    {
                        Queue<System.Drawing.Point> q = new Queue<System.Drawing.Point>();
                        q.Enqueue(new System.Drawing.Point(x, y));

                        int pixels = 0;
                        List<System.Drawing.Point> blob = new List<System.Drawing.Point>();
                        while (q.Count > 0)
                        {
                            System.Drawing.Point p = q.Dequeue();
                            if (p.X >= 0 && p.X < img.Width && p.Y >= 0 && p.Y < img.Height)
                            {
                                Color col = img.GetPixel(p.X, p.Y);
                                if (!burned[p.X, p.Y] && !compareColors(col, black))
                                {
                                    burned[p.X, p.Y] = true;
                                    pixels++;
                                    blob.Add(p);

                                    q.Enqueue(new System.Drawing.Point(p.X + 1, p.Y));
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
            biggestBlob = maxBlob(blobs);
            Bitmap shapeImg = new Bitmap(img.Width, img.Height, PixelFormat.Format24bppRgb);
            foreach (System.Drawing.Point coor in biggestBlob)
            {
                shapeImg.SetPixel(coor.X, coor.Y, Color.White);
            }
            return shapeImg;
        }

        public List<System.Drawing.Point> maxBlob(List<List<System.Drawing.Point>> list)
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
    }
}
