using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace Agent.model
{
    class BoxList
    {
        public Dictionary<char, Collection<Box>> boxNameGroups;// for looking up boxes by name
        public Dictionary<Color, Collection<Box>> boxColorGroups; // for looking up boxes by color
        // Collection<Node> boxes; // itterating across boxes
        public Box[] boxes;

        /*public BoxList()
        {
            boxNameGroups = new Dictionary<char, BoxGroup>();
            boxColorGroups = new Dictionary<Color, Collection<Node>>();
            boxes = new Collection<Node>();

            boxColorGroups[Color.FromKnownColor(KnownColor.Gray)] = new Collection<Node>(); // we use grey as default color
        }*/
        public BoxList(Collection<Box> newboxes)
        {
            boxes = new Box[newboxes.Count];
            boxNameGroups = new Dictionary<char, Collection<Box>>();
            boxColorGroups = new Dictionary<Color, Collection<Box>>();
            int i = 0;
            foreach (Box box in newboxes)
            {
                boxes[i] = box;
                if (!boxNameGroups.ContainsKey(box.name)) {
                    boxNameGroups[box.name] = new Collection<Box>(); }
                boxNameGroups[box.name].Add(box);
                if (!boxColorGroups.ContainsKey(box.color)) {
                    boxColorGroups[box.color] = new Collection<Box>();
                }
                if (!boxColorGroups[box.color].Contains(box)) { boxColorGroups[box.color].Add(box); }
                    i++;
            }
        }
        
        public Collection<Box> getBoxesOfColor(Color color) { return boxColorGroups[color]; }
        public Collection<Box> getBoxesOfName(char name) { return boxNameGroups[name]; }
        public Box[] getAllBoxes() { return boxes; }
    }
}