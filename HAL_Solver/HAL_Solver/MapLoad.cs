using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace HAL_Solver
{
    class MapLoad
    {
        public static void loadMap(string filename, out Map map)
        {
            int colcount = 0, rowcount = 0;
            getfilesize(filename, out colcount, out rowcount);
            Dictionary<Node, char> newboxes = new Dictionary<Node, char>();
            HashSet<Actor> newactors = new HashSet<Actor>();
            GoalList newgoals = new GoalList();
            Dictionary<char, Color> colorDict = new Dictionary<char, Color>();
            bool[] newwallmap = new bool[colcount*rowcount];

            Byte j = 0; // row count
            bool pastSetup = false;
            foreach (string line in File.ReadLines(@filename))
            {
                
                if (line.Contains("+"))
                {
                    pastSetup = true;

                    Byte i = 0; // col count
                    //map construction
                    foreach (char c in line)
                    {
                        if (c == '+') { newwallmap[i+ j*colcount] = true; }
                        else if (Char.IsLower(c)) { newgoals.Add(i, j, c); } // i,j is goal
                        else if (Char.IsDigit(c)) {// i,j is actor
                            if (!colorDict.ContainsKey(c)) { colorDict[c] = Color.blue; }
                            newactors.Add(new Actor(i, j, Convert.ToByte(c - '0')));
                        } 
                        else if (Char.IsUpper(c)) {
                            if (!colorDict.ContainsKey(Char.ToLower(c))) { colorDict[Char.ToLower(c)] = Color.blue;  }
                            newboxes.Add(new Node(i, j), Char.ToLower(c));
                        } // i,j is box

                        i++;
                    }
                    j++;
                }
                else if (!pastSetup)
                {
                    string[] splitline = line.Split(':');
                    string names = splitline[1].Remove(0, 1);
                    string[] splitnames = names.Split(',');
                    foreach (string name in splitnames)
                    {
                        
                        colorDict[Char.ToLower(name[0])] = (Color)Enum.Parse(typeof(Color), splitline[0].ToLower());    
                    }
                    //do color devision
                }
            }
            map = new Map(newwallmap, colcount, newactors, newboxes, newgoals, colorDict);

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
