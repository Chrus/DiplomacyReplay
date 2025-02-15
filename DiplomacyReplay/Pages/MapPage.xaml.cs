using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SkiaSharp;
using SkiaSharp.Views.WPF;
using SkiaSharp.Views;
using System.Runtime.InteropServices.ComTypes;
using System.Diagnostics.Eventing.Reader;

namespace DiplomacyReplay
{
    /// <summary>
    /// Interaction logic for MapPage.xaml
    /// </summary>
    public partial class MapPage : Page
    {
        internal DipMap MyMap
        {
            get { return (DipMap)GetValue(MyMapProperty); }
            set 
            {
                SetValue(MyMapProperty, value);
                DataContext = value;
                mapCanvas.InvalidateVisual();
            }
        }
        // Using a DependencyProperty as the backing store for MyMap.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyMapProperty =
            DependencyProperty.Register("MyMap", typeof(DipMap), typeof(MapPage), new PropertyMetadata(null));

        public MapPage()
        {
            InitializeComponent();
        }

        private void CloseBackgroundButton_Click(object sender, RoutedEventArgs e)
        {
            MyMap.BackgroundLocation = "No File Loaded";
            MyMap.BackgroundImage = null;
            mapCanvas.InvalidateVisual();
        }
        private void LoadImageButton_Click(object sender, RoutedEventArgs e)
        {
            string path = PromptUserForImageLocation();
            if (path == null)
                return;

            SKBitmap image = SKBitmap.Decode(path);
            if (image != null)
            {
                MyMap.BackgroundImage = image;
                MyMap.BackgroundLocation = path;
            }
        }
        private string PromptUserForImageLocation()
        {
            string path = string.Empty;
            Microsoft.Win32.OpenFileDialog fileDialog = new()
            {
                Filter = "PNG Files (*.png)|*.png"
            };

            if (fileDialog.ShowDialog() == true)
            {
                path = fileDialog.FileName;
                if (!File.Exists(path) || !path.EndsWith(".png"))
                {
                    MessageBox.Show("Error in PromptUserForImageLocation(): " + path);
                    return null;
                }
            }
            return path;
        }

        private void mapCanvas_PaintSurface(object sender, SkiaSharp.Views.Desktop.SKPaintSurfaceEventArgs e)
        {        
            if(MyMap == null)
                return;

            if (MyMap.BackgroundImage == null)
            {
                e.Surface.Canvas.Clear();
                return;
            }

            var canvas = e.Surface.Canvas;
            canvas.Clear();

            var dpi = VisualTreeHelper.GetDpi(this);

            float sc = (float)(dpi.PixelsPerInchX / 96);

            canvas.DrawBitmap(MyMap.BackgroundImage,
                new SKRect(0,0,MyMap.BackgroundImage.Width, MyMap.BackgroundImage.Height));

            mapCanvas.Width = MyMap.BackgroundImage.Width / sc;
            mapCanvas.Height = MyMap.BackgroundImage.Height / sc;

            var pp = new SKPoint(pointToDraw.X * sc, pointToDraw.Y * sc);
            canvas.DrawCircle(pp, 5, new SKPaint() { Color = SKColors.Red });
        }

        //temp code for testing
        public void tempDrawTarget(SKPoint point)
        {
            pointToDraw = point;
            mapCanvas.InvalidateVisual();
        }
        private SKPoint pointToDraw = SKPoint.Empty;
    }
}
