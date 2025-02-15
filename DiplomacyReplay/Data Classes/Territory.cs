using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static DiplomacyReplay.Territory;

namespace DiplomacyReplay
{
    internal class Territory : Editable
    {
        public enum TERRITORY_TYPE
        {
            UNDEFINED,
            LAND,
            COAST,
            WATER
        }

        public Territory()
        {
            ExtraGarrisons = new DipObservableCollection<ExtraGarrison>(this);
        }

        private string _name;
        /// <summary>
        /// The name that the program will display over the territory.
        /// </summary>
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

        private SKPoint _nameLoc;
        /// <summary>
        /// The position, relative to Map, where Name is placed
        /// </summary>
        public SKPoint NameLoc
        {
            get { return _nameLoc; }
            set
            {
                EditCheck();
                _nameLoc = value;
                OnPropertyChanged(nameof(NameLoc));
            }
        }

        private TERRITORY_TYPE _territoryType;
        public TERRITORY_TYPE TerritoryType
        {
            get { return _territoryType; }
            set
            {
                EditCheck();
                _territoryType = value;
                OnPropertyChanged(nameof(TerritoryType));
            }
        }

        private SKPoint _garrisonLoc;
        /// <summary>
        /// The default position, relative to Map, that troops will be drawn when inside the territory
        /// </summary>
        public SKPoint GarrisonLoc
        {
            get { return _garrisonLoc; }
            set
            { 
                EditCheck();
                _garrisonLoc = value;
                OnPropertyChanged(nameof(GarrisonLoc));
            }
        }

        public DipObservableCollection<ExtraGarrison> ExtraGarrisons { get; private set; }
        public ExtraGarrison GetExtraGarrison(string name)
        {
            foreach(ExtraGarrison x in ExtraGarrisons)
            {
                if(x.Name == name) 
                    return x;
            }
            
            return null;
        }
        public void AddExtraGarrison(string name, SKPoint location)
        {
            EditCheck();

            ExtraGarrisons.Add(new ExtraGarrison(name, location));
        }

        public virtual bool IsSupply {  get { return false; } }

        public override List<Editable> FinalizeCheck()
        {
            bool canF = false;
            canF = !string.IsNullOrEmpty(Name)
                && NameLoc != SKPoint.Empty
                && TerritoryType != TERRITORY_TYPE.UNDEFINED
                && GarrisonLoc != SKPoint.Empty;

            List<Editable> ret = [];
            foreach(ExtraGarrison extra in ExtraGarrisons)
            {
                if(extra.FinalizeCheck().Count != 0)
                    ret.Add(extra);
            }

            if (!canF || ret.Count != 0)
            {
                CanFinalize = false;
                ret.Insert(0, this);
                return ret;
            }

            CanFinalize = true;
            return [];
        }
        public override bool Finalize()
        {
            if (base.Finalize())
            {
                foreach (var x in ExtraGarrisons)
                    x.Finalize();

                return true;
            }
            return false;
        }

        internal class ExtraGarrison : Editable
        {
            public ExtraGarrison(string name, SKPoint location)
            {
                Name = name;
                Location = location;
            }

            private string _name;
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

            private SKPoint _location;
            public SKPoint Location 
            {
                get { return _location; }
                set
                {
                    EditCheck();

                    _location = value;
                    OnPropertyChanged(nameof(Location));
                }
            }

            public override List<Editable> FinalizeCheck()
            {
                CanFinalize =
                    !string.IsNullOrEmpty(Name) &&
                    Location != SKPoint.Empty;

                if (CanFinalize)
                    return [];
                else
                    return [this];
            }
        }
    }
}