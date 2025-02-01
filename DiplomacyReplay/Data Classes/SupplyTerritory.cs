using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomacyReplay
{
    public class SupplyTerritory : Territory
    {
        public SupplyTerritory() : base() { }

        private SKPoint _supplyLocation;
        public SKPoint SupplyLocation
        {
            get { return _supplyLocation; }
            set
            {
                finalizedCheck();
                _supplyLocation = value;
            }
        }

        //Used by .xaml stuff so I dont need to make a million visibility converters
        public override bool IsSupply { get { return true; } }

        public override bool CanFinalize()
        {
            return base.CanFinalize()
                && SupplyLocation != SKPoint.Empty;
        }
    }
}
