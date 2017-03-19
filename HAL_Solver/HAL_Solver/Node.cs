using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace HAL_Solver
{
    public struct Node
    {
        public Byte x;
        public Byte y;
        public Node (Byte x, Byte y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
