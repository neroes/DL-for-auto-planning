using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace HAL_Solver
{
    public class BoxList
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

        public override int GetHashCode()
        {
            int prime = 53;
            int result = 1;

            for (int i = 0; i < boxes.Length; i++)
            {
                result = prime * result + boxes[i].GetHashCode();
            }

            return result;
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
                return true;
            if (obj == null || !(obj is BoxList))
                return false;
            BoxList bl = (BoxList)obj;
            for (int i = 0; i < boxes.Length; i++)
            {
                if (this.boxes[i] != bl.boxes[i]) { return false; }
            }
            return true;
        }

        public BoxList(Dictionary<Node, char> newboxes, Dictionary<char, Color> colorDict)
        {
            boxes = new Node[newboxes.Count];
            boxNameGroups = new Dictionary<char, Collection<int>>();
            boxColorGroups = new Dictionary<Color, Collection<int>>();
            int i = 0;
            foreach (KeyValuePair<Node, char> box in newboxes)
            {
                Color col = colorDict[box.Value];
                boxes[i] = box.Key;
                if (!boxNameGroups.ContainsKey(box.Value)) {
                    boxNameGroups[box.Value] = new Collection<int>(); }
                boxNameGroups[box.Value].Add(i);
                if (!boxColorGroups.ContainsKey(col)) {
                    boxColorGroups[col] = new Collection<int>();
                }
                boxColorGroups[col].Add(i);
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