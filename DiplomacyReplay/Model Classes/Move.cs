using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomacyReplay
{
    internal class Move : Editable
    {
        public enum MOVE_TYPE
        {
            UNDEFINED = default,
            HOLD,
            MOVE,
            DISBAND,
            SPAWN,
            RETREAT,
            SUPPORT
        }

        public Move() { }
        public Move(Troop troop, Territory startTerritory, Territory endTerritory, MOVE_TYPE moveType, bool successful)
        {
            Troop = troop;
            StartTerritory = startTerritory;
            EndTerritory = endTerritory;
            MoveType = moveType;
            Successful = successful;
        }

        private Troop _troop;
        public Troop Troop 
        {
            get => _troop;
            set
            {
                EditCheck();
                _troop = value;
                OnPropertyChanged(nameof(Troop));
            }
        }

        private Territory _startTerritory;
        public Territory StartTerritory 
        {
            get => _startTerritory;
            set 
            {
                EditCheck();
                _startTerritory = value;
                OnPropertyChanged(nameof(StartTerritory));
            }
        }

        private Territory _endTerritory;
        public Territory EndTerritory
        {
            get => _endTerritory;
            set
            {
                EditCheck();
                _endTerritory = value;
                OnPropertyChanged(nameof(EndTerritory));
            }
        }

        private MOVE_TYPE _moveType;
        public virtual MOVE_TYPE MoveType
        {
            get => _moveType;
            set
            {
                EditCheck();
                _moveType = value;
                OnPropertyChanged(nameof(MoveType));
            }
        }

        private bool? _successful;
        public bool? Successful 
        {
            get => _successful;
            set
            {
                EditCheck();
                _successful = value;
                OnPropertyChanged(nameof(Successful));
            }
        }

        public override bool Finalize()
        {
            FinalizeCheck();
            if(CanFinalize)
            {
                Finalized = true;
                return true;
            }
            return false;
        }
        public override void FinalizeCheck()
        {
            CanFinalize =
                Troop != null
                && StartTerritory != null
                && EndTerritory != null
                && MoveType != MOVE_TYPE.UNDEFINED
                && Successful != null;
        }
    }
}
