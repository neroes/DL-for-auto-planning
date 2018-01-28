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
        static void Main(string[] args)
        {
            Dictionary<string, int> count = new Dictionary<string, int>();
            List<String> names = new List<string>();
            StreamReader sr = new StreamReader(args[0]);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] splitline = line.Split(' ');
                if (count.ContainsKey(splitline[1]))
                {
                    count[splitline[1]]++;
                }
                else
                {
                    count[splitline[1]] = 1;
                    names.Add(splitline[1]);
                }
            }
            StreamWriter sw = new StreamWriter("output.txt");
            foreach (String name in names)
            {
                sw.WriteLine(name + " " + count[name]);
            }
            sw.Flush();
        }
    }
}
