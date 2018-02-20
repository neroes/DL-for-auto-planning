using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Trainer
{
    // The structure of the Map containing all the properties of any current state along with the static properties such as walls
    // along with a series of basic function to demontate properties of the current state and an Equal function to determine if to states are the same
    public enum Color : Byte
    {
        blue, red, green, cyan, magenta, orange, pink, yellow
    }
    public class Map
    {
        static uint nextId = 0;
        public uint id;

        private int hash = 0;
        public int f = -1;
        public Map parent;
        public int steps;
        public static int mapWidth;
        public static bool[] wallMap;
        static GoalList goals;
        ActorList actors;
        BoxList boxes;
        public Path[] pathOfActor; // For heuristic. Temporary so it needs to be stored in map (or actorlist).
        public int[] targetOfActor; // For heuristic.
        public Dictionary<int, byte> boxPriority;  // For heuristic. Key is a boxID, value is the priority.
        public Dictionary<int, int> boxDistance; // The current distance of the box to its path. To avoid recalculating it unless the box is moved.

        public override int GetHashCode()
        {
            if (this.hash == 0)
            {
                int prime = 101;
                int result = 1;

                result = prime * result + actors.GetHashCode();
                this.hash = prime * result + boxes.GetHashCode();
            }

            return this.hash;
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
                return true;
            if (obj == null || !(obj is Map))
                return false;
            Map map = (Map)obj;
            if (!this.actors.Equals(map.actors))
                return false;
            if (!this.boxes.Equals(map.boxes))
                return false;
            return true;
        }

        public Map(bool[] newwallMap, int mapwidth, Collection<Actor> newactors, Dictionary<Node, char> newboxes, GoalList newgoals, Dictionary<char, Color> colorDict)
        {
            id = Map.nextId++;
            wallMap = newwallMap;
            mapWidth = mapwidth;

            actors = new ActorList(newactors, colorDict);
            boxes = new BoxList(newboxes, colorDict);
            goals = newgoals;
            steps = 0;
        }
        public Map(Map oldmap)
        {
            id = Map.nextId++;
            parent = oldmap;
            actors = new ActorList(oldmap.actors);
            boxes = new BoxList(oldmap.boxes);
            steps = oldmap.steps + 1;
            pathOfActor = (Path[])oldmap.pathOfActor.Clone();
            targetOfActor = (int[])oldmap.targetOfActor.Clone();
            boxPriority = new Dictionary<int, byte>(oldmap.boxPriority);
            boxDistance = new Dictionary<int, int>(oldmap.boxDistance);
        }
        public Node getbox(int id)
        {
            return boxes[id];
        }
        public HashSet<Node> getBoxGroup(char name)
        {
            return boxes.getBoxesOfName(name);
        }
        public Node[] getAllBoxes()
        {
            return boxes.getAllBoxes();
        }
        public Color getColorOfBox(int boxID)
        {
            return boxes.getColorOfBox(boxID);
        }
        public char getBoxName(int boxID)
        {
            return boxes.getBoxName(boxID);
        }
        public Actor getActor(byte name)
        {
            return actors[name];
        }
        public Actor[] getActors()
        {
            return actors.getAllActors();
        }
        public HashSet<Actor> getActorsByColor(Color color) {
            return actors[color];
        }

        public HashSet<act>[] getAllActions()
        {
            return actors.getAllActions(this);

        }

        internal bool isBox(int x, int y, Color color, out int box)
        {
            HashSet<int> checklist = boxes.getBoxesOfColor(color);
            foreach (int i in checklist)
            {
                if (boxes[i].x == x && boxes[i].y == y) { box = i; return true; }
            }
            box = 255;
            return false;
        }

        internal bool isEmptySpace(int x, int y)
        {
            if (isWall(x, y)) { return false; }
            Node[] checklist = boxes.getAllBoxes();
            foreach (Node n in checklist)
            {
                if (n.x == x && n.y == y) { return false; }
            }
            Actor[] checklist2 = actors.getAllActors();
            foreach (Actor n in checklist2)
            {
                if (n.x == x && n.y == y) { return false; }
            }
            return true;
        }

        public bool isWall(int x, int y) { return wallMap[x + y * mapWidth]; }

        public bool PerformActions(act[] actions)
        {

            return actors.PerformActions(actions, ref boxes);

        }
        public bool isGoal()
        {
            return goals.IsInGoal(boxes);
        }
        public int distToGoal()
        {
            return goals.ManDist(boxes);
        }

        public int ManDistAct(BoxList boxlist, HashSet<int> boxes)//manhattendistance
        {
            int totaldist = 0;

            foreach (Actor actor in actors.getAllActors())
            {
                int minDistToActor = 1000000;
                foreach (int boxnumber in boxes)
                {
                    Node actorpos = new Node(actor.x, actor.y);
                    int dist = boxlist[boxnumber] - actorpos;
                    if (dist < minDistToActor) { minDistToActor = dist; }
                    if (isGoal() == true) { minDistToActor = 0; }
                }
                totaldist += minDistToActor;
            }
            return totaldist;
        }

        public int ManDistAct(BoxList boxlist)
        {
            int Actdist = 0;
            foreach (Color color in ActorList.intToColorDict)
            {
                Actdist += ManDistAct(boxlist, BoxList.boxColorGroups[color]);
            }
            return Actdist;
        }

        public HashSet<char> getGoalNames()
        {
            return goals.getGoalNames();
        }

        public HashSet<Node> getGoals(char name)
        {
            return goals.getGoals(name);
        }

        public int distToActor()
        {
            return ManDistAct(boxes);
        }
    }
}
