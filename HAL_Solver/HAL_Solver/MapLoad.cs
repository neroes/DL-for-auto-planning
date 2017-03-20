using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Collections.ObjectModel;

namespace HAL_Solver
{
    class MapLoad
    {
        public static void loadMap(string filename, out Map map)
        {
            int colcount = 0, rowcount = 0;
            getfilesize(filename, out colcount, out rowcount);
            Collection<Node> newboxes = new Collection<Node>();
            Collection<Actor> newactors = new Collection<Actor>();
            GoalList newgoals = new GoalList();
            Dictionary<char, Color> colorDict = new Dictionary<char, Color>();
            byte actorid = 0;
            bool[,] newwallmap = new bool[colcount,rowcount];

            Collection<char> boxnames = new Collection<char>();

            Byte j = 0; // row count
            foreach (string line in File.ReadLines(@filename))
            {
                
                if (line.Contains("+"))
                {
                    

                    Byte i = 0; // col count
                    //map construction
                    foreach (char c in line)
                    {
                        if (c == '+') { newwallmap[i, j] = true; }
                        else if (Char.IsLower(c)) { newgoals.Add(i, j, c); } // i,j is goal
                        else if (Char.IsDigit(c)) {// i,j is actor
                            if (colorDict.ContainsKey(c)) { newactors.Add(new Actor(i,j,actorid++)); }
                        } 
                        else if (Char.IsUpper(c)) {
                            if (colorDict.ContainsKey(c)) { newboxes.Add(new Node(i,j)); }
                            boxnames.Add(c);
                        } // i,j is box

                        i++;
                    }
                    j++;
                }
                else
                {
                    string[] splitline = line.Split(':');
                    string names = splitline[1].Remove(0, 1);
                    string[] splitnames = names.Split(',');
                    foreach (string name in splitnames)
                    {
                        colorDict[name[0]] = Color.FromName(splitline[0]);
                    }
                    //do color devision
                }
            }
            map = new Map(newwallmap, newactors, newboxes, boxnames, newgoals, colorDict);

        }
        public static void getfilesize(string filename,out int colcount,out int rowcount)
        {
            colcount = 0;
            rowcount = 0;
            foreach (string line in File.ReadLines(@filename))
            {
                
                if (line.Contains("+")) {
                    rowcount++;
                    if (colcount< line.Length) { colcount = line.Length; }
                }
            }
        }
    }
}
