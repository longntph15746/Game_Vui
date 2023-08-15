using System;
using System.Drawing;
using System.Windows.Forms;
using AForge.Video;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;


namespace Camera
{
    public partial class Form1 : Form
    {
        

        public Form1()
        {
            InitializeComponent();
            InitializeCamera();
        }

        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource;

        public MainForm()
        {
            InitializeComponent();
            InitializeCamera();
        }

        private void InitializeCamera()
        {
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (videoDevices.Count > 0)
            {
                videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
                videoSource.NewFrame += VideoSource_NewFrame;
                videoSource.Start();
            }
            else
            {
                MessageBox.Show("No camera devices found.");
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.SignalToStop();
                videoSource.WaitForStop();
            }
        }

        private void VideoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();
            Image<Bgr, byte> imageFrame = new Image<Bgr, byte>(bitmap);
            Image<Gray, byte> grayFrame = imageFrame.Convert<Gray, byte>();

            using (var barcodeReader = new Emgu.CV.Barcode.BarcodeReader())
            {
                var results = barcodeReader.DecodeMultiple(grayFrame);

                if (results != null && results.Length > 0)
                {
                    string barcodes = string.Join(", ", results);
                    Invoke(new Action(() => textBoxResult.Text = barcodes));
                }
            }

            pictureBoxCamera.Image = bitmap;
        }
    }
}
