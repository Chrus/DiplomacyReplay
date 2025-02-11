using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomacyReplay
{
    internal class AttackTurn : Turn
    {
        public AttackTurn(int year) : base(year)
        {
            Retreats = [];
        }

        public ObservableCollection<Turn> Retreats { get; private set; }

        public void FromPrevious(AttackTurn previous)
        {

        }
        public void FromPrevious(BuildTurn previous) 
        {

        }
    }
}
