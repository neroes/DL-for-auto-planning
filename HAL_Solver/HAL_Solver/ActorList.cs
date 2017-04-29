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
        public Collection<act>[] getAllActions(Map map)
        {
            Collection<act>[] actions = new Collection<act>[actors.Length];
            int i = 0;
            foreach (Actor actor in actors)
            {
                actions[i] = actor.getActions(map);
                i++;
            }
            return actions;
        }
        public Actor this[Byte c]{ get { return actors[c]; } }
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

        internal void PerformActions(act[] actions, ref BoxList boxes)
        {
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
                        break;
                    case Interact.PULL:
                        actors[i] = new Actor(actors[i]);
                        Node pullbox = new Node(boxes[actions[i].box]);
                        Pull(actors[i], ref pullbox, actions[i].dir, actions[i].boxdir);
                        boxes[actions[i].box] = pullbox;
                        break;
                    case Interact.WAIT:
                        break;
                }
            }
        }
        public bool Push(Actor actor, ref Node box, Direction dir, Direction boxdir) {

            Move(actor, boxdir);
            Move(ref box, dir);
            return true;
        }
        public bool Pull(Actor actor, ref Node box, Direction dir, Direction boxdir) {
            Move(actor, dir);
            

            Move(ref box, dir);
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
        public bool Move(ref Node box, Direction dir)
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
        
        public int ManDistAct(BoxList boxlist, Collection<int> boxes)//manhattendistance
        {
            int totaldist = 0;

            foreach (Actor actor in actors)
            {
                int minDistToActor = 1000000;
                foreach (int boxnumber in boxes)
                {
                    Node actorpos = new Node(actor.x, actor.y);
                    int dist = boxlist[boxnumber] - actorpos;
                    if (dist < minDistToActor) { minDistToActor = dist; }
                }
                totaldist += minDistToActor;
            }
            return totaldist;
        }

        public int ManDistAct(BoxList boxlist)
        {
            int Actdist = 0;
            foreach (Actor actor in actors)
            {
                // Kan ikke helt få skrevet den her linje korrekt, den er direkte baseret på den tilsvarende i GoalList for box til goal manhatdist
                // Er derudover rimeligt sikker på at ovenstående ManDistAct nok ikke er fuldstændigt optimalt, da jeg laver en ny Node.

                // Skulle bare være actdist+= ManDistAct på alle actors som i kan se, men jeg kan ikke få BoxList delen til at virke, derudover er det jo egentligt ikke under actors at det
                // Skal være
                Actdist += actors.ManDistAct(boxlist, BoxList.boxNameGroups[actor.id]);
            }
            return Actdist;
        }
    }
}
