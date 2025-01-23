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
    public class SidebarControl : TabControl
    {
        public Visibility ContentVisibility
        {
            get { return (Visibility)GetValue(ContentVisibilityProperty); }
            set { SetValue(ContentVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ContentVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentVisibilityProperty =
            DependencyProperty.Register("ContentVisibility", typeof(Visibility), typeof(SidebarControl), new PropertyMetadata(Visibility.Visible));

        static SidebarControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SidebarControl), new FrameworkPropertyMetadata(typeof(SidebarControl)));
        }

        public SidebarControl()
        {
            SelectionChanged += (s, e) => 
            {
                if(this.SelectedItem != null) { ContentVisibility = Visibility.Visible; }
            };
        }
    }
}
