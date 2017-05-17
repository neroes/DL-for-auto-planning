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
        private Dictionary<Node, int> boxOfGoal; // int being index of the box.
        // Also some way for boxes that need to be moved out of the way to have a goal position perhaps.
        private Dictionary<int, Path> pathOfBox; // Intended path of a box with given ID.
        private Dictionary<int, int> boxDistance; // The current distance of the box to its path. To avoid recalculating it unless the box is moved.
        private Dictionary<int, Node> boxPos; // For checking whether the box has moved.
        private Dictionary<int, int> permBoxPrio;

        public static bool[] boxWallMap; // Wall map where boxes count as walls.

        // Global consts for tuning.
        private const int boxPathBlockMult = 8;
        private const int actorPathBlockMult = 14;
        private const int actorDistPrioMult = 30; // Inverse. Also uses manhattan distance not pathfinding distance.
        private const int prioWeight = 1; // Weight of priorities. Not a multiplier.
        private const int pdw = 4; // Player distance weight (inverted).

        // 1 by default. Changed in initHeuristic.
        private int gmult = 1;

        public Heuristic(Map initialState)
        {
            this.initHeuristic(initialState); // Separate function so it can easily be disabled for testing.
        }

        public void initHeuristic(Map m)
        {
            // Populate boxWallMap
            boxWallMap = (bool[])Map.wallMap.Clone();
            foreach (Node box in m.getAllBoxes())
            {
                boxWallMap[box.x + box.y * Map.mapWidth] = true;
            }
            
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

            this.permBoxPrio = new Dictionary<int, int>();
            this.boxPos = new Dictionary<int, Node>(); // Set this down here to ensure it's only done on the necessary boxes for efficiency.
            Node[] allBoxes = m.getAllBoxes();
            m.boxPriority = new int[allBoxes.Length];
            
            foreach (KeyValuePair<Node, int> goalBox in boxOfGoal)
            {
                boxPos[goalBox.Value] = new Node(m.getbox(goalBox.Value));

                int prio = prioWeight - 1;

                // Priority increases for every goal along the path.
                Path p = pathOfBox[goalBox.Value];

                int maxsteps = p.steps;

                while (p != null)
                {
                    p.rem = maxsteps - p.steps; // Also sets remaining steps since we're iterating the paths anyway.
                    for (int i = 0; i < allBoxes.Length; ++i)
                    {
                        if (p.me == allBoxes[i] && i != goalBox.Value)
                        {
                             m.boxPriority[i] += boxPathBlockMult;
                        }
                    }
                    foreach (Node g in boxOfGoal.Keys)
                    {
                        if (p.me == g) { ++prio; }
                    }
                    p = p.parent;
                }

                this.permBoxPrio[goalBox.Value] = prio;

                // Print which boxes connect to which goal with which priority.
                /*
                Console.Error.WriteLine("Goal {0},{1} connected to box {2} at {3},{4} with priority (permanent) {5} and (blocking) {6}",
                                        goalBox.Key.x, goalBox.Key.y, m.getBoxName(goalBox.Value), m.getbox(goalBox.Value).x,
                                        m.getbox(goalBox.Value).y, this.permBoxPrio[goalBox.Value], m.boxPriority[goalBox.Value]);
                                        */
            }

            m.targetOfActor = new int[10] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1}; // 10 possible agents.
            m.pathOfActor = new Path[10];

            foreach (Actor agent in m.getActors())
            {
                FindActorTarget(m, agent);
            }

            foreach (Actor agent in m.getActors()) // Update priority values from actor paths.
            {
                Path p = m.pathOfActor[agent.id].parent;

                while (p != null)
                {
                    for (int i = 0; i < allBoxes.Length; ++i)
                    {
                        if (p.me == allBoxes[i] && !m.getActorsByColor(m.getColorOfBox(i)).Contains(agent)) // If there is a box and it's not the same color as the agent.
                        {
                            m.boxPriority[i] += boxPathBlockMult;
                        }
                    }

                    p = p.parent;
                }
            }

            // Recalculate actor paths to account for blockades.
            m.targetOfActor = new int[10] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 }; // 10 possible agents.
            m.pathOfActor = new Path[10];
            
            foreach (Actor agent in m.getActors())
            {
                FindActorTarget(m, agent);
            }
            
            gmult = m.boxPriority.Max() * (pdw + 1); // The maximum possible amount of steps taken in one move by one agent.
        }

        private void FindActorTarget(Map m, Actor agent)
        {
            calculateBoxPriority(m); // Recalculate all box priorities first.

            Node[] allBoxes = m.getAllBoxes();
            int aprio = 0;
            int goal = 0;

            for (int i = 0; i < allBoxes.Length; ++i)
            {
                int prio = m.boxPriority[i];
                Node box = allBoxes[i];
                if (prio > 0)
                {
                    if (!m.targetOfActor.Contains(i) || m.targetOfActor[agent.id] == i) // If the box is not already taken by another agent.
                    {
                        int bdist = box - agent;
                        int val = prio * actorDistPrioMult + bdist;
                        if (val > aprio)
                        {
                            aprio = val;
                            goal = i;
                        }
                    }
                }
            }

            m.targetOfActor[agent.id] = goal;
            // After finding the best one, calculate the path.
            m.pathOfActor[agent.id] = BoxDistanceSolver(m, new Node(agent.x, agent.y), allBoxes[goal]);

            // Print which actor connects to which goal with which priority.
            /*
            Console.Error.WriteLine("Actor {0} connected to box {1} at {2},{3} with priority {4}",
                                    agent.id, m.getBoxName(goal), m.getbox(goal).x,
                                    m.getbox(goal).y, m.boxPriority[goal] + (this.permBoxPrio.ContainsKey(goal) ? this.permBoxPrio[goal] : 0));
                                    */
        }

        private Path BoxDistanceSolver(Map m, Node box, Node goal, bool[] map)
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
                    if (node == goal) { return new Path(spath, node); }
                    if (!map[node.x + node.y * Map.mapWidth])
                    {
                        search.addToFrontier(new Path(spath, node));
                    }
                }
            }

            return null;
        }

        private Path BoxDistanceSolver(Map m, Node box, Node goal)
        {
            Path boxpath = BoxDistanceSolver(m, box, goal, boxWallMap);
            Path normalpath = BoxDistanceSolver(m, box, goal, Map.wallMap);
            
            if (boxpath == null)
            {
                return normalpath;
            }
            else if (boxpath.steps < normalpath.steps * 1.2)
            {
                return boxpath;
            }

            return normalpath;
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
                    int adjsteps = p.steps;

                    while (p != null)
                    {
                        if (p.me == box)
                        {
                            boxDistance[goalBox.Value] = p.rem;
                            found = true;
                            break;
                        }
                        if (p.me - box == 1) // If the box is adjacent to the path it's that place +1 away.
                        {
                            if (p.rem < adjsteps) { adjsteps = p.rem; }
                        }
                        p = p.parent;
                    }
                    if (!found) // If it's not on the path.
                    {   // If it's adjacent it's the amount of steps of the adjacent part + 1.
                        // Otherwise it's the max distance + 1.
                        boxDistance[goalBox.Value] = adjsteps + 1;
                    }

                    if (boxDistance[goalBox.Value] == 0)
                    { // If it's on its goal recalculate the priority.
                        calculateBoxPriority(m, goalBox.Value, box);
                        int index = Array.IndexOf(m.targetOfActor, goalBox.Value); // If the box had an agent connected.
                        if (index >= 0) { FindActorTarget(m, m.getActor((byte)index)); } // Recalculate that agents target.
                    }
                }
                
                int thisDist = boxDistance[goalBox.Value];

                dist += thisDist * this.permBoxPrio[goalBox.Value]; // Multiply with priority for weight.
            }
            
            return dist * pdw; // Multiply with the player distance weight.
        }

        private int AgentDist(Map m)
        {
            int dist = 0;
            Node[] allBoxes = m.getAllBoxes();

            foreach (Actor actor in m.getActors())
            {
                int boxID = m.targetOfActor[actor.id];
                Node box = allBoxes[boxID];

                Path p = m.pathOfActor[actor.id];
                if (p != null) // Use path if there is one, else manhattan distance.
                {
                    bool found = false;
                    int maxSteps = p.steps;
                    int steps = p.steps;

                    while (p != null)
                    {
                        if (p.me == actor)
                        {
                            steps = maxSteps - p.steps;
                            found = true;
                            break;
                        }
                        if (p.me - actor == 1) // If the actar is adjacent to the path it's that place +1 away.
                        {
                            if (maxSteps - p.steps < steps) { steps = maxSteps - p.steps; }
                        }
                        p = p.parent;
                    }

                    if (!found) // If it's not on the path.
                    {   // If it's adjacent it's the amount of steps of the adjacent part + 1.
                        // Otherwise it's the max distance + 1.
                        ++steps;
                    }

                    if (steps == 1) // If next to the box use manhattan distance instead from now on -1 (meaning 0 this time).
                    {
                        m.pathOfActor[actor.id] = null;
                    }
                    else
                    {
                        int pbp = this.permBoxPrio.ContainsKey(boxID) ? this.permBoxPrio[boxID] : 1;
                        dist += (steps - 1) * m.boxPriority[boxID] + pbp; // Multiply with priority for weight.
                    }
                } else // Manhattan distance
                {
                    if (m.boxPriority[boxID] >= boxPathBlockMult) // Recalculate prio if the box (likely) is blocking a path.
                    {
                        int prio = m.boxPriority[boxID];
                        if (calculateBoxPriority(m, boxID, box) < prio) // If the priority of the box is lower than before.
                        {
                            FindActorTarget(m, actor); // Find the agent a new target.
                        }
                    }

                    dist += box - actor - 1; // Just to prioritise staying near the box.
                }
            }
            
            return dist;
        }

        public void calculateBoxPriority(Map m) // Calculate all boxes
        {
            Node[] allBoxes = m.getAllBoxes();

            for (int i = 0; i < allBoxes.Length; ++i)
            {
                calculateBoxPriority(m, i, allBoxes[i]);
            }
        }

        public int calculateBoxPriority(Map m, int boxID, Node box)
        {
            int prio = 0;
            Node[] allBoxes = m.getAllBoxes();

            foreach (int b in boxOfGoal.Values)
            {
                if (b != boxID)
                {
                    Path p = pathOfBox[b];
                    while (p != null)
                    {
                        if (p.me == box)
                        {
                            prio += boxPathBlockMult;
                        }
                        p = p.parent;
                    }
                }
            }
            
            m.boxPriority[boxID] = prio;

            return prio;
        }
        
        public int BoxBlocking(Map m)
        {
            return m.boxPriority.Sum();
        }

        public int h(Map m) { return DistFromGoals(m) + AgentDist(m) + BoxBlocking(m); } // This is the heuristic.

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
