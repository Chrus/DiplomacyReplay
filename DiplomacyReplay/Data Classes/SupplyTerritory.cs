using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomacyReplay
{
    internal class SupplyTerritory : Territory
    {
        public SupplyTerritory() : base() { }

        private SKPoint _supplyLocation;
        public SKPoint SupplyLocation
        {
            get { return _supplyLocation; }
            set
            {
                EditCheck();
                _supplyLocation = value;
                OnPropertyChanged(nameof(SupplyLocation));
            }
        }

        //Used by .xaml stuff so I dont need to make a million visibility converters
        public override bool IsSupply { get { return true; } }

        public override List<Editable> FinalizeCheck()
        {
            var ret = base.FinalizeCheck();

            if(SupplyLocation == SKPoint.Empty) 
            {
                CanFinalize = false;
                if (ret.Count == 0) //base already added 'this' if it had a failure, dont add a copy
                    ret.Add(this);

                return ret;
            }

            return ret;
        }
    }
}
