using System;
using System.Diagnostics;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

namespace Runner
{
    class MLInput
    {
        // Simple structure for converting the current state into a single string in a 16x16 structure of what is in the structure [Walkable, Wall, 4xActor, 4xBox, 4xGoal, 2xColor]
        public static ProcessStartInfo myProcessStartInfo;
        private static string appName;
        private static string path;
        private static string path2;
        static StreamWriter fs;
        static StreamWriter fs2;
        static Random rand;
        //cellpoint[] container;
        BitArray[,] container;

        public MLInput(Map map)
        {
            //[Wall,Space,ActorID*4,Boxes*4,goals*4,Color*2]
            //container = new cellpoint[70 * 70];
            container = new BitArray[16, 16];
            for (int i = 0; i < 16; i++)
            {
                for (int k = 0; k < 16; k++)
                {
                    container[i, k] = new BitArray(16);//= new cellpoint("");
                }
            }
            Node[] boxes = map.getAllBoxes();

            for (int j = 0; j < (Map.wallMap.Length / Map.mapWidth); j++)
            {
                for (int i = 0; i < Map.mapWidth; i++)
                {
                    container[i, j][1] = Map.wallMap[i + j * Map.mapWidth];
                    container[i, j][0] = (container[i, j][1] != true);
                }
            }

            foreach (Actor actor in map.getActors())
            {
                container[actor.x, actor.y][2 + actor.id] = true;
                container[actor.x, actor.y][14 + (byte)actor.getcolor()] = true;
            }
            for (int i = 0; i < boxes.Length; i++)
            {
                container[boxes[i].x, boxes[i].y][6 + (byte)map.getBoxName(i) - 'a'] = true;
                container[boxes[i].x, boxes[i].y][14 + (byte)map.getColorOfBox(i)] = true;

            }
            foreach (char name in map.getGoalNames())
            {
                foreach (Node goal in map.getGoals(name))
                {
                    container[goal.x, goal.y][10 + (byte)(name - 'a')] = true;
                }
            }


        }
        public override string ToString()
        {
            char[] str = new char[16 * 16 * 16];
            int k = 0;
            for (int i1 = 0; i1 < 16; i1++)
            {
                for (int i2 = 0; i2 < 16; i2++)
                {
                    for (int j = 0; j < 16; j++)
                    {
                        str[k] = (container[i1, i2][j] ? '1' : '0');
                        k++;
                    }
                }
                //char[] str2 = container[i].ToCharArray();

            }
            return new string(str);
        }
        public static void setup(string path, string path2)
        {
            MLInput.rand = new Random();
            MLInput.path = path;
            MLInput.path2 = path2;
            /*// Delete the file if it exists.
            if (File.Exists(path))
            {
                // Note that no lock is put on the
                // file and the possibility exists
                // that another process could do
                // something with it between
                // the calls to Exists and Delete.
                File.Delete(path);
            }*/
            using (FileStream fs = File.Create(path)) { }
            fs = new StreamWriter(path);
            using (FileStream fs = File.Create(path2)) { }
            fs2 = new StreamWriter(path2);
            //setup();
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
        public string run(int shortestRoute)
        {
            /*
            // make sure we can read the output from stdout 
            myProcessStartInfo.UseShellExecute = false;
            myProcessStartInfo.RedirectStandardOutput = true;

            myProcessStartInfo.Arguments = appName + " " + ToString()+ " " + shortestRoute;


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
            */
            Writer(shortestRoute);
            return "";
        }
        public bool Writer(int shortestRoute)
        {
            if (rand.Next() % 10 > 0)
            {
                try
                {
                    // Add some information to the file.
                    fs.WriteLine(this.ToString() + " " + shortestRoute);
                    //System.Console.WriteLine(this.ToString());
                    fs.Flush();
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            else
            {
                try
                {
                    // Add some information to the file.
                    fs2.WriteLine(this.ToString() + " " + shortestRoute);
                    //System.Console.WriteLine(this.ToString());
                    fs2.Flush();
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            return true;
        }
    }
    struct cellpoint
    {
        Byte[] bites;
        public cellpoint(string arg)
        {
            bites = new Byte[9];
            for (int i = 0; i < 9; i++) { bites[i] = 0; }
        }
        public bool this[int i] { get { return getbit(bites[i / 8],i%8); } set { /*byte newData =  (byte) (1 << i%8); bites[i/8] = (byte)((bites[i/8] & ~newData) & (value ? 1 : 0) << i);*/ if (value = !getbit(bites[i / 8], i % 8)) { bites[i / 8] = (byte)(value ? bites[i / 8] + (1 << i % 8) : bites[i / 8] - (1 << i % 8)); } } }
        public bool getbit(Byte row, int i)
        {
            return (row & (i<<i)) != 0;
        }
        public int getintbit(Byte row, int i)
        {
            return (1 & (row << i));
        }
        public char[] ToCharArray()
        {
            char[] str = new char[72];
            for (int i = 0; i < 72; i++) { str[i] = (this[i] ? '1' : '0'); if (this[i]) { System.Console.WriteLine(this[i].ToString()); } }
            return str;
        }
    }
}
