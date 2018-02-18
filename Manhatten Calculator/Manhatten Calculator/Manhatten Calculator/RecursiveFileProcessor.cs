// For Directory.GetFiles and Directory.GetDirectories
// For File.Exists, Directory.Exists
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhatten_Calculator
{
    public class RecursiveFileProcessor
    {
        // Based on recusiveFileProecessor found at https://msdn.microsoft.com/en-us/library/c1sez4sc(v=vs.110).aspx
        // Finds the last box, goal and agent and then finds the distance between agent and box and box and goal -1 as only the agent needs to reach goal.
        public static int[,] actorpos; //= new int[16, 16];
        public static int[,] boxpos;
        public static StreamWriter sw;
        public static void RFPMain(string[] args)
        {
            foreach (string path in args)
            {
                if (File.Exists(path))// This path is a file
                {
                    ProcessFile(path);
                }
                else if (Directory.Exists(path))// This path is a directory
                {
                    ProcessDirectory(path);
                }
                else
                {
                    Console.WriteLine("{0} is not a valid file or directory.", path);
                }
            }
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
            if (path.Contains("properties")|| !path.Contains(".txt")) { return; }// We don't wanna run with our properties file
            string[] splitpath = path.Split('\\');
            sw = new StreamWriter(splitpath.Last());
            StreamReader sr = new StreamReader(path);
            string line;
            while ((line = sr.ReadLine()) != null && line != "")
            {
                int[] actorpos = new int[2];
                int[] boxpos = new int[2];
                int[] goalpos = new int[2];
                string[] output = new string[16];
                for (int i = 0; i < 16; i++) { output[i] = ""; }
                int linenumber = 0;
                for (int startindex = 0; startindex < 16 * 16 * 16; startindex += 16)
                {
                    linenumber = startindex / (16 * 16);
                    if (line[startindex + 1] == '1') { output[linenumber] +=  "+"; }

                    else if (line[startindex + 2] == '1') { output[linenumber] = output[linenumber] + "0"; actorpos[0] = output[linenumber].Length-1; actorpos[1] = linenumber; }
                    else if (line[startindex + 6] == '1') { output[linenumber] = output[linenumber] + "A"; boxpos[0] = output[linenumber].Length - 1; boxpos[1] = linenumber; }
                    else if (line[startindex + 10] == '1') { output[linenumber] = output[linenumber] + "a"; goalpos[0] = output[linenumber].Length - 1; goalpos[1] = linenumber; }
                    else if (line[startindex] == '1') { output[linenumber] = output[linenumber] + " "; }
                }
                string[] splitline = line.Split(' ');
                string levelname = "";
                for (int i = 1; i<splitline.Length-1; i++)
                {
                    levelname += splitline[i] + " ";
                }
                int distance = Math.Abs(actorpos[0] - boxpos[0]) + Math.Abs(actorpos[1] - boxpos[1]);
                distance += Math.Abs(boxpos[0] - goalpos[0]) + Math.Abs(boxpos[1] - goalpos[1])-1;
                if (distance == 0)
                {
                    System.Console.WriteLine("hey?");
                }
                sw.WriteLine(distance + " " + levelname);
            }
            sr.Close();
            sw.Flush();
        }
    }
}
