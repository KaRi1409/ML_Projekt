/*using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace ML_GUI
{
    public partial class Form1 : Form
    {
        private string pathFolder = Application.StartupPath + @"\assets\inputs\imagesPredictions\";
        private string imagePath = String.Empty;
        Prediction pred;
        private void CaptureCamera()
        {
            camera = new Thread(new ThreadStart(CaptureCameraCallback));
            camera.Start();
        }


        private void CaptureCameraCallback()
        {
            frame = new Mat();
            capture = new VideoCapture(0);
            capture.Open(0);

            if (capture.IsOpened())
            {
                while (isCameraRunning)
                {
                    try
                    {
                        capture.Read(frame);
                        image = BitmapConverter.ToBitmap(frame);
                        if (pictureBox1.Image != null)
                        {
                            pictureBox1.Image.Dispose();
                        }
                        pictureBox1.Image = image;
                    }
                    catch { }
                }
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        VideoCapture capture;
        Mat frame;
        Bitmap image;
        private Thread camera;

        bool isCameraRunning = false;


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (button1.Text.Equals("Start"))
                {
                    CaptureCamera();
                    button1.Text = "Stop";
                    isCameraRunning = true;
                }
                else
                {
                    button1.Text = "Start";
                    isCameraRunning = false;
                    capture.Release();
                    capture.Dispose();
                    camera.Abort();
                    //camera.Interrupt();
                    // button1.Text = "Start";
                    //isCameraRunning = false;

                }
            }
            catch
            {

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (isCameraRunning)
                {
                    if (image != null)
                    {
                        pictureBox2.Image = image;

                        pictureBox2.Update();

                        string pictureName = "IMG_" + DateTime.Now.ToString("dd-MM-yyyy") + "_" + DateTime.Now.ToString("T").Replace(':', '-') + ".png";

                        if (Directory.Exists(pathFolder))
                        {
                            pictureBox1.Image.Save(pathFolder + pictureName, ImageFormat.Png);
                        }
                        else
                        {
                            Directory.CreateDirectory(pathFolder);
                            pictureBox1.Image.Save(pathFolder + pictureName, ImageFormat.Png);
                        }
                        imagePath = pathFolder + pictureName;
                    }
                    else
                    {
                        Console.WriteLine("Cannot take picture if the camera isn't capturing image!");
                    }
                }
            }
            catch
            {

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Training.IsTrained)
                {
                    label3.Text = "Cannot predict if the Model is not trained";
                }
                else
                {
                    if (pred == null)
                    {
                        pred = new Prediction();
                    };
                    string result = null;
                    Cursor.Current = Cursors.WaitCursor;
            

                    result = pred.predict(imagePath);
                    //thread.Start();
                    //thread.Join();
                    label3.Text = result;
                    Cursor.Current = Cursors.Default;

                }
            }
            catch
            {

            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {

            if (capture != null)
            {
                capture.Release();
                capture.Dispose();
            }
            isCameraRunning = false;
            if (camera != null)
            {
                camera.Interrupt();
            }
            Application.Exit();
        }

        OpenFileDialog ofd;
        DialogResult Action;//= ofd.ShowDialog();
        private void button4_Click(object sender, EventArgs e)
        {


            // using (OpenFileDialog ofd = new OpenFileDialog())
            /// {

            if (ofd == null)
            {
                ofd = new OpenFileDialog();
              //  Action = ofd.ShowDialog();
                ofd.Title = "Select an Image";
                ofd.InitialDirectory = "C:\\";
                ofd.Filter = "Image Files(*.JPG;*.PNG)|*.JPG;*.PNG";
                ofd.RestoreDirectory = true;
                Action = ofd.ShowDialog();
            }
            if (Action != DialogResult.OK)
            {
                Action = ofd.ShowDialog();
            }
            
       //     if (Action == null)
          
         //       Action = ofd.ShowDialog();
            
            //     if (ofd.ShowDialog() == DialogResult.OK)
            try
            {
                if (Action == DialogResult.OK)
                {
                    //  try
                    // {
                    var tmp = ofd.OpenFile();
                    string imageName = System.IO.Path.GetFileName(ofd.FileName);
                    pictureBox2.Image = new Bitmap(tmp);
                    imagePath = pathFolder + imageName;

                    string extension = Path.GetExtension(imagePath);

                    int i = 0;
                    while (File.Exists(imagePath))
                    {
                        if (i == 0)
                            imagePath = imagePath.Replace(extension, "(" + ++i + ")" + extension);
                        else
                            imagePath = imagePath.Replace("(" + i + ")" + extension, "(" + ++i + ")" + extension);
                    }
                    pictureBox2.Image.Save(imagePath, ImageFormat.Png);
                }

              //    catch
               //   {
                      MessageBox.Show(e.ToString());
               //   }
               // 
                ofd.Dispose();

            }

            catch
            {
                MessageBox.Show(e.ToString());
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (Training.IsTrained)
                {
                    MessageBox.Show("Model already trained");
                }
                else
                {
                    button5.Enabled = false;
                    progressBar1.Visible = true;
                    progressBar1.Style = ProgressBarStyle.Marquee;
                    progressBar1.MarqueeAnimationSpeed = 50;
                    backgroundWorker1.RunWorkerAsync();
                }
            }
            catch
            {

            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Training.train();

        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBar1.Style = ProgressBarStyle.Continuous;
            progressBar1.MarqueeAnimationSpeed = 0;
            progressBar1.Visible = false;
            button5.Enabled = true;
            Training.IsTrained = true;
            MessageBox.Show("Done");
        }
    }
}
*/using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace ML_GUI
{
    public partial class Form1 : Form
    {
        private string pathFolder = Application.StartupPath + @"\assets\inputs\imagesPredictions\";
        private string imagePath = String.Empty;
        Prediction pred;

        private void CaptureCamera()
        {
            camera = new Thread(new ThreadStart(CaptureCameraCallback));
            camera.Start();
        }

        private void CaptureCameraCallback()
        {
            frame = new Mat();
            capture = new VideoCapture(0);
            capture.Open(0);

            if (capture.IsOpened())
            {
                while (isCameraRunning)
                {
                    try
                    {
                        capture.Read(frame);
                        image = BitmapConverter.ToBitmap(frame);
                        if (pictureBox1.Image != null)
                        {
                            pictureBox1.Image.Dispose();
                        }
                        pictureBox1.Image = image;
                    }
                    catch { }
                }
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        VideoCapture capture;
        Mat frame;
        Bitmap image;
        private Thread camera;

        bool isCameraRunning = false;

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (button1.Text.Equals("Start"))
                {
                    CaptureCamera();
                    button1.Text = "Stop";
                    isCameraRunning = true;
                }
                else
                {
                    capture.Release();
                    button1.Text = "Start";
                    isCameraRunning = false;
                    camera.Interrupt();
                }
            }
            catch
            {

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (isCameraRunning)
                {
                    if (pictureBox2.InvokeRequired)
                    {
                        pictureBox2.Invoke(new MethodInvoker(
                        delegate ()
                        {
                            pictureBox2.Image = image;

                            pictureBox2.Update();
                        }));
                    }
                    else
                    {
                        pictureBox2.Image = image;

                        pictureBox2.Update();

                    }

                    string pictureName = "IMG_" + DateTime.Now.ToString("dd-MM-yyyy") + "_" + DateTime.Now.ToString("T").Replace(':', '-') + ".png";

                    if (Directory.Exists(pathFolder))
                    {
                        pictureBox1.Image.Save(pathFolder + pictureName, ImageFormat.Png);
                    }
                    else
                    {
                        Directory.CreateDirectory(pathFolder);
                        pictureBox1.Image.Save(pathFolder + pictureName, ImageFormat.Png);
                    }
                    imagePath = pathFolder + pictureName;
                }
                else
                {
                    Console.WriteLine("Cannot take picture if the camera isn't capturing image!");
                }
            }
            catch
            {

            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (!Training.IsTrained)
            {
                label3.Text = "Cannot predict if the Model is not trained";
            }
            else
            {
                if (pred == null)
                {
                    pred = new Prediction();
                };
                string result = null;
                Cursor.Current = Cursors.WaitCursor;
               
                result = pred.predict(imagePath);
        
                label3.Text = result;
                Cursor.Current = Cursors.Default;
                //thread.Interrupt();
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (capture != null)
            {
                capture.Release();
                capture.Dispose();
            }
            isCameraRunning = false;
            if (camera != null)
            {
                camera.Interrupt();
            }
            Application.Exit();
        }
       
        private void button4_Click(object sender, EventArgs e)
        {
            using OpenFileDialog ofd = new OpenFileDialog();
            ofd.Reset();
            ofd.Title = "Select an Image";
            ofd.InitialDirectory = "C:\\";
            ofd.Filter = "Image Files(*.JPG;*.PNG)|*.JPG;*.PNG";
            ofd.RestoreDirectory = true;

            DialogResult Action = ofd.ShowDialog();
            if (Action == DialogResult.OK)
            //if (ofd.ShowDialog() == DialogResult.OK)
            {
                var tmp = ofd.OpenFile();
                string imageName = System.IO.Path.GetFileName(ofd.FileName);
                Image image = Image.FromStream(tmp);
                Bitmap bitmap = new Bitmap(image);
                pictureBox2.Image = bitmap;
                // pictureBox2.Image = new Bitmap(tmp);
                tmp.Close();
                ofd.Dispose();  

                imagePath = pathFolder + imageName;
               
                string extension = Path.GetExtension(imagePath);
                imagePath=imagePath.Replace(extension, "");
            

                try
                {  
                     int i = 0;
                    if (File.Exists(imagePath + ".png"))
                    {
 
                            imagePath = imagePath + $" ({i})";
                        while (File.Exists(imagePath + ".png"))
                        {
                            imagePath = imagePath.Replace($"({i})", $"({++i})");

                            // i++;
                            /*
                            if (i == 0)
                                imagePath = imagePath.Replace(extension, "(" + ++i + ")" + extension);
                            else
                                imagePath = imagePath.Replace("(" + i + ")" + extension, "(" + ++i + ")" + extension);*/
                        }
                    }
                  imagePath = imagePath + ".png";
                  pictureBox2.Image.Save(imagePath, ImageFormat.Png);
                }
                catch
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            if (Training.IsTrained)
            {
                MessageBox.Show("Model already trained");
            }
            else
            {
                button5.Enabled = false;
                progressBar1.Visible = true;
                progressBar1.Style = ProgressBarStyle.Marquee;
                progressBar1.MarqueeAnimationSpeed = 50;
                backgroundWorker1.RunWorkerAsync();
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Training.train();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBar1.Style = ProgressBarStyle.Continuous;
            progressBar1.MarqueeAnimationSpeed = 0;
            progressBar1.Visible = false;
            button5.Enabled = true;
            Training.IsTrained = true;
            MessageBox.Show("Done");
        }
    }
}