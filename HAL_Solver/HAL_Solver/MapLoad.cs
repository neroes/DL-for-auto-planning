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
    public static class MapLoad
    {
        private static readonly Dictionary<string, byte> colorID
         = new Dictionary<string, byte>
        {
            { "blue", 0 },
            { "red", 1 },
            { "green", 2 },
            { "cyan", 3 },
            { "magenta", 4 },
            { "orange", 5 },
            { "pink", 6 },
            { "yellow", 7 },
        };

        public static Map loadMap(string filename)
        {
            byte colcount = 0, rowcount = 0;
            getfilesize(filename, out colcount, out rowcount);

            Collection<byte> availableAgents = new Collection<byte>();
            Collection<byte> availableBoxes = new Collection<byte>();
            Collection<byte> availableGoals = new Collection<byte>();

            Collection<byte[]>[] newboxes = new Collection<byte[]>[26];
            byte[][] newagents = new byte[10][];
            Collection<byte[]>[] newgoals = new Collection<byte[]>[26];

            Collection<byte>[] boxesOfColor = new Collection<byte>[8];
            byte[] colorOfBox = new byte[26];
            Collection<byte>[] agentsOfColor = new Collection<byte>[8];
            byte[] colorOfAgent = new byte[10];

            bool[] newwallmap = new bool[colcount + rowcount];

            foreach (string line in File.ReadLines(@filename))
            {
                byte j = 0; // row count
                if (line.Contains("+"))
                {
                    byte i = 0; // col count
                    //map construction
                    foreach (char c in line)
                    {
                        if (c == '+') { newwallmap[i + j * colcount] = true; }
                        else if (Char.IsLower(c)) { // i,j is goal
                            byte goal = (byte)(c - 'a');
                            newgoals[goal].Add(new byte[2] { i, j });
                            availableGoals.Add(goal);
                        } else if (Char.IsDigit(c)) { // i,j is agent
                            newagents[(byte)c] = new byte[2] { i, j };
                            availableAgents.Add((byte)c);
                        } else if (Char.IsUpper(c)) { // i,j is box
                            byte box = (byte)(c - 'a');
                            newboxes[box].Add(new byte[2] { i, j });
                            availableBoxes.Add(box);
                        }
                        i++;
                    }
                    j++;
                }
                else
                {
                    string[] splitline = line.Split(new[] { ": " }, StringSplitOptions.None);
                    string[] splitnames = splitline[1].Split(',');
                    foreach (string names in splitnames)
                    {
                        char c = names[0];
                        byte color = colorID[splitline[0]];
                        if (Char.IsDigit(c))
                        {
                            colorOfAgent[(byte)c] = color;
                            agentsOfColor[color].Add((byte)c);
                        }
                        else
                        {
                            colorOfBox[(byte)(c - 'A')] = color;
                            boxesOfColor[color].Add((byte)(c - 'A'));
                        }
                    }
                }
            }
            return new Map(newwallmap, newagents, newboxes, newgoals, availableAgents,
                availableBoxes, availableGoals, boxesOfColor, colorOfBox, agentsOfColor,
                colorOfAgent, rowcount, colcount);

        }
        public static void getfilesize(string filename, out byte colcount, out byte rowcount)
        {
            colcount = 0;
            rowcount = 0;
            foreach (string line in File.ReadLines(@filename))
            {
                
                if (line.Contains("+")) {
                    rowcount++;
                    if (colcount< line.Length) { colcount = (byte)line.Length; }
                }
            }
        }
    }
}
