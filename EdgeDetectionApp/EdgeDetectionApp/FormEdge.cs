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
        int i = 0;
        string filepath;
        Image2 Myimage;
        Bitmap img;
        private Button button7;
        Bitmap img2;

 

        public FormEdge(string filepath)
        {
            this.filepath = filepath;
            InitializeComponent();
            Myimage = new Image2(filepath);
            Myimage.preprocess(11);
            img = new Bitmap(Myimage.orgImg);
            Myimage.blobDetect();
            Myimage.shapeDetect(0);
            pictureBox1.Image = Myimage.draw(Myimage.orgImg);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
            
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //Image2 Myimage = new Image2();
            //pictureBox1.Image = Myimage.orgImg;
        }

        private void button6_Click(object sender, EventArgs e)
        {
        //    Image2 Myimage = new Image2();
        //    Myimage.preprocess(11);
        //    Myimage.blobDetect();
        //    Myimage.shapeDetect();
        //    Myimage.draw();
        //    pictureBox1.Image = Myimage.orgImg;
        }

        private void FormEdge_Load(object sender, EventArgs e)
        {

        }
    }
}
