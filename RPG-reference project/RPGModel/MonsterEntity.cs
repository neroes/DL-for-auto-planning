using System;
using System.Linq;
using System.Collections;
using System.Collections.ObjectModel;

namespace RPGModel
{
    public class MonsterEntity : CharacterEntity
    {
        
        public MonsterEntity(String imgPath, int x, int y, int hp, int minDp, int maxDp)
            : base(imgPath, x, y, hp, minDp, maxDp)
        {
        }

        public MonsterEntity(MonsterEntity monster)
            : this(monster.imgPath, monster.x, monster.y, monster.hp, monster.minDp, monster.maxDp)
        {
        }


        //Called after every player movement
        public void update(Map map, ObservableCollection<MonsterEntity> monsters, PlayerEntity player)
        {
            AIMoveAction(map, monsters, player);
        }

        //AI performs a movement action
        private void AIMoveAction(Map map, ObservableCollection<MonsterEntity> monsters, PlayerEntity player)
        {
            // we define our default direction as none
            Direction direction = Direction.NONE;

            

            if (this.hp == 1) { int rand = rnd.Next(0, 2); if (rand == 1) { return; } }
            //check if monster can see player
            int[] X = { this.x, player.x };
            int[] Y = { this.y, player.y };
            if (X.Max()-X.Min()+Y.Max()-Y.Min()<=4)
            {
                direction = Way(ref map, X, Y); // calls methode that returns the direction we want
                if (this.hp == 1)//if the monster has 1 hp we make it run the other way
                {
                    switch (direction)
                    {
                        case Direction.DOWN: direction = Direction.UP; break;
                        case Direction.UP: direction = Direction.DOWN; break;
                        case Direction.LEFT: direction = Direction.RIGHT; break;
                        case Direction.RIGHT: direction = Direction.LEFT; break; 
                        case Direction.NONE: break;
                    }
                }
            }
            

            if (direction == Direction.NONE)
            {
                switch (rnd.Next(1,5))
                {
                    case 1:
                        direction = Direction.DOWN;
                        break;
                    case 2:
                        direction = Direction.UP;
                        break;
                    case 3:
                        direction = Direction.RIGHT;
                        break;
                    default:
                        direction = Direction.LEFT;
                        break;
                }
            }
            if (this.hp == 1)// if the monster is fleaing it has 50% chance to not move, so that the player has a chance to catch up to it
            {
                if (rnd.Next(0, 2) == 1) { direction = Direction.NONE; }
            }

            //Get the new coordinates if monster moves in this direction
            System.Drawing.Point point = getCoordinatesFromDirection(this, direction);

            //Do interactions
            if (map.isWalkable(point.X, point.Y))
            {
                bool canMove = true;

                if (player.isInDirection(this, direction))
                {
                    this.interact(player, direction); //Do damage to player
                    canMove = false;
                }

                else
                    foreach (MonsterEntity monster in monsters)
                        if (monster.isInDirection(this, direction))
                            canMove = false;

                if (canMove)
                    Move(direction);

            }
        }
        public Direction Way(ref Map map, int[] X, int[] Y) 
        {
            Direction[] Dir = { Direction.NONE, Direction.NONE };// because we are dealing with a 2d space we need to be able to store a secondary direction when the methode calls recursive
            if (Y[0] < Y[1]) { Dir[0] = Direction.DOWN; if (CanWalk(ref map, X[0], X[1], Y[0], Y[1], Dir)) { return Dir[0]; } }// because we only want the monster to be able to react to the player if he is withing it's line of sight we split this methode into 4 different possibilities
            else if (Y[0] > Y[1]) { Dir[0] = Direction.UP; if (CanWalk(ref map, X[0], X[1], Y[0], Y[1], Dir)) { return Dir[0]; } }
            if (X[0] < X[1]) { Dir[0] = Direction.RIGHT;  if (CanWalk(ref map, X[0], X[1], Y[0], Y[1], Dir)) { return Dir[0]; } }
            else if (X[0] > X[1]) { Dir[0] = Direction.LEFT; if (CanWalk(ref map, X[0], X[1], Y[0], Y[1], Dir)) { return Dir[0]; } else { Dir[0] = Direction.NONE; } }
            return Direction.NONE;// if we get this far it means that there is no way to the player and we therefore return to pick a random direction
            
        }
        bool CanWalk (ref Map map, int X, int tarX, int Y, int tarY, Direction[] Dir)
        {
            if (X == tarX && Y == tarY) { return true; }// first we check if we are asking to move to the target, if we are we are we have found a way and know where to move to
            else if (map.isWalkable(X, Y)) //if we haven't found a way yet we try to check for the next spot assuming that we can infact walk there
            {
                foreach (Direction dir in Dir)
                {
                    switch (dir)
                    {
                        case Direction.DOWN: if (CanWalk(ref map, X, tarX, Y + 1, tarY, FindNearestDirection(X, tarX, Y + 1, tarY))) { return true; } else { break; }
                        case Direction.UP: if (CanWalk(ref map, X, tarX, Y - 1, tarY, FindNearestDirection(X, tarX, Y - 1, tarY))) { return true; } else { break; }
                        case Direction.LEFT: if (CanWalk(ref map, X - 1, tarX, Y, tarY, FindNearestDirection(X - 1, tarX, Y, tarY))) { return true; } else { break; }
                        case Direction.RIGHT: if (CanWalk(ref map, X + 1, tarX, Y, tarY, FindNearestDirection(X + 1, tarX, Y, tarY))) { return true; } else { break; }
                        case Direction.NONE: break;
                    }
                }
            }
            return false;
        }
        Direction[] FindNearestDirection(int X, int tarX, int Y, int tarY)
        {
            Direction[] Dir = {Direction.NONE, Direction.NONE} ;
            if (X == tarX) { if (Y > tarY ) { Dir[0] = Direction.UP; } else { Dir[0] = Direction.DOWN; } }
            else if (Y == tarY) { if (X > tarX) { Dir[0] = Direction.LEFT; } else { Dir[0] = Direction.RIGHT; } }
            else if ((Y - tarY)*(Y - tarY)>(X - tarX)*(X - tarX))
            {
                if (X > tarX) { Dir[1] = Direction.LEFT; } else { Dir[1] = Direction.RIGHT; }
                if (Y > tarY) { Dir[0] = Direction.UP; } else { Dir[0] = Direction.DOWN; }
            }
            else
            {
                if (X > tarX) { Dir[0] = Direction.LEFT; } else { Dir[0] = Direction.RIGHT; }
                if (Y > tarY) { Dir[1] = Direction.UP; } else { Dir[1] = Direction.DOWN; }
            }
            return Dir;
        }
    }
}