using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace instancecounter
{
    class Program
    {
        // Takes in a training file for training the network and counts the number of instances for each level
        // and counts the number of each solution length

        static void Main(string[] args)
        {
            Dictionary<string, int> count = new Dictionary<string, int>();
            List<String> names = new List<string>();
            Dictionary<string, int> count2 = new Dictionary<string, int>();
            List<string> ids = new List<string>();
            StreamReader sr = new StreamReader(args[0]);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] splitline = line.Split(' ');
                if (count.ContainsKey(splitline[splitline.Length - 2]))
                {
                    count[splitline[splitline.Length-2]]++;
                }
                else
                {
                    count[splitline[splitline.Length - 2]] = 1;
                    names.Add(splitline[splitline.Length - 2]);
                }
                if (count2.ContainsKey(splitline[splitline.Length - 1]))
                {
                    count2[splitline[splitline.Length - 1]]++;
                }
                else
                {
                    count2[splitline[splitline.Length - 1]] = 1;
                    ids.Add(splitline[splitline.Length - 1]);
                }
            }
            StreamWriter sw = new StreamWriter("output.txt");
            foreach (String name in names)
            {
                sw.WriteLine(name + " " + count[name]);
            }
            sw.Flush();

            
            StreamWriter sw2 = new StreamWriter("output2.txt");
            foreach (String id in ids)
            {
                sw2.WriteLine(id + " " + count2[id]);
            }
            sw2.Flush();
        }
    }
}
