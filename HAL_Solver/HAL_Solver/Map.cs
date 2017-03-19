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
        

        public Map (bool[,] newwallMap, Collection<Actor> newactors, Collection<Box> newboxes, GoalList newgoals)
        {
            wallMap = newwallMap;
            actors = new ActorList(newactors);
            boxes = new BoxList(newboxes);
            goals = newgoals;
        }
        public Map (Map oldmap)
        {
            actors = new ActorList(oldmap.actors);
            boxes = new BoxList(boxes);
        }

        public Collection<Box> getBoxGroup(char name)
        {
            return boxes.getBoxesOfName(name);
        }
        public Actor getActor(char name)
        {
            return actors[name];
        }


        internal bool isBox(int x, int y, Color color)
        {
            Collection<Box> checklist = boxes.getBoxesOfColor(color);
            foreach (Node n in checklist)
            {
                if (n.x == x && n.y == y) { return true; }
            }
            return false;
        }

        internal bool isEmptySpace(int x, int y)
        {
            if (isWall(x, y)) { return false; }
            Box[] checklist = boxes.getAllBoxes();
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
