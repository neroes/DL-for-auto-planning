using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace HAL_Solver
{
    public class Box : Node
    {
        public Color color;
        public char name;
        public Box(Byte x, Byte y, char name) : base(x, y)
        {
            this.name = name;
            color = Color.FromKnownColor(KnownColor.Gray);
        }
        public Box(Byte x, Byte y, Color color, char name) : base(x, y)
        {
            this.name = name;
            this.color = color;
        }
    }
}
