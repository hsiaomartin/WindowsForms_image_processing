using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WindowsForms_image_processing
{

    class readHeader{

        byte[] imageHeader = new byte[128];
        
        //read PCX image header
        public readHeader() { }
        
       
        public readHeader(byte[] FileData)
        {
            Array.Copy(FileData, imageHeader, 128);
        }

        public readHeader(String FilePath)
        {
            byte[] imageFile = File.ReadAllBytes(FilePath);
            Array.Copy(imageFile, imageHeader, 128);
        }

        //Manufacturer = 10
        public byte Manufacturer { get { return imageHeader[0]; } }
        //version = 5 (256 colors)
        public byte Version { get { return imageHeader[1]; } }
        //encoder =1 (RLE)
        public byte Encoding { get { return imageHeader[2]; } }
        //BitsPerPixel in a plane
        public byte BitsPerPixel { get { return imageHeader[3]; } }
        //image border (0,0) to (X,Y) , count in pixel real length and width remember +1
        public ushort Xmin { get { return BitConverter.ToUInt16(imageHeader, 4); } }
        public ushort Ymin { get { return BitConverter.ToUInt16(imageHeader, 6); } }
        public ushort Xmax { get { return BitConverter.ToUInt16(imageHeader, 8); } }
        public ushort Ymax { get { return BitConverter.ToUInt16(imageHeader, 10); } }
        //horizontal resolution , ench per pixel
        public ushort Hres { get { return BitConverter.ToUInt16(imageHeader, 12); } }
        //vertical resolution , ench per pixel
        public ushort Vres { get { return BitConverter.ToUInt16(imageHeader, 14); } }
        //default palette
        public byte[] Palette { get { byte[] palette = new byte[48]; Array.Copy(imageHeader, 16, palette, 0, 48); return palette; } }
        //reserve
        public byte Reserve { get { return imageHeader[64]; } }
        //how many planes color
        public byte ColorPlanes { get { return imageHeader[65]; } }
        //how many value in one line 
        public ushort BytesPerLine { get { return BitConverter.ToUInt16(imageHeader, 66); } }
        //PaletteType,1 = color or black&white , 2 = gray scale
        public ushort PaletteType { get { return BitConverter.ToUInt16(imageHeader, 68); } }
        //Filled space
        public byte[] Filled
        {
            get { byte[] filled = new byte[58]; Array.Copy(imageHeader, 70, filled, 0, 58); return filled; }
        }
        public int Width { get { return Xmax - Xmin + 1; } }

        public int Height { get { return Ymax - Ymin + 1; } }
    }

    //read PCX image content
    class readMidResource
    {
        private byte[] my_file;
        public readMidResource(String FilePath)
        {
            if (File.Exists(FilePath)) { DecoPcx(File.ReadAllBytes(FilePath)); my_file = File.ReadAllBytes(FilePath); } else { return; }
        }
        int R = 0, G = 1, B = 2,normal = -1;

        private Bitmap DecoImage;
        public Bitmap getDecoImage { get { return DecoImage; } }
        public int my_Color = -1;
        public readHeader FileHead;
        int readIndex;

        public myPixel[,] myRowData;

        public Color[] palette;

        public Bitmap getDecoImage_single_color(int color)
        {
            my_Color = color;
            DecoPcx(my_file);

            return DecoImage;
        }
        private void DecoPcx(byte[] FileBytes)
        {
            readIndex = 128;
            FileHead = new readHeader(FileBytes);
            if (FileHead.Manufacturer != 10) return;
            PixelFormat pixelFormat = PixelFormat.Format24bppRgb;

            DecoImage = new Bitmap(FileHead.Width, FileHead.Height, pixelFormat);
            myRowData = new myPixel[(FileHead.Width) , (FileHead.Height)];
            if (FileHead.ColorPlanes == 3 && FileHead.BitsPerPixel == 8)
            {
                BitmapData DecoImageData = DecoImage.LockBits(new Rectangle(0, 0, FileHead.Width, FileHead.Height), ImageLockMode.ReadWrite, pixelFormat);
                byte[] RGBData = new byte[DecoImageData.Height * DecoImageData.Stride];
                byte[] RowRGBData = new byte[0];
                
                for (int i = 0; i < DecoImageData.Height; i++)
                {
                    RowRGBData = decode24(FileBytes);
                    Array.Copy(RowRGBData, 0, RGBData, i * DecoImageData.Stride, RowRGBData.Length);
                }
                Marshal.Copy(RGBData, 0, DecoImageData.Scan0, RGBData.Length);
                
                DecoImage.UnlockBits(DecoImageData);
                int counter = 0;
                for (int Ycount = 0; Ycount < FileHead.Height; Ycount++)
                {
                    for (int Xcount = 0; Xcount < FileHead.Width; Xcount++)
                    {
                        myRowData[Xcount, Ycount] = new myPixel(RGBData[counter * 3 + 0], RGBData[counter * 3 + 1], RGBData[counter * 3 + 2]);

                                               
                        counter++;
                    }
                }
            }
            else if (FileHead.ColorPlanes == 1 && FileHead.BitsPerPixel == 8)
            {
                DecoImage = decode8(FileBytes);
            }
        }

        //decode 8bit(256 color) image
        private Bitmap decode8(byte[] FileBytes)
        {
            byte[] AllPixelData = new byte[FileHead.Height * FileHead.Width];
            int EndIndex = FileBytes.Length - 769;
            int HaveWriteTo = 0;
            readTailPalette rtp = new readTailPalette(FileBytes);
            palette = rtp.getPalette();

            if (my_Color == R)
                palette = rtp.getPalette(R);
            else if (my_Color == G)
                palette = rtp.getPalette(G);
            else if (my_Color == B)
                palette = rtp.getPalette(B);

            Bitmap Image24bit = new Bitmap(FileHead.Width, FileHead.Height, PixelFormat.Format24bppRgb);
            int index = 0;

            while (true)
            {
                if (readIndex >= EndIndex) break;
                byte ByteValue = FileBytes[readIndex];
                if (ByteValue > 0xC0)
                {
                    int Count = ByteValue - 0xC0;
                    readIndex++;
                    for (int i = 0; i < Count; i++)
                    {
                        int RGBIndex = i + HaveWriteTo;
                        AllPixelData[RGBIndex] = FileBytes[readIndex];
                    }
                    HaveWriteTo += Count;
                    readIndex++;
                }
                else
                {
                    int RGBIndex = HaveWriteTo;
                    AllPixelData[RGBIndex] = ByteValue;
                    readIndex++;
                    HaveWriteTo++;
                }
            }
            
            for (int j = 0; j < Image24bit.Height; j++)
            {
                for (int i = 0; i < Image24bit.Width; i++)
                {
                    Image24bit.SetPixel(i, j, palette[AllPixelData[index]]);
                    index++;
                }
            }
            int counter = 0;
            for (int Ycount = 0; Ycount < FileHead.Height; Ycount++)
            {
                for (int Xcount = 0; Xcount < FileHead.Width; Xcount++)
                {
                    byte R = (byte)palette[AllPixelData[counter]].R;
                    byte G = (byte)palette[AllPixelData[counter]].G;
                    byte B = (byte)palette[AllPixelData[counter]].B;
                    myRowData[Xcount, Ycount] = new myPixel(R,G,B);


                    counter++;
                }
            }

            return Image24bit;
        }

        //decoder 24bit true color image
        private byte[] decode24(byte[] FileBytes)
        {
            int lineWidth = FileHead.BytesPerLine;
            byte[] RowRGBData = new byte[lineWidth * 3];
            int HaveWriteTo = 0;
            int WriteToRGB = 2;//which color plane is written
            while (true)
            {
                byte ByteValue = FileBytes[readIndex];
                if (ByteValue > 0xC0)
                {
                    int Count = ByteValue - 0xC0;
                    readIndex++;
                    for (int i = 0; i < Count; i++)
                    {
                        if (HaveWriteTo + i >= lineWidth)
                        {
                            i = 0;
                            HaveWriteTo = 0;
                            WriteToRGB--;
                            Count = Count - i;
                            if (WriteToRGB == -1) break;
                        }
                        int RGBIndex = (i + HaveWriteTo) * 3 + WriteToRGB;
                        if (WriteToRGB == 2 && my_Color == R)
                            RowRGBData[RGBIndex] = FileBytes[readIndex];
                        else if (WriteToRGB == 1 && my_Color == G)
                            RowRGBData[RGBIndex] = FileBytes[readIndex];
                        else if (WriteToRGB == 0 && my_Color == B)
                            RowRGBData[RGBIndex] = FileBytes[readIndex];
                        else if (my_Color == normal)
                            RowRGBData[RGBIndex] = FileBytes[readIndex];
                        else
                            RowRGBData[RGBIndex] = 0;
                    }
                    HaveWriteTo += Count;
                    readIndex++;
                }
                else
                {
                    int RGBIndex = HaveWriteTo * 3 + WriteToRGB;
                    if (WriteToRGB == 2 && my_Color == R)
                        RowRGBData[RGBIndex] = ByteValue;
                    else if (WriteToRGB == 1 && my_Color == G)
                        RowRGBData[RGBIndex] = ByteValue;
                    else if (WriteToRGB == 0 && my_Color == B)
                        RowRGBData[RGBIndex] = ByteValue;
                    else if (my_Color == normal)
                        RowRGBData[RGBIndex] = ByteValue;
                    else
                        RowRGBData[RGBIndex] = 0;
                    readIndex++;
                    HaveWriteTo++;
                }
                if (HaveWriteTo >= lineWidth)
                {
                    HaveWriteTo = 0;
                    WriteToRGB--;
                }
                if (WriteToRGB == -1) break;

            }
            return RowRGBData;
        }
    }

    //read PCX tail palette
    class readTailPalette
    {
        byte[] imageTailPalette = new byte[768];

        public readTailPalette() { }

        public readTailPalette(String FilePath)
        {
            byte[] imageFile = File.ReadAllBytes(FilePath);
            Array.Copy(imageFile, imageFile.Length - 768, imageTailPalette, 0, 768);
        }

        public readTailPalette(byte[] FileBytes)
        {
            Array.Copy(FileBytes, FileBytes.Length - 768, imageTailPalette, 0, 768);
        }

        public byte[] TailPaletee { get { return imageTailPalette; } }

        
        //get palette color byte
        public Color[] getPalette()
        {
            int k = 3;
            Color[] palette = new Color[256];
            Color RGB;
            for (int i = 0; i < 256; i++)
            {
                RGB = Color.FromArgb(imageTailPalette[i * k], imageTailPalette[i * k + 1], imageTailPalette[i * k + 2]);
                palette.SetValue(RGB, i);
            }
            return palette;
        }
        //get single palette color byte
        public Color[] getPalette(int single_color)
        {
            int R = 0, G = 1, B = 2;
            int k = 3;
            Color[] palette = new Color[256];
            Color RGB;
            for (int i = 0; i < 256; i++)
            {
                RGB = Color.FromArgb(imageTailPalette[i * k], imageTailPalette[i * k + 1], imageTailPalette[i * k + 2]);
                if (single_color == R)
                    RGB = Color.FromArgb(imageTailPalette[i * k], 0,0);
                else if (single_color == G)
                    RGB = Color.FromArgb(0, imageTailPalette[i * k + 1], 0);
                else if (single_color == B)
                    RGB = Color.FromArgb(0,0, imageTailPalette[i * k + 2]);
                palette.SetValue(RGB, i);
            }
            return palette;
        }
    }
    class Class_PCX_decoder
    {
    }
}
