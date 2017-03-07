using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace Agent.model
{
    class BoxList
    {
        public Dictionary<char, BoxGroup> boxgroups;
        public BoxList()
        {
            boxgroups = new Dictionary<char, BoxGroup>();
        }
        public void addBox(int x, int y, char name)
        {
            if (!boxgroups.ContainsKey(name)) { boxgroups[name] = new BoxGroup(name); }
            boxgroups[name].addbox(x, y);
        }
        public void addBoxGroup( char name, Color color)
        {
            boxgroups[name] = new BoxGroup(name);
            boxgroups[name].setColor(color);
        }
        public class BoxGroup
        {
            char name;
            Color color;
            Collection<Node> boxes;
            public BoxGroup(char name)
            {
                this.name = name;
                boxes = new Collection<Node>();
                color = Color.FromKnownColor(KnownColor.Blue);
            }
            public void setColor (Color color)
            {
                this.color = color;
            }
            public void addbox(int x, int y)
            {
                boxes.Add(new Node(x, y));
            }
        }
    }
}