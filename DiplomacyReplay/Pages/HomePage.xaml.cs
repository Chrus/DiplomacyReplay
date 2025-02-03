using System;
using System.Collections.Generic;
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
        private readonly MainWindow main;
        //bool mapLoaded = false;
        //bool timelineLoaded = false;

        public HomePage(MainWindow main)
        {
            InitializeComponent();
            
            this.main = main;
        }

        private void NewMapButton_Click(object sender, RoutedEventArgs e)
        {
            main.MapPage.NewMap(new DipMap());
            //mapLoaded = true;
        }
    }
}
