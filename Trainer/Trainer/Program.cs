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
            string path = "TrainingData.txt";
            string path2 = "GoalData.txt";
            MLInput.setup(path,path2);

            
            RecursiveFileProcessor.RFPMain(args);
        }
    }
}
