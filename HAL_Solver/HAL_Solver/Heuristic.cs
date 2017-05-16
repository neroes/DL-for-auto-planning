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
        private Dictionary<int, int> priority; // Key is a boxID, value is the priority.
        private Dictionary<Node, int> boxOfGoal; // int being index of the box.
        // Also some way for boxes that need to be moved out of the way to have a goal position perhaps.
        private Dictionary<int, Path> pathOfBox; // Intended path of a box with given ID.
        private Dictionary<int, int> boxDistance; // The current distance of the box to its path.
        private Dictionary<int, Node> boxPos; // For checking whether the box has moved.

        // 1 by default. Changed in initHeuristic.
        internal int pdw = 1; // Player distance weight (inverted).
        internal int priow = 1; // Priority weight.

        private int gmult = 1;

        public Heuristic(Map initialState)
        {
            this.initHeuristic(initialState); // Separate function so it can easily be disabled for testing.
        }

        public void initHeuristic(Map m)
        {
            // pdw = 5; // Not used atm
            this.boxOfGoal = new Dictionary<Node, int>();
            this.pathOfBox = new Dictionary<int, Path>();
            this.boxDistance = new Dictionary<int, int>();
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
                        int shortestDist = int.MaxValue;
                        Path takenPath = null;
                        int dist;
                        int takenBox = -1;
                        foreach (Node box in untakenBoxes)
                        {
                            // A* search for distance.
                            Path p = BoxDistanceSolver(m, box, goal);

                            if (p != null)
                            {
                                dist = p.steps;

                                if (dist < shortestDist)
                                {
                                    shortestDist = dist;
                                    takenBox = Array.IndexOf(boxList, box);
                                    takenPath = p;
                                }
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
                            this.pathOfBox[takenBox] = takenPath;
                            this.boxOfGoal[goal] = takenBox;
                            this.boxDistance[takenBox] = takenPath.steps;
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
            this.boxPos = new Dictionary<int, Node>(); // Set this down here to ensure it's only done on the necessary boxes for efficiency.

            foreach (KeyValuePair<Node, int> goalBox in boxOfGoal)
            {
                boxPos[goalBox.Value] = new Node(m.getbox(goalBox.Value));

                int prio = 1; // Priority starts at 2 to weigh everything lower. -1 to ignore final goal makes it 1.

                // Priority increases for every goal along the path.
                Path p = pathOfBox[goalBox.Value];

                int maxsteps = p.steps;

                while (p != null)
                {
                    p.rem = maxsteps - p.steps; // Also sets remaining steps since we're iterating the paths anyway.
                    foreach (Node g in boxOfGoal.Keys)
                    {
                        if (p.me == g) { ++prio; }
                    }
                    p = p.parent;
                }
                
                if (priow < prio) { priow = prio; }

                priority[goalBox.Value] = prio;
                
                // Print which boxes connect to which goal with which priority.
                Console.Error.WriteLine("Goal {0},{1} connected to box {2},{3} with priority {4}",
                                        goalBox.Key.x, goalBox.Key.y, m.getbox(goalBox.Value).x,
                                        m.getbox(goalBox.Value).y, priority[goalBox.Value]);
            }

            gmult = priow * (pdw + 1);
        }

        private Path BoxDistanceSolver(Map m, Node box, Node goal)
        {
            BoxSearch search = new BoxSearch(goal);

            search.addToFrontier(new Path(box));

            Node[] newNodes = new Node[4];

            while (search.frontierSize() > 0)
            {
                Path spath = search.getFromFrontier();
                if (spath.me == goal) { return spath; }

                newNodes[0] = new Node((byte)(spath.me.x + 1), spath.me.y);
                newNodes[1] = new Node((byte)(spath.me.x - 1), spath.me.y);
                newNodes[2] = new Node(spath.me.x, (byte)(spath.me.y + 1));
                newNodes[3] = new Node(spath.me.x, (byte)(spath.me.y - 1));

                foreach (Node node in newNodes)
                {
                    if (!m.isWall(node.x, node.y))
                    {
                        search.addToFrontier(new Path(spath, node));
                    }
                }
            }

            return null;
        }

        private int DistFromGoals(Map m)
        {
            int dist = 0;
            Node[] allBoxes = m.getAllBoxes();

            foreach (KeyValuePair<Node, int> goalBox in boxOfGoal) // goalBox.Key is the goal. goalBox.Value is the box.
            {
                Node box = allBoxes[goalBox.Value];

                if (boxPos[goalBox.Value] != box)
                {
                    boxPos[goalBox.Value] = new Node(box);
                    Path p = pathOfBox[goalBox.Value];
                    bool found = false;
                    bool adj = false;
                    int adjsteps = int.MaxValue;

                    while (p != null)
                    {
                        if (p.me - box == 1) // If the box is adjacent to the path it's that place +1 away.
                        {
                            adj = true;
                            if (p.rem < adjsteps) { adjsteps = p.rem; }
                        }
                        if (p.me == box)
                        {
                            boxDistance[goalBox.Value] = p.rem;
                            found = true;
                            break;
                        }
                        p = p.parent;
                    }

                    if (!found) // If it's not on the path.
                    {
                        if (adj) // If it's adjacent it's the amount of steps of the adjacent part + 1.
                        {
                            boxDistance[goalBox.Value] = adjsteps + 1;
                        } else // Else it's 1 further away than before.
                        {
                            ++boxDistance[goalBox.Value];
                        }
                    }
                }

                int thisDist = boxDistance[goalBox.Value];

                if (thisDist > 0) // Only bother with distance if the box is not on the goal.
                {
                    dist += thisDist * priority[goalBox.Value] * pdw; // Multiply with priority for weight.
                                                                      // Multiply with pdw to weigh higher than player distance.
                   // dist += DistFromPlayer(m, box, goalBox.Value) * priority[goalBox.Value]; // Again multiply with priority.
                }
            }

            return dist;
        }

        private int DistFromPlayer(Map m, Node box, int boxID)
        {
            // Total distance of agents of the same color as the box.
            int dist = 0;
            foreach (Actor act in m.getActorsByColor(m.getColorOfBox(boxID)))
            {
                dist += Math.Abs(box.x - act.x) + Math.Abs(box.y - act.y) - 1; // -1 so it's 0 if touching the box.
            }
            return dist;
        }

        public int h(Map m) { return DistFromGoals(m); } // This is the heuristic.

        public int g(Map m) { return gmult * m.steps; } // Multiply g instead of dividing h for precision and speed.

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
            if (m.f == -1) { m.f = h(m) + g(m); }
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
            if (m.f == -1) { m.f = h(m) * W + g(m); }
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
