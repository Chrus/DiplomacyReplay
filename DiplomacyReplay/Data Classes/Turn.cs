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
        public Turn(int year)
        {
            Year = year;
            Season = SEASON.UNPLAYED;
            Moves = [];
            SupplyOwners = [];
        }

        public int Year { get; set; }
        public SEASON Season { get; set; }
        public ObservableCollection<Move> Moves { get; private set; }
        public ObservableDictionary<SupplyTerritory, Country> SupplyOwners { get; private set; }


        public override bool Finalize()
        {
            throw new NotImplementedException();
        }
        public override void FinalizeCheck()
        {
            throw new NotImplementedException();
        }
    }
}
