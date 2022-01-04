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
using System.Threading;
namespace WindowsForms_image_processing
{
    public partial class Form1 : Form
    {
        Form2_readImg form2_readImg = new Form2_readImg { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
        string filePath = string.Empty;
        public void SplashStart()
        {
            Application.Run(new Form_splash_screen());
        }
        public Form1()
        {
            Thread t = new Thread(new ThreadStart(SplashStart));
            t.Start();
            Thread.Sleep(1000);

            InitializeComponent();
            this.panel_form_container.Controls.Add(form2_readImg);
            //this.WindowState = FormWindowState.Minimized;
            //this.Show();
            //this.WindowState = FormWindowState.Normal;
            t.Abort();
            //           this.TopMost = true;            
            //           this.BringToFront();
            //           this.TopMost = false;
            menuToolStripMenuItem.Text = "&File";
            menuToolStripMenuItem.ShortcutKeys = Keys.Alt | Keys.F;

            bouncyBallToolStripMenuItem.Text = "&Bouncy ball";
            bouncyBallToolStripMenuItem.ShortcutKeys = Keys.Alt | Keys.B;

            OpenToolStripMenuItem.Text = "&Open";
            OpenToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.O;

            videoToolStripMenuItem.Text = "&Video";
            videoToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.V;

        }



        private void Form1_Shown(object sender, EventArgs e)
        {
            //https://stackoverflow.com/questions/1463417/what-is-the-right-way-to-bring-a-windows-forms-application-to-the-foreground
            this.Activate();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void bouncyBallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_bouncy_ball form_Bouncy_Ball = new Form_bouncy_ball();         
            form_Bouncy_Ball.Show();
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //var fileContent = string.Empty;

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
                    //var fileStream = openFileDialog.OpenFile();
                    //
                    //using (StreamReader reader = new StreamReader(fileStream))
                    //{
                    //    fileContent = reader.ReadToEnd();
                    //}
                }
            }
            form2_readImg.filePath = filePath;
            if (filePath != "")
            {
                form2_readImg.Visible = false;
                form2_readImg.Show();
            }
            else
            {
                //MessageBox.Show(fileContent, "File Content at path: " + filePath, MessageBoxButtons.OK);
            }
        }

        private void menuToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void videoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            

        }

        private void encodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> myList = new List<string>();

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
                        myList.Add(fname);
                    }
                    isSuccess = true;
                }
            }

            if (isSuccess)
            {
                Form_Video form_Video = new Form_Video();
                form_Video.video_mode = Form_Video.mode.encode;
                form_Video.myList = myList;
                form_Video.Show();
            }
            else
            {
                //MessageBox.Show(folderName + " path doesn't exist", "path doesn't exist  ", MessageBoxButtons.OK);
            }

            /*
            List<string> myList = new List<string>();
            string folderName = "E:/NSYSU/master3/image processing/sequences/6.1";
            foreach (string fname in System.IO.Directory.GetFiles(folderName))
            {
                myList.Add(fname);
            }
            Form_Video form_Video = new Form_Video();
            form_Video.myList = myList;
            form_Video.Show();
            */
        }

        private void decodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_Video form_Video = new Form_Video();
            form_Video.video_mode = Form_Video.mode.decode;
            form_Video.Show();
        }

        private void videoPlayerToolStripMenuItem_Click(object sender, EventArgs e)
        {

            List<string> myList = new List<string>();

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
                        myList.Add(fname);
                    }
                    isSuccess = true;
                }
            }

            if (isSuccess)
            {
                Form_Video form_Video = new Form_Video();
                form_Video.video_mode = Form_Video.mode.player;
                form_Video.myList = myList;
                form_Video.Show();
            }
            else
            {
                //MessageBox.Show(folderName + " path doesn't exist", "path doesn't exist  ", MessageBoxButtons.OK);
            }

        }

        private void projectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "image files (*.bmp)|*.bmp";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;
                }
            }            
            if (filePath != "")
            {
                Form_Pattern form_Pattern = new Form_Pattern();
                form_Pattern.filePath = filePath;
                form_Pattern.Visible = false;
                form_Pattern.Show();
            }
        }
    }
}
