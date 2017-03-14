using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Collections.ObjectModel;

namespace Agent.model
{
    class MapLoad
    {
        public void loadMap(string filename, Map map)
        {
            int colcount = 0, rowcount = 0;
            getfilesize(filename, colcount, rowcount);
            Collection<Box> newboxes = new Collection<Box>();
            Collection<Actor> newactors = new Collection<Actor>();
            Collection<Node> newgoals = new Collection<Node>();
            Dictionary<char, Color> colorDict = new Dictionary<char, Color>();
            bool[,] newwallmap = new bool[colcount,rowcount];
            foreach (string line in File.ReadLines(@filename))
            {
                Byte j = 0; // row count
                if (line.Contains("+"))
                {
                    

                    Byte i = 0; // col count
                    //map construction
                    foreach (char c in line)
                    {
                        if (c == '+') { newwallmap[i, j] = true; }
                        else if (Char.IsLower(c)) { newgoals.Add(new Node(i, j));  } // i,j is goal
                        else if (Char.IsDigit(c)) {// i,j is actor
                            if (colorDict.ContainsKey(c)) { newactors.Add(new Actor(i,j,colorDict[c], c)); }
                        } 
                        else if (Char.IsUpper(c)) {
                            if (colorDict.ContainsKey(c)) { newboxes.Add(new Box(i,j,colorDict[c], c)); }
                        } // i,j is box
                        i++;
                    }
                    j++;
                }
                else
                {
                    string[] splitline = line.Split(": ");
                    string[] splitnames = splitline[1].Split(',');
                    foreach (string names in splitnames)
                    {
                        colorDict[names[0]] = Color.FromName("splitline[0]");
                    }
                    Color slateBlue = Color.FromName("SlateBlue");
                    //do color devision
                }
            }
            map = new Map(newwallmap, newactors, newboxes);

        }
        public void getfilesize(string filename, int colcount, int rowcount)
        {
            rowcount = 0;
            colcount = 0;
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
