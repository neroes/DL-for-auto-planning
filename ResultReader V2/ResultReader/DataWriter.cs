using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ResultReader
{
    class DataWriter
    {
        // takes in the list of lists and then prints them where columns are the files
        private StreamWriter sr;
        public DataWriter(String filename)
        {
            sr = new StreamWriter(filename);
        }
        public void Write(List<List<String>> final)
        {
            List<IEnumerator<string>> enumerator = new List<IEnumerator<string>>();
            foreach (List<String> l in final)
            {
                enumerator.Add(l.GetEnumerator());
            }
            while (enumerator[0].MoveNext())
            {
                sr.Write(enumerator[0].Current);
                foreach (IEnumerator<String> enumer in enumerator.Skip(1))
                {
                    enumer.MoveNext();
                    sr.Write(","+ enumer.Current);
                }
                sr.WriteLine("");
            }
            sr.Flush();
        }
    }
}
