using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using AForge.Imaging.Filters;
using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Vision.GlyphRecognition;

namespace Glyphs
{
    public partial class Form1 : Form
    {
        #region Ctor

        // object used for synchronization
        private readonly object _sync = new object();

        private readonly GlyphImageProcessor _imageProcessor;
        private readonly GlyphDatabase _glyphDatabase;

        public Form1()
        {
            InitializeComponent();

            var databaseBuilder = new GlyphDatabaseBuilder();
            _glyphDatabase = databaseBuilder.Database;

            _imageProcessor = new GlyphImageProcessor(_glyphDatabase);
        }

        #endregion

        // Open local video capture device
        private void localVideoCaptureDeviceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new VideoCaptureDeviceForm();

            if (form.ShowDialog(this) == DialogResult.OK)
            {
                // open it
                OpenVideoSource(form.VideoDevice);
            }
        }

        // Open video source
        private void OpenVideoSource(IVideoSource source)
        {
            // set busy cursor
            this.Cursor = Cursors.WaitCursor;

            // reset glyph processor
            lock (_sync)
            {
                _imageProcessor.Reset();
            }

            // stop current video source
            videoSourcePlayer.SignalToStop();
            videoSourcePlayer.WaitForStop();

            // start new video source
            videoSourcePlayer.VideoSource = new AsyncVideoSource(source);
            videoSourcePlayer.Start();

            // reset stop watch
            //stopWatch = null;

            // start timer
            //timer.Start();

            this.Cursor = Cursors.Default;
        }

        // On new video frame
        private void videoSourcePlayer_NewFrame(object sender, ref Bitmap image)
        {
            if (image.PixelFormat == PixelFormat.Format8bppIndexed)
            {
                // convert image to RGB if it is grayscale
                var filter = new GrayscaleToRGB();

                var temp = filter.Apply(image);
                image.Dispose();
                image = temp;
            }

            lock (_sync)
            {
                var glyphs = _imageProcessor.ProcessImage(image);
            }
        }

        // Export glyphs
        private void exportGlyphDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() != DialogResult.OK)
                return;

            var imageFormat = ImageFormat.Jpeg;
            var imageWidth = 600;

            var path = fbd.SelectedPath;
            foreach (var glyph in _glyphDatabase)
            {
                var fileName = string.Format("{0}/{1}.{2}", path, glyph.Name, imageFormat);
                glyph.ToImage(imageWidth).Save(fileName, imageFormat);
            }

            // opens the folder in explorer
            Process.Start("explorer.exe", path);
        }
    }
}
