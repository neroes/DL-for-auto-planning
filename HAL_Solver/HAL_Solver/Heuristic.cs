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

        private Dictionary<int, int> priority; // This needs to be implemented. Key is a boxID, value is the priority.
        private Dictionary<Node, int> boxOfGoal; // int being index of the box.
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

            this.priority = new Dictionary<int, int>();

            foreach (KeyValuePair<Node, int> goalBox in boxOfGoal)
            {
                int prio = 2; // Priority starts at 2 to weigh everything lower.
                
                // Higher priority for each wall it touches.
                if (m.isWall(goalBox.Key.x + 1, goalBox.Key.y)) { ++prio; }
                if (m.isWall(goalBox.Key.x - 1, goalBox.Key.y)) { ++prio; }
                if (m.isWall(goalBox.Key.x, goalBox.Key.y + 1)) { ++prio; }
                if (m.isWall(goalBox.Key.x, goalBox.Key.y - 1)) { ++prio; }
                
                priority[goalBox.Value] = prio;
            }
        }

        private int DistFromGoals(Map m)
        {
            int dist = 0;
            Node[] allBoxes = m.getAllBoxes();

            foreach (KeyValuePair<Node, int> goalBox in boxOfGoal) // goalBox.Key is the goal. goalBox.Value is the box.
            {
                Node box = allBoxes[goalBox.Value];
                int thisDist = goalBox.Key - box;

                if (thisDist > 0) // Only bother with player distance if the box is not on the goal. Also normal dist because 0 will just be added.
                {
                    dist += thisDist * priority[goalBox.Value] * 2; // Multiply with priority for weight.
                                                                    // Multiply with 2 to weigh higher than player distance.
                                                                    // Possibly save A* path and check distance along that instead.
                    dist += DistFromPlayer(m, box, goalBox.Value) * priority[goalBox.Value]; // Again multiply with priority.
                }
            }

            return dist / 4; // Dist is divided by 4 as priority is weighted by 2 and so are boxes. Probably not the best solution.
                             // Maybe multiply g(m) with 4 instead.
        }

        private int DistFromPlayer(Map m, Node box, int boxID)
        {
            // Total distance of agents of the same color as the box.
            int dist = 0;
            foreach (Actor act in m.getActorsByColor(m.getColorOfBox(boxID)))
            {
                dist += Math.Abs(box.x - act.x) + Math.Abs(box.y - act.y);
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
