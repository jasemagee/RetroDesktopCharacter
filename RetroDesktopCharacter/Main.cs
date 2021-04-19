using System;
using System.IO;
using System.Windows.Forms;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;

namespace RetroDesktopCharacter
{
    // https://stackoverflow.com/questions/22979232/how-to-design-a-cool-semi-transparent-splash-screen
    // https://stackoverflow.com/questions/2664754/how-to-make-a-non-rectangular-winform
    public partial class Main : Form
    {
        private System.Drawing.Image[] _images;
        private int _frameIndex = 0;
        private int _estTaskHeight = 36;

        public Main()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            using var image = Image.Load(@"C:\Users\Jason\Desktop\anim3.gif");


            _images = new System.Drawing.Image[image.Frames.Count];

            for (int i = 0; i < image.Frames.Count; i++)
            {
                Image<Rgba32> currentFrame = (Image<Rgba32>)image.Frames.CloneFrame(i);

     

                _images[i] = currentFrame.ToBitmap();

               

                Width = _images[i].Width;
                Height = _images[i].Height;
            }
        }


        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            Location = new System.Drawing.Point(Location.X, 1440 - Height - _estTaskHeight);

            TopLevel = true;
            Focus();
            TopMost = true;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
/*            Invoke(new Action(() =>
            {
                Location = new System.Drawing.Point(Location.X + 1, Location.Y);
            }));*/
        }

        private void timerAnim_Tick(object sender, EventArgs e)
        {
            Invoke(new Action(() =>
            {
                if (_frameIndex >= _images.Length)
                {
                    _frameIndex = 0;
                }

                BackgroundImage = _images[_frameIndex++];
            }));

        }
    }

    public static class ImageSharpExtensions
    {
        public static Byte[] ToArray<TPixel>(this Image<TPixel> image, IImageFormat imageFormat) where TPixel : unmanaged, IPixel<TPixel>
        {
            using (var memoryStream = new MemoryStream())
            {
                var imageEncoder = image.GetConfiguration().ImageFormatsManager.FindEncoder(imageFormat);
                image.Save(memoryStream, imageEncoder);
                return memoryStream.ToArray();
            }
        }

        public static System.Drawing.Bitmap ToBitmap<TPixel>(this Image<TPixel> image) where TPixel : unmanaged, IPixel<TPixel>
        {
            using (var memoryStream = new MemoryStream())
            {
                var imageEncoder = image.GetConfiguration().ImageFormatsManager.FindEncoder(PngFormat.Instance);
                image.Save(memoryStream, imageEncoder);

                memoryStream.Seek(0, SeekOrigin.Begin);

                return new System.Drawing.Bitmap(memoryStream);
            }
        }

        public static Image<TPixel> ToImageSharpImage<TPixel>(this System.Drawing.Bitmap bitmap) where TPixel : unmanaged, IPixel<TPixel>
        {
            using (var memoryStream = new MemoryStream())
            {
                bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);

                memoryStream.Seek(0, SeekOrigin.Begin);

                return Image.Load<TPixel>(memoryStream);
            }
        }
    }

}
