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
        static void Main(string[] args)// simply takes in the level and then divide the data into groups of 16 for convertion into symbols
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
