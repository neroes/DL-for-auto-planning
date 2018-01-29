using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MLConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamReader sr = new StreamReader("level.lvl");
            string line = sr.ReadLine();
            string[] output = new string[16];
            for (int i = 0; i<16; i++) { output[i] = ""; }
            int linenumber = 0;
            for (int startindex = 0; startindex < 16 * 16 * 16; startindex += 16)
            {
                linenumber = startindex / (16 * 16);
                if (line[startindex + 1] == '1') { output[linenumber] = output[linenumber] + "#"; }
                
                else if (line[startindex + 2] == '1') { output[linenumber] = output[linenumber] + "0"; }
                else if (line[startindex + 6] == '1') { output[linenumber] = output[linenumber] + "A"; }
                else if (line[startindex + 10] == '1') { output[linenumber] = output[linenumber] + "a"; }
                else if (line[startindex] == '1') { output[linenumber] = output[linenumber] + " "; }
            }
            StreamWriter sw = new StreamWriter("output.txt");
            for (int i = 0; i<16; i++)
            {
                sw.WriteLine(output[i]);
            }
            sw.Flush();

        }
    }
}
/*
  public MLInput(Map map)
        {
            //[Wall,Space,ActorID*4,Boxes*4,goals*4,Color*2]
            //container = new cellpoint[70 * 70];
            container = new BitArray[16,16];
            for (int i = 0; i < 16; i++)
            {
                for (int k = 0; k < 16; k++)
                {
                    container[i,k] = new BitArray(16);//= new cellpoint("");
                }
            }
            Node[] boxes = map.getAllBoxes();
            
            for (int j = 0; j < (Map.wallMap.Length / Map.mapWidth); j++)
            {
                for (int i = 0; i < Map.mapWidth; i++)
                {
                    container[i, j][1] = Map.wallMap[i + j * Map.mapWidth];
                    container[i, j][0] = (container[i, j][1] != true);
                }
            }
            
            foreach (Actor actor in map.getActors())
            {
                container[actor.x, actor.y][2 + actor.id]=true;
                container[actor.x, actor.y][14 + (byte)actor.getcolor()]=true;
            }
            for (int i = 0; i<boxes.Length; i++)
            {
                container[boxes[i].x, boxes[i].y][6 + (byte)map.getBoxName(i)-'a'] = true;
                container[boxes[i].x, boxes[i].y][14 + (byte)map.getColorOfBox(i)] = true;
                
            }
            foreach (char name in map.getGoalNames())
            {
                foreach (Node goal in map.getGoals(name))
                {
                    container[goal.x, goal.y][10 + (byte)(name-'a')] = true;
                }
            }
            
            
        }
*/