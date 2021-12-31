using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsForms_image_processing
{
    public partial class Form_cut : Form
    {
        public myPixel[,] read_photo;
        public mySize size;
        public Bitmap bitmap,bitmap_cut;
        public enum mode { Square,Circle}
        mode current_mode;
        int click_time;
        Point pa, pb;
        Graphics gMyImg, gMyImg_cut;
        public void drawMyImgData()
        {
            //myPicture my_Picture;
            int w = size.Width;
            int h = size.Height;
            //e.Graphics.Clear(Color.Black);
            gMyImg.Clear(Color.Black);
            //my_Picture = new myPicture(w,h );
            int count = 0;
            for (int Ycount = 0; Ycount < h; Ycount++)
            {
                for (int Xcount = 0; Xcount < w; Xcount++)
                {
                    int R = read_photo[Xcount, Ycount].R;
                    int G = read_photo[Xcount, Ycount].G;
                    int B = read_photo[Xcount, Ycount].B;

                    gMyImg.FillRectangle(new SolidBrush(Color.FromArgb(R, G, B)), Xcount, Ycount, 1, 1);
                    count++;
                }
            }


        }

        public void drawMyImgData(Rectangle rectangle)
        {
            //myPicture my_Picture;
            int w = rectangle.Width;
            int h = rectangle.Height;
            //e.Graphics.Clear(Color.Black);
            gMyImg_cut.Clear(Color.Black);
            //my_Picture = new myPicture(w,h );
            int count = 0;
            for (int Ycount = 0; Ycount < h; Ycount++)
            {
                for (int Xcount = 0; Xcount < w; Xcount++)
                {
                    int R = read_photo[Xcount+ rectangle.X, Ycount+ rectangle.Y].R;
                    int G = read_photo[Xcount+ rectangle.X, Ycount+ rectangle.Y].G;
                    int B = read_photo[Xcount+ rectangle.X, Ycount+ rectangle.Y].B;

                    gMyImg_cut.FillRectangle(new SolidBrush(Color.FromArgb(R, G, B)), Xcount, Ycount, 1, 1);
                    count++;
                }
            }


        }

        public Form_cut()
        {
            InitializeComponent();


        }

        private void button1_Click(object sender, EventArgs e)
        {
            drawMyImgData();
           // button1.Text = "Square";
            current_mode = mode.Square;
            click_time = 2;
            pictureBox1.Image = bitmap;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            drawMyImgData();
            //button2.Text = "Circle";
            current_mode = mode.Circle;
            click_time = 2;
            pictureBox1.Image = bitmap;
        }

        private void Form_cut_VisibleChanged(object sender, EventArgs e)
        {
            bitmap = new Bitmap(size.Width,size.Height);
            gMyImg = Graphics.FromImage(bitmap);
            
            drawMyImgData();
            pictureBox1.Image = bitmap;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            Console.WriteLine(e.Location + " time:" +click_time);
            if (current_mode == mode.Square){
                if(click_time == 2)
                {
                    pa = e.Location;
                }
                else if(click_time == 1)
                {
                    //draw rectangle
                    pb = e.Location;
                    Rectangle rectangle = new Rectangle(pa.X, pa.Y, pb.X - pa.X, pb.Y - pa.Y);
                    Pen pen = new Pen(Color.Red, 5);
                    pen.DashStyle = DashStyle.Dot;
                    //Graphics g = gMyImg;
                    gMyImg.DrawRectangle(pen,rectangle);
                    pictureBox1.Image = bitmap;
                    Console.WriteLine("draw :"+ rectangle);

                    //cut
                    bitmap_cut = new Bitmap(rectangle.Width, rectangle.Height);
                    gMyImg_cut = Graphics.FromImage(bitmap_cut);
                    drawMyImgData(rectangle);
                    pictureBox2.Image = bitmap_cut;

                }

                click_time--;
            }
            else if(current_mode == mode.Circle){
                if (click_time == 2)
                {
                    pa = e.Location;
                }
                else if (click_time == 1)
                {
                    pb = e.Location;
                    
                    Rectangle rectangle = new Rectangle(pa.X, pa.Y, pb.X - pa.X, pb.Y - pa.Y);
                    Pen pen = new Pen(Color.Red, 5);
                    pen.DashStyle = DashStyle.Dot;
                    //Graphics g = gMyImg;

                    Console.WriteLine("draw :" + rectangle);
                    pictureBox2.Image = CutEllipse(bitmap, rectangle, rectangle.Size);
                    gMyImg.DrawEllipse(pen, rectangle);
                    pictureBox1.Image = bitmap;
                }

                click_time--;
            }

        }

        private Image CutEllipse(Image img, Rectangle rec, Size size)
        {
            Bitmap bitmap = new Bitmap(size.Width, size.Height);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                using (TextureBrush br = new TextureBrush(img, System.Drawing.Drawing2D.WrapMode.Clamp, rec))
                {
                    br.ScaleTransform(bitmap.Width / (float)rec.Width, bitmap.Height / (float)rec.Height);
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    g.FillEllipse(br, new Rectangle(Point.Empty, size));
                }
            }
            return bitmap;
        }

    }
}
