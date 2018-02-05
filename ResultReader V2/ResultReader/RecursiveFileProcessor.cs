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
            List<List<string>> finalData = new List<List<string>>();
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
        public static void ProcessDirectory(string targetDirectory, List<List<string>> finalData)
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
                    finalData.Add(result);
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
                List<string> DataList2 = new List<string>();
                StreamReader sr = new StreamReader(path);
                String line;
                DataList.Add("Classifier "+path.Substring(0, path.Length - 12));
                DataList2.Add("Regression " + path.Substring(0, path.Length - 12));
                while ((line = sr.ReadLine()) != null && line != "")
                {
                    line = line.Replace("[", string.Empty);
                    line = line.Replace("]", string.Empty);
                    String[] substrings = line.Split(' ');
                    substrings = substrings.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                    DataList.Add(substrings[2]);
                    DataList2.Add(substrings[3]);
                }
                DataList.Add("");
                DataList.AddRange(DataList2);
                return DataList;
            }
            return null;
        }
    }
}
