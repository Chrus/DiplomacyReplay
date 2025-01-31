using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using SkiaSharp;

namespace DiplomacyReplay
{
    public class DipMap
    {
        public DipMap()
        {
            IsEditable = true;
            _territories = [];
            _countries = [];
        }

        private readonly Dictionary<string, Territory> _territories;
        public IReadOnlyDictionary<string, Territory> Territories
        {
            get { return _territories; }
        }
        public void AddTerritory(Territory territory)
        {
            if (!IsEditable)
                throw new InvalidOperationException("Map has been finalized and cannot be edited");
            
            if (_territories.ContainsKey(territory.Name))
                _territories[territory.Name] = territory;
            else
                _territories.Add(territory.Name, territory);
        }

        private readonly Dictionary<string, Country> _countries;
        public IReadOnlyDictionary<string, Country> Countries
        {
            get { return _countries; }
        }
        public void AddCountry(Country country)
        {
            if (!IsEditable)
                throw new InvalidOperationException("Map has been finalized and cannot be edited");

            if (_countries.ContainsKey(country.Name))
                _countries[country.Name] = country;
            else
                _countries.Add(country.Name, country);
        }

        public BitmapSource BackgroundImage
        {
            get { return _backgroundImage; }
            set
            {
                if (!IsEditable)
                    throw new InvalidOperationException("Map has been finalized and cannot be edited");
                _backgroundImage = value;
            }
        }
        private BitmapSource _backgroundImage;

        public bool IsEditable { get; private set; }
        public virtual bool CanFinalize()
        {
            return BackgroundImage != null
                && _countries.Count > 0
                && _territories.Count > 0
                && Territories.All(x => x.Value.CanFinalize())
                && Countries.All(x => x.Value.CanFinalize());
        }
        public bool Finalize()
        {
            if(CanFinalize())
            {
                IsEditable = false;
                return true;
            }
            return false;
        }

        public static DipMap GetTestingMap()
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
                    territory.TerritoryType = Territory.TERRITORY_TYPE.OCEAN;

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
                country.Color = new SKColor((byte)(300 / x), (byte)(270 / x), (byte)(160 / x), 100);
                country.AddSpawnPoint(map.Territories["Territory" + x].Name);

                map.AddCountry(country);
            }

            map.BackgroundImage = MapPage.LoadTestingBackground();

            var xtdfsa = map.CanFinalize();

            return map;
        }
    }


}
