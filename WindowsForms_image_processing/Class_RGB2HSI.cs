using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WindowsForms_image_processing
{
    class Class_RGB2HSI
    {
        public static int HUE = 0, INTENSITY = 1, SATURATION = 2;
        public static double[][] RGB2HSI(Bitmap image)
        {
            int w = image.Width;
            int h = image.Height;
            // main.width = w;
            // main.height = h;

            BitmapData image_data = image.LockBits(
                new Rectangle(0, 0, w, h),
                ImageLockMode.ReadOnly,
                PixelFormat.Format24bppRgb);
            int bytes = image_data.Stride * image_data.Height;
            byte[] buffer = new byte[bytes];
            Marshal.Copy(image_data.Scan0, buffer, 0, bytes);
            image.UnlockBits(image_data);

            double[][] result = new double[w * h][];
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    int pos = x * 3 + y * image_data.Stride;
                    int r_pos = x + y * w;
                    result[r_pos] = new double[3];
                    double b = (double)buffer[pos];
                    double g = (double)buffer[pos + 1];
                    double r = (double)buffer[pos + 2];

                    double[] pixel = { r, g, b };
                    double num = 0.5 * (2 * r - g - b);
                    double den = Math.Sqrt(Math.Pow(r - g, 2) + (r - b) * (g - b));

                    double hue = Math.Acos(num / (den));
                    if (b > g)
                    {
                        hue = 2 * Math.PI - hue;
                    }

                    num = pixel.Min();
                    den = r + g + b;

                    //if (den == 0)
                    //{
                    //    den = 0.00000001;
                    //}

                    double saturation = 1d - 3d * num / den;
                    if (saturation == 0)
                    {
                        hue = 0;
                    }

                    double intensity = (r + g + b) / 3;

                    result[r_pos][0] = hue;
                    result[r_pos][1] = saturation;
                    result[r_pos][2] = intensity;
                }
            }
            //transpose
            double[][] result_t = new double[3][];
            for (int i = 0; i < 3; i++)
            {
                result_t[i] = new double[w * h];
                for (int j = 0; j < w * h; j++)
                {
                    result_t[i][j] = result[j][i];
                }
            }

            return result_t;
        }

        //double[] file
        //,Bitmap ref_Bitmap : reference format
        public static Bitmap getHSI(double[] file,Bitmap ref_Bitmap,int get_Type) {
            PixelFormat pixelFormat = PixelFormat.Format24bppRgb;
            Bitmap bitmap= new Bitmap(ref_Bitmap.Width, ref_Bitmap.Height, pixelFormat);

            BitmapData imageData = bitmap.LockBits(new Rectangle(0, 0, ref_Bitmap.Width, ref_Bitmap.Height), ImageLockMode.ReadWrite, pixelFormat);
            byte[] RGBData = new byte[imageData.Height * imageData.Stride];
            byte[] RowRGBData = new byte[imageData.Width*3];
            for (int i = 0; i < imageData.Height; i++)
            {
                if(get_Type ==HUE)
                    for (int j = 0; j < imageData.Width; j++) {   
                        RowRGBData[3*j+0] = (byte)(file[i * imageData.Width + j] );
                        RowRGBData[3*j+1] = (byte)(file[i * imageData.Width + j] );
                        RowRGBData[3*j+2] = (byte)(file[i * imageData.Width + j] );
                    }
                else if(get_Type==INTENSITY)
                    for (int j = 0; j < imageData.Width; j++)
                    {
                        RowRGBData[3 * j + 0] = (byte)(file[i * imageData.Width + j] * 255);
                        RowRGBData[3 * j + 1] = (byte)(file[i * imageData.Width + j] * 255);
                        RowRGBData[3 * j + 2] = (byte)(file[i * imageData.Width + j] * 255);
                    }
                else if (get_Type == SATURATION)
                    for (int j = 0; j < imageData.Width; j++)
                    {
                        RowRGBData[3 * j + 0] = (byte)(file[i * imageData.Width + j] );
                        RowRGBData[3 * j + 1] = (byte)(file[i * imageData.Width + j] );
                        RowRGBData[3 * j + 2] = (byte)(file[i * imageData.Width + j] );
                    }
                else
                    for (int j = 0; j < imageData.Width; j++)
                    {
                        RowRGBData[3 * j + 0] = (byte)(file[i * imageData.Width + j] * 255);
                        RowRGBData[3 * j + 1] = (byte)(file[i * imageData.Width + j] * 255);
                        RowRGBData[3 * j + 2] = (byte)(file[i * imageData.Width + j] * 255);
                    }
                Array.Copy(RowRGBData, 0, RGBData, i * imageData.Stride, RowRGBData.Length);
            }
            Marshal.Copy(RGBData, 0, imageData.Scan0, RGBData.Length);
            bitmap.UnlockBits(imageData);
            
            return bitmap;
        }

       
    }
}
