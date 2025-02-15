using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomacyReplay
{
    internal class DipObservableCollection<T>(Editable parent) : ObservableCollection<T>()
    {
        public readonly Editable Parent = parent;

        protected override void ClearItems()
        {
            Parent.EditCheck();

            base.ClearItems();
        }

        protected override void InsertItem(int index, T item)
        {
            Parent.EditCheck();

            base.InsertItem(index, item);
        }

        protected override void RemoveItem(int index)
        {
            Parent.EditCheck();

            base.RemoveItem(index);
        }

        protected override void SetItem(int index, T item)
        {
            Parent.EditCheck();

            base.SetItem(index, item);
        }
    }
}
