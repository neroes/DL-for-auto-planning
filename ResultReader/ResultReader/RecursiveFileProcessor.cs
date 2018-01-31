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

namespace ResultReader
{
    public class RecursiveFileProcessor
    {
        public static void RFPMain(string[] args)
        {
            List<string>[] finalData = new List<string>[120];
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
                    ProcessDirectory(path,finalData);
                }
                else
                {
                    Console.WriteLine("{0} is not a valid file or directory.", path);
                }
            }
            DataWriter dw = new DataWriter("finaldata.csv");

            dw.Write(finalData);
        }


        // Process all files in the directory passed in, recurse on any directories 
        // that are found, and process the files they contain.
        public static void ProcessDirectory(string targetDirectory, List<string>[] finalData)
        {
            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            List<string> result;
            
            
            foreach (string fileName in fileEntries)
            {
                result = ProcessFile(fileName);
                
                if (result != null)
                {
                    int i = 0;
                    while (finalData[i] != null)
                    {
                        i++;
                    }
                    finalData[i] = result;
                }
            }
            
                

            // Recurse into subdirectories of this directory.
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
                ProcessDirectory(subdirectory, finalData);
        }

        // Insert logic for processing found files here.
        public static List<string> ProcessFile(string path)
        {
            if (path.Contains("results.txt"))
            {
                List<string> DataList = new List<string>();
                StreamReader sr = new StreamReader(path);
                String line;
                DataList.Add(path.Substring(0, path.Length - 12));
                while ((line = sr.ReadLine()) != null && line != "")
                {
                    String[] substrings = line.Split(' ');
                    String linedata = substrings[1]; // for classifier networks
                    if (linedata == "[")
                    {
                        linedata = substrings[2]; // for regression networks
                        linedata = linedata.Substring(0, linedata.Length - 1);
                    }
                        
                    DataList.Add(linedata);
                }
                return DataList;
            }
            return null;
        }
    }
}
