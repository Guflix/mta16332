﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EdgeDetectionApp
{
    public partial class FormFilter : Form
    {

        public int i = 0;
        string filepath;
        Image2 Myimage;
        Bitmap img;
        private Button button7;
        Bitmap img2;

        public FormFilter(string filepath)
        {
            this.filepath = filepath;
            InitializeComponent();
            Myimage = new Image2(filepath);
            Myimage.preprocess(11);
            img = new Bitmap(Myimage.orgImg);
            Myimage.blobDetect();
            Myimage.shapeDetect(0);
            Myimage.orgImg = Myimage.drawShape(Myimage.orgImg);
            pictureBox1.Image = Myimage.orgImg;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            i++;
            if (i == 5 || i == Myimage.bd.blobs.Count)
            {
                Console.WriteLine("Nope");
            }
            else
            {
                img2 = new Bitmap(img);
                Myimage.shapeDetect(i);
                pictureBox1.Image = Myimage.drawShape(img2);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            FormEdge r;
            if (i == 0)
                r = new FormEdge(Myimage.drawColor(Myimage.orgImg), Myimage);
            else
                r = new FormEdge(Myimage.drawColor(img2), Myimage);
            r.Show();
            Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }
    }
}
