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
            foreach (Node box in map.getAllBoxes())
            {
                
            }
            foreach (char name in map.getGoalNames())
            {
                foreach (Node goal in map.getGoals(name))
                {

                }
            }
            foreach (Actor box in map.getActors())
            {

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
