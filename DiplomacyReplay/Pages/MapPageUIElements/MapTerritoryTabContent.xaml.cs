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

            foreach(var ter in GetMap().Territories)
            {
                results.AddRange(ter.FinalizeCheck());
            }

            if (results.Count == 0)
            {
                foreach( var ter in GetMap().Territories)
                {
                    ter.Finalize();
                }

                NotFinalizedPane.Visibility = Visibility.Collapsed;
                FinalizedPane.Visibility = Visibility.Visible;
            }
        }

        private void AddTerritoryButton_Click(object sender, RoutedEventArgs e)
        {
            for (int x = 1; ; x++)
            {
                if (GetMap().GetTerritory("Territory" + x) == null)
                {
                    Territory ter = new() { Name = "Territory" + x };
                    GetMap().Territories.Add(ter);

                    TerList.SelectedIndex = TerList.Items.Count - 1;
                    return;
                }
            }
        }
        private void RemoveTerritoryButton_Click(object sender, RoutedEventArgs e)
        {
            var i = TerList.SelectedItem as Territory;
            if (i != null)
            {
                int index = TerList.SelectedIndex;

                GetMap().Territories.Remove(i);

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

        private void TypeButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender == LandBut)
                ((Territory)TerList.SelectedItem).TerritoryType = Territory.TERRITORY_TYPE.LAND;
            else if (sender == CoastBut)
                ((Territory)TerList.SelectedItem).TerritoryType = Territory.TERRITORY_TYPE.COAST;
            else
                ((Territory)TerList.SelectedItem).TerritoryType = Territory.TERRITORY_TYPE.WATER;
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
                newTerritory.AddExtraGarrison(x.Name, x.Location);
            }

            GetMap().Territories.Remove(oldTerritory);
            GetMap().Territories.Insert(oldIndex, newTerritory);
            TerList.SelectedIndex = oldIndex;
        }

        private void AddExtraGarrisonBut_Click(object sender, RoutedEventArgs e)
        {
            var ter = TerList.SelectedItem as Territory;
            if (ter != null)
                ter.AddExtraGarrison("", SKPoint.Empty);
        }

        private void RemExtraGarrisonBut_Click(object sender, RoutedEventArgs e)
        {
            if (ExtraGarListBox.SelectedItem == null)
                return;

            var item = (Territory.ExtraGarrison)ExtraGarListBox.SelectedItem;
            var ter = TerList.SelectedItem as Territory;
            if (ter != null)
                ter.ExtraGarrisons.Remove(item);
        }

        //More of a quality of life addition because the textbox is so big.  When it is selected tell the listbox to select that item.
        private void ExtraGarrisonTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null)
            {
                var item = VisualTreeHelper.GetParent(textBox);
                while (item is not ListBoxItem)
                    item = VisualTreeHelper.GetParent(item);

                ExtraGarListBox.SelectedItem = ((ListBoxItem)item).DataContext;
            }
        }


    }
}
