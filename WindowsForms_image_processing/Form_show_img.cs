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

    public partial class Form_show_img : Form
    {
        read_Bitmap read_bitmap;
        Graphics gMyImg;
        public Bitmap bitmap;
        public void drawMyImgData()
        {
            //e.Graphics.Clear(Color.Black);
            gMyImg.Clear(Color.Black);
            
            int count = 0;
            for (int Ycount =read_bitmap.info_header.biHeight-1 ; Ycount >= 0; Ycount--)
            {
                for (int Xcount =0; Xcount < read_bitmap.info_header.biWidth; Xcount++)
                {
                    int R = read_bitmap.myRowData[Xcount, Ycount].R;
                    int G = read_bitmap.myRowData[Xcount, Ycount].G;
                    int B = read_bitmap.myRowData[Xcount, Ycount].B;

                    gMyImg.FillRectangle(new SolidBrush(Color.FromArgb(R, G, B)), Xcount, Ycount, 1, 1);
                    count++;
                }
            }
            
        }

        public Form_show_img(read_Bitmap read_bitmap = null)
        {
            InitializeComponent();
            //this.read_bitmap = read_bitmap;
            //bitmap = new Bitmap(read_bitmap.info_header.biWidth, read_bitmap.info_header.biHeight);
            //gMyImg = this.CreateGraphics();
            //gMyImg = pictureBox1.CreateGraphics();
            //gMyImg = Graphics.FromImage(bitmap);
            //pictureBox1.Image = bitmap;
        }

        private void Form_show_img_VisibleChanged(object sender, EventArgs e)
        {
            pictureBox1.Image = bitmap;
            //drawMyImgData();
        }

    }

}
