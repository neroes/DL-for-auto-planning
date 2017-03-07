using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Agent.model
{
    class Actor
    {
        Color color;
        char name;
        int x;
        int y;
        public enum Actions { UP, DOWN, LEFT, RIGHT, NONE,
                              UPUP, UPLEFT, UPRIGHT, UPDOWN,
                              DOWNUP, DOWNLEFT, DOWNRIGHT, DOWNDOWN,
                              LEFTUP, LEFTLEFT, LEFTRIGHT, LEFTDOWN,
                              RIGHTUP, RIGHTLEFT, RIGHTRIGHT, RIGHTDOWN};

        public Actor (int x, int y, char name)
        {
            this.x = x;
            this.y = y;
            this.color = Color.FromKnownColor(KnownColor.DarkGray);
            this.name = name;
        }
        public Actor (Color color, char name)
        {
            this.color = color;
            this.name = name;
        }
        public void addPos(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public List<Actions> getActions(Map currentMap)
        {
            List<Actions> actionList = new List<Actions>();
            if (!currentMap.isWall(x, y + 1))
            {
                if (currentMap.isEmptySpace(x, y + 1)) { actionList.Add(Actions.UP); }
                else if (currentMap.isBox(x, y + 1,color))
                {
                    if (currentMap.isEmptySpace(x + 1, y + 1)) { actionList.Add(Actions.UPRIGHT); }
                    if (currentMap.isEmptySpace(x - 1, y + 1)) { actionList.Add(Actions.UPLEFT); }
                    if (currentMap.isEmptySpace(x, y + 2)) { actionList.Add(Actions.UPUP); }
                    if (currentMap.isEmptySpace(x, y - 1)) { actionList.Add(Actions.UPDOWN); }
                }
            }
            if (!currentMap.isWall(x, y - 1))
            {
                if (currentMap.isEmptySpace(x, y - 1)) { actionList.Add(Actions.DOWN); }
                else if (currentMap.isBox(x, y - 1, color))
                {
                    if (currentMap.isEmptySpace(x + 1, y - 1)) { actionList.Add(Actions.DOWNRIGHT); }
                    if (currentMap.isEmptySpace(x - 1, y - 1)) { actionList.Add(Actions.DOWNLEFT); }
                    if (currentMap.isEmptySpace(x, y - 2)) { actionList.Add(Actions.DOWNDOWN); }
                    if (currentMap.isEmptySpace(x, y + 1)) { actionList.Add(Actions.DOWNUP); }
                }
            }
            if (!currentMap.isWall(x + 1, y))
            {
                if (currentMap.isEmptySpace(x + 1, y)) { actionList.Add(Actions.RIGHT); }
                else if (currentMap.isBox(x + 1, y, color))
                {
                    if (currentMap.isEmptySpace(x + 1, y + 1)) { actionList.Add(Actions.RIGHTUP); }
                    if (currentMap.isEmptySpace(x + 1, y - 1)) { actionList.Add(Actions.RIGHTDOWN); }
                    if (currentMap.isEmptySpace(x + 2, y)) { actionList.Add(Actions.RIGHTRIGHT); }
                    if (currentMap.isEmptySpace(x - 1, y)) { actionList.Add(Actions.RIGHTLEFT); }
                }
            }
            if (!currentMap.isWall(x - 1, y))
            {
                if (currentMap.isEmptySpace(x - 1, y)) { actionList.Add(Actions.LEFT); }
                else if (currentMap.isBox(x - 1, y, color))
                {
                    if (currentMap.isEmptySpace(x - 1, y + 1)) { actionList.Add(Actions.LEFTUP); }
                    if (currentMap.isEmptySpace(x - 1, y - 1)) { actionList.Add(Actions.LEFTDOWN); }
                    if (currentMap.isEmptySpace(x - 2, y)) { actionList.Add(Actions.LEFTLEFT); }
                    if (currentMap.isEmptySpace(x + 1, y)) { actionList.Add(Actions.LEFTRIGHT); }
                }
            }
            return actionList;
        }
    }
}
