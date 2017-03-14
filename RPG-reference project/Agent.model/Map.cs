using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Drawing;

namespace Agent.model
{
    public class Map
    {
        static bool[,] wallMap;
        static GoalList goals;
        ActorList actors;
        BoxList boxes;
        

        public Map (int x, int y)
        {
            wallMap = new bool[x, y];
            actors = new ActorList();
            boxes = new BoxList();
            goals = new GoalList();
        }
        public Map (Map map)
        {
            this = map;
        }
        public void setwall(int x, int y)
        {
            wallMap[x, y] = true;
        }
        public void addGoal(int x, int y, char name)
        {
            goals.Add(x, y, name);
        }

        public void addActor(int x, int y, char name)
        {
            actors.Add(new Actor(x, y, name));
        }
        public void addActor(Color color, char name)
        {
            actors.Add(new Actor(color, name));
        }

        public void addBox(int x, int y, char name)
        {
            boxes.Add(x, y, name);
        }
        public void addBox(Color color, char name)
        {
            boxes.setColor(color, name);
        }

        public BoxGroup getBoxGroup(char name)
        {
            return boxes.getBoxGroup(name);
        }
        public Actor getActor(char name)
        {
            return actors[name];
        }


        internal bool isBox(int x, int y, Color color)
        {
            Collection<Node> checklist = boxes.getBoxesOfColor(color);
            foreach (Node n in checklist)
            {
                if (n.x == x && n.y == y) { return true; }
            }
            return false;
            
        }

        internal bool isEmptySpace(int x, int y)
        {
            Collection<Node> checklist = boxes.getAllBoxes();
            foreach (Node n in checklist)
            {
                if (n.x == x && n.y == y) { return false; }
            }
            Collection<Actor> checklist2 = actors.getAllActors();
            foreach (Actor n in checklist2)
            {
                if (n.x == x && n.y == y) { return false; }
            }
            return true;
        }

        public bool isWall(int x, int y) { return wallMap[x, y]; }
    }
}
