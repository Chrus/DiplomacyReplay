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
    /// Interaction logic for MapPage.xaml
    /// </summary>
    public partial class MapPage : Page
    {
        private MainWindow main;
        private DipMap map;
        public MapPage(MainWindow main)
        {
            this.main = main;
            InitializeComponent();
        }

        public void NewMap(DipMap map)
        {
            if (map != null)
                throw new Exception("MapPage map not null and is being overwritten");

            this.map = map;
        }
    }
}
