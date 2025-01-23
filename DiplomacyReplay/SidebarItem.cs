using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
    public class SidebarItem : TabItem
    {
        static SidebarItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SidebarItem), new FrameworkPropertyMetadata(typeof(SidebarItem)));
        }

        public SidebarItem(string tabName)
        {
            Header = tabName;
            Content = new Rectangle()
            {
                Width = 50,
                Height = 50,
                Fill =  Brushes.Red,
                Margin = new Thickness(5)
            };
        }
    }
}
