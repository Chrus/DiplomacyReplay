using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Metrics;
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
using Xceed.Wpf.AvalonDock.Controls;
using Xceed.Wpf.Toolkit;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace DiplomacyReplay
{
    public class Xceed_SKColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var skcol = (SKColor)value;

            return System.Windows.Media.Color.FromArgb(skcol.Alpha, skcol.Red, skcol.Green, skcol.Blue);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var col = ((System.Windows.Media.Color)value);

            return new SKColor(col.R, col.G, col.B, col.A);
        }
    }
    public class SKColorToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var skcol = (SKColor)value;
            var col = System.Windows.Media.Color.FromArgb(skcol.Alpha, skcol.Red, skcol.Green, skcol.Blue);
            return new SolidColorBrush(col);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Interaction logic for MapCountryTabContent.xaml
    /// </summary>
    public partial class MapCountryTabContent : UserControl
    {
        public CollectionViewSource SupplyColView { get; set; } = new CollectionViewSource();
        public MapCountryTabContent()
        {
            InitializeComponent();

            colorPicker.AvailableColors =
            [
                new(Color.FromArgb(255,204, 0, 0), "Backstabbr_Red"),
                new(Color.FromArgb(255,0,0,170), "Backstabbr_DarkBlue"),
                new(Color.FromArgb(255,153,153,255), "Backstabbr_LightBlue"),
                new(Color.FromArgb(255,0,0,0), "Backstabbr_Black"),
                new(Color.FromArgb(255,0,170,0), "Backstabbr_Green"),
                new(Color.FromArgb(255,187,0,187), "Backstabbr_Purple"),
                new(Color.FromArgb(255,187,187,0), "Backstabbr_Yellow")
            ];

            SupplyColView.Filter += FilterTerritories;
        }

        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var context = e.NewValue as DipMap;
            if (context == null)
            {
                NotFinalizedPane.Visibility = Visibility.Collapsed;
                FinalizedPane.Visibility = Visibility.Collapsed;
            }
            else if (!context.Finalized)
            {
                NotFinalizedPane.Visibility = Visibility.Visible;
                FinalizedPane.Visibility = Visibility.Collapsed;

                SupplyColView.Source = GetMap().Territories;
            }
            else
            {
                NotFinalizedPane.Visibility = Visibility.Collapsed;
                FinalizedPane.Visibility = Visibility.Visible;
            }
        }

        private DipMap GetMap()
        {
            var x = DataContext as DipMap;
            if (x == null)
                throw new NullReferenceException("DataContext not set to a DipMap");

            return x;
        }

        private void FinalizeButton_Click(object sender, RoutedEventArgs e)
        {
            var results = new List<Editable>();

            foreach (var country in GetMap().Countries)
            {
                results.AddRange(country.FinalizeCheck());
            }

            if (results.Count == 0)
            {
                foreach (var country in GetMap().Countries)
                {
                    country.Finalize();
                }

                NotFinalizedPane.Visibility = Visibility.Collapsed;
                FinalizedPane.Visibility = Visibility.Visible;
            }
        }
        private void FilterTerritories(object sender, FilterEventArgs e)
        {
            //Filter out if not a supply territory
            if (e.Item is SupplyTerritory supply)
            {
                //Filter out if its already selected by a country
                foreach (Country country in GetMap().Countries)
                {
                    if (country.IsSpawnPoint(supply.Name))
                    {
                        e.Accepted = false;
                        return;
                    }
                }
                e.Accepted = true;
                return;
            }
            e.Accepted = false;
        }

        private void AddCountryButton_Click(object sender, RoutedEventArgs e)
        {
            for(int i = 1; ; i++) 
            {
                if (GetMap().GetCountry("Country" + i) == null)
                {
                    Country c = new() { Name = "Country" + i };
                    GetMap().Countries.Add(c);

                    CountryList.SelectedIndex = CountryList.Items.Count - 1;
                    return;
                }
            }
        }

        private void RemoveCountryButton_Click(object sender, RoutedEventArgs e)
        {
            var country = CountryList.SelectedItem as Country;
            if (country != null)
            {
                int index = CountryList.SelectedIndex;

                GetMap().Countries.Remove(country);

                if (!CountryList.HasItems)
                    return;

                if (CountryList.Items.Count == 1)
                    CountryList.SelectedIndex = 0;
                else if (index == CountryList.Items.Count)
                    CountryList.SelectedIndex = index - 1;
                else
                    CountryList.SelectedIndex = index;
            }
        }

        private void AddSpawnPointBtn_Click(object sender, RoutedEventArgs e)
        {
            var spawns = AvailableSpawns.SelectedItems;
            var country = CountryList.SelectedItem as Country;

            if(spawns == null || country == null)
                return;

            foreach (SupplyTerritory co in spawns)
            {
                country.SpawnPoints.Add(co);
            }

            AvailableSpawns.UnselectAll();
            SupplyColView.View.Refresh();
        }

        private void RemoveSpawnPointBtn_Click(object sender, RoutedEventArgs e)
        {
            //need to create a copy because otherwise we're looping through the list being modified
            var copy = CountrySpawns.SelectedItems.Cast<SupplyTerritory>().ToList();
            var country = CountryList.SelectedItem as Country;

            if (copy == null || country == null)
                return;

            foreach(SupplyTerritory co in copy)
            {
                country.SpawnPoints.Remove(co);
            }

            CountrySpawns.UnselectAll();
            SupplyColView.View.Refresh();
        }
    }
}
