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
    public partial class FormEnd : Form
    {
        public FormEnd(Bitmap image)
        {
            InitializeComponent();
            pictureBox1.Image = image;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormIntro f = new FormIntro();
            f.Show();
            Hide();

        }
    }
}
