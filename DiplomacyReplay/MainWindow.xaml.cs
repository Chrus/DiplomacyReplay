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
        private HomePage homePage;
        private MapPage mapPage;
        private TimelinePage timelinePage;
        private AnimationPage animationPage;

        public HomePage HomePage { get { return homePage; } }
        public MapPage MapPage { get { return mapPage; } }
        public TimelinePage TimelinePage { get { return timelinePage; } } 
        public AnimationPage AnimationPage { get { return animationPage; } }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            homePage = new HomePage(this);
            mapPage = new MapPage(this);
            timelinePage = new TimelinePage(this);
            animationPage = new AnimationPage(this);

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
