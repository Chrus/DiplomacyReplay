using System;
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
    class WidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SidebarControl x = (SidebarControl)value;
            int tabs = x.Items.Count;
            return (x.Width / tabs);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Interaction logic for DipPage.xaml
    /// </summary>
    public partial class DipPage : Page
    {
        public DipPage()
        {
            InitializeComponent();
        }

        //private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    if((int)testCanvas.GetValue(Grid.ColumnSpanProperty) == 1)
        //        testCanvas.SetValue(Grid.ColumnSpanProperty, 2);
        //    else
        //        testCanvas.SetValue(Grid.ColumnSpanProperty, 1);
        //}
    }
}
