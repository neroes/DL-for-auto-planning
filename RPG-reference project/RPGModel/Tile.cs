using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace RPGModel
{
    //Tile represents a tile on the map
    public class Tile : NotifyBase
    {
        public enum Type { FLOOR, WALL }

        private bool walkable;
        public bool Walkable { get { return walkable; } }
        public Image img { get; set; }
        public int Y { get; set; }
        public int X { get; set; }
        public int canvasX { get; private set; }
        public int canvasY { get; private set; }

        public Tile(String imgPath, bool walkable, int x, int y)
        {
            img = new Image();
            try
            {
                img.Source = new BitmapImage(new Uri("pack://application:,,,/Sprites/map/" + imgPath + ".png"));
            }
            catch
            {
                img.Source = new BitmapImage(new Uri("pack://application:,,,/Sprites/map/Error.png"));
            }

            img.Height = 50;
            img.Width = 50;

            this.walkable = walkable;
            this.X = x;
            this.Y = y;
            this.canvasX = X * (int)img.Width;
            this.canvasY = Y * (int)img.Height;

        }
    }
}