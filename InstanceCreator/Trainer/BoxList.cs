using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Trainer
{
    // Container for all the boxes allowing lookup and comparison of all the current boxes
    public class BoxList
    {
        public static Dictionary<char, HashSet<int>> boxNameGroups;// for looking up boxes by name
        public static Dictionary<Color, HashSet<int>> boxColorGroups; // for looking up boxes by color
        public static Dictionary<int, Color> colorBox; // for getting the color of a box
        // HashSet<Node> boxes; // itterating across boxes
        public Node[] boxes;

        /*public BoxList()
        {
            boxNameGroups = new Dictionary<char, BoxGroup>();
            boxColorGroups = new Dictionary<Color, HashSet<Node>>();
            boxes = new HashSet<Node>();

            boxColorGroups[Color.FromKnownColor(KnownColor.Gray)] = new HashSet<Node>(); // we use grey as default color
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
            boxNameGroups = new Dictionary<char, HashSet<int>>();
            boxColorGroups = new Dictionary<Color, HashSet<int>>();
            colorBox = new Dictionary<int, Color>(); // Note: For every box because it's easier to lookup.
            int i = 0;
            foreach (KeyValuePair<Node, char> box in newboxes)
            {
                Color col = colorDict[box.Value];
                boxes[i] = box.Key;
                colorBox[i] = col;
                if (!boxNameGroups.ContainsKey(box.Value)) {
                    boxNameGroups[box.Value] = new HashSet<int>();
                }
                boxNameGroups[box.Value].Add(i);
                if (!boxColorGroups.ContainsKey(col)) {
                    boxColorGroups[col] = new HashSet<int>();
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

        public Color getColorOfBox(int boxID)
        {
            return colorBox[boxID];
        }

        public char getBoxName(int boxID)
        {
            foreach (KeyValuePair<char, HashSet<int>> boxes in boxNameGroups)
            {
                if (boxes.Value.Contains(boxID))
                {
                    return boxes.Key;
                }
            }
            return '0';
        }

        public HashSet<int> getBoxesOfColor(Color color)  { return boxColorGroups.ContainsKey(color) ? boxColorGroups[color] : new HashSet<int>(); }
        public HashSet<Node> getBoxesOfName(char name)
        {
            HashSet<Node> returnHashSet = new HashSet<Node>();
            foreach (int i in boxNameGroups[name])
            {
                returnHashSet.Add(boxes[i]);
            }
            return returnHashSet;
        }
        public Node[] getAllBoxes() { return boxes; }
    }
}