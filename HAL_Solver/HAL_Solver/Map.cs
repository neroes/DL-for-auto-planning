﻿using System;
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
    public class Map
    {
        static uint nextId = 0;
        public uint id;

        private int hash = 0;
        public int f = -1;
        public Map parent;
        public int steps;
        static int mapWidth;
        static bool[] wallMap;
        static GoalList goals;
        ActorList actors;
        BoxList boxes;

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

        public Map (bool[] newwallMap, int mapwidth, Collection<Actor> newactors, Dictionary<Node, char> newboxes, GoalList newgoals, Dictionary<char, Color> colorDict)
        {
            id = Map.nextId++;
            wallMap = newwallMap;
            mapWidth = mapwidth;
            Collection<Color> newactorcolors = new Collection<Color>(); int i = 0;
            foreach (Actor a in newactors)
            {
                newactorcolors.Add(colorDict[i.ToString()[0]]);
                i++;
            }
            

            actors = new ActorList(newactors, newactorcolors);
            boxes = new BoxList(newboxes,colorDict);
            goals = newgoals;
            steps = 0;
        }
        public Map (Map oldmap)
        {
            id = Map.nextId++;
            parent = oldmap;
            actors = new ActorList(oldmap.actors);
            boxes = new BoxList(oldmap.boxes);
            steps = oldmap.steps + 1;
        }

        public Collection<Node> getBoxGroup(char name)
        {
            return boxes.getBoxesOfName(name);
        }
        public Actor getActor(byte name)
        {
            return actors[name];
        }
        public Collection<act>[] getAllActions()
        {
            return actors.getAllActions(this);

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

        public void PerformActions (act[] actions)
        {

            actors.PerformActions(actions, ref boxes);
        }
        public bool isGoal()
        {
            return goals.IsInGoal(boxes);
        }
        public int distToGoal()
        {
            return goals.ManDist(boxes);
        }
    }
}
