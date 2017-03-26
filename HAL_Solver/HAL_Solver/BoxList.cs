using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace HAL_Solver
{
    class BoxList
    {
        public static Dictionary<char, Collection<int>> boxNameGroups;// for looking up boxes by name
        public static Dictionary<Color, Collection<int>> boxColorGroups; // for looking up boxes by color
        // Collection<Node> boxes; // itterating across boxes
        public Node[] boxes;

        /*public BoxList()
        {
            boxNameGroups = new Dictionary<char, BoxGroup>();
            boxColorGroups = new Dictionary<Color, Collection<Node>>();
            boxes = new Collection<Node>();

            boxColorGroups[Color.FromKnownColor(KnownColor.Gray)] = new Collection<Node>(); // we use grey as default color
        }*/
        public BoxList(Collection<Node> newboxes, Collection<char> newboxnames, Collection<Color> newboxcolors)
        {
            boxes = new Node[newboxes.Count];
            boxNameGroups = new Dictionary<char, Collection<int>>();
            boxColorGroups = new Dictionary<Color, Collection<int>>();
            Byte i = 0;
            IEnumerator<char> nameEnum = newboxnames.GetEnumerator();
            IEnumerator<Color> colorEnum = newboxcolors.GetEnumerator();
            foreach (Node box in newboxes)
            {
                boxes[i] = box;
                if (!boxNameGroups.ContainsKey(nameEnum.Current)) {
                    boxNameGroups[nameEnum.Current] = new Collection<int>(); }
                boxNameGroups[nameEnum.Current].Add(i);
                if (!boxColorGroups.ContainsKey(colorEnum.Current)) {
                    boxColorGroups[colorEnum.Current] = new Collection<int>();
                }
                boxColorGroups[colorEnum.Current].Add(i);
                nameEnum.MoveNext();
                colorEnum.MoveNext();
                i++;
            }
        }
        public BoxList(BoxList oldlist)
        {
            boxes = (Node[])oldlist.boxes.Clone();
        }

        public Node this[int i] { get { return boxes[i]; } set { boxes[i] = value; } }

        public void MoveBox(int id, Byte x, Byte y)
        {
            boxes[id] = new Node(x, y);
        }

        public Collection<int> getBoxesOfColor(Color color)  { return boxColorGroups[color]; }
        public Collection<Node> getBoxesOfName(char name)
        {
            Collection<Node> returnCollection = new Collection<Node>();
            foreach (int i in boxNameGroups[name])
            {
                returnCollection.Add(boxes[i]);
            }
            return returnCollection;
        }
        public Node[] getAllBoxes() { return boxes; }
    }
}