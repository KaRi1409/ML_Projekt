using OpenCvSharp;
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
        private string pathFolder = Application.StartupPath + @"\assets\inputs\imagesPrediction\";
        private string imagePath = String.Empty;

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
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (isCameraRunning)
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

        private void button3_Click(object sender, EventArgs e)
        {
            string result = null;
            Cursor.Current = Cursors.WaitCursor;
            Prediction pred = new Prediction(imagePath);
            Thread thread = new System.Threading.Thread(() => {
                result = pred.predict();
            });
            thread.Start();
            thread.Join();
            label3.Text = result;
            Cursor.Current = Cursors.Default;
            thread.Interrupt();
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
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Select an Image";
                ofd.InitialDirectory = "C:\\";
                ofd.Filter = "Image Files(*.JPG;*.PNG)|*.JPG;*.PNG";
                ofd.RestoreDirectory = true;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
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
