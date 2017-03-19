using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace HAL_Solver
{
    class ActorList
    {
        public Actor[] actors;
        public static Dictionary<char,int> namedict;
        public static Dictionary<Color, Collection<int>> colordict;

        /*public ActorList()
        {
            actors = new Collection<Actor>();
            namedict = new Dictionary<char, Actor>();
            colordict = new Dictionary<Color, Collection<Actor>>();
        }*/
        public ActorList(Collection<Actor> newactors)
        {
            actors = new Actor[newactors.Count()];
            namedict = new Dictionary<char, int>();
            colordict = new Dictionary<Color, Collection<int>>();
            int i = 0;
            foreach (Actor actor in newactors)
            {
                this.Add(actor);
                actors[i] = actor;
                namedict.Add(actor.getName(), i);
                if (!colordict.ContainsKey(actor.getColor())) { colordict[actor.getColor()] = new Collection<int>(); }
                colordict[actor.getColor()].Add(i);
                i++;
            }
        }
        public void Add(Actor actor)
        {
            //actors.Add(actor);
            
            
        }

        public Actor[] getAllActors() { return actors; }
        public Actor this[char c]{ get { return actors[namedict[c]]; } }
        public Collection<Actor> this[Color c]
        { get {
                Collection<Actor> returnCollection = new Collection<Actor>();
                foreach (int i in colordict[c])
                {
                    returnCollection.Add(actors[i]);
                }
                return returnCollection;
            } }
    }
}
