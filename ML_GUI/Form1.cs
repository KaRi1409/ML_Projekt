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
        private string pathFolder = Application.StartupPath + @"\assets\inputs\imagesPredictions\";
        private string imagePath = String.Empty;
        Prediction pred;
        VideoCapture videoCapture;
        Mat pixelMat;
        Bitmap bitmapImage;
        private Thread cameraThread;
        bool isCameraON = false;

        public Form1()
        {
            InitializeComponent();
        }
        private void CaptureCamera()
        {
            cameraThread = new Thread(new ThreadStart(StartCamera));
            cameraThread.Start();
        }

        private void StartCamera()
        {
            pixelMat = new Mat();
            videoCapture = new VideoCapture(0);
            videoCapture.Open(0);

            if (videoCapture.IsOpened())
            {
                while (isCameraON)
                {
                    try
                    {
                        if (pixelMat != null)
                        {
                            videoCapture.Read(pixelMat);
                            bitmapImage = BitmapConverter.ToBitmap(pixelMat);
                            if (pictureBox1.Image != null)
                            {
                                pictureBox1.Image.Dispose();
                            }
                            if (bitmapImage != null)
                                pictureBox1.Image = bitmapImage;
                        }
                    }
                    catch
                    { 
                    }
                }
            }
        }

    

        private void start_button_Click(object sender, EventArgs e)
        {
            try
            {
                if (start_button.Text.Equals("Start"))
                {
                    CaptureCamera();
                    start_button.Text = "Stop";
                    isCameraON = true;
                }
                else
                {
                    videoCapture.Release();
                    start_button.Text = "Start";
                    isCameraON = false;
                    cameraThread.Interrupt();
                }
            }
            catch
            {

            }
        }

        private void snapshot_button_Click(object sender, EventArgs e)
        {
            try
            {
                if (isCameraON)
                {
                    if (pictureBox2.InvokeRequired)
                    {
                        pictureBox2.Invoke(new MethodInvoker(
                        delegate ()
                        {
                            pictureBox2.Image = bitmapImage;

                            pictureBox2.Update();
                        }));
                    }
                    else
                    {
                        Bitmap bitmap = new Bitmap(bitmapImage);
                        pictureBox2.Image =bitmap;
                        try {
                            pictureBox2.Update();
                        }catch
                        {

                        }
                    }

                    string pictureName = "IMG_" + DateTime.Now.ToString("dd-MM-yyyy") + "_" + DateTime.Now.ToString("T").Replace(':', '-') + ".png";

                    if (Directory.Exists(pathFolder))
                    {
                        var bitMap = new Bitmap(pictureBox1.Image);
                        bitMap.Save(pathFolder + pictureName, ImageFormat.Png);
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
        private void predict_button_Click(object sender, EventArgs e)
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
         
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (videoCapture != null)
            {
                videoCapture.Release();
                videoCapture.Dispose();
            }
            isCameraON = false;
            if (cameraThread != null)
            {
                cameraThread.Interrupt();
            }
            Application.Exit();
        }
       
        private void open_file_button_Click(object sender, EventArgs e)
        {
            using OpenFileDialog ofd = new OpenFileDialog();
            ofd.Reset();
            ofd.Title = "Select an Image";
            ofd.InitialDirectory = "C:\\";
            ofd.Filter = "Image Files(*.JPG;*.PNG)|*.JPG;*.PNG";
            ofd.RestoreDirectory = true;
      
            DialogResult Action = ofd.ShowDialog();
           
            if (Action == DialogResult.OK)
     
            {
                var tmp = ofd.OpenFile();
                string imageName = System.IO.Path.GetFileName(ofd.FileName);
                Image image = Image.FromStream(tmp);
                Bitmap bitmap = new Bitmap(image);
                
                pictureBox2.Image = bitmap;
                    
              
  
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

                          
                        }
                    }
                  imagePath = imagePath + ".png";
                    ImageProcessing.MedianFilter(bitmap, 3).Save(imagePath, ImageFormat.Png); 
      
                }
                catch
                {
                    MessageBox.Show(e.ToString());
                }
            }
         
        }
        private void train_button_Click(object sender, EventArgs e)
        {
            if (Training.IsTrained)
            {
                MessageBox.Show("Model already trained");
            }
            else
            {
                train_button.Enabled = false;
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
            train_button.Enabled = true;
            Training.IsTrained = true;
            MessageBox.Show("Done");
        }

       
    }
}