using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (Template.FindName("baseGrid", this) is Grid x)
            {
                x.PreviewMouseDown += SidebarItem_PreviewMouseDown;
            }

        }

        //Collapse the Content Panel if clicking the tab that is already selected.  Otherwise make sure to show it
        private void SidebarItem_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {            
            var control = VisualTreeHelper.GetParent(this);
            while (control != null && control is not SidebarControl)
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
