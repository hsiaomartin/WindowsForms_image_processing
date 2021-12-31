using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsForms_image_processing
{
    class Class_Zoom
    {
        //最鄰近值放大缩小
        public Bitmap zoomInImageNear(Bitmap Image, double times)
        {
            int width = Image.Width;
            int height = Image.Height;
            int IOwidth = (int)Math.Round(width * times);
            int IOheight = (int)Math.Round(height * times);
            Bitmap IOimage = new Bitmap(IOwidth, IOheight, PixelFormat.Format24bppRgb);

            for (int y1 = 0; y1 < IOheight; y1++)
            {
                for (int x1 = 0; x1 < IOwidth; x1++)
                {
                    int x = (int)Math.Round(x1 * (1 / times));
                    int y = (int)Math.Round(y1 * (1 / times));
                    //反向映射坐標，不在原圖內，直接取邊界作為Pixel。
                    if (x > (width - 1))
                    {
                        x = width - 1;
                    }
                    if (y > (height - 1))
                    {
                        y = height - 1;
                    }
                    IOimage.SetPixel(x1, y1, Image.GetPixel(x, y));
                }
            }

            return IOimage;
        }
        //雙線性放大與缩小
        public Bitmap zoomInImageLine(Bitmap Image, double times)
        {
            int width = Image.Width;
            int height = Image.Height;
            int IOwidth = (int)Math.Round(width * times);
            int IOheight = (int)Math.Round(height * times);
            Bitmap IOimage = new Bitmap(IOwidth, IOheight, PixelFormat.Format24bppRgb);

            for (int y1 = 0; y1 < IOheight; y1++)
            {
                for (int x1 = 0; x1 < IOwidth; x1++)
                {
                    double x = x1 * (1 / times);
                    double y = y1 * (1 / times);

                    //反向映射坐標在原圖内
                    if (x <= (width - 1) & y <= (height - 1))
                    {
                        Color RGB = new Color();

                        int a1 = (int)x;
                        int b1 = (int)y;
                        int a2 = (int)Math.Ceiling(x);
                        int b2 = (int)y;
                        int a3 = (int)x;
                        int b3 = (int)Math.Ceiling(y);
                        int a4 = (int)Math.Ceiling(x);
                        int b4 = (int)Math.Ceiling(y);

                        double xa13 = x - a1;
                        double xa24 = a2 - x;
                        double yb12 = y - b1;
                        double yb34 = b3 - y;

                        if (xa13 != 0 & xa24 != 0 & yb12 != 0 & yb34 != 0)
                        {//對應回original image 是非整數座標 -> 雙線性插值
                            byte R1 = Image.GetPixel(a1, b1).R;
                            byte G1 = Image.GetPixel(a1, b1).G;
                            byte B1 = Image.GetPixel(a1, b1).B;
                            byte R2 = Image.GetPixel(a2, b2).R;
                            byte G2 = Image.GetPixel(a2, b2).G;
                            byte B2 = Image.GetPixel(a2, b2).B;
                            byte R3 = Image.GetPixel(a3, b3).R;
                            byte G3 = Image.GetPixel(a3, b3).G;
                            byte B3 = Image.GetPixel(a3, b3).B;
                            byte R4 = Image.GetPixel(a4, b4).R;
                            byte G4 = Image.GetPixel(a4, b4).G;
                            byte B4 = Image.GetPixel(a4, b4).B;

                            byte R = (byte)((R1 * xa24 + R2 * xa13) * yb34 + (R3 * xa24 + R4 * xa13) * yb12);
                            byte G = (byte)((G1 * xa24 + G2 * xa13) * yb34 + (G3 * xa24 + G4 * xa13) * yb12);
                            byte B = (byte)((B1 * xa24 + B2 * xa13) * yb34 + (B3 * xa24 + B4 * xa13) * yb12);

                            RGB = System.Drawing.Color.FromArgb(R, G, B);
                        }
                        else
                        {//對應回original image 是非整數座標 -> 直接取pixel
                            RGB = Image.GetPixel((int)Math.Round(x), (int)Math.Round(y));
                        }
                        IOimage.SetPixel(x1, y1, RGB);
                    }
                    else
                    { //反向映射坐標，不在原圖內，直接取邊界作為新Pixel。
                        int x2 = (int)Math.Round(x);
                        int y2 = (int)Math.Round(y);
                        if (x2 > (width - 1))
                        {
                            x2 = width - 1;
                        }
                        if (y2 > (height - 1))
                        {
                            y2 = height - 1;
                        }
                        IOimage.SetPixel(x1, y1, Image.GetPixel(x2, y2));
                    }
                }
            }
            return IOimage;
        }

    }
}
