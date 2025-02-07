using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using SkiaSharp;

namespace DiplomacyReplay
{
    public class DipMap : DependencyObject
    {
        public DipMap()
        {
            IsEditable = true;
        }

        public static readonly DependencyProperty TerritoriesProperty =
            DependencyProperty.Register("Territories", typeof(Dictionary<string, Territory>), typeof(DipMap), new PropertyMetadata(new Dictionary<string, Territory>()));
        public Dictionary<string, Territory> Territories
        {
            get { return (Dictionary<string, Territory>)GetValue(TerritoriesProperty); }
        }
        public void AddTerritory(Territory territory)
        {
            finalizedCheck();
            
            var t = new Dictionary<string, Territory>(Territories);

            if (t.ContainsKey(territory.Name))
                t[territory.Name] = territory;
            else
                t.Add(territory.Name, territory);

            SetValue(TerritoriesProperty, t);
        }     
        public void RemoveTerritory(Territory territory)
        {
            finalizedCheck();

            var t = new Dictionary<string, Territory>(Territories);

            t.Remove(territory.Name);
            SetValue(TerritoriesProperty, t);

            //Make sure to remove this territory from Country.SpawnPoints too
            foreach(Country c in Countries.Values)
            {
                c.RemoveSpawnPoint(territory.Name);
            }
        }
        public void UpdateTerritoryKey(string oldKey, string newKey)
        {
            finalizedCheck();
            if (!Territories.ContainsKey(oldKey)
                || Territories.ContainsKey(newKey))
                return;

            var ter = Territories[oldKey];
            Territories.Remove(oldKey);
            Territories.Add(newKey, ter);

            //Make sure to update this territory in Country.SpawnPoints too
            foreach(Country c in Countries.Values)
            {
                if(c.IsSpawnPoint(oldKey))
                {
                    c.RemoveSpawnPoint(oldKey);
                    c.AddSpawnPoint(newKey);
                }
            }
        }


        public static readonly DependencyProperty CountriesProperty =
            DependencyProperty.Register("Countries", typeof(Dictionary<string, Country>), typeof(DipMap), new PropertyMetadata(new Dictionary<string,Country>()));
        public Dictionary<string, Country> Countries
        {
            get { return (Dictionary<string, Country>)GetValue(CountriesProperty); }
        }
        public void AddCountry(Country country)
        {
            finalizedCheck();

            var c = new Dictionary<string, Country>(Countries);

            if (c.ContainsKey(country.Name))
                c[country.Name] = country;
            else
                c.Add(country.Name, country);

            SetValue(CountriesProperty, c);
        }
        public void RemoveCountry(Country country)
        {
            finalizedCheck();

            var c = new Dictionary<string, Country>(Countries);

            c.Remove(country.Name);
            SetValue(CountriesProperty, c);
        }
        public void UpdateCountryKey(string oldKey, string newKey)
        {
            finalizedCheck();
            if (!Countries.ContainsKey(oldKey)
                || Countries.ContainsKey(newKey))
                return;

            var c = Countries[oldKey];
            Countries.Remove(oldKey);
            Countries.Add(newKey, c);
        }

        public SKBitmap BackgroundImage// BitmapSource BackgroundImage
        {
            get { return _backgroundImage; }
            set
            {
                finalizedCheck();
                _backgroundImage = value;
            }
        }
        private SKBitmap _backgroundImage; // BitmapSource _backgroundImage;
        public string BackgroundLocation
        {
            get { return (string)GetValue(BackgroundLocationProperty); }
            set 
            {
                finalizedCheck();
                SetValue(BackgroundLocationProperty, value); 
            }
        }
        public static readonly DependencyProperty BackgroundLocationProperty =
            DependencyProperty.Register("BackgroundLocation", typeof(string), typeof(MapPage), new PropertyMetadata("No File Loaded"));

        public bool IsEditable { get; private set; }
        public virtual bool CanFinalize()
        {
            //TODO Remove temps
            //var temp = Territories.All(x => x.Value.CanFinalize());
            //var temp2 = Countries.All(x => x.Value.CanFinalize());

            return BackgroundImage != null
                && Countries.Count > 0
                && Territories.Count > 0
                && Territories.All(x => x.Value.CanFinalize())
                && Countries.All(x => x.Value.CanFinalize());
        }
        protected void finalizedCheck()
        {
            if (!IsEditable)
                throw new InvalidOperationException("Territory is finalized and can't be edited");
        }
        public bool Finalize()
        {
            if(CanFinalize())
            {  
                Territories.All(ter => ter.Value.Finalize());
                Countries.All(con => con.Value.Finalize());

                IsEditable = false;
                return true;
            }
            return false;
        }

        public static DipMap GetTestingMap(bool finalize)
        {
            DipMap map = new DipMap();

            for (int x = 1; x < 15; x++)
            {
                Territory territory = (x % 3 == 0) ? new SupplyTerritory() : new Territory();
                territory.Name = "Territory" + x;
                territory.NameLoc = new SKPoint(x * 5 + 5, 50);
                territory.GarrisonLoc = new SKPoint(x * 5 + 5, 100);

                if (x % 3 == 0)
                    territory.TerritoryType = Territory.TERRITORY_TYPE.LAND;
                else if (x % 2 == 0)
                    territory.TerritoryType = Territory.TERRITORY_TYPE.COAST;
                else
                    territory.TerritoryType = Territory.TERRITORY_TYPE.WATER;

                if (x % 4 == 0)
                {
                    territory.AddGarrisonLoc("sc" + x, new SKPoint(x * 5 + 5, 130 + (x * 4)));
                    territory.AddGarrisonLoc("sc2" + x, new SKPoint(x * 5 + 5, 130 + (x * 4 + 10)));
                }

                if (x % 3 == 0)
                    ((SupplyTerritory)territory).SupplyLocation = new SKPoint(x * 5 + 5, 125);

                map.AddTerritory(territory);
            }

            for (int x = 1; x < 5; x++)
            {
                Country country = new Country();
                country.Name = "Country" + x;
                country.Color = new SKColor((byte)(300 / x), (byte)(270 / x), (byte)(160 / x), 255);
                country.AddSpawnPoint(map.Territories["Territory" + x].Name);
                country.AddSpawnPoint(map.Territories["Territory" + (x+1)].Name);

                map.AddCountry(country);
            }

            map.BackgroundLocation = "C:\\Users\\Chris\\Desktop\\DipMapC.png";
            SKBitmap bit = SKBitmap.Decode(map.BackgroundLocation);
            map.BackgroundImage = bit;

            var xtdfsa = map.CanFinalize();
            if (!xtdfsa)
                MessageBox.Show("Something in the Testing DipMap cant be finalized");

            if(finalize)
                map.Finalize();
            
            return map;
        }
    }


}
