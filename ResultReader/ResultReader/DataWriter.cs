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
        private StreamWriter sr;
        public DataWriter(String filename)
        {
            sr = new StreamWriter(filename);
        }
        public void Write(List<String>[] final)
        {
            IEnumerator<string>[] enumerator = new IEnumerator<string>[20];
            for (int i = 0; i < 20; i++)
            {
                enumerator[i] = final[i].GetEnumerator();
            }
            while (enumerator[0].MoveNext())
            {
                sr.Write(enumerator[0].Current);
                for (int i = 1; i < 20; i++){
                    enumerator[i].MoveNext();
                    sr.Write(","+enumerator[i].Current);
                }
                sr.WriteLine(";");
            }
            sr.Flush();
                
        

        }
    }
}
