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
                //only call OnPropertyChanged if its actually been changed
                if (value != _canFinalize)
                {
                    _canFinalize = value;
                    OnPropertyChanged(nameof(CanFinalize));
                }
            }
        }

        /// <summary>
        /// Evaluates whether the object is ready to be finalized. This method updates the `CanFinalize` property 
        /// but does not invoke the `Finalize()` method.
        /// </summary>
        /// <returns>
        /// Returns a list of `Editable` objects that failed the finalization check, including this object and any 
        /// other `Editable` objects held by it that did not pass the `FinalizeCheck()` criteria.
        /// </returns>
        public abstract List<Editable> FinalizeCheck();

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

        public virtual bool Finalize()
        {
            FinalizeCheck();
            if (CanFinalize)
            {
                Finalized = true;
                return true;
            }
            return false;
        }

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
