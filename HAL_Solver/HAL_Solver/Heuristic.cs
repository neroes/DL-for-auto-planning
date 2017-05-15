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
        private Dictionary<Node, int> priority; // This needs to be implemented. Key is a box, value is the priority.
        private Dictionary<Node, int> boxOfGoal; // int being index of the goal.
        // Also some way for boxes that need to be moved out of the way to have a goal position perhaps.

        public Heuristic(Map initialState)
        {
            this.initHeuristic(initialState); // Separate function so it can easily be disabled for testing.
        }

        public void initHeuristic(Map m)
        {
            this.boxOfGoal = new Dictionary<Node, int>();
            Node[] boxList = m.getAllBoxes();

            foreach (char name in m.getGoalNames())
            {
                // Until all goals have a box partnered up.
                HashSet<Node> redoGoals = new HashSet<Node>(m.getGoals(name));
                HashSet<Node> redoGoalsBuffer = new HashSet<Node>(redoGoals);
                HashSet<Node> untakenBoxes = new HashSet<Node>(m.getBoxGroup(name));
                Dictionary<Node, double> shortestGoalDist = new Dictionary<Node, double>();
                HashSet<Node> takenBoxes = new HashSet<Node>();
                while (redoGoals.Count > 0)
                {
                    // Foreach box x goal combination.
                    foreach (Node goal in redoGoals)
                    {
                        double shortestDist = double.MaxValue;
                        double dist;
                        int takenBox = -1;
                        foreach (Node box in untakenBoxes)
                        {
                            // A* search should be implemented, for now it's distance.
                            dist = goal - box;
                            if (dist < shortestDist)
                            {
                                shortestDist = dist;
                                takenBox = Array.IndexOf(boxList, box);
                            }
                        }
                        shortestGoalDist[goal] = shortestDist;
                        bool redoThis = false;
                        bool oldGoalExists = false;
                        Node oldGoal = new Node(0, 0);
                        // For each already taken goal/box combo.
                        foreach (KeyValuePair<Node, int> taken in this.boxOfGoal) {
                            // Check if the box is taken for a different goal with a shorter distance.
                            if (taken.Value == takenBox && taken.Key != goal)
                            {
                                oldGoalExists = true;
                                oldGoal = taken.Key;
                                if (shortestGoalDist[taken.Key] <= shortestDist)
                                {
                                    redoThis = true;
                                }
                            }
                        }
                        // Else 
                        if (!redoThis)
                        {
                            this.boxOfGoal[goal] = takenBox;
                            redoGoalsBuffer.Remove(goal);
                            takenBoxes.Add(boxList[takenBox]);
                            // If this replaces another goal, that goal has to be redone.
                            if (oldGoalExists)
                                redoGoalsBuffer.Add(oldGoal);
                        }
                    }
                    // Remove takenBoxes from untakenBoxes.
                    untakenBoxes.ExceptWith(takenBoxes);
                    // Redo goals stuff.
                    redoGoals = new HashSet<Node>(redoGoalsBuffer);
                    redoGoalsBuffer.Clear();
                }
            }
        }

        private int DistFromGoals(Map m)
        {
            int dist = 0;
            Node[] allBoxes = m.getAllBoxes();

            foreach (KeyValuePair<Node, int> goalBox in boxOfGoal)
            {
                dist += (goalBox.Key - allBoxes[goalBox.Value]) /* * priority[goalBox.Value]*/; // Multiply with priority for weight.
                // Possibly save A* path and check distance along that instead.
                // Possibly include agent distances to box. Closest box only or all boxes. Check color.
            }

            return dist;
        }
        
        public int h(Map m) { return DistFromGoals(m); } // This is the heuristic.

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
        public Astar(Map initialState) : base(initialState) { }

        public override int f(Map m)
        {
            if (m.f == -1) { m.f = h(m) + m.steps; }
            return m.f;
        }
    }

    public class WAstar : Heuristic
    {
        private int W;

        public WAstar(Map initialState, int W) : base(initialState)
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
        public Greedy(Map initialState) : base(initialState) { }

        public override int f(Map m)
        {
            if (m.f == -1) { m.f = h(m); }
            return m.f;
        }
    }

    public class BFS : Heuristic
    {
        public BFS(Map initialState) : base(initialState) { }
    }

    public class DFS : Heuristic
    {
        public DFS(Map initialState) : base(initialState) { }

        public override int Compare(Map x, Map y)
        {
            return y.id.CompareTo(x.id);
        }
    }
}
