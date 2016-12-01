using System;
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

        int i = 0;
        string filepath;
        Image2 image;
        Bitmap img;
        private Button button7;
        Bitmap img2;

        public FormFilter(Image2 image)
        {
            //this.filepath = filepath;
            this.image = image;
            InitializeComponent();
            image.preprocess(11);
            img = new Bitmap(image.orgImg);
            image.blobDetect();
            image.shapeDetect(0);
            pictureBox1.Image = image.draw(image.orgImg);
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
            if (i == 5 || i == image.bd.blobs.Count)
            {
                Console.WriteLine("Nope");
            }
            else
            {
                img2 = new Bitmap(img);
                image.shapeDetect(i);
                pictureBox1.Image = image.draw(img2);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            FormEdge r = new FormEdge(filepath);
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
