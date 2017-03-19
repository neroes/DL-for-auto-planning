using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace HAL_Solver
{
    class BoxList
    {
        public static Dictionary<char, Collection<int>> boxNameGroups;// for looking up boxes by name
        public static Dictionary<Color, Collection<int>> boxColorGroups; // for looking up boxes by color
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
            boxNameGroups = new Dictionary<char, Collection<int>>();
            boxColorGroups = new Dictionary<Color, Collection<int>>();
            int i = 0;
            foreach (Box box in newboxes)
            {
                boxes[i] = box;
                if (!boxNameGroups.ContainsKey(box.name)) {
                    boxNameGroups[box.name] = new Collection<int>(); }
                boxNameGroups[box.name].Add(i);
                if (!boxColorGroups.ContainsKey(box.color)) {
                    boxColorGroups[box.color] = new Collection<int>();
                }
                boxColorGroups[box.color].Add(i);
                i++;
            }
        }
        
        public Collection<Box> getBoxesOfColor(Color color)
        {
            Collection<Box> returnCollection = new Collection<Box>();
            foreach (int i in boxColorGroups[color])
            {
                returnCollection.Add(boxes[i]);
            }
            return returnCollection;
        }
        public Collection<Box> getBoxesOfName(char name)
        {
            Collection<Box> returnCollection = new Collection<Box>();
            foreach (int i in boxNameGroups[name])
            {
                returnCollection.Add(boxes[i]);
            }
            return returnCollection;
        }
        public Box[] getAllBoxes() { return boxes; }
    }
}