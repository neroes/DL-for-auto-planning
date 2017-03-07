using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Drawing;
using System;

namespace Agent.model
{
    public class Map
    {
        static bool[,] wallMap;
        Collection<Actor> actors;
        BoxList boxes;
        GoalList goals;

        public Map (int x, int y)
        {
            wallMap = new bool[x, y];
            actors = new Collection<Actor>();
            boxes = new BoxList();
            goals = new GoalList();
        }

        public void setwall(int x, int y)
        {
            map[x, y] = true;
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
            boxes.Add(new Box(x, y),name);
        }
        public void addBox(Color color, char name)
        {
            boxes.setColor(color, name);
        }

        public Box getBox(char name)
        {
            return boxes.getBox(name);
        }
        public Actor getActor(char name)
        {
            return actors.getActor(name);
        }


        internal bool isBox(int x, int v, Color color)
        {
            throw new NotImplementedException();
        }

        internal bool isEmptySpace(int x, int v)
        {
            throw new NotImplementedException();
        }

        public bool isWall(int x, int y) { return map[x, y]; }
    }
}
