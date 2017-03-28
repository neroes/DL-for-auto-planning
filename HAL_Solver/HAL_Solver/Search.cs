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
        private static Heuristic<Map> h = new Heuristic<Map>();
        private SortedList<Map, byte> frontier = new SortedList<Map, byte>(h);
        private Collection<Map> explored = new Collection<Map>();

        public void addToFrontier(Map map)
        {
            frontier.Add(map, 0);
        }

        public Map getFromFrontier()
        {
            Map first = frontier.First().Key;
            frontier.RemoveAt(0);
            explored.Add(first);
            return first;
        }

        public bool inFrontier(Map map)
        {
            return frontier.ContainsKey(map);
        }
    }
}
