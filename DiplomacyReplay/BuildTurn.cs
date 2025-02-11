using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomacyReplay
{
    internal class BuildTurn : Turn
    {
        public BuildTurn(int year) : base(year) { }

        public void SupplyChange(SupplyTerritory supply, Country newOwner)
        {
            if(SupplyOwners.ContainsKey(supply))
                SupplyOwners[supply] = newOwner;
            else
                SupplyOwners.Add(supply, newOwner);
        }
    }
}