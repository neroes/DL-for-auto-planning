using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;

namespace RPGModel
{
    //An entity represent any object which might be on the main map
    public abstract class Entity : NotifyBase
    {     
        public static StateRandom rnd = new StateRandom(System.DateTime.Now.Millisecond);

        public enum Direction { UP, DOWN, LEFT, RIGHT, NONE }

        public TranslateTransform position { get; set; }

        public int x { get; set; }
        public int y { get; set; } 
        

        public String imgPath { get; set; }
        public Image img { get; set; }

        
        //Constructor
        public Entity(String imgPath, int x, int y) {
            img = new Image();
            this.imgPath = imgPath;
            try { img.Source = new BitmapImage(new Uri("pack://application:,,,/Sprites/Entities/" + imgPath + ".png")); }
            catch (NullReferenceException) { }
            catch (ArgumentException) { }
            position = new TranslateTransform();
            img.Height = 50;
            img.Width = 50;
            this.x = x;
            this.y = y;

            position.X = x * img.Width;
            position.Y = y * img.Height;
            
        }

        //Check if this entity is in the given direction of the given other entity
        public bool isInDirection(Entity entity, Entity.Direction direction)
        {
            System.Drawing.Point point = getCoordinatesFromDirection(entity, direction);

            if (point.Y == this.y && point.X == this.x)
                return true;

            return false;
        }


        //Return a point with the given entity's new coordinates if it was to go in the given direction
        public static System.Drawing.Point getCoordinatesFromDirection(Entity entity, Direction direction)
        {
            int newX, newY;
            switch (direction)
            {
                case Direction.DOWN:
                    newX = entity.x;
                    newY = entity.y + 1;
                    break;
                case Direction.UP:
                    newX = entity.x;
                    newY = entity.y - 1;
                    break;
                case Direction.LEFT:
                    newX = entity.x - 1;
                    newY = entity.y;
                    break;
                default:
                    newX = entity.x + 1;
                    newY = entity.y;
                    break;
            }

            return new System.Drawing.Point(newX, newY);
        }
    }
}