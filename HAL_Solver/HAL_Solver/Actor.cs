using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace HAL_Solver
{
    public enum Direction : Byte { N, S, E, W, NONE };
    public enum Interact : Byte { PUSH, PULL, MOVE, WAIT };
    class Actor
    {
        public Byte x;
        public Byte y;
        public Byte id;
        

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
        public Collection<act> getActions(Map currentMap )
        {
            Collection<act> actionList = new Collection<act>();
            int box = 0;
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
                if (isempty[1,0]) { actionList.Add(new act(Interact.MOVE,Direction.N)); }
                else if (currentMap.isBox(x, y + 1,getcolor(), out box))
                {
                    if (isempty[2, 0]) { actionList.Add(new act(Interact.PUSH,Direction.E,Direction.N,box)); }
                    if (isempty[0, 0]) { actionList.Add(new act(Interact.PUSH, Direction.W, Direction.N, box)); }
                    if (currentMap.isEmptySpace(x, y + 2)) { actionList.Add(new act(Interact.PUSH, Direction.N, Direction.N, box)); }
                    if (isempty[1, 2]) { actionList.Add(new act(Interact.PULL, Direction.S, Direction.N, box)); }
                    if (isempty[2, 1]) { actionList.Add(new act(Interact.PULL, Direction.E, Direction.N, box)); }
                    if (isempty[0, 1]) { actionList.Add(new act(Interact.PULL, Direction.W, Direction.N, box)); }
                }
            }
            if (!currentMap.isWall(x, y - 1))
            {
                if (isempty[1,2]) { actionList.Add(new act(Interact.MOVE, Direction.S)); }
                else if (currentMap.isBox(x, y - 1, getcolor(), out box))
                {
                    if (isempty[2, 2]) { actionList.Add(new act(Interact.PUSH, Direction.E, Direction.S, box)); }
                    if (isempty[0, 2]) { actionList.Add(new act(Interact.PUSH, Direction.W, Direction.S, box)); }
                    if (currentMap.isEmptySpace(x, y - 2)) { actionList.Add(new act(Interact.PUSH, Direction.S, Direction.S, box)); }
                    if (isempty[1, 0]) { actionList.Add(new act(Interact.PULL, Direction.N, Direction.S, box)); }
                    if (isempty[2, 1]) { actionList.Add(new act(Interact.PULL, Direction.E, Direction.S, box)); }
                    if (isempty[0, 1]) { actionList.Add(new act(Interact.PULL, Direction.W, Direction.S, box)); }
                }
            }
            if (!currentMap.isWall(x + 1, y))
            {
                if (isempty[2,1]) { actionList.Add(new act(Interact.MOVE, Direction.E)); }
                else if (currentMap.isBox(x + 1, y, getcolor(), out box))
                {
                    if (isempty[2, 0]) { actionList.Add(new act(Interact.PUSH, Direction.N, Direction.E, box)); }
                    if (isempty[2, 2]) { actionList.Add(new act(Interact.PUSH, Direction.S, Direction.E, box)); }
                    if (currentMap.isEmptySpace(x + 2, y)) { actionList.Add(new act(Interact.PUSH, Direction.E, Direction.E, box)); }
                    if (isempty[0, 1]) { actionList.Add(new act(Interact.PULL, Direction.W, Direction.E, box)); }
                    if (isempty[1, 0]) { actionList.Add(new act(Interact.PULL, Direction.N, Direction.E, box)); }
                    if (isempty[1, 2]) { actionList.Add(new act(Interact.PULL, Direction.S, Direction.E, box)); }
                }
            }
            if (!currentMap.isWall(x - 1, y))
            {
                if (isempty[0, 1]) { actionList.Add(new act(Interact.MOVE, Direction.W)); }
                else if (currentMap.isBox(x - 1, y, getcolor(), out box))
                {
                    if (isempty[0, 0]) { actionList.Add(new act(Interact.PUSH, Direction.N, Direction.W, box)); }
                    if (isempty[0, 2]) { actionList.Add(new act(Interact.PUSH, Direction.S, Direction.W, box)); }
                    if (currentMap.isEmptySpace(x - 1, y)) { actionList.Add(new act(Interact.PUSH, Direction.W, Direction.W, box)); }
                    if (isempty[2, 1]) { actionList.Add(new act(Interact.PULL, Direction.E, Direction.W, box)); }
                    if (isempty[1, 0]) { actionList.Add(new act(Interact.PULL, Direction.N, Direction.W, box)); }
                    if (isempty[1, 2]) { actionList.Add(new act(Interact.PULL, Direction.S, Direction.W, box)); }
                }
            }
            actionList.Add(new act(Interact.WAIT));
            return actionList;
        }
    }
    public struct act
    {
        public Interact inter;
        public Direction dir;
        public Direction boxdir;
        public int box;
        public act(Interact inter, Direction dir, Direction boxdir, int box)
        {
            this.inter = inter;
            this.dir = dir;
            this.boxdir = boxdir;
            this.box = box;
        }
        public act(Interact inter, Direction dir)
        {
            this.inter = inter;
            this.dir = dir;
            this.boxdir = Direction.NONE;
            this.box = 255;
        }
        public act(Interact inter)
        {
            this.inter = inter;
            this.dir = Direction.NONE;
            this.boxdir = Direction.NONE;
            this.box = 255;
        }
    }
}
