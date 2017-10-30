using System;
using System.Diagnostics;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

namespace Trainer
{
    class MLInput
    {
        public static ProcessStartInfo myProcessStartInfo;
        private static string appName;
        private static string path;
        private static string path2;
        static StreamWriter fs;
        static StreamWriter fs2;
        static Random rand;
        //cellpoint[] container;
        BitArray[] container;
        
        public MLInput(Map map)
        {
            //[Wall,Space,ActorID*4,Boxes*4,goals*4,Color*2]
            //container = new cellpoint[70 * 70];
            container = new BitArray[16*16];
            for (int i = 0; i < 16*16; i++)
            {
                container[i] = new BitArray(16);//= new cellpoint("");
            }
            Node[] boxes = map.getAllBoxes();
            int j = 0;
            while (j < Map.wallMap.Length)
            {
                for (int i = 0; i < Map.mapWidth; i++)
                {
                    container[i + j / Map.mapWidth * 16][1] = Map.wallMap[i + j / Map.mapWidth];
                    container[i + j / Map.mapWidth * 16][0] = (container[i + j / Map.mapWidth * 16][1] != true);
                    j++;
                }
            }
            
            foreach (Actor actor in map.getActors())
            {
                container[actor.x + actor.y * 16][2 + actor.id]=true;
                container[actor.x + actor.y * 16][14 + (byte)actor.getcolor()]=true;
            }
            for (int i = 0; i<boxes.Length; i++)
            {
                container[boxes[i].x + boxes[i].y * 16][6 + (byte)map.getBoxName(i)-'a'] = true;
                container[boxes[i].x + boxes[i].y * 16][14 + (byte)map.getColorOfBox(i)] = true;
                
            }
            foreach (char name in map.getGoalNames())
            {
                foreach (Node goal in map.getGoals(name))
                {
                    container[goal.x + goal.y * 16][10 + (byte)(name-'a')] = true;
                }
            }
            
            
        }
        public override string ToString()
        {
            char[] str = new char[16*16*16];
            int k = 0;
            for (int i = 0; i < 16*16; i++)
            {
                //char[] str2 = container[i].ToCharArray();
                for (int j = 0; j < 16; j++)
                {
                    str[k] = (container[i][j] ? '1' : '0');
                    k++;
                }
            }
            return new string(str);
        }
        public string ToStringAlt1()
        {
            char[] str = new char[16 * 16 * 16];
            int k = 0;
            for (int i = 0; i < 16 * 16; i++)
            {
                //char[] str2 = container[i].ToCharArray();
                for (int j = 0; j < 16; j++)
                {
                    str[k] = (container[15-i][j] ? '1' : '0');
                    k++;
                }
            }
            return new string(str);
        }
        public string ToStringAlt2()
        {
            char[] str = new char[16 * 16 * 16];
            int k = 0;
            for (int i = 0; i < 16 * 16; i++)
            {
                //char[] str2 = container[i].ToCharArray();
                for (int j = 0; j < 16; j++)
                {
                    str[k] = (container[i][15-j] ? '1' : '0');
                    k++;
                }
            }
            return new string(str);
        }
        public string ToStringAlt3()
        {
            char[] str = new char[16 * 16 * 16];
            int k = 0;
            for (int i = 0; i < 16 * 16; i++)
            {
                //char[] str2 = container[i].ToCharArray();
                for (int j = 0; j < 16; j++)
                {
                    str[k] = (container[15-i][15-j] ? '1' : '0');
                    k++;
                }
            }
            return new string(str);
        }
        public string ToStringAlt4()
        {
            char[] str = new char[16 * 16 * 16];
            int k = 0;
            for (int i = 0; i < 16 * 16; i++)
            {
                //char[] str2 = container[i].ToCharArray();
                for (int j = 0; j < 16; j++)
                {
                    str[k] = (container[j][i] ? '1' : '0');
                    k++;
                }
            }
            return new string(str);
        }
        public string ToStringAlt5()
        {
            char[] str = new char[16 * 16 * 16];
            int k = 0;
            for (int i = 0; i < 16 * 16; i++)
            {
                //char[] str2 = container[i].ToCharArray();
                for (int j = 0; j < 16; j++)
                {
                    str[k] = (container[15-j][i] ? '1' : '0');
                    k++;
                }
            }
            return new string(str);
        }
        public string ToStringAlt6()
        {
            char[] str = new char[16 * 16 * 16];
            int k = 0;
            for (int i = 0; i < 16 * 16; i++)
            {
                //char[] str2 = container[i].ToCharArray();
                for (int j = 0; j < 16; j++)
                {
                    str[k] = (container[j][15-i] ? '1' : '0');
                    k++;
                }
            }
            return new string(str);
        }
        public string ToStringAlt7()
        {
            char[] str = new char[16 * 16 * 16];
            int k = 0;
            for (int i = 0; i < 16 * 16; i++)
            {
                //char[] str2 = container[i].ToCharArray();
                for (int j = 0; j < 16; j++)
                {
                    str[k] = (container[15-j][15-i] ? '1' : '0');
                    k++;
                }
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
            AddLine(ToString(), shortestRoute);
            AddLine(ToStringAlt1(), shortestRoute);
            AddLine(ToStringAlt2(), shortestRoute);
            AddLine(ToStringAlt3(), shortestRoute);
            AddLine(ToStringAlt4(), shortestRoute);
            AddLine(ToStringAlt5(), shortestRoute);
            AddLine(ToStringAlt6(), shortestRoute);
            AddLine(ToStringAlt7(), shortestRoute);
            return true;
        }
        public bool AddLine(string input, int shortestRoute)
        {
            if (rand.Next() % 10 > 0)
            {
                try
                {
                    // Add some information to the file.
                    fs.WriteLine(input + " " + shortestRoute);
                    //System.Console.WriteLine(this.ToString());
                    fs.Flush();
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            else
            {
                try
                {
                    // Add some information to the file.
                    fs2.WriteLine(input + " " + shortestRoute);
                    //System.Console.WriteLine(this.ToString());
                    fs2.Flush();
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return false;
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
