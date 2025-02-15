using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomacyReplay
{
    internal class Country : Editable
    {
        public Country() 
        {
            SpawnPoints = new DipObservableCollection<SupplyTerritory>(this);
        }

        public string Name
        {
            get { return _name; }
            set
            {
                EditCheck();
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        private string _name;
        
        public SKColor Color 
        {
            get { return _color; }
            set
            {
                EditCheck();
                _color = value;
                OnPropertyChanged(nameof(Color));
            }
        }
        private SKColor _color;

        public DipObservableCollection<SupplyTerritory> SpawnPoints { get; private set; }
        public bool IsSpawnPoint(string territoryName)
        {
            foreach(var territory in SpawnPoints)
            {
                if(territory.Name == territoryName)
                    return true;
            }

            return false;
        }

        public override List<Editable> FinalizeCheck()
        {
            CanFinalize =
                !string.IsNullOrEmpty(Name)
                && Color != SKColor.Empty
                && SpawnPoints.Count > 0;

            if (CanFinalize)
                return [];
            return [this];
        }
    }
}