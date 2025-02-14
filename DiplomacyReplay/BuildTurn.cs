using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomacyReplay
{
    internal class BuildTurn : Turn
    {
        public BuildTurn(DipYear year) : base(year) { }

        public void SupplyChange(SupplyTerritory supply, Country newOwner)
        {
            if (GetSupplyOwner(supply) == null)
                SupplyOwners.Add(new SupplyOwner(supply, newOwner));
            else
                ReplaceSupplyOwner(supply, newOwner);            
        }

        public void ReplaceSupplyOwner(SupplyTerritory supply, Country newOwner)
        {
            for(int x = 0; x < SupplyOwners.Count; x++)
            {
                if (SupplyOwners[x].Territory == supply)
                    SupplyOwners.RemoveAt(x);
            }

            SupplyOwners.Add(new SupplyOwner(supply,newOwner));
        }
    }
}