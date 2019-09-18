using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;

namespace RenderDemo.Pages
{
    public class WriteableBitmapPage : UserControl
    {
        int _frame = 0;
        private WriteableBitmap _bitmap;
        private int _scroll = 0;
         public WriteableBitmapPage()
        {
            this.InitializeComponent();
            CreateImage();
        }
        private void CreateImage()
        {
            int width = 512;
            int height = 512;
            _bitmap = new WriteableBitmap(new PixelSize(width, height), new Vector(96, 96), PixelFormat.Bgra8888);

            unsafe
            {
                using (var pixels = _bitmap.Lock())
                {
                    for (int y = 0; y < height; y++)
                    {
                        uint* p = (uint*)pixels.Address + y * pixels.RowBytes / 4;
                        for (int x = 0; x < width; x++)
                        {
                            uint bri = (uint)((x * y) % 256);
                            *(p++) = 0xff000000U + (bri << 16) + (bri << 8) + bri;
                        }
                    }
                }
            }
        }
        public override void Render(DrawingContext context)
        {
            int scale = 1;
            context.DrawImage(_bitmap, 1.0,
                         new Rect(0, _scroll, _bitmap.PixelSize.Width/2, _bitmap.PixelSize.Height/2),
                         new Rect(0, 0, _bitmap.PixelSize.Width/2 , _bitmap.PixelSize.Height/2));

            _scroll = (_scroll + 1) % 256;
           

            if ((_frame & 1) == 0)
            {
                context.FillRectangle(Brushes.Cyan, new Rect(300, 0, 100, 10));
            }
            else
            {
                context.FillRectangle(Brushes.Red, new Rect(300, 0, 100, 10));
            }
            _frame++;
            Dispatcher.UIThread.InvokeAsync(InvalidateVisual, DispatcherPriority.Background);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
