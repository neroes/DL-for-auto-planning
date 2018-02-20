using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Trainer
{
    //Finds the size and location of all the boxes, agents, walls and goals, and then store this into groups of static and mutable containers
    class MapLoad
    {
        public static void loadMap(StreamReader lines, out Map map)
        {
            List<string> mapLines = new List<string>();

            string l;
            while ((l = lines.ReadLine()) != null && l != "")
            {
                
                //System.Console.WriteLine(l);
                mapLines.Add(l);
            }
            

            int colcount = 0, rowcount = 0;
            getfilesize(mapLines, out colcount, out rowcount);
            Dictionary<Node, char> newboxes = new Dictionary<Node, char>();
            Collection<Actor> newactors = new Collection<Actor>();
            GoalList newgoals = new GoalList();
            Dictionary<char, Color> colorDict = new Dictionary<char, Color>();
            bool[] newwallmap = new bool[colcount*rowcount];

            Byte j = 0; // row count
            bool pastSetup = false;

            foreach (string line in mapLines)
            {
                if (line.Contains("+"))
                {
                    pastSetup = true;

                    Byte i = 0; // col count
                    //map construction
                    foreach (char c in line)
                    {
                        if (c == '+') { newwallmap[i + j * colcount] = true; }
                        else if (Char.IsLower(c)) { newgoals.Add(i, j, c); } // i,j is goal
                        else if (Char.IsDigit(c))
                        {// i,j is actor
                            if (!colorDict.ContainsKey(c)) { colorDict[c] = Color.blue; }
                            newactors.Add(new Actor(i, j, Convert.ToByte(c - '0')));
                        }
                        else if (Char.IsUpper(c))
                        {
                            if (!colorDict.ContainsKey(Char.ToLower(c))) { colorDict[Char.ToLower(c)] = Color.blue; }
                            newboxes.Add(new Node(i, j), Char.ToLower(c));
                        } // i,j is box

                        i++;
                    }
                    j++;
                }
                else if (!pastSetup)
                {
                    string[] splitline = line.Split(':');
                    string names = splitline[1].Replace(" ", ""); ;
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
        public static void getfilesize(List<string> mapLines, out int colcount, out int rowcount)
        {
            colcount = 0;
            rowcount = 0;

            foreach (string line in mapLines)
            {
                if (line.Contains("+"))
                {
                    rowcount++;
                    if (colcount < line.Length) { colcount = line.Length; }
                }
            }
        }
    }
}
