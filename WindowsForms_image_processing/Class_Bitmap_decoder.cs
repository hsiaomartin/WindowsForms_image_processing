using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsForms_image_processing
{
    public class read_Bitmap
    {
        //public RawImage outputImg;
        private byte[] my_file;
        public myPixel[,] myRowData;
        public tagBITMAPFILEHEADER file_header;
        public tagBMP_INFOHEADER info_header;

        byte[] file = null;
        public read_Bitmap(String FilePath)
        {
            if (File.Exists(FilePath)) { Start(File.ReadAllBytes(FilePath)); my_file = File.ReadAllBytes(FilePath); } else { return; }
        }

        void Start(byte[] FileBytes)
        {
            //file = File.ReadAllBytes("C:/A/0.bmp");
            file = FileBytes;

            file_header = new tagBITMAPFILEHEADER();
            file_header.bfType = System.Text.Encoding.Default.GetString(file, 0, 2);
            file_header.bfSize = _4ByteToInt(0x0002);
            file_header.bfReserved1 = _2ByteToInt(0x0006);
            file_header.bfReserved2 = _2ByteToInt(0x0008);
            file_header.bfOffBits = _4ByteToInt(0x000A);

            info_header = new tagBMP_INFOHEADER();
            info_header.biSize = _4ByteToInt(0x000E);
            info_header.biWidth = _4ByteToInt(0x0012);
            info_header.biHeight = _4ByteToInt(0x0016);
            info_header.biPlanes = _2ByteToInt(0x001A);
            info_header.biBitCount = _2ByteToInt(0x001C);
            info_header.biCompression = _4ByteToInt(0x001E);
            info_header.biSizeImage = _4ByteToInt(0x0022);
            info_header.biXPelsPerMeter = _4ByteToInt(0x0026);
            info_header.biYPelsPerMeter = _4ByteToInt(0x002A);
            info_header.biClrUsed = _4ByteToInt(0x002E);
            info_header.biClrImportant = _4ByteToInt(0x0032);

            /*
            print("--------------------------------------------- File Header");

            print("Type : " + file_header.bfType);
            print("Size : " + file_header.bfSize);
            print("Reserved1 : " + file_header.bfReserved1);
            print("Reserved2 : " + file_header.bfReserved2);
            print("OffBits : " + file_header.bfOffBits);

            print("--------------------------------------------- Info Header");

            print("BitmapHeaderSize : " + info_header.biSize);
            print("Width : " + info_header.biWidth);
            print("Height : " + info_header.biHeight);
            print("Planes : " + info_header.biPlanes);
            print("BitCount : " + info_header.biBitCount);
            print("Compression : " + info_header.biCompression);
            print("SizeImage : " + info_header.biSizeImage);
            print("XPelsPerMeter : " + info_header.biXPelsPerMeter);
            print("YPelsPerMeter : " + info_header.biYPelsPerMeter);
            print("ClrUsed : " + info_header.biClrUsed);
            print("ClrImportant : " + info_header.biClrImportant);
            // --------------------------------------------------------
            */
            /*
            if (info_header.biBitCount != 24)
            {
                //Debug.LogError("本範例只能讀取 24 bit 寬度的圖像。");
                System.Windows.Forms.MessageBox.Show("Didn't implement "+ info_header.biBitCount+" format.");
                return;
            }

            */
            int step = 3;
            if (info_header.biBitCount == 32)
            {
                step = 4;
            }

            //Texture2D t = new Texture2D(info_header.biWidth, info_header.biHeight);
            myRowData = new myPixel[(info_header.biWidth) , (info_header.biHeight)];
            // skip : 微軟規定圖片寬度 / 4 必須 等於 0
            // 如果寬度無法整除 4 ，那麼必須要補 0

            int skip = 0;

            if (info_header.biWidth * 3 % 4 != 0)
            {
                skip = 4 - (info_header.biWidth * 3 % 4);
            }

            int i = 0,count = 0;
            for (int y =  info_header.biHeight-1 ; y >= 0; y--)
            {
                for (int x = 0; x < info_header.biWidth; x++)
                {
                    int k = file_header.bfOffBits + i;
                    myRowData[x, y] = new myPixel(file[k + 2],file[k + 1],file[k + 0]);
                    
                    count++;
                    //t.SetPixel(x, y, Color.FromArgb(1, (int)(file[k + 2] / 255.0f), (int)(file[k + 1] / 255.0f), (int)(file[k] / 255.0f)));
                    
                    i += step;
                }
                i += skip;
            }

            //t.Apply();

            //outputImg.rectTransform.sizeDelta = new Vector2(t.width, t.height);
            //outputImg.texture = t;
        }

        int _2ByteToInt(int offset)
        {
            int value1 = file[offset + 1] << 8;
            int value2 = file[offset];
            return value1 | value2;
        }

        int _4ByteToInt(int offset)
        {
            int value1 = file[offset + 3] << 24;
            int value2 = file[offset + 2] << 16;
            int value3 = file[offset + 1] << 8;
            int value4 = file[offset];

            return value1 | value2 | value3 | value4;
        }

        public struct tagBITMAPFILEHEADER
        {
            public string bfType;    //2Bytes，必须为"BM"，即0x424D 才是Windows位图文件
            public int bfSize;       //4Bytes，整个BMP文件的大小
            public int bfReserved1;  //2Bytes，保留，为0
            public int bfReserved2;  //2Bytes，保留，为0
            public int bfOffBits;    //4Bytes，文件起始位置到图像像素数据的字节偏移量
        }

        public struct tagBMP_INFOHEADER
        {
            public int biSize;   //4Bytes，INFOHEADER结构体大小，存在其他版本I NFOHEADER，用作区分
            public int biWidth;  //4Bytes，图像宽度（以像素为单位）
            public int biHeight; //4Bytes，图像高度，+：图像存储顺序为Bottom2Top，-：Top2Bottom
            public int biPlanes;        //2Bytes，图像数据平面，BMP存储RGB数据，因此总为1
            public int biBitCount;      //2Bytes，图像像素位数
            public int biCompression;   //4Bytes，0：不压缩，1：RLE8，2：RLE4
            public int biSizeImage;     //4Bytes，4字节对齐的图像数据大小
            public int biXPelsPerMeter; //4 Bytes，用象素/米表示的水平分辨率
            public int biYPelsPerMeter; //4 Bytes，用象素/米表示的垂直分辨率
            public int biClrUsed;       //4 Bytes，实际使用的调色板索引数，0：使用所有的调色板索引
            public int biClrImportant;  //4 Bytes，重要的调色板索引数，0：所有的调色板索引都重要
        }

        // 1，4，8位图像才会使用调色板数据，16,24,32位图像不需要调色板数据，即调色板最多只需要256项（索引0 - 255）。
        // 本範例將不會用到此結構體。
        public struct tagRGBQUAD
        {
            public byte rgbBlue;       //指定蓝色强度
            public byte rgbGreen;      //指定绿色强度
            public byte rgbRed;        //指定红色强度
            public byte rgbReserved;  //保留，设置为0
        }
    }
    //https://blog.csdn.net/weixin_38884324/article/details/80609858
}
