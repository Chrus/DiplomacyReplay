using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DiplomacyReplay
{
    internal class DipYear : Editable
    {
        private DipYear(int year) { Year = year; }
        public static DipYear FromNewGame(DipMap map, int startYear)
        {
            DipYear year = new(startYear);
            AttackTurn turn = new(year);
            year.Spring = turn;

            turn.Season = SEASON.SPRING;
            foreach(Country c in map.Countries)
            {
                //foreach(SupplyTerritory supply in c.SpawnPoints)

                //TODO
            }

            return year;

        }

        public int Year { get; private set; }
        public Turn Spring { get; private set; }
        public Turn Fall { get; private set; }
        public Turn Winter { get; private set; }

        public Turn ActiveTurn()
        {
            //if (Finalized)
                return null;


        }




        public override bool Finalize()
        {
            throw new NotImplementedException();
        }

        public override List<Editable> FinalizeCheck()
        {
            throw new NotImplementedException();
        }
    }
}
