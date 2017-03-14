using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Agent.model
{
    public class Node
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
