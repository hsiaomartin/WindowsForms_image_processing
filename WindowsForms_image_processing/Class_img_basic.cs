using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsForms_image_processing
{
    static class Class_img_basic
    {
        /*
         * https://resources.pcb.cadence.com/blog/2020-what-is-signal-to-noise-ratio-and-how-to-calculate-it
         * 5 dB to 10 dB: is below the minimum level to establish a connection, due to the noise level being nearly indistinguishable from the desired signal (useful information).
          10 dB to 15 dB: is the accepted minimum to establish an unreliable connection.
          15 dB to 25 dB: is typically considered the minimally acceptable level to establish poor connectivity.
          25 dB to 40 dB: is deemed to be good.
          41 dB or higher: is considered to be excellent.
         */

        //can't work
        static public String mySNR(myPixel[,] original_Img, myPixel[,] noise_Img, mySize size)
        {
            // compute sum of signal
            double sumU = 0.0;
            for (int y = 0; y != size.Height; y++)
            {
                for (int x = 0; x != size.Width; x++)
                {
                    sumU += (original_Img[x, y].pixel_Value);
                }
            }

            // compute sum of noise
            double sumN = 0.0;
            for (int y = 0; y != size.Height; y++)
            {
                for (int x = 0; x != size.Width; x++)
                {
                    sumN += (noise_Img[x, y].pixel_Value - original_Img[x, y].pixel_Value);
                }
            }
            Console.WriteLine(sumU);
            Console.WriteLine(sumN);
            if (Math.Sqrt(sumN) != 0)
                return Math.Round(10 * Math.Log10(Math.Sqrt(sumU) / Math.Sqrt(sumN)), 5) + " dB";
            else
                return "0 dB";
        }
        static public String SNR(myPixel[,] original_Img, myPixel[,] noise_Img, mySize size)
        {
            int n = size.Height * size.Width;
            double sumU = 0.0;
            // compute mean of signal
            for (int y= 0; y != size.Height; y++)
            {
                for(int x = 0; x != size.Width; x++)
                {
                    sumU += original_Img[x, y].pixel_Value;
                }
            }
            double u = sumU / n;
            double diffVS = 0.0, sumVS = 0.0;
            Console.WriteLine(u);
            Console.WriteLine(sumU);
            // compute variance of signal
            for (int y = 0; y != size.Height; y++)
            {
                for (int x = 0; x != size.Width; x++)
                {
                    diffVS = original_Img[x, y].pixel_Value - u;
                    sumVS += diffVS * diffVS;
                }
            }
            
            double VS = (double)sumVS / n;
            // compute mean of noise
            double sumN = 0.0;
            for (int y = 0; y != size.Height; y++)
            {
                for (int x = 0; x != size.Width; x++)
                {
                    sumN += noise_Img[x, y].pixel_Value - original_Img[x, y].pixel_Value;
                }
            }
            double un = (double)sumN / n;
            // compute variance of noise
            double diffVN = 0.0, sumVN = 0.0;
            for (int y = 0; y != size.Height; y++)
            {
                for (int x = 0; x != size.Width; x++)
                {
                    diffVN = (noise_Img[x, y].pixel_Value - original_Img[x, y].pixel_Value)-un;
                    sumVN += diffVN * diffVN;
                }
            }

            double VN = (double)sumVN / n;
            Console.WriteLine(diffVN);
            Console.WriteLine(sumVN);
            if (Math.Sqrt(VN) != 0)
                return Math.Round(20 * Math.Log10(Math.Sqrt(VS) / Math.Sqrt(VN)),5)+" dB";
            else
                return "0 dB";
        }

        static public String PSNR(myPixel[,] original_Img, myPixel[,] noise_Img, mySize size)
        {
            int n = size.Height * size.Width;
            double sumU = 0.0;
            // compute mean of signal
            for (int y = 0; y != size.Height; y++)
            {
                for (int x = 0; x != size.Width; x++)
                {
                    sumU += original_Img[x, y].pixel_Value;
                }
            }
            double u = sumU / n;
            double diffVS = 0.0, sumVS = 0.0;
            Console.WriteLine(u);
            Console.WriteLine(sumU);
            // compute variance of signal
            for (int y = 0; y != size.Height; y++)
            {
                for (int x = 0; x != size.Width; x++)
                {
                    diffVS = original_Img[x, y].pixel_Value - u;
                    sumVS += diffVS * diffVS;
                }
            }

            double VS = (double)sumVS / n;
            // compute mean of noise
            double sumN = 0.0;
            for (int y = 0; y != size.Height; y++)
            {
                for (int x = 0; x != size.Width; x++)
                {
                    sumN += noise_Img[x, y].pixel_Value - original_Img[x, y].pixel_Value;
                }
            }
            double un = (double)sumN / n;
            // compute variance of noise
            double diffVN = 0.0, sumVN = 0.0;
            for (int y = 0; y != size.Height; y++)
            {
                for (int x = 0; x != size.Width; x++)
                {
                    diffVN = (noise_Img[x, y].pixel_Value - original_Img[x, y].pixel_Value) - un;
                    sumVN += diffVN * diffVN;
                }
            }

            double VN = (double)sumVN / n;
            Console.WriteLine(diffVN);
            Console.WriteLine(sumVN);
            if (Math.Sqrt(VN) != 0)
                return Math.Round(10 * Math.Log10((255) / (VN)), 5) + " dB";
            else
                return "0 dB";
        }

    }
    public class myPixel
    {
        public int R;
        public int G;
        public int B;
        public int Gray;
        public double hue;
        public double saturation;
        public double intensity;
        public SolidBrush myColor;
        public Color color;
        public int pixel_Value { get => Gray; set => pixel_Value= value; }
        public myPixel()
        {
            R = G = B = 0;
        }

        public myPixel(byte r, byte g, byte b)
        {
            R =r;
            G =g;
            B =b;
            Gray = (R+G+B)/3;
            myColor = new SolidBrush(Color.FromArgb(R, G, B));
            color = Color.FromArgb(R, G, B);

            //hsi
            double[] pixel = { R, G, B };
            double num = 0.5 * (2 * R - G - B);
            double den = Math.Sqrt(Math.Pow(R - G, 2) + (R - B) * (G - B));
            hue = Math.Acos(num / (den));
            if (B > G)
            {
                hue = 2 * Math.PI - hue;
            }
            num = pixel.Min();
            den = R + G + B;
            saturation = 1d - 3d * num / den;
            if (saturation == 0)
            {
                hue = 0;
            }
            hue = Math.Round(hue, 5);
            saturation = Math.Round(saturation, 5);
            intensity = Gray;
        }
        public myPixel(int r, int g, int b)
        {
            R = r;
            G = g;
            B = b;
            Gray = (R + G + B) / 3;
            myColor = new SolidBrush(Color.FromArgb(R, G, B));
            color = Color.FromArgb(R, G, B);

            //hsi
            double[] pixel = { R, G, B };
            double num = 0.5 * (2 * R - G - B);
            double den = Math.Sqrt(Math.Pow(R - G, 2) + (R - B) * (G - B));
            hue = Math.Acos(num / (den));
            if (B > G)
            {
                hue = 2 * Math.PI - hue;
            }
            num = pixel.Min();
            den = R + G + B;
            saturation = 1d - 3d * num / den;
            if (saturation == 0)
            {
                hue = 0;
            }
            hue = Math.Round(hue, 5);
            saturation = Math.Round(saturation, 5);
            intensity = Gray;
        }

    }

    public class myPoint
    {
        

        public myPoint(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get => X; set => X = value; }
        public int Y { get => Y; set => Y = value; }
    }

    public class mySize
    {
        int width, height;

        public mySize(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public mySize()
        {
        }

        public int Width { get => width; set => width = value; }
        public int Height { get => height; set => height = value; }
    }

    public class myPicture
    {
        public myPixel[,] my_Pixel;
        int width, height;
        public Size picture_size;
        public myPicture(int width,int height)
        {
            my_Pixel = new myPixel[width , height];
            this.width = width;
            this.height = height;
            picture_size = new Size(width, height);
        }
        public myPicture(Size size)
        {
            my_Pixel = new myPixel[size.Width, size.Height];
            this.width = size.Width;
            this.height = size.Height;
            picture_size = size;
        }

        public void draw(Graphics g)
        {
            for (int Ycount = 0; Ycount < height; Ycount++)
            {
                for (int Xcount = 0; Xcount < width; Xcount++)
                {
                    int R = my_Pixel[Xcount, Ycount].R;
                    int G = my_Pixel[Xcount, Ycount].G;
                    int B = my_Pixel[Xcount, Ycount].B;

                    g.FillRectangle(new SolidBrush(Color.FromArgb(R, G, B)), Xcount, Ycount, 1, 1);
                }
            }
        }
        public void draw(Graphics g,string gray)
        {
            for (int Ycount = 0; Ycount < height; Ycount++)
            {
                for (int Xcount = 0; Xcount < width; Xcount++)
                {
                    int Gray = my_Pixel[Xcount, Ycount].Gray;
                    g.FillRectangle(new SolidBrush(Color.FromArgb(Gray, Gray, Gray)), Xcount, Ycount, 1, 1);
                }
            }
        }
        public int Width { get => width; set => width = value; }
        public int Height { get => height; set => height = value; }
    }

    public class myHistogram
    {
        int[] r;
        int[] g;
        int[] b;

        int[] cdf_R;
        int[] cdf_G;
        int[] cdf_B;
        public int[] r_R;
        public int[] r_G;
        public int[] r_B;
        public myHistogram(int bit_depth,int size)
        {
            r = new int[bit_depth];
            g = new int[bit_depth];
            b = new int[bit_depth];

            cdf_R = new int[bit_depth];
            cdf_G = new int[bit_depth];
            cdf_B = new int[bit_depth];

            r_R = new int[size+1];
            r_G = new int[size+1];
            r_B = new int[size+1];

            Array.Clear(r, 0, r.Length);
            Array.Clear(g, 0, g.Length);
            Array.Clear(b, 0, b.Length);

            Array.Clear(cdf_R, 0, cdf_R.Length);
            Array.Clear(cdf_G, 0, cdf_G.Length);
            Array.Clear(cdf_B, 0, cdf_B.Length);

            Array.Clear(r_R, 0, r_R.Length);
            Array.Clear(r_G, 0, r_G.Length);
            Array.Clear(r_B, 0, r_B.Length);
        }

        public int[] R { get => r; set => r = value; }
        public int[] G { get => g; set => g = value; }
        public int[] B { get => b; set => b = value; }
        public int[] Cdf_R { get => cdf_R; set => cdf_R = value; }
        public int[] Cdf_G { get => cdf_G; set => cdf_G = value; }
        public int[] Cdf_B { get => cdf_B; set => cdf_B = value; }
    }

}
