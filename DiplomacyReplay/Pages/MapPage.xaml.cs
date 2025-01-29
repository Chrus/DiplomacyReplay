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
        private DipMap map;

        public string BackgroundLocation
        {
            get { return (string)GetValue(BackgroundLocationProperty); }
            set { SetValue(BackgroundLocationProperty, value); }
        }
        public static readonly DependencyProperty BackgroundLocationProperty =
            DependencyProperty.Register("BackgroundLocation", typeof(string), typeof(MapPage), new PropertyMetadata("No File Loaded"));

        public MapPage(MainWindow main)
        {
            this.main = main;
            InitializeComponent();

            DataContext = this;

            /////
            //Test code, to remove
            /////
            LoadBackground("C:\\Users\\Chris\\Desktop\\DipMapC.png");
        }

        private SKBitmap LoadSkBitmapFromPngFile(string filePath)
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
        private BitmapSource ConvertSkBitmapToBitmapSource(SKBitmap skBitmap) 
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

            this.map = map;
        }

        private void LoadBackground(string path)
        {
            SKBitmap bit = LoadSkBitmapFromPngFile(path);
            map.BackgroundImage = ConvertSkBitmapToBitmapSource(bit);

            canvasElement.Source = map.BackgroundImage;
            BackgroundLocation = path;

        }

        private void LoadImageButton_Click(object sender, RoutedEventArgs e)
        {
            LoadBackground(PromptUserForImageLocation());
        }
    }
}
