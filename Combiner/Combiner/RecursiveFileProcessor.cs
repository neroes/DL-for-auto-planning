using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace Combiner
{
    class RecursiveFileProcessor
    {
        // Based on recusiveFileProecessor found at https://msdn.microsoft.com/en-us/library/c1sez4sc(v=vs.110).aspx
        // Combines all files called trainingdata into a final TrainingData.txt and all called goaldata in a final GoalData.txt along with a count of the number of properties
        public static StreamWriter writerTraining = new StreamWriter("TrainingData.txt");
        public static StreamWriter writerGoal = new StreamWriter("GoalData.txt");
        public static StreamWriter properties = new StreamWriter("properties.txt");
        public static int trainingcount = 0;
        public static int evalcount = 0;
        public static void RFPMain(string[] args)
        {
            foreach (string path in args)
            {
                if (File.Exists(path))
                {
                    // This path is a file
                    ProcessFile(path);
                }
                else if (Directory.Exists(path))
                {
                    // This path is a directory
                    ProcessDirectory(path);
                }
                else
                {
                    Console.WriteLine("{0} is not a valid file or directory.", path);
                }
            }
            properties.WriteLine(trainingcount);
            properties.WriteLine(evalcount);
            properties.Flush();
        }


        // Process all files in the directory passed in, recurse on any directories 
        // that are found, and process the files they contain.
        public static void ProcessDirectory(string targetDirectory)
        {
            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
                ProcessFile(fileName);

            // Recurse into subdirectories of this directory.
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
                ProcessDirectory(subdirectory);
        }

        // Insert logic for processing found files here.
        public static void ProcessFile(string path)
        {
            string line;
            path = path.ToLower();
            StreamReader reader = new StreamReader(path);
            if (path.Contains("TrainingData".ToLower()))
            {
                while((line = reader.ReadLine()) != null){
                    writerTraining.WriteLine(line);
                    trainingcount++;
                }
                writerTraining.Flush();
            }
            else if (path.Contains("GoalData".ToLower()))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    writerGoal.WriteLine(line);
                    evalcount++;
                }
                writerGoal.Flush();
            }
        }
    }
}
