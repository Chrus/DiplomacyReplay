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

namespace DiplomacyReplay
{
    /// <summary>
    /// Interaction logic for MapPage.xaml
    /// </summary>
    public partial class MapPage : Page
    {
        private readonly MainWindow main;

        public DipMap MyMap
        {
            get { return (DipMap)GetValue(MyMapProperty); }
            set { SetValue(MyMapProperty, value); }
        }
        // Using a DependencyProperty as the backing store for MyMap.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyMapProperty =
            DependencyProperty.Register("MyMap", typeof(DipMap), typeof(MapPage), new PropertyMetadata(null));

        public string BackgroundLocation
        {
            get { return (string)GetValue(BackgroundLocationProperty); }
            set { SetValue(BackgroundLocationProperty, value); }
        }
        public static readonly DependencyProperty BackgroundLocationProperty =
            DependencyProperty.Register("BackgroundLocation", typeof(string), typeof(MapPage), new PropertyMetadata("No File Loaded"));

        public MapPage(MainWindow main)
        {

            InitializeComponent();

            MyMap = DipMap.GetTestingMap();
            this.main = main;
            DataContext = MyMap;

            /////
            ///TODO
            //Test code, to remove
            /////
            ///
            canvasElement.Source = MyMap.BackgroundImage;
            BackgroundLocation = "C:\\Users\\Chris\\Desktop\\DipMapC.png";


        }

        private static SKBitmap LoadSkBitmapFromPngFile(string filePath)
        {
            try
            {
                using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                return SKBitmap.Decode(stream);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading image: {ex.Message}");
                return null;
            }
        }
        private static BitmapSource ConvertSkBitmapToBitmapSource(SKBitmap skBitmap) 
        {

            using SKImage skImage = SKImage.FromBitmap(skBitmap);
            // Get the SKImage's data as a byte array
            SKData skData = skImage.Encode();

            // Create a MemoryStream from the SKData
            using var stream = new System.IO.MemoryStream(skData.ToArray());
            // Create and return a BitmapSource from the stream
            return BitmapFrame.Create(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
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

        public void NewMap(DipMap map)
        {
            if (map != null)
                MessageBox.Show("MapPage map not null and is being overwritten");

            MyMap = map;
        }

        public static BitmapSource LoadTestingBackground()
        {
            string path = "C:\\Users\\Chris\\Desktop\\DipMapC.png";
            SKBitmap bit = LoadSkBitmapFromPngFile(path);
            return ConvertSkBitmapToBitmapSource(bit);
        }

        private void LoadImageButton_Click(object sender, RoutedEventArgs e)
        {
            LoadTestingBackground(/*PromptUserForImageLocation()*/);
        }
    }
}
