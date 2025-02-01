using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class TypeToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SupplyTerritory)
                return Visibility.Visible;
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class ExtraGarrisonVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Territory x = (Territory)value;
            if (x.ExtraGarrisons.Count == 0)
                return Visibility.Collapsed;
            else
                return Visibility.Visible;
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Interaction logic for MapTerritoryTabContent.xaml
    /// </summary>
    public partial class MapTerritoryTabContent : UserControl
    {
        public MapTerritoryTabContent()
        {
            InitializeComponent();
        }

        private DipMap GetMap()
        {
            var x = DataContext as DipMap;
            if (x == null)
                throw new NullReferenceException("DataContext not set to a DipMap");

            return x;
        }

        private void RemoveTerritoryButton_Click(object sender, RoutedEventArgs e)
        {
            var i = TerList.SelectedItem as Territory;
            if(i != null)
                GetMap().RemoveTerritory(i);
        }

        private void AddTerritoryButton_Click(object sender, RoutedEventArgs e)
        {
            for (int x = 1; ; x++)
            {
                if (!GetMap().Territories.Keys.Contains("Territory" + x))
                {
                    var ter = new Territory();
                    ter.Name = "Territory" + x;
                    GetMap().AddTerritory(ter);
                    return;
                }
            }
        }

        private void AddGarrisonBut_Click(object sender, RoutedEventArgs e)
        {
        }

        private void AddRemSupply_Click(object sender, RoutedEventArgs e)
        {
            var oldIndex = TerList.SelectedIndex;
            var oldTerritory = TerList.SelectedItem as Territory;
            var newTerritory = (TerList.SelectedItem is SupplyTerritory)
                ? new Territory()
                : new SupplyTerritory();

            newTerritory.Name = oldTerritory.Name;
            newTerritory.NameLoc = oldTerritory.NameLoc;
            newTerritory.TerritoryType = oldTerritory.TerritoryType;
            newTerritory.GarrisonLoc = oldTerritory.GarrisonLoc;
            foreach (var x in oldTerritory.ExtraGarrisons)
            {
                newTerritory.AddGarrisonLoc(x.Item1, x.Item2);
            }

            GetMap().AddTerritory(newTerritory);
            TerList.SelectedIndex = oldIndex;
        }
    }
}
