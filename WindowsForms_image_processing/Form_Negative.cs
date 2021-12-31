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
    public partial class Form_Negative : Form
    {
        public Bitmap bitmap { get; set; }
        public myPixel[,] read_photo;

        Bitmap bitmap_Negative;
        Bitmap bitmap_Gray_Negative;
        Graphics g_Negative;
        Graphics g_Gray_Negative;
        public Form_Negative()
        {
            InitializeComponent();
        }

        private void Form_Negative_Shown(object sender, EventArgs e)
        {
            bitmap_Negative = new Bitmap(bitmap.Width, bitmap.Height);
            bitmap_Gray_Negative = new Bitmap(bitmap.Width, bitmap.Height);

            g_Negative = Graphics.FromImage(bitmap_Negative);
            g_Gray_Negative = Graphics.FromImage(bitmap_Gray_Negative);


            g_Negative.Clear(Color.White);
            g_Gray_Negative.Clear(Color.White);


            for (int Ycount = 0; Ycount < bitmap.Height; Ycount++)
            {
                for (int Xcount = 0; Xcount < bitmap.Width; Xcount++)
                {
                    int R = read_photo[Xcount, Ycount].R;
                    int G = read_photo[Xcount, Ycount].G;
                    int B = read_photo[Xcount, Ycount].B;

                    int nR = 255 - R;
                    int nG = 255 - G;
                    int nB = 255 - B;

                    g_Negative.FillRectangle(new SolidBrush(Color.FromArgb(nR, nG, nB)), Xcount, Ycount, 1, 1);

                    int nGray = 255-(int)(R * 0.299 + G * 0.587 + B * 0.114);
                    g_Gray_Negative.FillRectangle(new SolidBrush(Color.FromArgb(nGray, nGray, nGray)), Xcount, Ycount, 1, 1);

                }
            }

            pictureBox1.Image = bitmap_Negative;
            pictureBox2.Image = bitmap_Gray_Negative;
        }
    }
}
