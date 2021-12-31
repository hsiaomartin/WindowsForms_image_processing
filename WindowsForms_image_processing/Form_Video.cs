using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsForms_image_processing
{
    public partial class Form_Video : Form
    {
        bool continuous_mode = true;
        bool show_run = false;
        public enum mode { decode,encode,player };
        public mode video_mode =mode.encode;
        int continue_frame = 0;
        int total_frame = 0;
        enum matching_Criteria {MAD,MSD,PDC,IP};
        string[] matching_Criteria_Name = { "MAD", "MSD", "PDC", "IP" };
        matching_Criteria current_Criteria = matching_Criteria.PDC;

        string old_encode_file_path = "";

        string fileName = "";
        int refreshUI = 0;

        public List<string> myList = new List<string>();
        public List<string> my_player_List = new List<string>();
        public myPixel[,] my_Data;
        public myPixel[,] my_Data_current;

        int isFinish = 0;
        int isDrawMV = 0;
        int isDecode = 0;

        Bitmap bitmap_draw; //read from source target
        Bitmap bitmap_current_draw;//read from source current

        Bitmap target_Bitmap;
        Bitmap current_Bitmap;
        
        Pen pen = new Pen(Color.Red, 1);//moving block pen
        Rectangle rectangle;
        Bitmap img_Block;
        Bitmap current_img_Block;

        //motion vector
        Bitmap bitmap_motion_vector;
        Pen pen_arrow = new Pen(Color.FromArgb(255, 255, 0, 0), 1);
        
        //matching criteria threshold
        double threshold = 0.5;
        double PDC_thres = 50;

        //draw the rebuild image from motion vector
        Bitmap bitmap_Rebuild;
        public myPixel[,] my_Rebuild_Data;

        //for next rebuild
        Bitmap next_Bitmap_Rebuild;
        string current_PSNR = "";

        private bool _ok = true;
        bool isPause=false;
        bool isStep = false;
        bool isStep_2 = false;

        bool doFirstFrame = true;
        string first_img="";
        string second_img="";

        StreamWriter sw;

        DateTime timeStamp;
        DateTime encode_video_time;
        public Form_Video()
        {
            InitializeComponent();
            comboBox3.Items.Add("MAD");
            comboBox3.Items.Add("MSD");
            comboBox3.Items.Add("PDC");
            comboBox3.Items.Add("IP");
            button_file.Visible = false;
            continuous_mode = true;
            checkBox1.Visible = false;
            checkBox2.Visible = false;
        }

        private void Form_Video_Shown(object sender, EventArgs e)
        {

            Bitmap bitmap = new Bitmap(100, 100, PixelFormat.Format24bppRgb);
            Graphics g = Graphics.FromImage(bitmap);
            g.Clear(Color.White);
            pictureBox1.Image = bitmap;
            pictureBox2.Image = bitmap;
            pictureBox3.Image = bitmap;
            pictureBox4.Image = bitmap;
            pictureBox5.Image = bitmap;
            pictureBox6.Image = bitmap;
            if (video_mode == mode.encode)
            {
                this.Text = "Video encode";
                fileToolStripMenuItem.Visible = false;
                checkBox1.Visible = true;
                checkBox2.Visible = true;
                pictureBox6.Visible = false;
                label4.Visible = false;
            }
            else if (video_mode == mode.decode)
            {
                this.Text = "Video decode";
                pictureBox4.Visible = false;
                pictureBox5.Visible = false;
                pictureBox6.Visible = false;
                label4.Visible = false;
                comboBox1.Visible = false;
                comboBox2.Visible = false;
                comboBox3.Enabled = false;
                button_start.Visible = false;
                button_step.Visible = false;
                button_pause.Visible = false;

            }
            else if(video_mode == mode.player)
            {
                this.Text = "Video player";
                pictureBox4.Visible = false;
                pictureBox5.Visible = false;
                comboBox1.Visible = false;
                comboBox2.Visible = false;
                comboBox3.Enabled = false;
                button_file.Visible = false;
                //menuStrip1.Visible = false;
                menuStrip1.Text = "Source file";
                button_start.Enabled = false;
                button_step.Enabled = false;
                chart1.Visible = true;
                chart1.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;
                chart1.ChartAreas[0].AxisY.MajorGrid.LineWidth = 0;
                chart1.ChartAreas[0].AxisX.Minimum = -2;
                chart1.ChartAreas[0].AxisX.Maximum = 20;
                chart1.ChartAreas[0].AxisX.Interval = 20;
                chart1.ChartAreas[0].AxisY2.Enabled = AxisEnabled.False;
                var dataPointSeries_PSNR = new Series
                {
                    Name = "PSNR",
                    Color = Color.Red,
                    ChartType = SeriesChartType.Line
                };
                chart1.Series.Add(dataPointSeries_PSNR);
                checkBox3.Checked = true;
                chart1.Series["PSNR"].IsValueShownAsLabel = true;
            }

            foreach (string path in myList)
            {
                comboBox1.Items.Add(Path.GetFileName(path));
                comboBox2.Items.Add(Path.GetFileName(path));

            }
            label1.Text = "Target frame : ";
            label2.Text = "Current frame : ";
            label3.Text = "Motion vector";
            label4.Text = "rebuild";
            label5.Text = "PSNR :\ncost time :";
            label7.Text = "( ? / " + myList.Count+" )";
            label8.Text = "( ? / " + myList.Count+" )";

            if(video_mode == mode.decode)
            {
                label7.Text = "";
                label8.Text = "";
            }
            else if((video_mode == mode.player))
            {
                label1.Text = "";
                label2.Text = "";
                label3.Text = "";
                label4.Text = "";
                label5.Text = "";
                label7.Text = "";
                label8.Text = "";
            }

            pen_arrow.StartCap = LineCap.ArrowAnchor;//for motion vector

            button_pause.Enabled = false;
            //pictureBox1.Image = bitmap_current;
        }

        private void button_start_Click(object sender, EventArgs e)
        {
            checkBox1.Enabled = false;
            checkBox2.Enabled = false;
            if (checkBox1.Checked == true) continuous_mode = true;
            else continuous_mode = false;
            if (checkBox2.Checked == true) show_run = true;
            else show_run = false;
            if(video_mode == mode.encode) {
                
                if (continuous_mode)
                {
                    comboBox1.SelectedIndex = 0;
                    comboBox2.SelectedIndex = 1;
                    //comboBox3.SelectedIndex = 0;
                    continue_frame = comboBox2.SelectedIndex;
                }
                encode_video_time = DateTime.Now;
                if (!isPause)
                {
                    setup();
                }
                else if (isPause)
                {
                    isPause = false;
                    button_start.Enabled = false;
                    button_pause.Enabled = true;
                    isStep = false;
                    isStep_2 = false;
                }
                else if (isStep || isStep_2)
                {
                    isStep = false;
                    isStep_2 = false;
                }  
            }
            else if(video_mode == mode.player)
            {
                if (!isPause)
                {
                    if(chart1.Series["PSNR"].Points != null)
                    {
                        chart1.Series["PSNR"].Points.Clear();
                    }
                    button_start.Enabled = false;
                    button_pause.Enabled = true;
                    //button_step.Enabled = false;
                    backgroundWorker1.RunWorkerAsync();//移到執行緒執行
                }
                else if (isPause)
                {
                    isPause = false;
                    button_start.Enabled = false;
                    button_pause.Enabled = true;
                    button_step.Enabled = true;
                    isStep = false;
                    isStep_2 = false;
                    button_file.Enabled = false;
                }
                else if (isStep || isStep_2)
                {
                    isStep = false;
                    isStep_2 = false;
                }
            }
        }
        void setup() {

            if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1)
            {
                current_Criteria = (matching_Criteria)comboBox3.SelectedIndex;
                comboBox1.Enabled = false;
                comboBox2.Enabled = false;
                comboBox3.Enabled = false;

                first_img = myList[comboBox1.SelectedIndex];
                second_img = myList[comboBox2.SelectedIndex];
                label1.Text = "Target frame : " + Path.GetFileName(first_img);
                label2.Text = "Current frame : " + Path.GetFileName(second_img);
                label7.Text = "( " + (comboBox1.SelectedIndex+1 ) + " / " + myList.Count + " ) "+ first_img;
                label8.Text = "( " + (comboBox2.SelectedIndex+1 ) + " / " + myList.Count + " ) "+ second_img;
                if(doFirstFrame)
                    fileName = Path.GetFileName(first_img) + "_" + Path.GetFileName(second_img) + "_motion_vector_" + matching_Criteria_Name[(int)current_Criteria] + ".txt";
                else
                    fileName = Path.GetFileName(first_img) + "_rebuild_" + Path.GetFileName(second_img) + "_motion_vector_" + matching_Criteria_Name[(int)current_Criteria] + ".txt";

                try
                {
                    sw = new StreamWriter(fileName);
                    sw.WriteLine(first_img + "\n" + second_img+"\n"+ (int)current_Criteria);
                    sw.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: " + ex.Message);
                }



                button_start.Enabled = false;
                button_pause.Enabled = true;

                backgroundWorker1.RunWorkerAsync();//移到執行緒執行

            }
            else
            {
                MessageBox.Show("please select two file and Matching Criteria.");
            }
        }


        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            switch (video_mode)
            {
                case mode.player:
                    play_video();
                    break;
                case mode.encode:
                    if (doFirstFrame)
                    {
                        doFirstFrame = false;
                        Bitmap bitmap = new Bitmap(first_img);
                        next_Bitmap_Rebuild =new Bitmap(compare_Image(bitmap, second_img));
                        //Form_show_img form_Show_Img = new Form_show_img();
                        //form_Show_Img.bitmap = next_Bitmap_Rebuild;
                        //form_Show_Img.Show();
                        Console.WriteLine("first time");
                    }
                    else
                    {
                        next_Bitmap_Rebuild = new Bitmap(compare_Image(next_Bitmap_Rebuild, second_img));
                        //Form_show_img form_Show_Img = new Form_show_img();
                        //form_Show_Img.bitmap = next_Bitmap_Rebuild;
                        //form_Show_Img.Show();
                        Console.WriteLine("next time");
                    }
                    break;
                default:
                    break;
            }
        }

        private void button_pause_Click(object sender, EventArgs e)
        {
            isPause = true;
            button_start.Enabled = true;
            button_pause.Enabled = false;
            button_step.Enabled = true;
        }

        public Bitmap compare_Image(Bitmap bitmap, string img_current)
        {
            
            timeStamp = DateTime.Now;
            //--target image

            bitmap_draw = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format24bppRgb);
            Graphics g_bitmap = Graphics.FromImage(bitmap_draw);
            Graphics g_modified_bitmap;
            img_Block = new Bitmap(64, 64);//show block clearly
            Graphics g_Block = Graphics.FromImage(img_Block);
            my_Data = new myPixel[bitmap.Width, bitmap.Height];


            //--current image
            Bitmap bitmap_current = new Bitmap(img_current);
            bitmap_current_draw = new Bitmap(bitmap_current.Width, bitmap_current.Height, PixelFormat.Format24bppRgb);
            Graphics g_bitmap_current = Graphics.FromImage(bitmap_current_draw);
            current_img_Block = new Bitmap(64, 64);//show block clearly
            Graphics g_current_Block = Graphics.FromImage(current_img_Block);
            Graphics g_modified_current_bitmap;
            my_Data_current = new myPixel[bitmap_current.Width, bitmap_current.Height];
            int block_len = 8;
            int block_counter = 0;
            int n = (bitmap.Width* bitmap.Height) / (block_len * block_len);

            //--motion vector
            bitmap_motion_vector = new Bitmap(bitmap_current.Width, bitmap_current.Height, PixelFormat.Format24bppRgb);
            Graphics g_bitmap_motion_vector = Graphics.FromImage(bitmap_motion_vector);
            g_bitmap_motion_vector.Clear(Color.White);
            pictureBox3.Image = bitmap_motion_vector;

            // rebuild
            bitmap_Rebuild = new Bitmap(bitmap_current.Width, bitmap_current.Height, PixelFormat.Format24bppRgb);
            Graphics g_bitmap_Rebuild = Graphics.FromImage(bitmap_Rebuild);
            g_bitmap_Rebuild.Clear(Color.White);
            //pictureBox6.Image = bitmap_Rebuild;
            my_Rebuild_Data =  new myPixel[bitmap.Width, bitmap.Height];

            myPicture[] block_My_Data_current = new myPicture[n];
            Point[] block_original_Location = new Point[n];
            // load img to array
            for (int Ycount = 0; Ycount < bitmap.Height; Ycount++)
            {
                for (int Xcount = 0; Xcount < bitmap.Width; Xcount++)
                {
                    
                    int R = bitmap.GetPixel(Xcount, Ycount).R;
                    int G = bitmap.GetPixel(Xcount, Ycount).G;
                    int B = bitmap.GetPixel(Xcount, Ycount).B;
                    my_Data[Xcount, Ycount] = new myPixel(R,G,B);
                    
                    g_bitmap.FillRectangle(my_Data[Xcount, Ycount].myColor, Xcount, Ycount, 1, 1);

                    int R_current = bitmap_current.GetPixel(Xcount, Ycount).R;
                    int G_current = bitmap_current.GetPixel(Xcount, Ycount).G;
                    int B_current = bitmap_current.GetPixel(Xcount, Ycount).B;
                    my_Data_current[Xcount, Ycount] = new myPixel(R_current, G_current,B_current);

                    g_bitmap_current.FillRectangle(my_Data_current[Xcount, Ycount].myColor, Xcount, Ycount, 1, 1);

                }
            }


            pictureBox1.Image = new Bitmap(bitmap);
            pictureBox2.Image = new Bitmap(bitmap_current_draw);

            //generate block of current img 
            for (int Y = 0; Y <= bitmap_current.Height - 8; Y+=8)
            {
                for (int X = 0; X <= bitmap_current.Width - 8; X+=8)
                {
                    rectangle = new Rectangle(X, Y, 8, 8);
                    block_original_Location[block_counter] = new Point(X, Y);
                    // Set each pixel in target_Bitmap to black.

                    block_My_Data_current[block_counter] = new myPicture(new Size(block_len, block_len));
                    for (int Ycount = 0; Ycount < 8; Ycount++)
                    {
                        for (int Xcount = 0; Xcount < 8; Xcount++)
                        {
                            int R = my_Data_current[Xcount + X, Ycount + Y].R;
                            int G = my_Data_current[Xcount + X, Ycount + Y].G;
                            int B = my_Data_current[Xcount + X, Ycount + Y].B;
                            
                            block_My_Data_current[block_counter].my_Pixel[Xcount, Ycount] = new myPixel(R,G,B);
                            //g_current_Block.FillRectangle(new SolidBrush(Color.FromArgb(R, G, B)), Xcount * 8, Ycount * 8, 8, 8);
                        }
                    }
                    block_counter++;
                }
            }

            //double correlation_coeffi = 0.0;
            
            
            for (int counter = 0; counter < block_counter; counter++)
            {

                //refreshUI = 1;
                //backgroundWorker1.ReportProgress(refreshUI);
                //while (refreshUI == 1) ;

                DateTime step_time = DateTime.Now;
                //while (isPause) ;
                //draw current block
                if (show_run)
                {
                    rectangle = new Rectangle(block_original_Location[counter], new Size(8, 8));
                    for (int Ycount = 0; Ycount < 8; Ycount++)
                        for (int Xcount = 0; Xcount < 8; Xcount++)
                            g_current_Block.FillRectangle(block_My_Data_current[counter].my_Pixel[Xcount, Ycount].myColor, Xcount * 8, Ycount * 8, 8, 8);

                    current_Bitmap = new Bitmap(bitmap_current_draw);
                    g_modified_current_bitmap = Graphics.FromImage(current_Bitmap);
                    g_modified_current_bitmap.DrawRectangle(pen, rectangle);


                    pictureBox2.Image = current_Bitmap;
                    pictureBox5.Image = new Bitmap(current_img_Block);
                }
                double MAD = Double.MaxValue; //mean absolute difference
                double MSD = Double.MaxValue; //mean square difference
                int PDC = 0;//pel difference classification

                //determine current block from target frame
                Point target_Point = new Point(0,0);
                for (int Y = 0; Y <= bitmap.Height - 8; Y++)
                {
                    for (int X = 0; X <= bitmap.Width - 8; X++)
                    {
                        if (show_run)
                        {
                            while (isFinish == 1) ; //wait drawing
                            rectangle = new Rectangle(X, Y, 8, 8);
                        }
                            
                        
                        // Set each pixel in target_Bitmap to black.
                        while (isPause) ;
                        while (isStep) ;
                        double sum_abs = 0.0;//mean absolute difference
                        double sum_square = 0.0;//mean square difference
                        int ord_sum = 0;

                        for (int Ycount = 0; Ycount < 8; Ycount++)
                        {
                            for (int Xcount = 0; Xcount < 8; Xcount++)
                            {
                                if(show_run)
                                    g_Block.FillRectangle(my_Data[Xcount + X, Ycount + Y].myColor, Xcount * 8, Ycount * 8, 8, 8);


                                int diff = (my_Data[Xcount + X, Ycount + Y].Gray) - (block_My_Data_current[counter].my_Pixel[Xcount, Ycount].Gray);
                                sum_abs += Math.Abs(diff);//MAD                                
                                sum_square += (diff) * (diff);//MSD
                                if (Math.Abs(diff) < 1) ord_sum++;//PDC
                            }
                        }

                        double tmp_MAD = sum_abs / (8 * 8);
                        double tmp_MSD = sum_square / (8 * 8);
                        if(current_Criteria == matching_Criteria.MAD)
                        {
                            if (MAD > tmp_MAD)
                            {
                                //Console.WriteLine("MAD : "+ tmp_MAD);
                                target_Point.X=X;
                                target_Point.Y=Y;
                                MAD = tmp_MAD;
                            }
                            if (MAD < threshold) break;
                        }
                        else if(current_Criteria == matching_Criteria.MSD)
                        {
                            if (MSD > tmp_MSD)
                            {
                                //Console.WriteLine("MSD : " + tmp_MSD);
                                target_Point.X = X;
                                target_Point.Y = Y;
                                MSD = tmp_MSD;
                            }
                            if (MSD < threshold) break;
                        }
                        else if (current_Criteria == matching_Criteria.PDC)
                        {
                            if (PDC < ord_sum)
                            {
                                //Console.WriteLine("ord_sum : " + ord_sum );
                                PDC = ord_sum;
                                target_Point.X = X;
                                target_Point.Y = Y;
                            }
                            if (PDC > PDC_thres)  break; 
                        }
                        else if (current_Criteria == matching_Criteria.IP)
                        {

                        }

                        if (show_run)
                        {
                            target_Bitmap = new Bitmap(bitmap_draw);
                            g_modified_bitmap = Graphics.FromImage(target_Bitmap);
                            g_modified_bitmap.DrawRectangle(pen, rectangle);
                        }


                        if (show_run)
                        {
                            isFinish = 1;
                            backgroundWorker1.ReportProgress(isFinish);
                        }

                        while (isStep_2) ;
                    }

                    if (current_Criteria == matching_Criteria.MAD && (MAD < threshold)) break;
                    else if (current_Criteria == matching_Criteria.MSD && (MSD < threshold))break;       
                    else if (current_Criteria == matching_Criteria.PDC && (PDC > PDC_thres))break; 
                    else if (current_Criteria == matching_Criteria.IP){ }
                }

                if (show_run) { 
                    //Console.WriteLine("draw new vector.");
                    while (isDrawMV == 1) ; //wait drawing
                    //draw motion vector

                    Point line_Start = new Point(block_original_Location[counter].X + 4, block_original_Location[counter].Y + 4);
                    Point line_End = new Point(target_Point.X + 4, target_Point.Y + 4);
                    g_bitmap_motion_vector.DrawLine(pen_arrow, line_Start, line_End);
                }

                //-draw rebuild img
                int y = 0;
                for (int Ycount = block_original_Location[counter].Y; Ycount < block_original_Location[counter].Y+8; Ycount++)
                {
                    int x = 0;
                    for (int Xcount = block_original_Location[counter].X; Xcount < block_original_Location[counter].X+8; Xcount++)
                    {
                        int R = my_Data[target_Point.X+x, target_Point.Y+y].R;
                        int G = my_Data[target_Point.X+x, target_Point.Y+y].G;
                        int B = my_Data[target_Point.X+x, target_Point.Y+y].B;
                        my_Rebuild_Data[Xcount, Ycount] = new myPixel(R, G, B);

                        //if (show_run)
                            g_bitmap_Rebuild.FillRectangle(my_Data[target_Point.X + x, target_Point.Y + y].myColor, Xcount , Ycount , 1, 1);
                        x++;
                    }
                    y++;
                }

                if (show_run)
                {
                    isDrawMV = 1;
                    backgroundWorker1.ReportProgress(isDrawMV);
                }

                using (StreamWriter sw = File.AppendText(fileName))
                {
                    sw.WriteLine((target_Point.X).ToString() + " " + (target_Point.Y).ToString());
                }

                long costTime = DateTime.Now.Ticks - step_time.Ticks;
                TimeSpan timeSpan = new TimeSpan(costTime);
                Console.WriteLine("\nOne step cost time : \n" + (timeSpan).ToString());


            }

            _ok = true;
            return bitmap_Rebuild;
        }


        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if(e.ProgressPercentage ==1)
            {
                //Console.WriteLine("report : ");
                if (isFinish == 1 && show_run)
                {
                    pictureBox1.Image = target_Bitmap;
                    pictureBox4.Image = new Bitmap(img_Block);
                }

                if(isDrawMV == 1)
                {
                    pictureBox3.Image = bitmap_motion_vector;
                    pictureBox6.Image = bitmap_Rebuild;
                }

                if (refreshUI == 1)
                {
                    label5.Text = "time : " + (DateTime.Now).ToString();
                    //Console.WriteLine("time : " + (DateTime.Now.Ticks).ToString());

                }
                refreshUI = 0;
                isDrawMV = 0;
                isFinish = 0;               
            }
            else if(e.ProgressPercentage == 3)
            {
                if(isDecode == 3 && video_mode == mode.decode)
                {
                    pictureBox1.Image = current_Bitmap;
                    pictureBox2.Image = bitmap_Rebuild;
                    pictureBox3.Image = bitmap_motion_vector;

                    //label1.Text = "original frame : " + Path.GetFileName(second_img);
                    label1.Text = "original frame : " ;
                    label2.Text = "decode frame  ";
                    //label7.Text = "( " + continue_frame + " / " + (total_frame + 1) + " ) " + second_img;
                    //label8.Text = "( " + continue_frame + " / " + (total_frame + 1) + " ) ";

                    mySize msize = new mySize(bitmap_draw.Width, bitmap_draw.Height);
                    comboBox3.SelectedIndex = (int)current_Criteria;
                    label5.Text = "PSNR : " + Class_img_basic.PSNR(my_Data_current, my_Rebuild_Data, msize);
                }
                else if (isDecode == 3 && video_mode == mode.player)
                {
                    pictureBox1.Image = new Bitmap(second_img);
                    pictureBox2.Image = new Bitmap(bitmap_Rebuild);
                    pictureBox3.Image = bitmap_motion_vector;
                    //pictureBox6.Image = bitmap_Rebuild;

                    label1.Text = "original frame : " + Path.GetFileName(second_img);
                    label2.Text = "decode frame  ";
                    label7.Text = "( " + continue_frame + " / " + (total_frame+1) + " ) " + second_img ;
                    label8.Text = "( " + continue_frame + " / " + (total_frame+1) + " ) " ;

                    comboBox3.SelectedIndex = (int)current_Criteria;
                    mySize msize = new mySize(bitmap_Rebuild.Width, bitmap_Rebuild.Height);
                    string psnr = Class_img_basic.PSNR(my_Data_current, my_Rebuild_Data, msize);
                    label5.Text = "PSNR : " + psnr;

                    chart1.Series["PSNR"].Points.AddXY(continue_frame-1, double.Parse((psnr.Split(' '))[0]));
                    
                }
                isDecode = 0;
            }


                
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (_ok && video_mode!=mode.player)
            {
                button_start.Enabled = true;
                refresh_Info();

                if (continuous_mode)
                {
                    //next round
                    if (continue_frame < myList.Count-1)
                    {
                        continue_frame++;
                        comboBox1.SelectedIndex = continue_frame-1;
                        comboBox2.SelectedIndex = continue_frame;
                        button_start.Enabled = false;
                        button_pause.Enabled = true;
                        setup();
                    }
                    else
                    {
                        long costTime = DateTime.Now.Ticks - encode_video_time.Ticks;
                        TimeSpan timeSpan = new TimeSpan(costTime);
                        doFirstFrame = true;
                        MessageBox.Show("cost time : " + (timeSpan).ToString() , "\n編碼完畢( "+ (continue_frame+1) +" / "+ myList.Count+" )");  //顯示訊息
                    }
                }


            }
            else if (!continuous_mode)
            {
                button_start.Enabled = true;
                button_pause.Enabled = false;
                isPause = false;

                checkBox1.Enabled = true;
                checkBox2.Enabled = true;
            }


        }

        public void refresh_Info()
        {
            long costTime = DateTime.Now.Ticks - timeStamp.Ticks;
            TimeSpan timeSpan = new TimeSpan(costTime);
            mySize msize = new mySize(bitmap_draw.Width, bitmap_draw.Height);
            label5.Text = "PSNR : "+Class_img_basic.PSNR(my_Data_current, my_Rebuild_Data, msize);
            label5.Text += "\ncost time : \n" + (timeSpan).ToString();
            if(continuous_mode == false)
                MessageBox.Show("cost time : \n" + (timeSpan).ToString() + "\nPSNR : " + Class_img_basic.PSNR(my_Data_current, my_Rebuild_Data, msize), "編碼完畢");  //顯示訊息
            using (StreamWriter sw = File.AppendText(fileName))
            {
                sw.WriteLine(label5.Text);
            }
            try
            {
                sw.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1)
            {
                Console.WriteLine(myList[comboBox1.SelectedIndex]);
                Bitmap show_bitmap = new Bitmap(myList[comboBox1.SelectedIndex]);
                pictureBox1.Image = show_bitmap;
                label7.Text = "( " + (comboBox1.SelectedIndex + 1) + " / " + myList.Count + " )";
                
            }
            else
            {
                MessageBox.Show("please select a file");
            }

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex != -1)
            {
                Console.WriteLine(myList[comboBox2.SelectedIndex]);
                Bitmap show_bitmap = new Bitmap(myList[comboBox2.SelectedIndex]);
                pictureBox2.Image = show_bitmap;
                label8.Text = "( " + (comboBox2.SelectedIndex + 1) + " / " + myList.Count + " )";
            }
            else
            {
                MessageBox.Show("please select a file");
            }
}

        public Bitmap decode_Image(Bitmap target_Bitmap, string img_current, Point[] motion_vector)
        {
            while (isDecode == 3) ;
            while (isPause ) ;
            while (isStep) ;
            //target_Bitmap = new Bitmap(img_target);
            bitmap_draw = new Bitmap(target_Bitmap.Width, target_Bitmap.Height, PixelFormat.Format24bppRgb);
            Graphics g_bitmap = Graphics.FromImage(bitmap_draw);
            Graphics g_modified_bitmap;
            img_Block = new Bitmap(64, 64);//show block clearly

            my_Data = new myPixel[target_Bitmap.Width, target_Bitmap.Height];


            //--current image
            current_Bitmap = new Bitmap(img_current);
            bitmap_current_draw = new Bitmap(current_Bitmap.Width, current_Bitmap.Height, PixelFormat.Format24bppRgb);
            Graphics g_bitmap_current = Graphics.FromImage(bitmap_current_draw);
            current_img_Block = new Bitmap(64, 64);//show block clearly

            Graphics g_modified_current_bitmap;
            my_Data_current = new myPixel[current_Bitmap.Width, current_Bitmap.Height];
            int block_len = 8;
            int block_counter = 0;
            int n = (current_Bitmap.Width * current_Bitmap.Height) / (block_len * block_len);

            //--motion vector
            bitmap_motion_vector = new Bitmap(current_Bitmap.Width, current_Bitmap.Height, PixelFormat.Format24bppRgb);
            Graphics g_bitmap_motion_vector = Graphics.FromImage(bitmap_motion_vector);
            g_bitmap_motion_vector.Clear(Color.White);
            

            // rebuild
            bitmap_Rebuild = new Bitmap(current_Bitmap.Width, current_Bitmap.Height, PixelFormat.Format24bppRgb);
            Graphics g_bitmap_Rebuild = Graphics.FromImage(bitmap_Rebuild);
            g_bitmap_Rebuild.Clear(Color.White);
            
            my_Rebuild_Data = new myPixel[target_Bitmap.Width, target_Bitmap.Height];

            Point[] block_original_Location = new Point[n];
            // load img to array
            for (int Ycount = 0; Ycount < target_Bitmap.Height; Ycount++)
            {
                for (int Xcount = 0; Xcount < target_Bitmap.Width; Xcount++)
                {

                    int R = target_Bitmap.GetPixel(Xcount, Ycount).R;
                    int G = target_Bitmap.GetPixel(Xcount, Ycount).G;
                    int B = target_Bitmap.GetPixel(Xcount, Ycount).B;
                    my_Data[Xcount, Ycount] = new myPixel(R, G, B);

                    g_bitmap.FillRectangle(my_Data[Xcount, Ycount].myColor, Xcount, Ycount, 1, 1);

                    int R_current = current_Bitmap.GetPixel(Xcount, Ycount).R;
                    int G_current = current_Bitmap.GetPixel(Xcount, Ycount).G;
                    int B_current = current_Bitmap.GetPixel(Xcount, Ycount).B;
                    my_Data_current[Xcount, Ycount] = new myPixel(R_current, G_current, B_current);

                    g_bitmap_current.FillRectangle(my_Data_current[Xcount, Ycount].myColor, Xcount, Ycount, 1, 1);

                }
            }

            for (int Y = 0; Y <= current_Bitmap.Height - 8; Y += 8)
            {
                for (int X = 0; X <= current_Bitmap.Width - 8; X += 8)
                {
                    block_original_Location[block_counter] = new Point(X, Y);
                    block_counter++;
                }
            }          

            for (int counter = 0; counter < motion_vector.Length; counter++)
            {
                Point line_Start = new Point(block_original_Location[counter].X + 4, block_original_Location[counter].Y + 4);
                Point line_End = new Point(motion_vector[counter].X + 4, motion_vector[counter].Y + 4);
                if (line_Start != line_End)
                    g_bitmap_motion_vector.DrawLine(pen_arrow, line_Start, line_End);
                else
                    g_bitmap_motion_vector.FillEllipse(new SolidBrush(Color.FromArgb(255, 0, 0)), motion_vector[counter].X + 4, motion_vector[counter].Y + 4, 2,2);
                //-draw rebuild img
                int y = 0;
                for (int Ycount = block_original_Location[counter].Y; Ycount < block_original_Location[counter].Y + 8; Ycount++)
                {
                    int x = 0;
                    for (int Xcount = block_original_Location[counter].X; Xcount < block_original_Location[counter].X + 8; Xcount++)
                    {
                        int R = my_Data[motion_vector[counter].X + x, motion_vector[counter].Y + y].R;
                        int G = my_Data[motion_vector[counter].X + x, motion_vector[counter].Y + y].G;
                        int B = my_Data[motion_vector[counter].X + x, motion_vector[counter].Y + y].B;
                        my_Rebuild_Data[Xcount, Ycount] = new myPixel(R, G, B);
                        g_bitmap_Rebuild.FillRectangle(my_Data[motion_vector[counter].X + x, motion_vector[counter].Y + y].myColor, Xcount, Ycount, 1, 1);
                        x++;
                    }
                    y++;
                }

            }



            isDecode = 3;
            backgroundWorker1.ReportProgress(isDecode);
            while (isStep_2) ;
            return bitmap_Rebuild;
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if(video_mode == mode.decode)
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    //openFileDialog.InitialDirectory = filePath;
                    openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                    openFileDialog.FilterIndex = 2;
                    openFileDialog.RestoreDirectory = true;

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        //Get the path of specified file
                        old_encode_file_path = openFileDialog.FileName;

                    }
                }
                if (old_encode_file_path != "")
                {
                    List<string> my_token_List = new List<string>();
                    try
                    { // Create an instance of StreamReader to read from a file.
                      // The using statement also closes the StreamReader.
                        using (StreamReader sr = new StreamReader(old_encode_file_path))     //小寫TXT
                        {
                            String line;
                            // Read and display lines from the file until the end of
                            // the file is reached.
                            while ((line = sr.ReadLine()) != null)
                            {
                                Console.WriteLine(line);
                                my_token_List.Add(line);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Let the user know what went wrong.
                        Console.WriteLine("The file could not be read:");
                        Console.WriteLine(ex.Message);
                    }
                    try
                    {
                        string[] location;
                        Point[] motion_vector = new Point[1024];
                        for (int i = 0; i < my_token_List.Count - 6; i++)
                        {
                            location = my_token_List[i + 3].Split(' ');
                            motion_vector[i] = new Point(int.Parse(location[0]), int.Parse(location[1]));
                        }
                        //if (doFirstFrame)
                        //{
                        //    doFirstFrame = false;
                            Bitmap first_Bitmap = new Bitmap(my_token_List[0]);
                            next_Bitmap_Rebuild = new Bitmap(decode_Image(first_Bitmap, my_token_List[1], motion_vector));
                        //}
                        //else
                        //{
                        //    next_Bitmap_Rebuild = decode_Image(next_Bitmap_Rebuild, my_token_List[1], motion_vector);
                        //}
                        comboBox3.SelectedIndex = int.Parse(my_token_List[2]);
                        label1.Text +=  Path.GetFileName(my_token_List[0]);
                        //label2.Text = "Current frame : " + Path.GetFileName(my_token_List[1]);
                        //label7.Text = my_token_List[0];
                        //label8.Text = my_token_List[1];
                    }
                    catch (Exception ex)
                    {
                        // Let the user know what went wrong.
                        MessageBox.Show("Something wrong.\n"+ex);
                    }
                }
                else
                {
                    //MessageBox.Show( "no File Content at path: " + old_encode_file_path);
                }
            }
            else if(video_mode == mode.player)
            {
                my_player_List = new List<string>();

                // 執行檔路徑下的 MyDir 資料夾
                //string folderName = System.Windows.Forms.Application.StartupPath + @"\MyDir";
                string folderName = "";

                bool isSuccess = false;
                using (FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog())
                {
                    if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                    {
                        //Get the path of specified file
                        folderName = folderBrowserDialog1.SelectedPath;
                        // 取得資料夾內所有檔案
                        foreach (string fname in System.IO.Directory.GetFiles(folderName))
                        {
                            my_player_List.Add(fname);
                        }
                        isSuccess = true;
                    }
                }
                if (isSuccess)
                {
                    button_start.Enabled = true;
                    button_step.Enabled = true;
                }
            }
            
        }

        private void button_step_Click(object sender, EventArgs e)
        {
            //isStep = true;
            Console.WriteLine("step mode " +isStep+" " + isStep_2);
            if (isStep)
            {
               
                isStep = false;
                isStep_2 = true;
            }
            else
            {
                isStep = true;
                isStep_2 = false;
            }
                
        }

        private void button_file_Click(object sender, EventArgs e)
        {
            my_player_List = new List<string>();

            // 執行檔路徑下的 MyDir 資料夾
            //string folderName = System.Windows.Forms.Application.StartupPath + @"\MyDir";
            string folderName = "";

            bool isSuccess = false;
            using (FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog())
            {
                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    folderName = folderBrowserDialog1.SelectedPath;
                    // 取得資料夾內所有檔案
                    foreach (string fname in System.IO.Directory.GetFiles(folderName))
                    {
                        my_player_List.Add(fname);
                    }
                    isSuccess = true;
                }
            }
            if (isSuccess)
            {
                button_start.Enabled = true;
                button_step.Enabled = true;
            }
 
            
        }
    
        public void play_video()
        {
            continue_frame = 1;
            total_frame = myList.Count;
            doFirstFrame = true;
            foreach (string path in myList)
            {
                
                List<string> my_token_List = new List<string>();
                try
                { // Create an instance of StreamReader to read from a file.
                  // The using statement also closes the StreamReader.
                    using (StreamReader sr = new StreamReader(path))     //小寫TXT
                    {
                        String line;
                        // Read and display lines from the file until the end of
                        // the file is reached.
                        while ((line = sr.ReadLine()) != null)
                        {
                            Console.WriteLine(line);
                            my_token_List.Add(line);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Let the user know what went wrong.
                    Console.WriteLine("The file could not be read:");
                    Console.WriteLine(ex.Message);
                }
                try
                {
                    string[] location;
                    Point[] motion_vector = new Point[1024];
                    for (int i = 0; i < my_token_List.Count - 6; i++)
                    {
                        location = my_token_List[i + 3].Split(' ');
                        motion_vector[i] = new Point(int.Parse(location[0]), int.Parse(location[1]));
                    }
                    //comboBox3.SelectedIndex = int.Parse(my_token_List[2]);
                    //label1.Text = "Target frame : " + Path.GetFileName(my_token_List[0]);
                    //label2.Text = "Current frame : " + Path.GetFileName(my_token_List[1]);
                    //label7.Text = my_token_List[0];
                    //label8.Text = my_token_List[1];
                    first_img = my_token_List[0];
                    second_img = my_token_List[1];
                    current_Criteria = ((matching_Criteria)int.Parse(my_token_List[2]));
                    if (doFirstFrame)
                    {
                        doFirstFrame = false;
                        Bitmap first_Bitmap = new Bitmap(my_token_List[0]);
                        next_Bitmap_Rebuild = new Bitmap(decode_Image(first_Bitmap, my_token_List[1], motion_vector));
                    }
                    else
                    {
                        next_Bitmap_Rebuild = new Bitmap(decode_Image(next_Bitmap_Rebuild, my_token_List[1], motion_vector));
                    }
                }
                catch (Exception ex)
                {
                    // Let the user know what went wrong.
                    MessageBox.Show("Someting wrong.\n"+ex);
                    Console.WriteLine(ex);
                }
                continue_frame++;
            }


        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox3.Checked == true)
            {
                chart1.Series["PSNR"].IsValueShownAsLabel = true;
            }
            else
            {
                chart1.Series["PSNR"].IsValueShownAsLabel = false;
            }
        }
    }
    
}