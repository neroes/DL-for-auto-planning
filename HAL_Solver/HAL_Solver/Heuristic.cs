using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HAL_Solver
{
    public abstract class Heuristic<Map> : IComparer<Map>
    {
        public int h(Map m) { return 0; } // This is the heuristic.

        public virtual int f(Map m) { return 0; }

        public virtual int Compare(Map x, Map y)
        {
            int comp = f(x).CompareTo(f(y));
            if (comp == 0) { return 1; } // Might need to return 0 at last index.
            return comp;
        }
    }

    public class Astar<Map> : Heuristic<Map>
    {
        public override int f(Map m)
        {
            return h(m); // + steps.
        }
    }

    public class WAstar<Map> : Heuristic<Map>
    {
        private int W;

        public WAstar(int W)
        {
            this.W = W;
        }

        public override int f(Map m)
        {
            return h(m) * W; // + steps.
        }
    }

    public class Greedy<Map> : Heuristic<Map>
    {
        public override int f(Map m)
        {
            return h(m);
        }
    }

    public class BFS<Map> : Heuristic<Map>{}

    public class DFS<Map> : Heuristic<Map>
    {
        public override int Compare(Map x, Map y)
        {
            return -1; // Might need to be 0 at first index.
        }
    }
}
