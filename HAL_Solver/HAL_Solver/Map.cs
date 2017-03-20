using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace HAL_Solver
{
    public enum Color : Byte
    {
        blue, red, green, cyan, magenta, orange, pink, yellow
    }
    class Map
    {
        static int mapWidth;
        static bool[] wallMap;
        static GoalList goals;
        ActorList actors;
        BoxList boxes;
        

        public Map (bool[] newwallMap, int mapwidth, Collection<Actor> newactors, Collection<Node> newboxes, Collection<char> boxnames,GoalList newgoals, Dictionary<char, Color> colorDict)
        {
            wallMap = newwallMap;
            mapWidth = mapwidth;
            Collection<Color> newboxcolors = new Collection<Color>();
            foreach (char name in boxnames)
            {
                newboxcolors.Add(colorDict[name]);
            }
            Collection<Color> newactorcolors = new Collection<Color>(); int i = 0;
            foreach (Actor a in newactors)
            {
                newactorcolors.Add(colorDict[i.ToString()[0]]);
                i++;
            }
            

            actors = new ActorList(newactors, newactorcolors);
            boxes = new BoxList(newboxes,boxnames,newboxcolors);
            goals = newgoals;
        }
        public Map (Map oldmap)
        {
            actors = new ActorList(oldmap.actors);
            boxes = new BoxList(boxes);
        }

        public Collection<Node> getBoxGroup(char name)
        {
            return boxes.getBoxesOfName(name);
        }
        public Actor getActor(char name)
        {
            return actors[name];
        }


        internal bool isBox(int x, int y, Color color, out int box)
        {
            Collection<int> checklist = boxes.getBoxesOfColor(color);
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

        public bool isWall(int x, int y) { return wallMap[x + y*mapWidth]; }

        public Map PerformActions (act[] actions)
        {
            Map newmap = new Map(this);

            newmap.actors.PerformActions(actions, newmap.boxes);

            return newmap;
        }
    }
}
