using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Trainer
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = "data.txt";
            MLInput.setup(path);

            // Delete the file if it exists.
            if (File.Exists(path))
            {
                // Note that no lock is put on the
                // file and the possibility exists
                // that another process could do
                // something with it between
                // the calls to Exists and Delete.
                File.Delete(path);
            }
            RecursiveFileProcessor.RFPMain(args);
        }
    }
}
