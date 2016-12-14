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

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog o = new OpenFileDialog();
            o.InitialDirectory = "C:\\Github\\P3\\Kids\\Testpics";
            o.Filter = "Images only. |*.jpg; *.jpeg; *.png; *.bmp;";

            DialogResult dr = o.ShowDialog();
            filk = o.FileName.Replace("\\", "\\\\");

            FormCheckPic f = new FormCheckPic(filk);
            f.Show();
            Hide();
        }
    }
}
