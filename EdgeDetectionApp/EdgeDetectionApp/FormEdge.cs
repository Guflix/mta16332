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
    public partial class FormEdge : Form
    {
        int i;
        string filepath;
        Image2 Myimage;
        Bitmap image;
        private Button button7;
        Bitmap img2;

 

        public FormEdge(Bitmap image, Image2 Myimage)
        {
            this.image = image;
            this.Myimage = Myimage;
            InitializeComponent();
            pictureBox1.Image = image;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Myimage.sc.circle)
            {
                PopYes p = new PopYes(image, Myimage);
                p.Show();
            }
            else
            {
                PopNo o = new PopNo();
                o.Show();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Myimage.sc.triangle)
            {
                PopYes p = new PopYes(image, Myimage);
                p.Show();
            }
            else
            {
                PopNo o = new PopNo();
                o.Show();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (Myimage.sc.square)
            {
                PopYes p = new PopYes(image, Myimage);
                p.Show();
            }
            else
            {
                PopNo o = new PopNo();
                o.Show();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (Myimage.sc.rect)
            {
                PopYes p = new PopYes(image, Myimage);
                p.Show();
            }
            else
            {
                PopNo o = new PopNo();
                o.Show();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
      
        }

        private void FormEdge_Load(object sender, EventArgs e)
        {

        }
    }
}
