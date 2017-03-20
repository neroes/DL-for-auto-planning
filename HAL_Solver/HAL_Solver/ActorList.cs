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
    }
}
