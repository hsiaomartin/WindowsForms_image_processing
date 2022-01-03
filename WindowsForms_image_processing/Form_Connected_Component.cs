using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsForms_image_processing
{
    public partial class Form_Connected_Component : Form
    {
        public myPicture image;
        Graphics g_Connected;
        Bitmap bitmap;
        int threshold = 117;
        int[,] connect_Table;
        public Form_Connected_Component()
        {
            InitializeComponent();
        }
        public void connect_component(Graphics g,myPicture picture,int[,] table)
        {
            g.Clear(Color.White);
            int marker = 0;
            int[] connect_Mask = new int[9];
            Array.Clear(connect_Mask, 0, connect_Mask.Length);

            for (int Ycount = 0; Ycount < picture.Height; Ycount++)
            {
                for (int Xcount = 0; Xcount < picture.Width; Xcount++)
                {
                    //((Xcount - 1)>0 && (Ycount - 1)>0)
                    //((Xcount + 1)<picture.Width && (Ycount + 1)<picture.Height)
                    connect_Mask[0] = ((Xcount - 1) >= 0 && (Ycount - 1) >= 0)? picture.my_Pixel[Xcount-1, Ycount-1].Gray:0;
                    connect_Mask[1] = ( (Ycount - 1) >= 0) ? picture.my_Pixel[Xcount, Ycount-1].Gray:0;
                    connect_Mask[2] = ((Xcount + 1) < picture.Width && (Ycount - 1) >= 0) ? picture.my_Pixel[Xcount+1, Ycount-1].Gray:0;
                    connect_Mask[3] = ((Xcount - 1) >= 0 )?picture.my_Pixel[Xcount-1, Ycount].Gray:0;
                    connect_Mask[4] = picture.my_Pixel[Xcount, Ycount].Gray;
                    connect_Mask[5] = ((Xcount + 1) < picture.Width)?picture.my_Pixel[Xcount+1, Ycount].Gray:0;
                    connect_Mask[6] = ((Xcount - 1) >= 0 && (Ycount + 1) < picture.Height) ?picture.my_Pixel[Xcount-1, Ycount+1].Gray:0;
                    connect_Mask[7] = (Ycount + 1) < picture.Height ? picture.my_Pixel[Xcount, Ycount+1].Gray:0;
                    connect_Mask[8] = ((Xcount + 1) < picture.Width && (Ycount + 1) < picture.Height)?picture.my_Pixel[Xcount+1, Ycount+1].Gray:0;

                    for(int i = 0; i < connect_Mask.Length; i++)
                    {
                        if (connect_Mask[i] > threshold)
                            connect_Mask[i] = 255;
                        else
                            connect_Mask[i] = 0;
                    }


                    if(((Xcount - 1) >= 0 && (Ycount - 1) >= 0) && ((Xcount + 1) < picture.Width && (Ycount + 1) < picture.Height))
                    {
                        //check central not 0
                        if(connect_Mask[4] != 0)
                        {
                            int[] chk_Table = new int[3];
                            chk_Table[0] = table[Xcount - 1, Ycount - 1];//0
                            chk_Table[1] = table[Xcount , Ycount - 1];//1
                            chk_Table[2] = table[Xcount - 1, Ycount ];//3
                            foreach(int chk in chk_Table)
                                if(chk!=0)
                                    table[Xcount, Ycount] = chk;

                            //if connected before , check new connected to other
                            if (table[Xcount, Ycount] != 0)
                            {
                                if (connect_Mask[5] != 0)
                                    table[Xcount - 1, Ycount - 1] = table[Xcount, Ycount];

                                if (connect_Mask[7] != 0)
                                    table[Xcount, Ycount - 1] = table[Xcount, Ycount];

                                if (connect_Mask[8] != 0)
                                    table[Xcount - 1, Ycount] = table[Xcount, Ycount];
                                

                            }

                            //if new point ,assign a new marker
                            if (table[Xcount, Ycount] == 0)
                            {
                                marker++;
                                table[Xcount, Ycount] = marker;
                            }

                        }                      
                    }
                }
            }
            //draw color in marked area 
            Color []color = new Color[marker+1];
            var rand = new Random();
            for(int i = 1;i <= marker; i++)
            {
                color[i] = Color.FromArgb(rand.Next(128, 255), rand.Next(128, 255), rand.Next(128, 255));
            }

            for (int Ycount = 0; Ycount < picture.Height; Ycount++)
            {
                for (int Xcount = 0; Xcount < picture.Width; Xcount++)
                {
                    for(int i = 1;i <= marker; i++)
                    {
                        if(i == table[Xcount, Ycount])
                        {
                            g.FillRectangle(new SolidBrush(color[i]), Xcount, Ycount, 1, 1);
                            break;
                        }

                    }                    
                }
            }
        }
        private void Form_Connected_Component_Shown(object sender, EventArgs e)
        {
            connect_Table = new int[image.Width, image.Height];
            Array.Clear(connect_Table, 0, connect_Table.Length);
            bitmap = new Bitmap(image.Width, image.Height);
            g_Connected = Graphics.FromImage(bitmap);
            //image.draw(g_Connected);
            connect_component(g_Connected, image, connect_Table);
            pictureBox1.Image = bitmap;
            for (int Ycount = 0; Ycount < image.Height; Ycount++)
            {
                for (int Xcount = 0; Xcount < image.Width; Xcount++)
                {
                    richTextBox1.AppendText( connect_Table[Xcount, Ycount]+" ");
                }
                richTextBox1.AppendText("\n");
            }
            
        }
    }
}
