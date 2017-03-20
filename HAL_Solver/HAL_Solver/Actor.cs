using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Collections.ObjectModel;

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
        public List<Actions> getActions(Map currentMap, out Collection<Byte> boxesToMove )
        {
            boxesToMove = new Collection<Byte>();
            List<Actions> actionList = new List<Actions>();
            Byte box = 0;
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
                else if (currentMap.isBox(x, y + 1,getcolor(), out box))
                {
                    if (isempty[2,0]) { actionList.Add(Actions.UPRIGHT); boxesToMove.Add(box); }
                    if (isempty[0,0]) { actionList.Add(Actions.UPLEFT); boxesToMove.Add(box); }
                    if (currentMap.isEmptySpace(x, y + 2)) { actionList.Add(Actions.UPUP); boxesToMove.Add(box); }
                    if (isempty[1,2]) { actionList.Add(Actions.UPDOWN); boxesToMove.Add(box); }
                }
            }
            if (!currentMap.isWall(x, y - 1))
            {
                if (isempty[1,2]) { actionList.Add(Actions.DOWN); }
                else if (currentMap.isBox(x, y - 1, getcolor(), out box))
                {
                    if (isempty[2,2]) { actionList.Add(Actions.DOWNRIGHT); boxesToMove.Add(box); }
                    if (isempty[1,2]) { actionList.Add(Actions.DOWNLEFT); boxesToMove.Add(box); }
                    if (currentMap.isEmptySpace(x, y - 2)) { actionList.Add(Actions.DOWNDOWN); boxesToMove.Add(box); }
                    if (isempty[1,0]) { actionList.Add(Actions.DOWNUP); boxesToMove.Add(box); }
                }
            }
            if (!currentMap.isWall(x + 1, y))
            {
                if (isempty[2,1]) { actionList.Add(Actions.RIGHT); }
                else if (currentMap.isBox(x + 1, y, getcolor(), out box))
                {
                    if (isempty[2,0]) { actionList.Add(Actions.RIGHTUP); boxesToMove.Add(box); }
                    if (isempty[2,2]) { actionList.Add(Actions.RIGHTDOWN); boxesToMove.Add(box); }
                    if (currentMap.isEmptySpace(x + 2, y)) { actionList.Add(Actions.RIGHTRIGHT); boxesToMove.Add(box); }
                    if (isempty[0,1]) { actionList.Add(Actions.RIGHTLEFT); boxesToMove.Add(box); }
                }
            }
            if (!currentMap.isWall(x - 1, y))
            {
                if (currentMap.isEmptySpace(x - 1, y)) { actionList.Add(Actions.LEFT); }
                else if (currentMap.isBox(x - 1, y, getcolor(), out box))
                {
                    if (isempty[0,0]) { actionList.Add(Actions.LEFTUP); boxesToMove.Add(box); }
                    if (isempty[0,2]) { actionList.Add(Actions.LEFTDOWN); boxesToMove.Add(box); }
                    if (currentMap.isEmptySpace(x - 2, y)) { actionList.Add(Actions.LEFTLEFT); boxesToMove.Add(box); }
                    if (isempty[2,1]) { actionList.Add(Actions.LEFTRIGHT); boxesToMove.Add(box); }
                }
            }
            return actionList;
        }
    }
}
