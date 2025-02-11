using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomacyReplay
{
    internal class DipYear(int year)
    {
        public int Year { get; } = year;
        public Turn Spring { get; private set; } = new AttackTurn(year);
        public Turn Fall { get; private set; } = new AttackTurn(year);
        public Turn Winter { get; private set; } = new BuildTurn(year);
    }
}
