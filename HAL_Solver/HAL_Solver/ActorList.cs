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
        public static Color[] intToColorDict;

        public ActorList(Collection<Actor> newactors, Collection<Color> newactorsColors, Collection<char> newactorsNames)
        {
            actors = new Actor[newactors.Count()];
            namedict = new Dictionary<char, int>();
            colordict = new Dictionary<Color, Collection<int>>();
            intToColorDict = new Color[newactors.Count()];
            int i = 0;
            IEnumerator<Color> colorenum = newactorsColors.GetEnumerator();
            IEnumerator<char> namesenum = newactorsNames.GetEnumerator();
            foreach (Actor actor in newactors)
            {
                actors[i] = actor;
                intToColorDict[i] = colorenum.Current;
                namedict.Add(namesenum.Current, i);
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
        public void performMove() { }
    }
}
