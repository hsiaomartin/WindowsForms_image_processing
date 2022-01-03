using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/*
 string filename = @"C:\test.txt";

if (File.Exists(filename)) {
    MessageBox.Show("取得檔名(不包含附檔名)" + Path.GetFileNameWithoutExtension(filename));
    MessageBox.Show("取得副檔名:" + Path.GetExtension(filename));
    MessageBox.Show("資料根目錄:" + Path.GetPathRoot(filename));
    MessageBox.Show("取得路徑:" + Path.GetFullPath(filename));
}
 */

namespace WindowsForms_image_processing
{

    public partial class Form2_readImg : Form
    {
        public enum img_format { bmp,pcx }
        img_format current_format;
        //-pcx
        readMidResource read_photo;
        Bitmap bitmap;

        //-my bmp
        read_Bitmap read_bitmap;
        //private string filePath = string.Empty;
        public string filePath { get;set; }
        public myPicture my_Picture;
        public myPixel[,] myImgData;
        public mySize sz;
        Graphics gMyImg;
        public void drawMyImgData()
        {
            //myPicture my_Picture;
            int w = read_bitmap.info_header.biWidth;
            int h = read_bitmap.info_header.biHeight;
            //e.Graphics.Clear(Color.Black);
            gMyImg.Clear(Color.Black);
            //my_Picture = new myPicture(w,h );

            for (int Ycount = h - 1; Ycount >= 0; Ycount--)
            {
                for (int Xcount = 0; Xcount < w; Xcount++)
                {
                    int R = read_bitmap.myRowData[Xcount, Ycount].R;
                    int G = read_bitmap.myRowData[Xcount, Ycount].G;
                    int B = read_bitmap.myRowData[Xcount, Ycount].B;

                    //my_Picture.my_Pixel[count] = new myPixel(read_bitmap.myRowData[count].R, read_bitmap.myRowData[count].G, read_bitmap.myRowData[count].B);
                    //my_Picture.location[count] = new Point(Xcount, h-Ycount-1);
                    //Console.WriteLine("(" + my_Picture.location[count].X + "," + my_Picture.location[count].Y + ")");
                    gMyImg.FillRectangle(new SolidBrush(Color.FromArgb(R, G, B)), Xcount, Ycount, 1, 1);

                }
            }            
        }

        public Form2_readImg()
        {
            InitializeComponent();
            toolStripStatusLabel2.ForeColor = Color.Red;
            toolStripStatusLabel3.ForeColor = Color.Green;
            toolStripStatusLabel4.ForeColor = Color.Blue;
        }

