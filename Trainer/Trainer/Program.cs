﻿using System;
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

            
            RecursiveFileProcessor.RFPMain(args);
        }
    }
}
