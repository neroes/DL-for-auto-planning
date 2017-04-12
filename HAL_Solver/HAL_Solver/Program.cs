using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace HAL_Solver
{
    class Program
    {
        static void Main(string[] args)
        {
            Map map = null;
            MapLoad.loadMap("testmapeasy.lvl", out map);
            Map map2 = new Map(map);
            act[] actions = new act[1];
            actions[0] = new act(Interact.MOVE, Direction.E);
            map2.PerformActions(actions);
            map2.PerformActions(actions);
            map2.PerformActions(actions);
            Search search = new Search(new Astar<Map>());
            Map finalmap = solver(search, map);

            //map.GetHashCode();
            /*Map map = null;
            MapLoad.loadMap(args[0], out map);
            switch (args[1])
            {
                case "BFS":
                case "DFS":
                case "Astar":
                case "AWstar":
                case "Greedy":
                    break;
            }*/

            System.Console.Write("pizza");
            
        }
        public static Map solver(Search search, Map map)
        {
            search.addToFrontier(map);

            while (true)
            {
                Map smap = search.getFromFrontier();
                if (smap.isGoal()) { return smap; }
                Collection<act>[] actionlist = smap.getAllActions();
                foreach (Collection<act> actorlist in actionlist)
                {
                    foreach (act action in actorlist)
                    {
                        Map nmap = new Map(smap);
                        act[] actions = new act[1];
                        actions[0] = action;
                        nmap.PerformActions(actions);
                        search.addToFrontier(nmap);
                    }
                }
            }
        }
    }
}
