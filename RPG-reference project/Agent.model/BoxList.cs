using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace Agent.model
{
    class BoxList
    {
        public Dictionary<char, BoxGroup> boxNameGroups;// for looking up boxes by name
        public Dictionary<Color, Collection<Node>> boxColorGroups; // for looking up boxes by color
        Collection<Node> boxes; // itterating across boxes
        public BoxList()
        {
            boxNameGroups = new Dictionary<char, BoxGroup>();
            boxColorGroups = new Dictionary<Color, Collection<Node>>();
            boxes = new Collection<Node>();

            boxColorGroups[Color.FromKnownColor(KnownColor.Gray)] = new Collection<Node>(); // we use grey as default color
        }
        public void Add(int x, int y, char name)
        {
            Node box = new Node(x, y);
            if (!boxNameGroups.ContainsKey(name)) { boxNameGroups[name] = new BoxGroup(name); boxColorGroups[Color.FromKnownColor(KnownColor.Gray)].Add(box); }
            else { boxColorGroups[boxNameGroups[name].getColor()].Add(box); }
            boxNameGroups[name].addbox(box);
            boxes.Add(box);
        }
        public void addBoxGroup( char name, Color color)
        {
            boxNameGroups[name] = new BoxGroup(name);
            boxNameGroups[name].setColor(color);
        }
        public void setColor(Color color, char name)
        {
            boxNameGroups[name].setColor(color);
        }
        public BoxGroup getBoxGroup (char name)
        {
            return boxNameGroups[name];
        }
        public Collection<Node> getBoxesOfColor(Color color) { return boxColorGroups[color]; }
        public Collection<Node> getAllBoxes() { return boxes; }
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
        public void setColor(Color color)
        {
            this.color = color;
        }
        public Color getColor() { return color; }
        public void addbox(Node box)
        {
            boxes.Add(box);
        }
    }
}