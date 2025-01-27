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
    public class SidebarControl : TabControl
    {
        static SidebarControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SidebarControl), new FrameworkPropertyMetadata(typeof(SidebarControl)));
        }
        public Visibility ContentVisibility
        {
            get { return (Visibility)GetValue(ContentVisibilityProperty); }
            set { SetValue(ContentVisibilityProperty, value); }
        }
        public static readonly DependencyProperty ContentVisibilityProperty =
            DependencyProperty.Register("ContentVisibility", typeof(Visibility), typeof(SidebarControl), new PropertyMetadata(Visibility.Hidden));

        public SidebarControl()
        {
            //Initialized += SidebarControl_Initialized;
        }

        private void SidebarControl_Initialized(object sender, EventArgs e)
        {
            //var x = Window.GetWindow(this) as Window;
            //if (x != null)
            //{
            //    x.PreviewMouseDown += (o, i) =>
            //    {
            //        if (!IsMouseOver) ContentVisibility = Visibility.Hidden;
            //    };
            //}
        }
    }
}
