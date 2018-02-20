using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Trainer
{
    // the actor contains the position of an actor and can gernerate the list of possible moves of the current actor
    public enum Direction : Byte { N, S, E, W, NONE };
    public enum Interact : Byte { PUSH, PULL, MOVE, WAIT };
    public class Actor
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
        public Actor(Actor old)
        {
            this.x = old.x;
            this.y = old.y;
            this.id = old.id;
        }

        public override int GetHashCode()
        {
            int prime = 7;
            int result = 1;

            result = prime * result + x;
            result = prime * result + y;

            return result;
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
                return true;
            if (obj == null)
                return false;
            Actor ac = (Actor)obj;
            if (ac.x != x || ac.y != y) { return false;}
            return true;
        }

        public Color getcolor()
        {
            return ActorList.intToColorDict[id];
        }
        public HashSet<act> getActions(Map currentMap )
        {
            HashSet<act> actionList = new HashSet<act>();
            int box = 0;
            bool[,] isempty = new bool[3, 3];
            isempty[0, 0] = currentMap.isEmptySpace(x - 1, y - 1);
            isempty[1, 0] = currentMap.isEmptySpace(x    , y - 1);
            isempty[2, 0] = currentMap.isEmptySpace(x + 1, y - 1);
            isempty[0, 1] = currentMap.isEmptySpace(x - 1, y    );
            isempty[2, 1] = currentMap.isEmptySpace(x + 1, y    );
            isempty[0, 2] = currentMap.isEmptySpace(x - 1, y + 1);
            isempty[1, 2] = currentMap.isEmptySpace(x    , y + 1);
            isempty[2, 2] = currentMap.isEmptySpace(x + 1, y + 1);

            bool checkUp = false;
            bool checkDown = false;
            bool checkLeft = false;
            bool checkRight = false;

            // This order is so it doesn't push and pull unnecessary boxes around
            actionList.Add(new act(Interact.WAIT));
            if (!currentMap.isWall(x, y - 1))
            {
                if (isempty[1, 0]) { actionList.Add(new act(Interact.MOVE, Direction.N)); }
                else { checkUp = true; }
            }
            if (!currentMap.isWall(x, y + 1))
            {
                if (isempty[1,2]) { actionList.Add(new act(Interact.MOVE, Direction.S)); }
                else { checkDown = true; }
            }
            if (!currentMap.isWall(x + 1, y))
            {
                if (isempty[2,1]) { actionList.Add(new act(Interact.MOVE, Direction.E)); }
                else { checkRight = true; }
            }
            if (!currentMap.isWall(x - 1, y))
            {
                if (isempty[0, 1]) { actionList.Add(new act(Interact.MOVE, Direction.W)); }
                else { checkLeft = true; }
            }
            if (checkUp)
            {
                if (currentMap.isBox(x, y - 1, getcolor(), out box))
                {
                    if (isempty[2, 0]) { actionList.Add(new act(Interact.PUSH, Direction.N, Direction.E, box)); }
                    if (isempty[0, 0]) { actionList.Add(new act(Interact.PUSH, Direction.N, Direction.W, box)); }
                    if (currentMap.isEmptySpace(x, y - 2)) { actionList.Add(new act(Interact.PUSH, Direction.N, Direction.N, box)); }
                    if (isempty[1, 2]) { actionList.Add(new act(Interact.PULL, Direction.S, Direction.S, box)); }
                    if (isempty[2, 1]) { actionList.Add(new act(Interact.PULL, Direction.E, Direction.S, box)); }
                    if (isempty[0, 1]) { actionList.Add(new act(Interact.PULL, Direction.W, Direction.S, box)); }
                }
            }
            if (checkDown) {
                if (currentMap.isBox(x, y + 1, getcolor(), out box))
                {
                    if (isempty[2, 2]) { actionList.Add(new act(Interact.PUSH, Direction.S, Direction.E, box)); }
                    if (isempty[0, 2]) { actionList.Add(new act(Interact.PUSH, Direction.S, Direction.W, box)); }
                    if (currentMap.isEmptySpace(x, y + 2)) { actionList.Add(new act(Interact.PUSH, Direction.S, Direction.S, box)); }
                    if (isempty[1, 0]) { actionList.Add(new act(Interact.PULL, Direction.N, Direction.N, box)); }
                    if (isempty[2, 1]) { actionList.Add(new act(Interact.PULL, Direction.E, Direction.N, box)); }
                    if (isempty[0, 1]) { actionList.Add(new act(Interact.PULL, Direction.W, Direction.N, box)); }
                }
            }
            if (checkRight) {
                if (currentMap.isBox(x + 1, y, getcolor(), out box))
                {
                    if (isempty[2, 0]) { actionList.Add(new act(Interact.PUSH, Direction.E, Direction.N, box)); }
                    if (isempty[2, 2]) { actionList.Add(new act(Interact.PUSH, Direction.E, Direction.S, box)); }
                    if (currentMap.isEmptySpace(x + 2, y)) { actionList.Add(new act(Interact.PUSH, Direction.E, Direction.E, box)); }
                    if (isempty[0, 1]) { actionList.Add(new act(Interact.PULL, Direction.W, Direction.W, box)); }
                    if (isempty[1, 0]) { actionList.Add(new act(Interact.PULL, Direction.N, Direction.W, box)); }
                    if (isempty[1, 2]) { actionList.Add(new act(Interact.PULL, Direction.S, Direction.W, box)); }
                }
            }
            if (checkLeft)
            {
                if (currentMap.isBox(x - 1, y, getcolor(), out box))
                {
                    if (isempty[0, 0]) { actionList.Add(new act(Interact.PUSH, Direction.W, Direction.N, box)); }
                    if (isempty[0, 2]) { actionList.Add(new act(Interact.PUSH, Direction.W, Direction.S, box)); }
                    if (currentMap.isEmptySpace(x - 2, y)) { actionList.Add(new act(Interact.PUSH, Direction.W, Direction.W, box)); }
                    if (isempty[2, 1]) { actionList.Add(new act(Interact.PULL, Direction.E, Direction.E, box)); }
                    if (isempty[1, 0]) { actionList.Add(new act(Interact.PULL, Direction.N, Direction.E, box)); }
                    if (isempty[1, 2]) { actionList.Add(new act(Interact.PULL, Direction.S, Direction.E, box)); }
                }
            }
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
        public override string ToString()
        {
            string operation = "";
            string boxstring ="";
            string actorstring = "";
            switch (dir)
            {
                case Direction.N:
                    actorstring = "N";
                    break;
                case Direction.S:
                    actorstring = "S";
                    break;
                case Direction.E:
                    actorstring = "E";
                    break;
                case Direction.W:
                    actorstring = "W";
                    break;
            }
            switch (boxdir)
            {
                case Direction.N:
                    boxstring = "N";
                    break;
                case Direction.S:
                    boxstring = "S";
                    break;
                case Direction.E:
                    boxstring = "E";
                    break;
                case Direction.W:
                    boxstring = "W";
                    break;
            }
            switch (inter)
            {
                case Interact.WAIT:
                    operation = "NoOp";
                    break;
                case Interact.MOVE:
                    operation = "Move(" + actorstring + ")";
                    break;
                case Interact.PUSH:
                    operation = "Push(" + actorstring + "," + boxstring + ")";
                    break;
                case Interact.PULL:
                    operation = "Pull(" + actorstring + "," + boxstring + ")";
                    break;
                
            }
            return operation;
        }
    }
}
