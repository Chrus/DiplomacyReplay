using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
    /// <summary>
    /// Interaction logic for LocationSelector.xaml
    /// </summary>
    public partial class LocationSelector : UserControl
    {
        public LocationSelector()
        {
            InitializeComponent();
        }

        public bool TextFirst
        {
            get { return (bool)GetValue(TextFirstProperty); }
            set { SetValue(TextFirstProperty, value); }
        }
        public static readonly DependencyProperty TextFirstProperty =
            DependencyProperty.Register("TextFirst", typeof(bool), typeof(LocationSelector), new PropertyMetadata(true));

        public SKPoint SelectedLocation
        {
            get { return (SKPoint)GetValue(SelectedLocationProperty); }
            set
            {
                SetValue(SelectedLocationProperty, value);                
            }
        }
        private static void OnSelectedLocationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as LocationSelector;
            if (control != null)
            {
                control.LocationText = control.UpdateLocationText(); 
            }
        }
        public static readonly DependencyProperty SelectedLocationProperty =
            DependencyProperty.Register("SelectedLocation", typeof(SKPoint), typeof(LocationSelector), new PropertyMetadata(new SKPoint(), OnSelectedLocationChanged));

        public bool CanEdit
        {
            get { return (bool)GetValue(CanEditProperty); }
            set 
            {
                SetValue(CanEditProperty, value);
                UpdateLocationText();
            }
        }
        public static readonly DependencyProperty CanEditProperty =
            DependencyProperty.Register("CanEdit", typeof(bool), typeof(LocationSelector), new PropertyMetadata(true));

        public string LocationName { get; set; }
        public string LocationText
        {
            get { return (string)GetValue(LocationTextProperty); }
            set { SetValue(LocationTextProperty, value); }
        }
        public static readonly DependencyProperty LocationTextProperty =
            DependencyProperty.Register("LocationText", typeof(string), typeof(LocationSelector), new PropertyMetadata("Unset"));

        private string UpdateLocationText()
        {
            if (CanEdit)
            {
                if (SelectedLocation == SKPoint.Empty)
                    return "No Set Location";
                else
                    return LocationName + "<" + SelectedLocation.X + "," + SelectedLocation.Y + ">";
            }
            else
            {
                if (SelectedLocation == SKPoint.Empty)
                    return LocationName + "<,>";
                else
                    return LocationName + "<" + SelectedLocation.X + "," + SelectedLocation.Y + ">";
            }
        }

        private void HighlightBut_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SelectBut_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
