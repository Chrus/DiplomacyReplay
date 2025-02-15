using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomacyReplay
{
    internal class Troop
    {
        public enum TROOP_TYPE
        {
            UNDEFINED = default,
            ARMY,
            FLEET
        }

        public Troop(Country owner, TROOP_TYPE type)
        {
            Owner = owner;
            TroopType = type;
        }

        public Country Owner { get; }
        public TROOP_TYPE TroopType { get; }
    }
}
