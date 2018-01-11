using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HAL_Solver
{
    class MLInput
    {
        public static ProcessStartInfo myProcessStartInfo;
        private static string appName;
        cellpoint[] container;
        public MLInput(Map map)
        {
            container = new cellpoint[70 * 70];
            Node[] boxes = map.getAllBoxes();
            int j = 0;
            while (j < Map.wallMap.Length)
            {
                for (int i = 0; i < Map.mapWidth; i++)
                {
                    container[i + j / Map.mapWidth * 70][1] = Map.wallMap[i + j / Map.mapWidth];
                    container[i + j / Map.mapWidth * 70][0] = container[i + j / Map.mapWidth * 70][1] != true;
                    j++;
                }
            }
            foreach (Actor actor in map.getActors())
            {
                container[actor.x + actor.y * 70][2 + actor.id]=true;
                container[actor.x + actor.y * 70][60 + (byte)actor.getcolor()]=true;
            }
            for (int i = 0; i<boxes.Length; i++)
            {
                container[boxes[i].x + boxes[i].y * 70][22 + (byte)map.getBoxName(i)] = true;
                container[boxes[i].x + boxes[i].y * 70][60 + (byte)map.getColorOfBox(i)] = true;
                
            }
            
            foreach (char name in map.getGoalNames())
            {
                foreach (Node goal in map.getGoals(name))
                {
                    container[goal.x + goal.y * 70][36 + (byte)(name-'0')] = true;
                }
            }
            
            
        }
        public override string ToString()
        {
            char[] str = new char[70*70*72];
            int k = 0;
            for (int i = 0; i < 70*70; i++)
            {
                char[] str2 = container[i].ToCharArray();
                for (int j = 0; j < 72; j++)
                {
                    str[k] = str[j];
                    k++;
                }
            }
            return str.ToString();
        }
        public static void setup()
        {
            // full path of python interpreter  
            string python = @"C:\Program Files\Python36\python.exe";

            // python app to call  
            appName = "PythonApplication1.py";

            // Create new process start info 
            ProcessStartInfo myProcessStartInfo = new ProcessStartInfo(python);
        }
        public string run()
        {
            // make sure we can read the output from stdout 
            myProcessStartInfo.UseShellExecute = false;
            myProcessStartInfo.RedirectStandardOutput = true;

            myProcessStartInfo.Arguments = appName + " " + ToString();


            Process myProcess = new Process();
            // assign start information to the process 
            myProcess.StartInfo = myProcessStartInfo;

            // start process 
            myProcess.Start();

            // Read the standard output of the app we called.  
            StreamReader myStreamReader = myProcess.StandardOutput;
            string myString = myStreamReader.ReadLine();

            // wait exit signal from the app we called 
            myProcess.WaitForExit();

            // close the process 
            myProcess.Close();


            return myString;
        }
    }
    struct cellpoint
    {
        Byte[] bites;
        public cellpoint(string arg)
        {
            bites = new Byte[9];
        }
        public bool this[int i] { get { return getbit(bites[i / 8],i%8); } set { byte newData =  (byte) (1 << i%8); bites[i/8] = (byte)((bites[i/8] & ~newData) & (value ? 1 : 0) << i); } }
        public bool getbit(Byte row, int i)
        {
            return (1&(row<<i)) != 0;
        }
        public int getintbit(Byte row, int i)
        {
            return (1 & (row << i));
        }
        public char[] ToCharArray()
        {
            char[] str = new char[72];
            for (int i = 0; i < 64; i++) { str[i] = getintbit(bites[i / 8], i % 8).ToString()[0]; }
            return str;
        }
    }
}
