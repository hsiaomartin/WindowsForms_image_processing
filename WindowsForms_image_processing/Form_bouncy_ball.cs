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
    public partial class Form_bouncy_ball : Form
    {
        Graphics GP;

        int ball_counter = 1;
        List<Class_bouncy_ball> myBalls;
        Point pA, pB;
        int click_Counter = 0;
        Pen pen_arrow = new Pen(Color.FromArgb(255, 255, 0, 0), 5);

        public Form_bouncy_ball()
        {
            InitializeComponent();
            GP = panel1.CreateGraphics();
            myBalls = new List<Class_bouncy_ball>();
            timer1.Start();
            pen_arrow.StartCap = LineCap.ArrowAnchor;//r
            Text = "Bouncy Ball";
        }
        public void myDrawPoint(Graphics graphics, List<Class_bouncy_ball> balls)
        {
            graphics.Clear(Color.White);
            foreach(Class_bouncy_ball ball in balls)
            {
                ball.moving();
                graphics.FillEllipse(ball.MyColor, ball.X , ball.Y , ball.W, ball.H);
            }
                
            //graphics.FillEllipse(Brushes.Red, X0 + x * scale - 2, Y0 + y * scale - 2, 4, 4);
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            Console.WriteLine("click "+click_Counter);
            if (click_Counter == 0)
            {
                pA = new Point(e.X, e.Y);
                click_Counter++;
            }
            else if(click_Counter == 1)
            {
                Console.WriteLine(pB);
                Class_bouncy_ball newBall = new Class_bouncy_ball(pA.X, pA.Y, panel1.Location.X, panel1.Location.Y, panel1.Width, panel1.Height,pB);
                myBalls.Add(newBall);
                click_Counter = 0;
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            myDrawPoint(GP, myBalls);
        }

        private void Form_bouncy_ball_Load(object sender, EventArgs e)
        {
            
        }

        private void Form_bouncy_ball_Shown(object sender, EventArgs e)
        {
            
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {

            if(click_Counter == 1)
            {
                GP.Clear(Color.White);
                pB = new Point(e.X, e.Y);
                GP.DrawLine(pen_arrow, pB, pA);
            }
        }

        private void Form_bouncy_ball_FormClosed(object sender, FormClosedEventArgs e)
        {
            timer1.Stop();
        }
    }
}
