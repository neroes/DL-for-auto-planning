using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Counter
{
    static class OutputClass
    {
        public static int[] countingArray = new int[103];
        public static void setup()
        {
            for (int i = 0; i < 103; i++)
            {
                countingArray[i] = 0;
            }
        }
        public static void printer()
        {
            StreamWriter counting = new StreamWriter("count.txt");
            for (int i = 0; i < 103; i++)
            {
                counting.WriteLine(i+" "+countingArray[i]);
            }
            counting.Flush();
        }
            
    }
}
