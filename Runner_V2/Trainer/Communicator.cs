using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
namespace Runner
{
    // The Communication interface between the Main application and the python script
    class Communicator
    {
        // full path of python interpreter  
        private static string python = @"C:\Program Files\Python36\python.exe";

        // python app to call  
        private static string appName = @"..\..\..\PythonPart\PythonPart.py";

        // Create new process start info // is needed for the pipe we may later create
        // public static ProcessStartInfo  StartInfo = new ProcessStartInfo(python);

        public static int singlerunner(string input)
        {
            // Create new process start info 
            ProcessStartInfo myProcessStartInfo = new ProcessStartInfo(python);

            // make sure we can read the output from stdout 
            myProcessStartInfo.UseShellExecute = false;
            myProcessStartInfo.RedirectStandardOutput = true;

            // start python app with 3 arguments  
            // 1st arguments is pointer to itself,  
            // 2nd and 3rd are actual arguments we want to send 
            myProcessStartInfo.Arguments = appName + " " + input;

            Process myProcess = new Process();
            // assign start information to the process 
            myProcess.StartInfo = myProcessStartInfo;

            Console.WriteLine("Calling Python script");
            // start the process 
            myProcess.Start();
            StreamReader myStreamReader = myProcess.StandardOutput;
            string myString = myStreamReader.ReadLine();

            /*if you need to read multiple lines, you might use: 
                string myString = myStreamReader.ReadToEnd() */

            // wait exit signal from the app we called and then close it. 
            myProcess.WaitForExit();
            myProcess.Close();

            // write the output we got from python app 
            Console.WriteLine("Value received from script: " + myString);
            return Convert.ToInt32(Convert.ToSingle(myString));
        }
    }
}
