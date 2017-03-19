using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace HAL_Solver
{
    class Actor
    {
        public Byte x;
        public Byte y;
        public Byte id;
        public enum Actions { UP, DOWN, LEFT, RIGHT, NONE,
                              UPUP, UPLEFT, UPRIGHT, UPDOWN,
                              DOWNUP, DOWNLEFT, DOWNRIGHT, DOWNDOWN,
                              LEFTUP, LEFTLEFT, LEFTRIGHT, LEFTDOWN,
                              RIGHTUP, RIGHTLEFT, RIGHTRIGHT, RIGHTDOWN};

        public Actor (Byte x, Byte y, Byte id)
        {
            this.x = x;
            this.y = y;
            this.id = id;
        }
        public Color getcolor()
        {
            return ActorList.intToColorDict[id];
        }
        public List<Actions> getActions(Map currentMap)
        {
            List<Actions> actionList = new List<Actions>();

            bool[,] isempty = new bool[3, 3];
            isempty[0, 0] = currentMap.isEmptySpace(x - 1, y + 1);
            isempty[1, 0] = currentMap.isEmptySpace(x    , y + 1);
            isempty[2, 0] = currentMap.isEmptySpace(x + 1, y + 1);
            isempty[0, 1] = currentMap.isEmptySpace(x - 1, y    );
            isempty[2, 1] = currentMap.isEmptySpace(x + 1, y    );
            isempty[0, 2] = currentMap.isEmptySpace(x - 1, y - 1);
            isempty[1, 2] = currentMap.isEmptySpace(x    , y - 1);
            isempty[2, 2] = currentMap.isEmptySpace(x + 1, y - 1);


            if (!currentMap.isWall(x, y + 1))
            {
                if (isempty[1,0]) { actionList.Add(Actions.UP); }
                else if (currentMap.isBox(x, y + 1,getcolor()))
                {
                    if (isempty[2,0]) { actionList.Add(Actions.UPRIGHT); }
                    if (isempty[0,0]) { actionList.Add(Actions.UPLEFT); }
                    if (currentMap.isEmptySpace(x, y + 2)) { actionList.Add(Actions.UPUP); }
                    if (isempty[1,2]) { actionList.Add(Actions.UPDOWN); }
                }
            }
            if (!currentMap.isWall(x, y - 1))
            {
                if (isempty[1,2]) { actionList.Add(Actions.DOWN); }
                else if (currentMap.isBox(x, y - 1, getcolor()))
                {
                    if (isempty[2,2]) { actionList.Add(Actions.DOWNRIGHT); }
                    if (isempty[1,2]) { actionList.Add(Actions.DOWNLEFT); }
                    if (currentMap.isEmptySpace(x, y - 2)) { actionList.Add(Actions.DOWNDOWN); }
                    if (isempty[1,0]) { actionList.Add(Actions.DOWNUP); }
                }
            }
            if (!currentMap.isWall(x + 1, y))
            {
                if (isempty[2,1]) { actionList.Add(Actions.RIGHT); }
                else if (currentMap.isBox(x + 1, y, getcolor()))
                {
                    if (isempty[2,0]) { actionList.Add(Actions.RIGHTUP); }
                    if (isempty[2,2]) { actionList.Add(Actions.RIGHTDOWN); }
                    if (currentMap.isEmptySpace(x + 2, y)) { actionList.Add(Actions.RIGHTRIGHT); }
                    if (isempty[0,1]) { actionList.Add(Actions.RIGHTLEFT); }
                }
            }
            if (!currentMap.isWall(x - 1, y))
            {
                if (currentMap.isEmptySpace(x - 1, y)) { actionList.Add(Actions.LEFT); }
                else if (currentMap.isBox(x - 1, y, getcolor()))
                {
                    if (isempty[0,0]) { actionList.Add(Actions.LEFTUP); }
                    if (isempty[0,2]) { actionList.Add(Actions.LEFTDOWN); }
                    if (currentMap.isEmptySpace(x - 2, y)) { actionList.Add(Actions.LEFTLEFT); }
                    if (isempty[2,1]) { actionList.Add(Actions.LEFTRIGHT); }
                }
            }
            return actionList;
        }
    }
}
