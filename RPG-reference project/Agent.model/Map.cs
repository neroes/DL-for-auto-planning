using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Agent.model
{
    class Map
    {
        static bool[,] map;
        Collection<Actor> actors;
        Collection<Box> boxes;
        Collection<Goal> goals;
        public Map (List<char[]> mapdata)
        {
            int rowlength = 0;
            foreach (char[] row in mapdata) { if (row.Length > rowlength) { rowlength = row.Length; } }
            map = new bool[mapdata.Count, rowlength];
            int i = 0;
            foreach (char[] row in mapdata) {
                int j = 0;
                foreach (char name in row) {
                    if (name == '+') { map[i, j] = true; }
                    else if(true) { actors.Add(new Actor(i,j,)); }
                }
                i++;
            }
        }

        internal bool isBox(int x, int v, char color)
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
