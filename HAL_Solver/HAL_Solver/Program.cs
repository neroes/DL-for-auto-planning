using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HAL_Solver
{
    class Program
    {
        static void Main(string[] args)
        {
            Map map = null;
            MapLoad.loadMap("MAsimple1.lvl", out map);

            System.Console.Write("pizza");
             
        }
    }
}
