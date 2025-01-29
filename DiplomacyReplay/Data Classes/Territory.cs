using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DiplomacyReplay
{
    public class Territory
    {
        public enum TERRITORY_TYPE
        {
            UNDEFINED,
            LAND,
            COAST,
            OCEAN
        }

        #region 
//Constructors//

        public Territory()
        {
            IsEditable = true;
            extraGarrisons = [];
        }

        #endregion
        #region
//Properties//

        public bool IsEditable { get; private set; }

        /// <summary>
        /// The name that the program will display over the territory.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                finalizedCheck();
                _name = value;
            }
        }

        /// <summary>
        /// The position, relative to Map, where Name is placed
        /// </summary>
        public SKPoint NameLoc
        {
            get { return _nameLoc; }
            private set
            {
                finalizedCheck();
                _nameLoc = value;
            }
        }

        public TERRITORY_TYPE TerritoryType
        {
            get { return _territoryType; }
            private set
            {
                finalizedCheck();
                _territoryType = value;
            }
        }

        /// <summary>
        /// The default position, relative to Map, that troops will be drawn when inside the territory
        /// </summary>
        public SKPoint GarrisonLoc
        {
            get { return _garrisonLoc; }
            set
            {
                finalizedCheck();
                _garrisonLoc = value;
            }
        }

        #endregion
        #region
//Public Functions//

        /// <summary>
        /// Get a position, relative to Map, for a garrisoned troop to be drawn when inside the territory.  Used 
        /// when a territory has multiple garrison locations, such having multiple coasts.  
        /// </summary>
        /// <param name="tag">The identifier used for an extra garrison location.  An empty string will return the default 
        /// garrison location (GarrisonLoc)</param>
        /// <returns>The SKPoint where a garrisoned troop should be drawn, relative to Map.  Returns SKPoint.Empty if
        /// there is no match</returns>
        public SKPoint GetGarrisonLoc(string tag)
        {
            if (tag == "")
                return GarrisonLoc;
            else
            {
                foreach (Tuple<string, SKPoint> x in extraGarrisons)
                {
                    if (x.Item1 == tag)
                        return x.Item2;
                }
            }

            return SKPoint.Empty;
        }

        /// <summary>
        /// Add a location for a troop to be drawn, relative to Map, when it is inside the territory.  Throws an error if Territory !IsEditable
        /// </summary>
        /// <param name="tag">The identifier used for that garrison location.  An empty tag denotes the default location (GarrisonLoc)</param>
        /// <param name="loc">The location for a troop to be drawn, relative to Map</param>
        /// <returns>If tag was already used, override that tag and return the old location.  Otherwise return SKPoint.Empty</returns>
        public SKPoint AddGarrisonLoc(string tag, SKPoint loc)
        {
            //error checking
            if (loc == SKPoint.Empty)
                throw new ArgumentException("loc cannot be SKPoint.Empty", nameof(loc));
            finalizedCheck();

            //empty string denotes the default location.  Dont add it to the extra garrisons
            if (tag == "")
            {
                var old = GarrisonLoc;
                GarrisonLoc = loc;
                return old;
            }

            Tuple<string, SKPoint> temp = null;
            foreach (Tuple<string, SKPoint> x in extraGarrisons)
            {
                if (x.Item1 == tag)
                {
                    temp = x;
                    break;
                }
            }

            //tag was already used.  override location and return old
            if (temp != null)
            {
                extraGarrisons.Remove(temp);
                extraGarrisons.Add(new Tuple<string, SKPoint>(tag, loc));
                return temp.Item2;
            }
            else //tag wasnt used.  
            {
                extraGarrisons.Add(new Tuple<string, SKPoint>(tag, loc));
                return SKPoint.Empty;
            }
        }

        public virtual bool CanFinalize()
        {
            return Name != null
                && NameLoc != SKPoint.Empty
                && TerritoryType != TERRITORY_TYPE.UNDEFINED
                && GarrisonLoc != SKPoint.Empty;
        }

        /// <summary>
        /// Attempt to finalize Territory, trying to edit properties in the future will throw an error
        /// </summary>
        /// <returns>True if IsFinalizable() == true and IsEditable is set to false.  Otherwise return false</returns>
        public bool Finalize()
        {
            if (CanFinalize())
            {
                IsEditable = false;
                return true;
            }
            return false;
        }

        #endregion
        #region
