using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
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

        private void RemoveTerritoryButton_Click(object sender, RoutedEventArgs e)
        {
            var i = TerList.SelectedItem as Territory;
            if (i != null)
            {
                int index = TerList.SelectedIndex;

                GetMap().RemoveTerritory(i);

                if (!TerList.HasItems)
                    return;

                if (TerList.Items.Count == 1)
                    TerList.SelectedIndex = 0;
                else if (index == TerList.Items.Count)
                    TerList.SelectedIndex = index - 1;
                else
                    TerList.SelectedIndex = index;
            }
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

                    TerList.SelectedIndex = TerList.Items.Count - 1;
                    return;
                }
            }
        }

        private void TypeButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender == LandBut)
                ((Territory)TerList.SelectedItem).TerritoryType = Territory.TERRITORY_TYPE.LAND;
            else if (sender == CoastBut)
                ((Territory)TerList.SelectedItem).TerritoryType = Territory.TERRITORY_TYPE.COAST;
            else
                ((Territory)TerList.SelectedItem).TerritoryType = Territory.TERRITORY_TYPE.WATER;
        }

        private void NameBox_LostFocus(object sender, RoutedEventArgs e)
        {
            UpdateTerritoryName();
        }
        private void NameBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                UpdateTerritoryName();
        }
        private void UpdateTerritoryName()
        {
            //Since the Name is also used as the key for the territory
            //we have to update DipMap.Territories map key too
            //Instead of just relying on the Binding Mode=TwoWay

            var ter = TerList.SelectedItem as Territory;
            if (ter != null)
            {
                if (NameBox.Text == ter.Name)
                    return;

                //Dont allow an emptry string to be accepted
                //Dont allow the user to input a key that already exists
                //Reset the textbox also so the user knows it didnt work
                if (NameBox.Text == "" ||
                    GetMap().Territories.ContainsKey(NameBox.Text))
                {
                    NameBox.Text = ter.Name;
                    return;
                }

                string old = ter.Name;
                ter.Name = NameBox.Text;
                GetMap().UpdateTerritoryKey(old, NameBox.Text);

                TerList.UnselectAll();
                TerList.Items.Refresh();
                TerList.SelectedItem = ter;
            }
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
                newTerritory.AddGarrisonLoc(x.Key, x.Value);
            }

            GetMap().AddTerritory(newTerritory);
            TerList.SelectedIndex = oldIndex;
        }

        private void AddExtraGarrisonBut_Click(object sender, RoutedEventArgs e)
        {
            var ter = TerList.SelectedItem as Territory;
            if (ter != null)
            {
                for (int x = 1; ; x++)
                {
                    //There was already a location added that wasnt set.  Use that instead.
                    if (ter.ExtraGarrisons.ContainsKey("Extra Garrison" + x) &&
                        ter.GetGarrisonLoc("Extra Garrison" + x) == SKPoint.Empty)
                    {
                        var existing = new KeyValuePair<string, SKPoint>("Extra Garrison" + x, SKPoint.Empty);
                        int index = ter.ExtraGarrisonsToList.IndexOf(
                            new KeyValuePair<string, SKPoint>("Extra Garrison" + x, SKPoint.Empty));
                        ExtraGarListBox.SelectedIndex = index;
                        return;
                    }
                    else if(!ter.ExtraGarrisons.ContainsKey("Extra Garrison" + x))
                    { 
                        ter.AddGarrisonLoc("Extra Garrison" + x, SKPoint.Empty);
                        int i = TerList.SelectedIndex;
                        TerList.UnselectAll();
                        TerList.SelectedIndex = i;

                        return;
                    }
                }
            }
        }

        private void RemExtraGarrisonBut_Click(object sender, RoutedEventArgs e)
        {
            if (ExtraGarListBox.SelectedItem == null)
                return;

            var item = (KeyValuePair<string, SKPoint>)ExtraGarListBox.SelectedItem;
            var ter = TerList.SelectedItem as Territory;
            if (ter != null)
            {
                ter.RemoveGarrisonLoc(item.Key);

                int i = TerList.SelectedIndex;
                TerList.UnselectAll();
                TerList.SelectedIndex = i;
            }
        }

        private void ExtraGarrisonTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var x = sender as TextBox;
            if (x != null)
                UpdateExtraGarrisonName(x);
        }

        private void ExtraGarrisonTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                var x = sender as TextBox;
                if (x != null)
                    UpdateExtraGarrisonName(x);
            }
        }
        private void UpdateExtraGarrisonName(TextBox xGarrisonTextBox)
        {
            var ter = TerList.SelectedItem as Territory;        
            if (ter != null && ExtraGarListBox.SelectedItem != null) 
            {
                KeyValuePair<string, SKPoint> gar = (KeyValuePair<string, SKPoint>)ExtraGarListBox.SelectedItem;
                if (xGarrisonTextBox.Text == gar.Key)
                    return;
                //Territory wont allow a "" to be added anyways.
                //Dont allow the user to input a key that already exists
                //but reset the textbox also so the user knows it didnt work
                if (xGarrisonTextBox.Text == ""
                    || ter.ExtraGarrisons.ContainsKey(xGarrisonTextBox.Text))
                {
                    xGarrisonTextBox.Text = gar.Key;
                    return;
                }

                string old = gar.Key;
                ter.UpdateExtraGarrisonKey(old, xGarrisonTextBox.Text);

                int i = TerList.SelectedIndex;
                TerList.UnselectAll();
                TerList.SelectedIndex = i;
            }
        }

        private void ExtraGarrisonTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if(textBox != null) 
            {
                var item = VisualTreeHelper.GetParent(textBox);
                while (item is not ListBoxItem)
                    item = VisualTreeHelper.GetParent(item);

                ExtraGarListBox.SelectedItem = ((ListBoxItem)item).DataContext;
            }
        }

        //Need to update extra garrison location changes manually 
        private void ExtraGarrisonLocSel_LostMouseCapture(object sender, MouseEventArgs e)
        {
            var sel = sender as LocationSelector;
            var ter = TerList.SelectedItem as Territory;

            if (sel != null && ter != null)
            {
                string key = sel.LocationName;
                if (ter.ExtraGarrisons[key] != sel.SelectedLocation)
                {
                    //AddGarrisonLoc overrides the existing location for that key
                    ter.AddGarrisonLoc(key, sel.SelectedLocation);
                }
            }
        }
    }
}
