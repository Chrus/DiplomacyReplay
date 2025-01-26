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
    public class SidebarItem : TabItem
    {
        static SidebarItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SidebarItem), new FrameworkPropertyMetadata(typeof(SidebarItem)));
        }
        public SidebarItem()
        {
            PreviewMouseDown += SidebarItem_PreviewMouseDown;
        }

        //Collapse the Content Panel if clicking the tab that is already selected.  Otherwise make sure to show it
        private void SidebarItem_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var control = VisualTreeHelper.GetParent(this);
            while (control != null && !(control is SidebarControl))
            {
                control = VisualTreeHelper.GetParent(control);
            }

            if (control != null)
            {
                SidebarControl x = control as SidebarControl;
                if (IsSelected && x.ContentVisibility == Visibility.Visible)
                    x.ContentVisibility = Visibility.Collapsed;
                else if (x.ContentVisibility == Visibility.Collapsed || x.ContentVisibility == Visibility.Hidden)
                    x.ContentVisibility = Visibility.Visible;
            }
        }
    }
}
