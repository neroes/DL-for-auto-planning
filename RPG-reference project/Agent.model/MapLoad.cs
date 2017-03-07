using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace Agent.model
{
    class MapLoad
    {
        public void loadMap(string filename, Map map)
        {
            int filerows = getfilesize(filename);
            bool mapCreated = false;
            int mapstartline = 0;
            foreach (string line in File.ReadLines(@filename))
            {
                int j = 0; // row count
                if (line.Contains("+"))
                {
                    if (!mapCreated)
                    {
                        mapCreated = true;
                        int colcount = line.Length;
                        int rowcount = filerows - mapstartline;
                        map = new Map(colcount, rowcount);
                    }
                    

                    int i = 0; // col count
                    //map construction
                    foreach (char c in line)
                    {
                        if (c == '+') { map.setwall(i, j); }
                        else if (Char.IsLower(c)) { map.addGoal(i, j, c); } // i,j is goal
                        else if (Char.IsDigit(c)) { map.addActor(i, j, c); } // i,j is actor
                        else if (Char.IsUpper(c)) { map.addBox(i, j, c); } // i,j is box
                        i++;
                    }
                    j++;
                }
                else
                {
                    mapstartline++;
                    Color slateBlue = Color.FromName("SlateBlue");
                    //do color devision
                }
            }
            
            
        }
        public int getfilesize(string filename)
        {
            int lineCount = 0;
            using (var reader = File.OpenText(@filename))
            {
                while (reader.ReadLine() != null)
                {   
                    lineCount++;
                }
            }
            return lineCount;
        }
    }
}
