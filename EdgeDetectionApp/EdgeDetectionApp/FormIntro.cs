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
    public partial class FormIntro : Form
    {
        //public string fil;
        public string filk;
        
        public FormIntro()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
        //    Image2 Myimage = new Image2(filk);
        //    pictureBox1.Image = Myimage.orgImg;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormFilter f = new FormFilter(filk);
            f.Show();
            Hide();
        }

        public void loadImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog o = new OpenFileDialog();
            o.InitialDirectory = "C:\\Github\\P3\\Kids";
            o.Filter = "Images only. |*.jpg; *.jpeg; *.png; *.bmp;";

            DialogResult dr = o.ShowDialog();
            pictureBox1.Image = Image.FromFile(o.FileName);
            filk = o.FileName.Replace("\\", "\\\\");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
