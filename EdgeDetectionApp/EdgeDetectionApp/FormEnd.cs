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
    public partial class FormEnd : Form
    {
        Bitmap image;

        public FormEnd(Bitmap image)
        {
            this.image = image;
            InitializeComponent();
            pictureBox1.Image = image;

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormIntro f = new FormIntro();
            f.Show();
            Hide();

        }
    }
}
