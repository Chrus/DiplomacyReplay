using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomacyReplay
{
    public class Troop
    {
        public enum TROOP_TYPE
        {
            UNDEFINED,
            ARMY,
            FLEET
        }

        public Troop(Country owner, TROOP_TYPE type)
        {
            this.owner = owner.Name;
            this.type = type;
        }
        public Troop(string owner, TROOP_TYPE type)
        {
            this.owner = owner;
            this.type = type;
        }

        public readonly string owner;
        public readonly TROOP_TYPE type;
    }
}
