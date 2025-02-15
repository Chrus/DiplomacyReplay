using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
    internal class DipMap : Editable
    {
        public DipMap() 
        {
            Territories = new DipObservableCollection<Territory>(this);
            Territories.CollectionChanged += Territories_CollectionChanged;
            Countries = new DipObservableCollection<Country>(this);
        }

        private void Territories_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach(var ter in e.OldItems)
                {
                    if (ter is SupplyTerritory)
                    {
                        //Make sure to remove this territory from Country.SpawnPoints too
                        foreach (Country c in Countries)
                        {
                            c.SpawnPoints.Remove((SupplyTerritory)ter);
                        }
                    }
                }
            }
        }

        public DipObservableCollection<Territory> Territories { get; private set; }
        public Territory GetTerritory(string name)
        {
            foreach(var territory in Territories)
            {
                if (territory.Name == name)
                    return territory;
            }
            return null;
        }

        public DipObservableCollection<Country> Countries {  get; private set; }
        public Country GetCountry(string name)
        {
            foreach(var country in Countries)
            {
                if(country.Name == name)
                    return country;
            }
            return null;
        }

        private SKBitmap _backgroundImage;
        public SKBitmap BackgroundImage
        {
            get { return _backgroundImage; }
            set
            {
                EditCheck();
                _backgroundImage = value;
                OnPropertyChanged(nameof(BackgroundImage));
            }
        }

        private string _backgroundLocation;
        public string BackgroundLocation
        {
            get { return _backgroundLocation; }
            set 
            {
                EditCheck();
                _backgroundLocation = value;
                OnPropertyChanged(nameof(BackgroundLocation));
            }
        }

        public override List<Editable> FinalizeCheck()
        {
            List<Editable> ret = [];

            if (BackgroundImage == null)
                ret.Add(this);

            foreach(var terr in Territories)
                ret.AddRange(terr.FinalizeCheck());
            foreach (var country in Countries)
                ret.AddRange(country.FinalizeCheck());

            CanFinalize = ret.Count == 0 ? true : false;
            return ret;
        }
        public override bool Finalize()
        {
            if(base.Finalize())
            {
                Territories.All(ter => ter.Finalize());
                Countries.All(cou => cou.Finalize());
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
                //Territory territory = new SupplyTerritory();
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
                    territory.AddExtraGarrison("sc" + x, new SKPoint(x * 5 + 5, 130 + (x * 4)));
                    territory.AddExtraGarrison("sc2" + x, new SKPoint(x * 5 + 5, 130 + (x * 4 + 10)));
                }

                if (x % 3 == 0)
                    ((SupplyTerritory)territory).SupplyLocation = new SKPoint(x * 5 + 5, 125);

                map.Territories.Add(territory);
            }

            for (int x = 1; x < 4; x++)
            {
                Country country = new Country();
                country.Name = "Country" + x;
                country.Color = new SKColor((byte)(300 / x), (byte)(270 / x), (byte)(160 / x), 255);

                int count = 0;
                foreach (var ter in map.Territories)
                {
                    if (ter is SupplyTerritory)
                    {
                        count++;
                        if(count == x)
                            country.SpawnPoints.Add((SupplyTerritory)ter);
                    }
                }

                map.Countries.Add(country);
            }

            map.BackgroundLocation = "C:\\Users\\Chris\\Desktop\\DipMapC.png";
            SKBitmap bit = SKBitmap.Decode(map.BackgroundLocation);
            map.BackgroundImage = bit;

            map.FinalizeCheck();
            if (!map.CanFinalize)
                MessageBox.Show("Something in the Testing DipMap cant be finalized");

            if(finalize)
                map.Finalize();
            
            return map;
        }
    }
}
