using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Drawing;

namespace HAL_Solver
{
    class Map
    {
        static bool[,] wallMap;
        static GoalList goals;
        ActorList actors;
        BoxList boxes;
        

        public Map (bool[,] newwallMap, Collection<Actor> newactors, Collection<Node> newboxes, Collection<char> boxnames,GoalList newgoals, Dictionary<char, Color> colorDict)
        {
            wallMap = newwallMap;

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


        internal bool isBox(int x, int y, Color color, out Byte box)
        {
            Collection<Byte> checklist = boxes.getBoxesOfColor(color);
            foreach (Byte i in checklist)
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

        public bool isWall(int x, int y) { return wallMap[x, y]; }
    }
}
