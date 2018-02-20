using System;

namespace Trainer
{
    // A structure for discribing an object such as the goal or the box's location with a series of basic operators
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
        public static bool operator ==(Node x, Node y)
        {
            return (x.x == y.x && x.y == y.y);
        }
        public static bool operator ==(Actor x, Node y) // Is on top of Actor
        {
            return (x.x == y.x && x.y == y.y);
        }
        public static bool operator ==(Node x, Actor y) // Ditto
        {
            return (x.x == y.x && x.y == y.y);
        }
        public static bool operator !=(Node x, Node y)
        {
            return !(x.x == y.x && x.y == y.y);
        }
        public static bool operator !=(Actor x, Node y) // Is not on top of Actor
        {
            return !(x.x == y.x && x.y == y.y);
        }
        public static bool operator !=(Node x, Actor y) // Ditto
        {
            return !(x.x == y.x && x.y == y.y);
        }
        public static int operator -(Node x, Node y) // Distance from other node.
        {
            return (Math.Abs(x.x - y.x) + Math.Abs(x.y - y.y));
        }
        public static int operator -(Actor x, Node y) // Distance from actor as well.
        {
            return (Math.Abs(x.x - y.x) + Math.Abs(x.y - y.y));
        }
        public static int operator -(Node x, Actor y) // Ditto
        {
            return (Math.Abs(x.x - y.x) + Math.Abs(x.y - y.y));
        }
        public override bool Equals(Object obj)
        {
            return obj is Node && this == (Node)obj;
        }
        public override int GetHashCode()
        {
            return 37 * (37 + this.x) + this.y;
        }
    }
}
