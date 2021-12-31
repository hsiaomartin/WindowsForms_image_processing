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
    public partial class Form_Brightness : Form
    {
        public Bitmap bitmap { get; set; }
        public myPixel[,] read_photo;

        Bitmap bitmap_Bright;
        Graphics gBright;
        public Form_Brightness()
        {
            InitializeComponent();
        }

        public void adjust_bright(int bR,int bG , int bB)
        {

            gBright.Clear(Color.White);

            for (int Ycount = 0; Ycount < bitmap.Height; Ycount++)
            {
                for (int Xcount = 0; Xcount < bitmap.Width; Xcount++)
                {
                    int R = (int)(read_photo[Xcount, Ycount].R * (bR / 100.0));
                    int G = (int)(read_photo[Xcount, Ycount].G * (bG / 100.0));
                    int B = (int)(read_photo[Xcount, Ycount].B * (bB / 100.0));

                    gBright.FillRectangle(new SolidBrush(Color.FromArgb(R, G, B)),Xcount, Ycount,  1, 1);

                }
            }
        }

        private void trackBar_Scroll(object sender, EventArgs e)
        {
            label4.Text = trackBar1.Value + " %";
            label5.Text = trackBar2.Value + " %";
            label6.Text = trackBar3.Value + " %";
            adjust_bright(trackBar1.Value, trackBar2.Value, trackBar3.Value);
            pictureBox1.Image = bitmap_Bright;
        }

        private void Form_Brightness_Shown(object sender, EventArgs e)
        {
            pictureBox1.Image = bitmap;
            bitmap_Bright = new Bitmap(bitmap.Width, bitmap.Height);
            gBright = Graphics.FromImage(bitmap_Bright);
            trackBar1.Value = trackBar2.Value = trackBar3.Value = 100;
            label4.Text = trackBar1.Value + " %";
            label5.Text = trackBar2.Value + " %";
            label6.Text = trackBar3.Value + " %";
        }
    }
}
