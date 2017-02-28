using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace RPGModel
{
    /// <summary>
    /// Main Menu static class
    /// </summary>
    public static class MainMenu
    {

        public static MapData mapData()
        {
            MapData mapData = new MapData();
            mapData.Mapst = new int[12] { 4095, 2508, 3754, 716, 2538, 4095, 0, 1902, 756, 1020, 244, 64 };
            mapData.Monstats = new int[3,7] { { 1, 1, 2, 4, 5, 6, 7 }, { 4, 6, 5, 4, 3, 2, 1}, {3, 9, 2, 4, 3, 2, 1 } };
            mapData.StartnEnd = new int[4] { 6, 11, 5, 7 };
            mapData.itemstats = new int[0,0];
            return mapData;
        }      
    }
}
