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
        private HomePage home;
        private MapPage map;
        private TimelinePage timeline;
        private AnimationPage animation;

        public HomePage HomePage { get { return home; } }
        public MapPage MapPage { get { return map; } }
        public TimelinePage TimelinePage { get { return timeline; } } 
        public AnimationPage AnimationPage { get { return animation; } }    

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            home = new HomePage(this);
            map = new MapPage(this);
            timeline = new TimelinePage(this);
            animation = new AnimationPage(this);

            HomeFrame.Content = HomePage;
            MapFrame.Content = MapPage;
            TimelineFrame.Content = TimelinePage;
            AnimationFrame.Content = AnimationPage;

            GoToHomePage();
        }

        //Listeners
        private void Home_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            GoToHomePage();
        }

        //Private Functions
        private void GoToHomePage()
        {
            MainTabControl.SelectedItem = null;
        }
    }
}
