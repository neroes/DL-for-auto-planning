using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
namespace HAL_Solver
{
    class ActorList
    {
        public Actor[] actors;
        public static Dictionary<Color, Collection<int>> colordict;
        public static Color[] intToColorDict;

        public ActorList(Collection<Actor> newactors, Collection<Color> newactorsColors)
        {
            actors = new Actor[newactors.Count()];
            colordict = new Dictionary<Color, Collection<int>>();
            intToColorDict = new Color[newactors.Count()];
            int i = 0;
            IEnumerator<Color> colorenum = newactorsColors.GetEnumerator();
            foreach (Actor actor in newactors)
            {
                actors[i] = actor;
                intToColorDict[i] = colorenum.Current;
                if (!colordict.ContainsKey(colorenum.Current)) { colordict[colorenum.Current] = new Collection<int>(); }
                colordict[colorenum.Current].Add(i);
                i++;
            }
        }
        public ActorList(ActorList oldlist)
        {
            actors = (Actor[]) oldlist.actors.Clone();
        }

        public Actor[] getAllActors() { return actors; }
        public Actor this[char c]{ get { return actors[c]; } }
        public Collection<Actor> this[Color c]
        { get {
                Collection<Actor> returnCollection = new Collection<Actor>();
                foreach (int i in colordict[c])
                {
                    returnCollection.Add(actors[i]);
                }
                return returnCollection;
            } }
        public void performMove() { }

        internal void PerformActions(act[] actions, BoxList boxes)
        {
            for (int i = 0; i<actions.Count(); i++)
            {
                switch (actions[i].inter) {
                    case Interact.MOVE:
                        Move(actors[i], actions[i].dir);
                        break;
                    case Interact.PUSH:
                        Push(actors[i], boxes[actions[i].box], actions[i].dir, actions[i].boxdir);
                        break;
                    case Interact.PULL:
                        Pull(actors[i], boxes[actions[i].box], actions[i].dir, actions[i].boxdir);
                        break;
                    case Interact.WAIT:
                        break;
                }
            }
        }
        public bool Push(Actor actor, Node box, Direction dir, Direction boxdir) {

            Move(actor, boxdir);
            Move(box, dir);
            return true;
        }
        public bool Pull(Actor actor, Node box, Direction dir, Direction boxdir) {
            Move(actor, dir);
            switch (boxdir)
            {
                case Direction.N:
                    box.y--;
                    break;
                case Direction.S:
                    box.y++;
                    break;
                case Direction.E:
                    box.x--;
                    break;
                case Direction.W:
                    box.x++;
                    break;
            }

            Move(box, dir);
            return true;
        }
        public bool Move(Actor actor, Direction dir) {
            switch (dir)
            {
                case Direction.N:
                    actor.y++;
                    break;
                case Direction.S:
                    actor.y--;
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
        public bool Move(Node box, Direction dir)
        {
            switch (dir)
            {
                case Direction.N:
                    box.y++;
                    break;
                case Direction.S:
                    box.y--;
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
