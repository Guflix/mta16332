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
    public partial class FormCheckPic : Form
    {
        FormIntro f = new FormIntro();
        string filepath;
        Image2 Myimage;
        

        public FormCheckPic(string filepath)
        {
            this.filepath = filepath;
            InitializeComponent();
            Myimage = new Image2(filepath);
            pictureBox1.Image = Myimage.orgImg;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormFilter q = new FormFilter(Myimage);
            q.Show();
            Hide();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog o = new OpenFileDialog();
            o.InitialDirectory = "C:\\Github\\P3\\Kids";
            o.Filter = "Images only. |*.jpg; *.jpeg; *.png; *.bmp;";

            DialogResult dr = o.ShowDialog();
            filepath = o.FileName.Replace("\\", "\\\\");

            pictureBox1.Image = Image.FromFile(filepath);
        }
    }
}
