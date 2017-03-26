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
            System.Console.Write("pizza");
             
        }
    }
}
