using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
namespace Runner
{
    // List of actors along with comparison and move functions and legality checks
    class ActorList
    {
        public Actor[] actors;
        public static Dictionary<Color, HashSet<int>> colordict;
        public static Color[] intToColorDict;

        public IEnumerator GetEnumerator()
        {
            return actors.GetEnumerator();
        }

        public override int GetHashCode()
        {
            int prime = 37;
            int result = 1;

            for (int i = 0; i < actors.Length; i++)
            {
                result = prime * result + actors[i].GetHashCode();
            }
                
            return result;
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
                return true;
            if (obj == null)
                return false;
            ActorList al = (ActorList)obj;
            for (int i = 0; i< actors.Length; i++)
            {
                if (!this.actors[i].Equals(al.actors[i])) { return false; }
            }
            return true;
        }

        public ActorList(Collection<Actor> newactors, Dictionary<char, Color> colorDict)
        {
            actors = new Actor[newactors.Count()];
            colordict = new Dictionary<Color, HashSet<int>>();
            intToColorDict = new Color[newactors.Count()];

            foreach (Actor actor in newactors)
            {
                int i = actor.id;
                Color col = colorDict[i.ToString()[0]];
                actors[i] = actor;
                intToColorDict[i] = col;
                if (!colordict.ContainsKey(col)) { colordict[col] = new HashSet<int>(); }
                colordict[col].Add(i);
            }
        }
        public ActorList(ActorList oldlist)
        {
            actors = (Actor[]) oldlist.actors.Clone();
        }

        public Actor[] getAllActors() { return actors; }
        public HashSet<act>[] getAllActions(Map map)
        {
            HashSet<act>[] actions = new HashSet<act>[actors.Length];
            int i = 0;
            foreach (Actor actor in actors)
            {
                actions[i] = actor.getActions(map);
                i++;
            }
            return actions;
        }
        public Actor this[Byte c]{ get { return actors[c]; } }
        public HashSet<Actor> this[Color c]
        { get {
                HashSet<Actor> returnHashSet = new HashSet<Actor>();
                foreach (int i in colordict[c])
                {
                    returnHashSet.Add(actors[i]);
                }
                return returnHashSet;
            } }
        

        internal bool PerformActions(act[] actions, ref BoxList boxes)
        {
            bool[] activeboxlist = new bool[boxes.boxes.Length];
            for (int i = 0; i<actions.Count(); i++)
            {
                switch (actions[i].inter) {
                    case Interact.MOVE:
                        actors[i] = new Actor(actors[i]);
                        Move(actors[i], actions[i].dir);
                        break;
                    case Interact.PUSH:
                        actors[i] = new Actor(actors[i]);
                        Node pushbox = new Node(boxes[actions[i].box]);
                        Push(actors[i], ref pushbox, actions[i].dir, actions[i].boxdir);
                        boxes[actions[i].box] = pushbox;

                        if (activeboxlist[actions[i].box] == true) { return false; }
                        else { activeboxlist[actions[i].box] = true; }
                        break;
                    case Interact.PULL:
                        actors[i] = new Actor(actors[i]);
                        Node pullbox = new Node(boxes[actions[i].box]);
                        Pull(actors[i], ref pullbox, actions[i].dir, actions[i].boxdir);
                        boxes[actions[i].box] = pullbox;

                        if (activeboxlist[actions[i].box] == true) { return false; }
                        else { activeboxlist[actions[i].box] = true; }
                        break;
                    case Interact.WAIT:
                        break;
                    
                }
            }
            /*string line = "[";
            for (int i = 0; i < actions.Count() - 1; i++)
            {
                line = line + actions[i].ToString();
                line = line + ", ";
            }
            line = line + actions[actions.Count() - 1].ToString();
            line = line + "]";
            System.Console.WriteLine(line);*/ //debug code for seeing what moves are sent

            return isLegal(boxes);
        }
        public bool isLegal(BoxList boxes)
        {
            bool[] tempmap = new bool[Map.wallMap.Length];
            for (int i = 0; i<actors.Length; i++)
            {
                if (tempmap[actors[i].y * Map.mapWidth + actors[i].x] == true) {  return false; }
                else { tempmap[actors[i].y * Map.mapWidth + actors[i].x] = true; }
            }
            for (int i = 0; i < boxes.boxes.Length; i++)
            {
                if (tempmap[boxes.boxes[i].y * Map.mapWidth + boxes.boxes[i].x] == true) { return false; }
                else { tempmap[boxes.boxes[i].y * Map.mapWidth + boxes.boxes[i].x] = true; }
            }
            return true;
        }
        public bool Push(Actor actor, ref Node box, Direction dir, Direction boxdir) {

            Move(actor, dir);
            Move(ref box, boxdir);
            return true;
        }
        public bool Pull(Actor actor, ref Node box, Direction dir, Direction boxdir) {
            Move(actor, dir);
            Move(ref box, boxdir);
            return true;
        }
        public bool Move(Actor actor, Direction dir) {
            switch (dir)
            {
                case Direction.N:
                    actor.y--;
                    break;
                case Direction.S:
                    actor.y++;
                    break;
                case Direction.E:
                    actor.x++;
                    break;
                case Direction.W:
                    actor.x--;
                    break;
            }
            return true;
        }
        public bool Move(ref Node box, Direction dir)
        {
            switch (dir)
            {
                case Direction.N:
                    box.y--;
                    break;
                case Direction.S:
                    box.y++;
                    break;
                case Direction.E:
                    box.x++;
                    break;
                case Direction.W:
                    box.x--;
                    break;
            }
            return true;
        }
    }
}
