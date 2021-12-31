using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsForms_image_processing
{
    public partial class Form_splash_screen : Form
    {
        public Form_splash_screen()
        {
            InitializeComponent();
        }

        private void Form_splash_screen_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            if (progressBar1.Value <= 1000)
            {
                progressBar1.Increment( timer1.Interval);
            }
            else
            {
                timer1.Stop();
            }


        }
    }
}
