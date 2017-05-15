using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HAL_Solver
{
    public abstract class Heuristic : IComparer<Map>
    {
        public int h(Map m) { return m.distToGoal() +  m.distToActor(); } // This is the heuristic.

        public virtual int f(Map m) { return 0; }

        public virtual int Compare(Map x, Map y)
        {
            int comp = f(x).CompareTo(f(y));
            if (comp == 0) { return x.id.CompareTo(y.id); } // Might need to return 0 at last index.
            return comp;
        }
    }

    public class Astar : Heuristic
    {
        public override int f(Map m)
        {
            if (m.f == -1) { m.f = h(m) + m.steps; }
            return m.f;
        }
    }

    public class WAstar : Heuristic
    {
        private int W;

        public WAstar(int W)
        {
            this.W = W;
        }

        public override int f(Map m)
        {
            if (m.f == -1) { m.f = h(m) * W + m.steps; }
            return m.f;
        }
    }

    public class Greedy : Heuristic
    {
        public override int f(Map m)
        {
            if (m.f == -1) { m.f = h(m); }
            return m.f;
        }
    }

    public class BFS : Heuristic{}

    public class DFS : Heuristic
    {
        public override int Compare(Map x, Map y)
        {
            return y.id.CompareTo(x.id);
        }
    }
}
