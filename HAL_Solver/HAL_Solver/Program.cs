using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HAL_Solver
{
    class Program
    {
        static void Main(string[] args)
        {
            Map map = null;
            MapLoad.loadMap("SACrash.lvl", out map);
            Map map2 = new Map(map);
            act[] actions = new act[1];
            actions[0] = new act(Interact.MOVE, Direction.E);
            map2.PerformActions(actions);
            map2.PerformActions(actions);
            map2.PerformActions(actions);


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
    }
}
