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

namespace DiplomacyReplay
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        private string DipMapSaveLocation
        {
            get { return (string)GetValue(DipMapSaveLocationProperty); }
            set { SetValue(DipMapSaveLocationProperty, value); }
        }
        public static readonly DependencyProperty DipMapSaveLocationProperty =
            DependencyProperty.Register("DipMapSaveLocation", typeof(string), typeof(HomePage), new PropertyMetadata("No File Loaded"));

        public bool MapLoaded
        {
            get { return (bool)GetValue(MapLoadedProperty); }
            set 
            {
                SetValue(MapLoadedProperty, value);
                GetWindow().MapTab.IsEnabled = value;
            }
        }
        public static readonly DependencyProperty MapLoadedProperty =
            DependencyProperty.Register("MapLoaded", typeof(bool), typeof(HomePage), new PropertyMetadata(false));

        //bool timelineLoaded = false;

        public HomePage()
        {
            InitializeComponent();
            DataContext = this;
        }
        private MainWindow GetWindow()
        {
            return Window.GetWindow(this) as MainWindow;
        }

        private void NewMapButton_Click(object sender, RoutedEventArgs e)
        {
            GetWindow().MapPage.MyMap = new DipMap();
            DipMapSaveLocation = "New Map";
            MapLoaded = true;
        }

        private void LoadMapButton_Click(object sender, RoutedEventArgs e)
        {
            //TODO

            /*Promp User for save location*/
            DipMapSaveLocation = PromptUserForMapFile();

            /*Load DipMap from save location*/
            GetWindow().MapPage.MyMap = LoadDipMapFromFile(DipMapSaveLocation);
            MapLoaded = true;
        }
        private string PromptUserForMapFile()
        {
            //Temp, for Testing
            return "A DipMap File Location";

            //Microsoft.Win32.OpenFileDialog fileDialog = new() { };
            //string path = "";
            //if (fileDialog.ShowDialog() == true)
            //{
            //    path = fileDialog.FileName;
            //    if (!File.Exists(path))
            //    {
            //        MessageBox.Show("Error in Loading Location " + path);
            //        return "" or throw error here?;
            //    }
            //}
            //return path;
        }
        private DipMap LoadDipMapFromFile(string fileLocation)
        {
            DipMap dip = new DipMap();
            //Complex loading logic here

            //Temp, for Testing
            dip = DipMap.GetTestingMap(false);

            return dip;
        }

        private void CloseMapButton_Click(object sender, RoutedEventArgs e)
        {
            //TODO
            //Check if unsaved

            GetWindow().MapPage.MyMap = null;
            MapLoaded = false;
        }
        private void SaveMapButton_Click(object sender, RoutedEventArgs e)
        {
            //TODO
        }
        private void SaveAsMapButton_Click_1(object sender, RoutedEventArgs e)
        {
            //TODO
        }
    }
}
