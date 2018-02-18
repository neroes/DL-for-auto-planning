using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Combiner
{
    class Program
    {
        // Takes in a folder location and then combines all files named 
        // ...trainingdata... and ...goaldata... 
        // and create a properties filecontaining the size of these new combined files
        // along with a trainingdata and goaldata file containing the combinations
        static void Main(string[] args)
        {
            RecursiveFileProcessor.RFPMain(args);
        }
    }
}
