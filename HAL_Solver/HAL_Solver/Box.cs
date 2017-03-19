using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace HAL_Solver
{
    public class Box
    {
        public Color color;
        public char name;
        public Box(Byte x, Byte y, char name) 
        {
            this.name = name;
            color = Color.FromKnownColor(KnownColor.Gray);
        }
        public Box(Byte x, Byte y, Color color, char name)
        {
            this.name = name;
            this.color = color;
        }
    }
}
