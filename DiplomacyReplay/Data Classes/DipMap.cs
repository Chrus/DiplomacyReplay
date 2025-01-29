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
                && _territories.Count > 0;
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
    }


}
