using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomacyReplay
{
    internal abstract class Editable : INotifyPropertyChanged
    {
        private bool _canFinalize = false;
        public bool CanFinalize
        {
            get => _canFinalize;
            protected set
            {
                if (value != _canFinalize)
                {
                    _canFinalize = value;
                    OnPropertyChanged(nameof(CanFinalize));
                }
            }
        }
        public abstract void FinalizeCheck();

        private bool _finalized = false;
        public bool Finalized
        {
            get => _finalized;
            protected set
            {
                //only call OnPropertyChanged the first time Finalized is set to true
                if (value && !_finalized)
                {
                    _finalized = value;
                    OnPropertyChanged(nameof(Finalized));
                }
                //Finalize is currently one way, dont allow it to be set back to false
                else if (!value && _finalized)
                    throw new InvalidOperationException("Finalized cannot be reversed once set in " + GetType().ToString());
            }
        }

        public abstract bool Finalize();
        public void EditCheck()
        {
            if (Finalized)
                throw new InvalidOperationException(GetType().ToString() + " is Finalized and can't be edited");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
