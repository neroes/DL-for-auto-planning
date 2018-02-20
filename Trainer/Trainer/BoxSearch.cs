using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainer
{
    // BoxSearch is essentially for finding the distance for each box to goal helping find the solution, it is however not used in this version as it utilizes BFS inorder to be sure of an exact solution
    class BoxSearch
    {
        private SortedSet<Path> frontier;
        private HashSet<Path> explored = new HashSet<Path>();

        public BoxSearch(Node goal)
        {
            frontier = new SortedSet<Path>(new BoxHeuristic(goal));
        }

        public void addToFrontier(Path path)
        {
            if (!inExplored(path))
            {
                frontier.Add(path);
                explored.Add(path);
            }
        }

        public Path getFromFrontier()
        {
            Path first = frontier.First();
            frontier.Remove(first);

            return first;
        }

        public bool inFrontier(Path path)
        {
            return frontier.Contains(path);
        }
        public bool inExplored(Path path)
        {
            return explored.Contains(path);
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

    public class BoxHeuristic : IComparer<Path>
    {
        Node goal;

        public BoxHeuristic(Node goal)
        {
            this.goal = goal;
        }

        public int h(Node n) { return n - goal; }

        public int f(Path p) { return p.me - goal + p.steps; }

        public int Compare(Path x, Path y)
        {
            int comp = f(x).CompareTo(f(y));
            if (comp == 0) { return x.id.CompareTo(y.id); } // Might need to return 0 at last index.
            return comp;
        }
    }

    public class Path
    {
        private static long _id = 0;

        public long id;
        public int hashCode = 0;

        public Node me;
        public Path parent;

        public int steps = 0;
        public int rem = 0;

        public Path(Node node)
        {
            id = _id++;
            me = node;
        }
        public Path(Path parent, Node node) : this(node)
        {
            steps = parent.steps + 1;
            this.parent = parent;
        }
        public override bool Equals(Object obj)
        {
            return obj is Path && me == ((Path)obj).me;
        }
        public override int GetHashCode()
        {
            return me.GetHashCode();
        }
    }
}