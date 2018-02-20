using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainer
{
    // our search tree data (which has been visited and which has yet to be checked around)
    public class Search
    {
        private static Heuristic h;
        private SortedSet<Map> frontier; 
        private HashSet<Map> explored = new HashSet<Map>();

        public Search(Heuristic nh)
        {
            h = nh;
            frontier = new SortedSet<Map>(h);
        }


        public void addToFrontier(Map map)
        {
            if (!inExplored(map))
            {
                frontier.Add(map);
                explored.Add(map);
            }
            
        }

        public Map getFromFrontier()
        {
            Map first = frontier.First();
            frontier.Remove(first);
            
            return first;
        }
        public int currentSteps()
        {
            return frontier.First().steps;
        }
        public bool inFrontier(Map map)
        {
            return frontier.Contains(map);
        }
        public bool inExplored(Map map)
        {
            return explored.Contains(map);
        }
        public int exploredSize()
        {
            return explored.Count;
        }
        public int frontierSize()
        {
            return frontier.Count;
        }
    }
}
