using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;
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

namespace DiplomacyReplay
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public HomePage HomePage { get { return Home; } }
        public MapPage MapPage { get { return Map; } }
        public TimelinePage TimelinePage { get { return Timeline; } } 
        public AnimationPage AnimationPage { get { return Animation; } }

        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            GoToHomePage();
        }

        private void Home_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            GoToHomePage();
        }

        private void GoToHomePage()
        {
            MainTabControl.SelectedItem = null;
        }
    }
}
