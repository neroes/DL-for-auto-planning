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
            MapLoad.loadMap("SAPHOBAR.lvl", out map);/*
            Map map2 = new Map(map);
            act[] actions = new act[1];
            actions[0] = new act(Interact.MOVE, Direction.E);
            map2.PerformActions(actions);
            map2.PerformActions(actions);
            map2.PerformActions(actions);*/
            Search search = new Search(new Astar());
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
            Map printmap = finalmap;
            while (true)
            {
                System.Console.WriteLine("Box1 Position: {0},{1}\t Actor1 Position: {2},{3}\t Steps: {4}", printmap.getBoxGroup('a')[0].x, printmap.getBoxGroup('a')[0].y, printmap.getActor(0).x, printmap.getActor(0).y, printmap.steps);
                if (printmap.parent == null) { break; }
                else { printmap = printmap.parent; }
            }
            System.Console.Write("pizza");
            
        }
        public static Map solver(Search search, Map map)
        {
            search.addToFrontier(map);

            int i = -1;

            while (true)
            {
                i++;
                if (i == 1000)
                {
                    System.Console.Write("Explored: {0}\t Frontier: {1}\n", search.exploredSize(), search.frontierSize());
                    i = 0;
                }
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
                        if (nmap.isGoal()) { return nmap; }
                        search.addToFrontier(nmap);
                    }
                }
            }
        }
    }
}
