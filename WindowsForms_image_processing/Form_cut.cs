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
        public bool isMagicWand = false;
        public mySize size;
        public Bitmap bitmap,bitmap_cut;
        public enum mode { Square,Circle,Magic_Wand}
        public mode current_mode;
        int click_time;
        Point pa, pb;
        Graphics gMyImg, gMyImg_cut;
        myPicture original_Image,cut_Image;
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
                    int R = original_Image.my_Pixel[Xcount, Ycount].R;
                    int G = original_Image.my_Pixel[Xcount, Ycount].G;
                    int B = original_Image.my_Pixel[Xcount, Ycount].B;

                    gMyImg.FillRectangle(new SolidBrush(Color.FromArgb(R, G, B)), Xcount, Ycount, 1, 1);
                    count++;
                }
            }


        }

        public void drawMyImgData(Graphics g,mode m,Rectangle rectangle)
        {
            //myPicture my_Picture;
            int w = rectangle.Width;
            int h = rectangle.Height;
            cut_Image = new myPicture(w, h);
            //e.Graphics.Clear(Color.Black);
            g.Clear(Color.Black);
            //my_Picture = new myPicture(w,h );

            for (int Ycount = 0; Ycount < h; Ycount++)
            {
                for (int Xcount = 0; Xcount < w; Xcount++)
                {
                    int R = original_Image.my_Pixel[Xcount+ rectangle.X, Ycount+ rectangle.Y].R;
                    int G = original_Image.my_Pixel[Xcount+ rectangle.X, Ycount+ rectangle.Y].G;
                    int B = original_Image.my_Pixel[Xcount+ rectangle.X, Ycount+ rectangle.Y].B;
                    cut_Image.my_Pixel[Xcount, Ycount] = new myPixel(R, G, B);
                    if(m==mode.Circle)
                        g.FillRectangle(new SolidBrush(Color.FromArgb(R, G, B)), Xcount, Ycount, 1, 1);
                    else if (m == mode.Square)
                        g.FillRectangle(new SolidBrush(Color.FromArgb(R, G, B)), Xcount, Ycount, 1, 1);
                }
            }
        }

        public void drawCutData(myPicture cut,int x,int y)
        {

            for (int Ycount = y; Ycount < y+cut.Height; Ycount++)
            {
                for (int Xcount = x; Xcount < x+cut.Width; Xcount++)
                {
                    if(Xcount<size.Width && Ycount < size.Height)
                    {
                        int R = cut.my_Pixel[Xcount-x , Ycount-y ].R;
                        int G = cut.my_Pixel[Xcount-x , Ycount-y ].G;
                        int B = cut.my_Pixel[Xcount-x , Ycount-y ].B;
                        original_Image.my_Pixel[Xcount, Ycount] = new myPixel(R, G, B);
                        gMyImg.FillRectangle(new SolidBrush(Color.FromArgb(R, G, B)), Xcount, Ycount, 1, 1);

                    }

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

        private void button4_Click(object sender, EventArgs e)
        {
            drawMyImgData();
            current_mode = mode.Magic_Wand;
        }

        private void Form_cut_VisibleChanged(object sender, EventArgs e)
        {
            if (isMagicWand)
            {
                Text = "Magic Wand";
                button2.Visible = false;
            }
            else
            {
                Text = "Cut Image";
                button4.Visible = false;
            }
            original_Image = new myPicture(size.Width, size.Height);
            for (int Ycount = 0; Ycount < size.Height; Ycount++)
            {
                for (int Xcount = 0; Xcount < size.Width; Xcount++)
                {
                    int R = read_photo[Xcount, Ycount].R;
                    int G = read_photo[Xcount, Ycount].G;
                    int B = read_photo[Xcount, Ycount].B;
                    original_Image.my_Pixel[Xcount, Ycount] = new myPixel(R, G, B);
                }
            }
            bitmap = new Bitmap(size.Width,size.Height);
            gMyImg = Graphics.FromImage(bitmap);
            button4.Enabled = false;
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
                    Point LeftTop = new Point(0,0);
                    if (pb.X > pa.X)
                        LeftTop.X = pa.X;
                    else
                        LeftTop.X = pb.X;

                    if (pb.Y > pa.Y)
                        LeftTop.Y = pa.Y;
                    else
                        LeftTop.Y = pb.Y;
                    Rectangle rectangle = new Rectangle(LeftTop.X, LeftTop.Y, Math.Abs(pb.X - pa.X), Math.Abs(pb.Y - pa.Y));
                    Pen pen = new Pen(Color.Red, 5);
                    pen.DashStyle = DashStyle.Dot;
                    //Graphics g = gMyImg;
                    gMyImg.DrawRectangle(pen,rectangle);
                    pictureBox1.Image = bitmap;
                    Console.WriteLine("draw :"+ rectangle);

                    //cut
                    bitmap_cut = new Bitmap(rectangle.Width, rectangle.Height);
                    gMyImg_cut = Graphics.FromImage(bitmap_cut);
                    drawMyImgData(gMyImg_cut,mode.Square, rectangle);
                    pictureBox2.Image = bitmap_cut;
                    button4.Enabled = true;
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
                    Point LeftTop = new Point(0, 0);
                    if (pb.X > pa.X)
                        LeftTop.X = pa.X;
                    else
                        LeftTop.X = pb.X;

                    if (pb.Y > pa.Y)
                        LeftTop.Y = pa.Y;
                    else
                        LeftTop.Y = pb.Y;
                    Rectangle rectangle = new Rectangle(LeftTop.X, LeftTop.Y, Math.Abs(pb.X - pa.X), Math.Abs(pb.Y - pa.Y));
                    Pen pen = new Pen(Color.Red, 5);
                    pen.DashStyle = DashStyle.Dot;
                    //Graphics g = gMyImg;

                    Console.WriteLine("draw :" + rectangle);

                    
                    //cut
                    bitmap_cut = new Bitmap(rectangle.Width, rectangle.Height);
                    gMyImg_cut = Graphics.FromImage(bitmap_cut);
                    drawMyImgData(gMyImg_cut,mode.Circle,rectangle);
                    //pictureBox2.Image = bitmap_cut;
                    pictureBox2.Image = CutEllipse(bitmap, rectangle, new Size(rectangle.Width,rectangle.Height));
                    button4.Enabled = true;

                    gMyImg.DrawEllipse(pen, rectangle);
                    pictureBox1.Image = bitmap;
                }

                click_time--;
            }
            else if(current_mode == mode.Magic_Wand)
            {
                drawCutData(cut_Image, e.Location.X, e.Location.Y);
                pictureBox1.Image = bitmap;
            }

        }

        private Image CutEllipse(Image img, Rectangle rectangle, Size size)
        {
            Bitmap bitmap = new Bitmap(size.Width, size.Height);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                using (TextureBrush br = new TextureBrush(img, System.Drawing.Drawing2D.WrapMode.Clamp, rectangle))
                {
                    br.ScaleTransform(bitmap.Width / (float)rectangle.Width, bitmap.Height / (float)rectangle.Height);
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    g.FillEllipse(br, new Rectangle(Point.Empty, size));
                }
            }

            return bitmap;
        }

    }
}
