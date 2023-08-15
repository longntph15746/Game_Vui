using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;

namespace Camera_Barcode
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitializeCamera();
        }
        private IntPtr cameraCaptureGraphBuilder = IntPtr.Zero;
        private IntPtr cameraControl = IntPtr.Zero;
        private IntPtr mediaEventEx = IntPtr.Zero;
        private IntPtr sampleGrabber = IntPtr.Zero;
        private IntPtr graphBuilder = IntPtr.Zero;
        private IntPtr nullRenderer = IntPtr.Zero;

        private int width = 640;
        private int height = 480;

        private void InitializeCamera()
        {
            // Create filter graph
            DirectShow.CreateGraphBuilder(out graphBuilder);
            graphBuilder.AddFilter(DirectShow.VideoInputDeviceFilter, "Video Capture");

            // Create null renderer
            DirectShow.CreateNullRenderer(out nullRenderer);
            graphBuilder.AddFilter(nullRenderer, "Null Renderer");

            // Set camera resolution
            DirectShow.SetCameraResolution(graphBuilder, "Video Capture", width, height);

            // Connect filters
            graphBuilder.Connect(DirectShow.GetPin(graphBuilder, "Video Capture", "Capture"), DirectShow.GetPin(graphBuilder, nullRenderer, "In"));

            // Get interfaces for camera control and capture graph builder
            cameraControl = DirectShow.GetCameraControl(graphBuilder, "Video Capture");
            cameraCaptureGraphBuilder = DirectShow.GetCameraCaptureGraphBuilder(graphBuilder, "Video Capture");

            // Start capturing
            DirectShow.StartCameraCapture(cameraControl, cameraCaptureGraphBuilder);

            // Create Sample Grabber
            DirectShow.CreateSampleGrabber(out sampleGrabber);
            graphBuilder.AddFilter(sampleGrabber, "Sample Grabber");

            // Set Sample Grabber media type
            DirectShow.SetSampleGrabberMediaType(sampleGrabber, width, height);

            // Connect filters
            graphBuilder.Connect(DirectShow.GetPin(graphBuilder, "Video Capture", "Capture"), DirectShow.GetPin(graphBuilder, sampleGrabber, "In"));
            graphBuilder.Connect(DirectShow.GetPin(graphBuilder, sampleGrabber, "Out"), DirectShow.GetPin(graphBuilder, nullRenderer, "In"));

            // Get media event
            mediaEventEx = DirectShow.GetMediaEventEx(graphBuilder);
            DirectShow.SetNotifyWindow(mediaEventEx, this.Handle, DirectShow.WM_GRAPHNOTIFY, IntPtr.Zero);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == DirectShow.WM_GRAPHNOTIFY)
            {
                // Handle media event, e.g., frame captured
                // Implement barcode scanning here
            }
            base.WndProc(ref m);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            // Release resources and stop capturing
            DirectShow.StopCameraCapture(cameraControl, cameraCaptureGraphBuilder);
            DirectShow.ReleaseGraphInterfaces(graphBuilder, sampleGrabber, nullRenderer);
            base.OnClosing(e);
        }
    }

    public static class DirectShow
    {
        // Constants and Native Methods here
        // Implementations for DirectShow methods
    }
}
}
