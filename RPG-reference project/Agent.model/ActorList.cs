using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace Agent.model
{
    class ActorList
    {
        public Collection<Actor> actors;
        public Dictionary<char,Actor> namedict;
        public Dictionary<Color, Collection<Actor>> colordict;

        public ActorList()
        {
            actors = new Collection<Actor>();
            namedict = new Dictionary<char, Actor>();
            colordict = new Dictionary<Color, Collection<Actor>>();
        }
        public void Add(Actor actor)
        {
            actors.Add(actor);
            namedict[actor.getName()] = actor;
            colordict[actor.getColor()].Add(actor);
        }

        public Collection<Actor> getAllActors() { return actors; }
        public Actor this[char c]{ get { return namedict[c]; } }
        public Collection<Actor> this[Color c] { get { return colordict[c]; } }
    }
}
