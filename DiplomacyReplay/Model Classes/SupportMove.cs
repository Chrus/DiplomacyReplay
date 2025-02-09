using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomacyReplay
{
    internal class SupportMove : Move
    {
        public enum SUPPORT_TYPE
        {
            UNDEFINED = default,
            HOLD,
            MOVE,
            CONVOY
        }

        public SupportMove() { MoveType = MOVE_TYPE.SUPPORT; }
        public SupportMove(Troop troop, Territory startTerritory, Territory endTerritory, MOVE_TYPE moveType, bool successful,
            Troop supportedTroop, SUPPORT_TYPE supportType)
            : base(troop,startTerritory,endTerritory,moveType,successful)
        {
            SupportedTroop = supportedTroop;
            SupportType = supportType;
        }

        public override MOVE_TYPE MoveType 
        {
            get => base.MoveType; 
            set
            {
                if (value != MOVE_TYPE.SUPPORT)
                    throw new InvalidOperationException("The base MoveType of a SupportMove must be MOVE_TYPE.SUPPORT");
            }
        }

        private Troop _supportedTroop;
        public Troop SupportedTroop
        {
            get => _supportedTroop; 
            set
            {
                EditCheck();
                _supportedTroop = value; 
                OnPropertyChanged(nameof(SupportedTroop));
            }
        }

        private SUPPORT_TYPE _supportType;
        public SUPPORT_TYPE SupportType
        {
            get => _supportType;
            set 
            {
                EditCheck();
                _supportType = value; 
                OnPropertyChanged(nameof(SupportType));
            }
        }

        public override void FinalizeCheck()
        {
            CanFinalize =
                Troop != null
                && StartTerritory != null
                && EndTerritory != null
                && MoveType == MOVE_TYPE.SUPPORT
                && Successful != null
                && SupportedTroop != null
                && SupportType != SUPPORT_TYPE.UNDEFINED;
        }
    }
}
