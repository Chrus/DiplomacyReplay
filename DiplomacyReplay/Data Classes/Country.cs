using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomacyReplay
{
    public class Country
    {
        public Country() 
        {
            IsEditable = true;
            spawnPoints = new List<string>();
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (!IsEditable)
                    throw new InvalidOperationException("Country is finalized and cant be edited");
                _name = value;
            }
        }
        private string _name;
        
        public SKColor Color 
        {
            get { return _color; }
            set
            {
                if (!IsEditable)
                    throw new InvalidOperationException("Country is finalized and cant be edited");
                _color = value;
            }
        }
        private SKColor _color;
        
        public bool IsEditable {  get; private set; }
        public virtual bool CanFinalize()
        {
            return Name != null
                && Color != SKColor.Empty
                && spawnPoints.Count > 0;
        }
        public bool Finalize()
        {
            if(CanFinalize())
            {
                IsEditable = false;
                return true;
            }
            return false;
        }

        private List<string> spawnPoints { get; set; }
        public bool IsSpawnPoint(string territoryName)
        {
            return spawnPoints.Contains(territoryName);
        }
        public void AddSpawnPoint(string territoryName)
        {
            if (!IsEditable)
                throw new InvalidOperationException("Country is finalized and cannot be edited");
            if(!spawnPoints.Contains(territoryName))
                spawnPoints. Add(territoryName);
        }
    }
}