//Private Variables//

        private string _name;
        private SKPoint _nameLoc;
        private TERRITORY_TYPE _territoryType;
        private SKPoint _garrisonLoc;
        //Non default garrison locations.  GarrisonLoc, the default loc, is not in here
        private readonly List<Tuple<string, SKPoint>> extraGarrisons;

        #endregion
        #region
//Private Functions//

        protected void finalizedCheck()
        {
            if (!IsEditable)
                throw new InvalidOperationException("Territory is finalized and can't be edited");
        }

        #endregion






        //Old code when I was doing edit mode differently. Remove if I end up never needing to switch back
        // TODO 

        /// <summary>
        /// Set the default garrison location for a troop to be draw, relative to Map, when it is inside the territory.  
        /// Throws an error if Territory !IsEditable
        /// </summary>
        /// <param name="loc">The default garrison location</param>
        /// <returns>If GarrisonLoc was overwritten return the overwritten value.  Otherwise return SKPoint.Empty</returns>
        //public SKPoint SetGarrisonLoc(SKPoint loc)
        //{
        //    finalizedCheck();

        //    if (GarrisonLoc == null)
        //    {
        //        GarrisonLoc = loc;
        //        return SKPoint.Empty;
        //    }
        //    else
        //    {
        //        var temp = GarrisonLoc;
        //        GarrisonLoc = loc;
        //        return (SKPoint)temp;
        //    }
        //}


        /// <summary>
        /// Set DisplayName.  Throws an error if Territory !IsEditable
        /// </summary>
        /// <param name="name">The new DisplayName</param>
        /// <returns>If DisplayName was overwritten return the overwritten value.  Otherwise return String.Empty</returns>
        //public string SetDisplayName(string name)
        //{
        //    finalizedCheck();

        //    if (Name == null)
        //    {
        //        Name = name;
        //        return String.Empty;
        //    }
        //    else
        //    {
        //        var temp = Name;
        //        Name = name;
        //        return temp;
        //    }
        //}

        /// <summary>
        /// Set NameLoc.  Throws an error if Territory !IsEditable
        /// </summary>
        /// <param name="point">The new NameLoc</param>
        /// <returns>If NameLoc was overwritten return the overwritten value.  Otherwise return SKPoint.Empty</returns>
        //public SKPoint SetNameLoc (SKPoint point)
        //{
        //    finalizedCheck();

        //    if(NameLoc == null)
        //    {
        //        NameLoc = point;
        //        return SKPoint.Empty;
        //    }
        //    else
        //    {
        //        SKPoint temp = (SKPoint)NameLoc;
        //        NameLoc = point;
        //        return temp;
        //    }
        //}

        /// <summary>
        /// Set TerritoryType.  Throws an error if Territory !IsEditable
        /// </summary>
        /// <param name="type">The new TerritoryType</param>
        /// <returns>If TerritoryType was overwritten return the overwritten value.  Otherwise return TYPE.INVALID</returns>
        //public TERRITORY_TYPE SetTerritoryType(TERRITORY_TYPE type)
        //{
        //    finalizedCheck();

        //    if(TerritoryType == TERRITORY_TYPE.UNDEFINED)
        //    {
        //        TerritoryType = type;
        //        return TERRITORY_TYPE.UNDEFINED;
        //    }
        //    else
        //    {
        //        var temp = TerritoryType;
        //        TerritoryType = type;
        //        return (TERRITORY_TYPE)temp;
        //    }

    }
}