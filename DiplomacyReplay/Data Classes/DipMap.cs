using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using SkiaSharp;

namespace DiplomacyReplay
{
    public class DipMap
    {
        public enum MAP_STATE
        {
            EDIT,
            FINAL
        }

        public MAP_STATE state = MAP_STATE.EDIT;
        public BitmapSource bitmapSource;
        public DipMap() 
        {

        }
    }


}
