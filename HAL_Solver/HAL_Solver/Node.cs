using System;

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
        public Node(Node old)
        {
            this.x = old.x;
            this.y = old.y;
        }
    }
}
