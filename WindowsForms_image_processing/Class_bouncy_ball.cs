﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsForms_image_processing
{
    public class Class_bouncy_ball
    {
        int x, y;
        double speed = 20, directionX = 1, directionY = 1;
        int w, h;
        int x_min_boundary = 0, y_min_boundary = 0;
        int x_max_boundary = 0, y_max_boundary = 0;

        SolidBrush myColor;
        public Class_bouncy_ball(int x, int y, int x_min_boundary, int y_min_boundary, int x_max_boundary, int y_max_boundary,Point direction)
        {

            this.x_min_boundary = x_min_boundary;
            this.y_min_boundary = y_min_boundary;
            this.x_max_boundary = x_max_boundary;
            this.y_max_boundary = y_max_boundary;
            var rand = new Random();
            int size = rand.Next(20, 50);
            w = h = size;
            this.x = x-w/2;
            this.y = y-h/2;
            //speed = rand.Next(10, 30);
            speed = 1;
            //directionX = (rand.Next(0, 100) > 50 ? 1 : -1);
            //directionY = (rand.Next(0, 100) > 50 ? 1 : -1);
            int dx = direction.X - x;
            int dy = direction.Y - y;

            directionX = ((dx != 0) ? (dx ) : 0);
            directionY = ((dy != 0) ? (dy ) : 0);
            myColor = new SolidBrush(Color.FromArgb(rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255)));

            Console.WriteLine(x+" "+y+" "+ direction + " "+" , "+ directionX + " " + directionY);
        }
        public void moving()
        {
            if (x > x_max_boundary - W)
                directionX *= -1;
            else if (y > y_max_boundary - H)
                directionY *= -1;
            else if (x < 0)
                directionX *= -1;
            else if (y < 0)
                directionY *= -1;
            x = (int)(x + speed * directionX);
            y = (int)(y + speed * directionY);
            
        }

        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
        public int W { get => w; set => w = value; }
        public int H { get => h; set => h = value; }
        public SolidBrush MyColor { get => myColor; set => myColor = value; }
    }
}
