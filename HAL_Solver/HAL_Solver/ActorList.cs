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
        public Dictionary<char,Actor> namedict;
        public Dictionary<Color, Collection<Actor>> colordict;

        /*public ActorList()
        {
            actors = new Collection<Actor>();
            namedict = new Dictionary<char, Actor>();
            colordict = new Dictionary<Color, Collection<Actor>>();
        }*/
        public ActorList(Collection<Actor> newactors)
        {
            actors = new Actor[newactors.Count()];
            namedict = new Dictionary<char, Actor>();
            colordict = new Dictionary<Color, Collection<Actor>>();
            int i = 0;
            foreach (Actor actor in newactors)
            {
                this.Add(actor);
                actors[i] = actor;
                i++;
            }
        }
        public void Add(Actor actor)
        {
            //actors.Add(actor);
            namedict[actor.getName()] = actor;
            if (!colordict.ContainsKey(actor.getColor())) { colordict[actor.getColor()] = new Collection<Actor>(); }
            colordict[actor.getColor()].Add(actor);
        }

        public Actor[] getAllActors() { return actors; }
        public Actor this[char c]{ get { return namedict[c]; } }
        public Collection<Actor> this[Color c] { get { return colordict[c]; } }
    }
}