        private void button_filefilepath_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            //var filePath = string.Empty;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                //openFileDialog.InitialDirectory = filePath;
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;

                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                    }
                }
            }
            this.textBox_filepath.Text = filePath;
            readMidResource read_photo = new readMidResource(filePath);
            bitmap = read_photo.getDecoImage;
            pictureBox1.Image = bitmap;

            //MessageBox.Show(fileContent, "File Content at path: " + filePath, MessageBoxButtons.OK);
        }



        private void Form2_PCX_VisibleChanged(object sender, EventArgs e)
        {
            this.textBox_filepath.Text = filePath;
            read_photo = null;
            read_bitmap = null;
            pictureBox2.Image = null;

            if(Path.GetExtension(filePath) == ".bmp")
            {                
                current_format = img_format.bmp;
                read_bitmap = new read_Bitmap(filePath);
                sz = new mySize(read_bitmap.info_header.biWidth, read_bitmap.info_header.biHeight);
                bitmap = new Bitmap(read_bitmap.info_header.biWidth, read_bitmap.info_header.biHeight);
                gMyImg = Graphics.FromImage(bitmap);
                myImgData = read_bitmap.myRowData;
                drawMyImgData();
            }
            else if(Path.GetExtension(filePath) == ".pcx")
            {
                current_format = img_format.pcx;
                read_photo = new readMidResource(filePath);
                bitmap = read_photo.getDecoImage;
                myImgData = read_photo.myRowData;
                sz = new mySize(read_photo.FileHead.Width, read_photo.FileHead.Height);
            }
            else
            {
                MessageBox.Show(Path.GetExtension(filePath)+" file didn't implement.");
                return;
            }

            if (myImgData != null)
            {
                my_Picture = new myPicture(sz.Width, sz.Height);
                my_Picture.my_Pixel = myImgData;
            }

            
            if (bitmap != null)
            {
                set_pictureBox_size(pictureBox1, bitmap);

                pictureBox1.Image = bitmap;

                //setup pcx infomation
                setDataGridView();

                //draw palette if exists
                pictureBox2.Image = null;
                if(read_photo!=null)
                    if (read_photo.palette != null)
                    {
                        Bitmap pcx_Palette = new Bitmap(128,128);
                        Color[] palette = read_photo.palette;
                        // Set each pixel in myBitmap to black.
                        Graphics g = Graphics.FromImage(pcx_Palette);
                        //palette size 16*16
                        for (int Xcount = 0; Xcount < 16; Xcount++)
                        {
                            for (int Ycount = 0; Ycount < 16; Ycount++)
                            {
                                //let block is 8*8
                                Bitmap block = new Bitmap(8, 8);
                                Graphics g_block = Graphics.FromImage(block);
                                g_block.Clear(palette[Ycount + Xcount * 16]);
                                g.DrawImage(block, new Point(Xcount*8, Ycount*8));
                          
                            }
                        }
                        pictureBox2.Image = pcx_Palette;
                    }

            }
            
            

        }

        private void set_pictureBox_size(PictureBox pictureBox,Bitmap bitmap)
        {
            pictureBox.Width = bitmap.Width;
            pictureBox.Height = bitmap.Height;
        }

        public void setDataGridView()
        {
            DataGridViewTextBoxColumn dataGridViewCol1 = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn dataGridViewCol2 = new DataGridViewTextBoxColumn();

            dataGridViewCol1.Name = "dataGridViewCol1";
            dataGridViewCol1.HeaderText = "Item";
            
            dataGridViewCol2.Name = "dataGridViewCol2";
            dataGridViewCol2.HeaderText = "Value";

            if(dataGridView1.Columns.Count<2)
                dataGridView1.Columns.AddRange(new DataGridViewColumn[] { dataGridViewCol1, dataGridViewCol2 });

            //是否允許使用者編輯
            dataGridView1.ReadOnly = true;


            //add info
            Dictionary<string, string> dictionaryCount = new Dictionary<string, string> {{ "initial" , "0" } ,}; 
            if (Path.GetExtension(filePath) == ".bmp")
            {
                dictionaryCount = new Dictionary<string, string> {
                    { "Type"        , read_bitmap.file_header.bfType } ,
                    { "Size "       , read_bitmap.file_header.bfSize.ToString() } ,
                    { "Reserved1"   , read_bitmap.file_header.bfReserved1.ToString() } ,
                    { "Reserved2"   , read_bitmap.file_header.bfReserved2.ToString() } ,
                    { "OffBits"     , read_bitmap.file_header.bfOffBits.ToString() } ,
                    { "BitmapHeaderSize" , read_bitmap.info_header.biSize.ToString() } ,
                    { "Width"            , read_bitmap.info_header.biWidth.ToString() } ,
                    { "Height"           , read_bitmap.info_header.biHeight.ToString() } ,
                    { "Planes"           , read_bitmap.info_header.biPlanes.ToString() } ,
                    { "BitCount"         , read_bitmap.info_header.biBitCount.ToString() } ,
                    { "Compression"      , read_bitmap.info_header.biCompression.ToString() } ,
                    { "SizeImage"        , read_bitmap.info_header.biSizeImage.ToString() } ,
                    { "XPelsPerMeter"    , read_bitmap.info_header.biXPelsPerMeter.ToString() } ,
                    { "YPelsPerMeter"    , read_bitmap.info_header.biYPelsPerMeter.ToString() } ,
                    { "ClrUsed"          , read_bitmap.info_header.biClrUsed.ToString() } ,
                    { "ClrImportant"     , read_bitmap.info_header.biClrImportant.ToString() } ,
                };
            }
            else if (Path.GetExtension(filePath) == ".pcx")
            {
                
                dictionaryCount = new Dictionary<string, string> {
                    { "Manufacturer", read_photo.FileHead.Manufacturer.ToString() } ,
                    { "Version", read_photo.FileHead.Version.ToString() } ,
                    { "Encoding", read_photo.FileHead.Encoding.ToString() },
                    { "BitsPerPixel", read_photo.FileHead.BitsPerPixel.ToString() },
                    { "Xmin", read_photo.FileHead.Xmin.ToString() },
                    { "Ymin", read_photo.FileHead.Ymin.ToString() },
                    { "Xmax", read_photo.FileHead.Xmax.ToString() },
                    { "Ymax", read_photo.FileHead.Ymax.ToString() },
                    { "Hres", read_photo.FileHead.Hres.ToString() },
                    { "Vres", read_photo.FileHead.Vres.ToString() },
                    //{ "Palette", BitConverter.ToInt32(read_photo.FileHead.Palette,0) },
                    { "Reserve", read_photo.FileHead.Reserve.ToString() },
                    { "ColorPlanes", read_photo.FileHead.ColorPlanes.ToString() },
                    { "BytesPerLine", read_photo.FileHead.BytesPerLine.ToString() },
                    { "PaletteType", read_photo.FileHead.PaletteType.ToString() },
                   // { "Filled", Encoding.Default.GetString ( read_photo.FileHead.Filled )},
                };
                
            }
            


            foreach (var item in dictionaryCount.Select((value, index) => new { name = value, classIdx = index }))
            {
                if(dataGridView1.Rows.Count < dictionaryCount.Count )
                    dataGridView1.Rows.Add();
                dataGridView1.Rows[item.classIdx].Cells[0].Value = item.name.Key;
                dataGridView1.Rows[item.classIdx].Cells[1].Value = item.name.Value;
            }
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
        }

        private void rotateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_rotate form_Rotate = new Form_rotate();
            form_Rotate.bitmap = bitmap;
            form_Rotate.Show();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {

            
            toolStripStatusLabel1.Text = "( "+e.X +" , "+ e.Y+" )";
            toolStripStatusLabel2.Text = "R : " + myImgData[e.X, e.Y].R;
            toolStripStatusLabel3.Text = "G : " + myImgData[e.X, e.Y].G;
            toolStripStatusLabel4.Text = "B : " + myImgData[e.X, e.Y].B;
            toolStripStatusLabel5.Text = "H : " + myImgData[e.X, e.Y].hue;
            toolStripStatusLabel6.Text = "S : " + myImgData[e.X, e.Y].saturation;
            toolStripStatusLabel7.Text = "I : " + myImgData[e.X, e.Y].intensity;
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.Cross;
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void rGBHSIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_RGBHSI form_RGBHSI = new Form_RGBHSI();
            form_RGBHSI.bitmap = bitmap;
            form_RGBHSI.read_photo = myImgData;
            form_RGBHSI.Show();
        }

        private void enlargeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            Form_Enlarge form_Enlarge = new Form_Enlarge();
            form_Enlarge.myPicture = my_Picture;          
            form_Enlarge.Show();
            

        }

        private void brightnessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_Brightness form_Brightness = new Form_Brightness();
            form_Brightness.bitmap = bitmap;
            form_Brightness.read_photo = myImgData;
            form_Brightness.Show();
        }

        private void negativeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_Negative form_Negative = new Form_Negative();
            form_Negative.bitmap = bitmap;
            form_Negative.read_photo = myImgData;
            form_Negative.Show();
        }

        private void bitPlaneSlicingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_Slicing form_Slicing = new Form_Slicing();
            form_Slicing.bitmap = bitmap;
            form_Slicing.read_photo = myImgData;
            form_Slicing.Show();
        }

        private void wartermarkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_Watermark form_Watermark = new Form_Watermark();
            form_Watermark.bitmap = bitmap;
            form_Watermark.read_photo = myImgData;
            form_Watermark.Show();
        }

        private void transparencyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_Transparency form_Transparency = new Form_Transparency();
            form_Transparency.bitmap = bitmap;
            form_Transparency.read_photo = myImgData;
            form_Transparency.Show();
        }

        private void histogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_Histogram_Equalization form_Histogram_Equalization = new Form_Histogram_Equalization();
            form_Histogram_Equalization.Histogram_mode = Form_Histogram_Equalization.mode.Histogram;
            form_Histogram_Equalization.bitmap = bitmap;
            form_Histogram_Equalization.read_photo = myImgData;
            form_Histogram_Equalization.Show();
        }

        private void outlierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_Filter form_Filter = new Form_Filter();
            form_Filter.current_mode = Form_Filter.filter_mode.Outlier;
            form_Filter.bitmap = bitmap;
            form_Filter.read_photo = myImgData;
            form_Filter.Show();
        }

        private void thresholdingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_Thresholding form_Thresholding = new Form_Thresholding();
            form_Thresholding.bitmap = bitmap;
            form_Thresholding.read_photo = myImgData;
            form_Thresholding.Show();
        }

        private void medianToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_Filter form_Filter = new Form_Filter();
            form_Filter.current_mode = Form_Filter.filter_mode.Median;
            form_Filter.bitmap = bitmap;
            form_Filter.read_photo = myImgData;
            form_Filter.Show();
        }


        private void lowpassToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_Filter form_Filter = new Form_Filter();
            form_Filter.current_mode = Form_Filter.filter_mode.Lowpass;
            form_Filter.bitmap = bitmap;
            form_Filter.read_photo = myImgData;
            form_Filter.Show();
        }

        private void highpassToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_Filter form_Filter = new Form_Filter();
            form_Filter.current_mode = Form_Filter.filter_mode.Highpass;
            form_Filter.bitmap = bitmap;
            form_Filter.read_photo = myImgData;
            form_Filter.Show();
        }



        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_cut form_Cut = new Form_cut();
            form_Cut.read_photo = myImgData;
            form_Cut.size = sz;
            form_Cut.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Bitmap b = new Bitmap(pictureBox1.Image);
            ConvertToIco(pictureBox1.Image, "icon.ico", 32);
            //b.Save("Icon.ico", System.Drawing.Imaging.ImageFormat.Icon);
        }
        public void ConvertToIco(Image img, string file_name, int size)
        {
            Icon icon;
            using (var msImg = new MemoryStream())
            using (var msIco = new MemoryStream())
            {
                img.Save(msImg, System.Drawing.Imaging.ImageFormat.Png);
                using (var bw = new BinaryWriter(msIco))
                {
                    bw.Write((short)0);           //0-1 reserved
                    bw.Write((short)1);           //2-3 image type, 1 = icon, 2 = cursor
                    bw.Write((short)1);           //4-5 number of images
                    bw.Write((byte)size);         //6 image width
                    bw.Write((byte)size);         //7 image height
                    bw.Write((byte)0);            //8 number of colors
                    bw.Write((byte)0);            //9 reserved
                    bw.Write((short)0);           //10-11 color planes
                    bw.Write((short)32);          //12-13 bits per pixel
                    bw.Write((int)msImg.Length);  //14-17 size of image data
                    bw.Write(22);                 //18-21 offset of image data
                    bw.Write(msImg.ToArray());    // write image data
                    bw.Flush();
                    bw.Seek(0, SeekOrigin.Begin);
                    icon = new Icon(msIco);
                }
            }
            using (var fs = new FileStream(file_name, FileMode.Create, FileAccess.Write))
            {
                icon.Save(fs);
            }
        }

        private void contrastStretchingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_Constrast_Stretching form_Constrast_Stretching = new Form_Constrast_Stretching();
            form_Constrast_Stretching.bitmap = bitmap;
            form_Constrast_Stretching.read_photo = myImgData;
            form_Constrast_Stretching.Show();
        }

        private void edgeCrispeningToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_Filter form_Filter = new Form_Filter();
            form_Filter.current_mode = Form_Filter.filter_mode.Edge_Crispening;
            form_Filter.bitmap = bitmap;
            form_Filter.read_photo = myImgData;
            form_Filter.Show();
        }

        private void highboostFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_Filter form_Filter = new Form_Filter();
            form_Filter.current_mode = Form_Filter.filter_mode.High_boost;
            form_Filter.bitmap = bitmap;
            form_Filter.read_photo = myImgData;
            form_Filter.Show();
        }

        private void gradientToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Form_Filter form_Filter = new Form_Filter();
            form_Filter.current_mode = Form_Filter.filter_mode.Gradient;
            form_Filter.bitmap = bitmap;
            form_Filter.read_photo = myImgData;
            form_Filter.Show();
            
        }

        private void histogramEqualizationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_Histogram_Equalization form_Histogram_Equalization = new Form_Histogram_Equalization();
            form_Histogram_Equalization.Histogram_mode = Form_Histogram_Equalization.mode.Histogram_Equalization;
            form_Histogram_Equalization.bitmap = bitmap;
            form_Histogram_Equalization.read_photo = myImgData;
            form_Histogram_Equalization.Show();
        }

        private void magicWandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_cut form_Cut = new Form_cut();
            form_Cut.isMagicWand = true;
            form_Cut.current_mode = Form_cut.mode.Magic_Wand;
            form_Cut.read_photo = myImgData;
            form_Cut.size = sz;
            form_Cut.Show();
        }

        private void histogramSpecificationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_Histogram_Specification form_Histogram_Specification = new Form_Histogram_Specification();

            form_Histogram_Specification.bitmap = bitmap;
            form_Histogram_Specification.read_photo = myImgData;
            form_Histogram_Specification.Show();
        }

        private void huffmanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_Huffman form_Huffman = new Form_Huffman();
            form_Huffman.image = new myPicture(sz.Width,sz.Height);
            form_Huffman.image.my_Pixel = myImgData;
            form_Huffman.Show();
        }

        private void zoomoutToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void pseudoMedianToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_Filter form_Filter = new Form_Filter();
            form_Filter.current_mode = Form_Filter.filter_mode.pseudo_Median;
            form_Filter.bitmap = bitmap;
            form_Filter.read_photo = myImgData;
            form_Filter.Show();
        }
    }

}
