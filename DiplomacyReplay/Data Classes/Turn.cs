using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomacyReplay
{
    public enum SEASON
    {
        UNPLAYED = default,
        SPRING,
        FALL,
        WINTER                    
    }

    internal abstract class Turn : Editable
    {
        public Turn(DipYear year)
        {
            Year = year;
            Season = SEASON.UNPLAYED;
            Moves = [];
            SupplyOwners = [];
        }

        public DipYear Year { get; set; }
        public SEASON Season { get; set; }
        public ObservableCollection<Move> Moves { get; private set; }

        public ObservableCollection<SupplyOwner> SupplyOwners { get; private set; }
        public SupplyOwner GetSupplyOwner(SupplyTerritory territory)
        {
            foreach(SupplyOwner x in SupplyOwners)
            {
                if (x.Territory == territory)
                    return x;
            }

            return null;
        }
        public int SupplyCount(Country country)
        {
            int ret = 0;
            foreach (SupplyOwner y in SupplyOwners)
                if (y.Country == country)
                    ret++;

            return ret;
        }
        public List<SupplyTerritory> GetOwnedSupplies(Country country)
        {
            List<SupplyTerritory> ret = [];
            foreach (SupplyOwner x in SupplyOwners)
                if (x.Country == country)
                    ret.Add(x.Territory);

            return ret;
        }


        public override bool Finalize()
        {
            throw new NotImplementedException();
        }
        public override List<Editable> FinalizeCheck()
        {
            throw new NotImplementedException();
        }

        public class SupplyOwner(SupplyTerritory territory, Country country)
        {
            public SupplyTerritory Territory { get; private set; } = territory;
            public Country Country { get; private set; } = country;
        }
    }
}
