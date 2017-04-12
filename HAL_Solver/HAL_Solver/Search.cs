using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HAL_Solver
{
    class Search
    {

        private static Heuristic<Map> h;
        private SortedList<Map, byte> frontier;
        private Collection<Map> explored = new Collection<Map>();

        public Search(Heuristic<Map> nh)
        {
            h = nh;
            frontier = new SortedList<Map, byte>(h);
        }


        public void addToFrontier(Map map)
        {
            if (!inExplored(map))
            {
                frontier.Add(map, 0);
                explored.Add(map);
            }
            
        }

        public Map getFromFrontier()
        {
            Map first = frontier.First().Key;
            frontier.RemoveAt(0);
            
            return first;
        }

        public bool inFrontier(Map map)
        {
            return frontier.ContainsKey(map);
        }
        public bool inExplored(Map map)
        {
            return explored.Contains(map);
        }
    }
}
