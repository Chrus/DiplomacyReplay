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
            _spawnPoints = new List<string>();
        }

        public string Name
        {
            get { return _name; }
            set
            {
                finalizedCheck();
                _name = value;
            }
        }
        private string _name;
        
        public SKColor Color 
        {
            get { return _color; }
            set
            {
                finalizedCheck();
                _color = value;
            }
        }
        private SKColor _color;

        public List<string> SpawnPoints 
        {
            get { return _spawnPoints; }
            set
            {
                finalizedCheck();
                _spawnPoints = value;
            }
        }
        private List<string> _spawnPoints { get; set; }
        public bool IsSpawnPoint(string territoryName)
        {
            return _spawnPoints.Contains(territoryName);
        }
        public void AddSpawnPoint(string territoryName)
        {
            finalizedCheck();

            if (!_spawnPoints.Contains(territoryName))
                _spawnPoints.Add(territoryName);
        }
        public void RemoveSpawnPoint(string territoryName)
        {
            finalizedCheck();

            if(_spawnPoints.Contains(territoryName))
                _spawnPoints.Remove(territoryName);
        }

        public bool IsEditable {  get; private set; }
        public virtual bool CanFinalize()
        {
            return Name != null
                && Color != SKColor.Empty
                && _spawnPoints.Count > 0;
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

        protected void finalizedCheck()
        {
            if (!IsEditable)
                throw new InvalidOperationException("Territory is finalized and can't be edited");
        }
    }
}