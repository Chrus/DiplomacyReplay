using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class GetTerritoryLocationConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            
            var territories = values[1] as Dictionary<string,Territory>.ValueCollection;
            string name = values[0] as string;
            if (territories != null && name != null)
            {
                foreach(Territory t in territories)
                {
                    if (t.Name == name)
                        return t.GarrisonLoc;
                }
            }

            return SKPoint.Empty;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Interaction logic for MapCountryTabContent.xaml
    /// </summary>
    public partial class MapCountryTabContent : UserControl
    {
        public MapCountryTabContent()
        {
            InitializeComponent();

            var availableColors = new ObservableCollection<ColorItem>
            {
                new(Color.FromArgb(255,204, 0, 0), "Backstabbr_Red"),
                new(Color.FromArgb(255,0,0,170), "Backstabbr_DarkBlue"),
                new(Color.FromArgb(255,153,153,255), "Backstabbr_LightBlue"),
                new(Color.FromArgb(255,0,0,0), "Backstabbr_Black"),
                new(Color.FromArgb(255,0,170,0), "Backstabbr_Green"),
                new(Color.FromArgb(255,187,0,187), "Backstabbr_Purple"),
                new(Color.FromArgb(255,187,187,0), "Backstabbr_Yellow")
            };
            colorPicker.AvailableColors = availableColors;
        }

        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var contex = e.NewValue as DipMap;
            if (contex == null)
            {
                NotFinalizedPane.Visibility = Visibility.Collapsed;
                FinalizedPane.Visibility = Visibility.Collapsed;
            }
            else if (contex.IsEditable)
            {
                NotFinalizedPane.Visibility = Visibility.Visible;
                FinalizedPane.Visibility = Visibility.Collapsed;
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
            if (GetMap().CanFinalize())
            {
                GetMap().Finalize();
                NotFinalizedPane.Visibility = Visibility.Collapsed;
                FinalizedPane.Visibility = Visibility.Visible;
            }
        }

        private void CountryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var c = CountryList.SelectedItem as Country;
            if (c == null)
                return;

            List<Territory> selected = new List<Territory>();
            foreach (string item in c.SpawnPoints)
            {
                if (GetMap().Territories.ContainsKey(item))
                    selected.Add(GetMap().Territories[item]);
            }

            spawnSelector.SelectedItemsOverride = selected;
        }

        private void AddCountryButton_Click(object sender, RoutedEventArgs e)
        {
            for(int i = 1; ; i++) 
            {
                if (!GetMap().Countries.ContainsKey("Country" + i))
                {
                    Country c = new Country();
                    c.Name = "Country" + i;
                    GetMap().AddCountry(c);

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

                GetMap().RemoveCountry(country);

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

        private void NameBox_LostFocus(object sender, RoutedEventArgs e)
        {
            UpdateCountryName();
        }

        private void NameBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                UpdateCountryName();
        }

        private void UpdateCountryName()
        {
            //Since the Name is also used as the key for the country
            //we have to update DipMap.Countries map key too
            //Instead of just relying on the Binding Mode=TwoWay
            var country = CountryList.SelectedItem as Country;
            if (country != null)
            {
                if (NameBox.Text == country.Name)
                    return;

                //Dont allow an emptry string to be added
                //Dont allow the user to input a key that already exists
                //Reset the textbox also so the user knows it didnt work
                if (NameBox.Text == ""
                    || GetMap().Countries.ContainsKey(NameBox.Text))
                {
                    NameBox.Text = country.Name;
                    return;
                }

                string old = country.Name;
                country.Name = NameBox.Text;
                GetMap().UpdateCountryKey(old, NameBox.Text);

                CountryList.UnselectAll();
                CountryList.Items.Refresh();
                CountryList.SelectedItem = country;
            }
        }

        private void spawnSelector_Closed(object sender, RoutedEventArgs e)
        {
            //Update Country.SpawnPoints manually with new spawn points
            var updated = spawnSelector.SelectedItems;
            List<string> newSpawnPoints = new List<string>();

            foreach (Territory terr in updated)
            {
                newSpawnPoints.Add(terr.Name);
            }
            ((Country)CountryList.SelectedItem).SpawnPoints = newSpawnPoints;

            //Update UI visuals, spawnSelector.SelectedItemsOverride and selectedSpawns
            var country = CountryList.SelectedItem;
            CountryList.UnselectAll();
            CountryList.Items.Refresh();
            CountryList.SelectedItem = country;
        }
    }
}
