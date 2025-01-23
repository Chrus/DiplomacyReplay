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

namespace DiplomacyReplay
{
    /// <summary>
    /// Interaction logic for Sidebar.xaml
    /// </summary>
    public partial class Sidebar : UserControl
    {
        public Sidebar()
        {
            InitializeComponent();

            SidebarItem item1 = new SidebarItem("TEST1");
            sidebar.Items.Add(item1);
            sidebar.Items.Add(new SidebarItem("test2"));
            sidebar.Items.Add(new SidebarItem("test500"));

            Loaded += Sidebar_Loaded;
        }

        private void Sidebar_Loaded(object sender, RoutedEventArgs e)
        {
            sidebar.ContentVisibility = Visibility.Collapsed;
            sidebar.SelectedItem = null;
        }
    }
}
