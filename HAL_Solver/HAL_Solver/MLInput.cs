using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HAL_Solver
{
    class MLInput
    {
        cellpoint[] container;
        public MLInput(Map map)
        {
            container = new cellpoint[70 * 70];
            Node[] boxes = map.getAllBoxes();
            int j = 0;
            while (j < Map.wallMap.Length)
            {
                for (int i = 0; i < Map.mapWidth; i++)
                {
                    container[i + j / Map.mapWidth * 70][1] = Map.wallMap[i + j / Map.mapWidth];
                    container[i + j / Map.mapWidth * 70][0] = container[i + j / Map.mapWidth * 70][1] != true;
                    j++;
                }
            }
            foreach (Actor actor in map.getActors())
            {
                container[actor.x + actor.y * 70][2 + actor.id]=true;
                container[actor.x + actor.y * 70][60 + (byte)actor.getcolor()]=true;
            }
            for (int i = 0; i<boxes.Length; i++)
            {
                
            }
            
            foreach (char name in map.getGoalNames())
            {
                foreach (Node goal in map.getGoals(name))
                {

                }
            }
            
            
        }
    }
    struct cellpoint
    {
        Byte[] bites;
        public cellpoint(string arg)
        {
            bites = new Byte[9];
        }
        public bool this[int i] { get { return getbit(bites[i / 8],i%8); } set { byte newData =  (byte) (1 << i%8); bites[i/8] = (byte)((bites[i/8] & ~newData) & (value ? 1 : 0) << i); } }
        public bool getbit(Byte row, int i)
        {
            return (1&(row<<i)) != 0;
        }
    }
}
