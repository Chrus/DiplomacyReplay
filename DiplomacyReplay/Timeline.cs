using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomacyReplay
{
    internal class Timeline
    {
        public Timeline(DipMap map, int startYear)
        {
            Years.Add(new DipYear(startYear));
            
        }

        public DipYear MostRecentYear { get; private set; }
        public DipYear SelectedYear { get; private set; }

        public Turn MostRecentTurn { get; private set; }
        public Turn SelectedTurn { get; private set; }

        public ObservableCollection<DipYear> Years { get; private set; }
    }
}
