using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomacyReplay
{
    internal class Timeline : Editable
    {
        public Timeline() { Years = []; }
        public static Timeline FromNewGame(DipMap map, int startYear)
        {
            Timeline time = new Timeline();

            time.Years.Add(DipYear.FromNewGame(map, startYear));

            return time;
        }

        public DipYear MostRecentYear { get; private set; }
        public DipYear SelectedYear { get; private set; }

        public Turn MostRecentTurn { get; private set; }
        public Turn SelectedTurn { get; private set; }

        public ObservableCollection<DipYear> Years { get; private set; }

        public int StartYear { get { return Years?.FirstOrDefault()?.Year ?? 0; } }




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
