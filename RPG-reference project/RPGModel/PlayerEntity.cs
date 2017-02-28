using System;
using System.Windows;
using System.Windows.Media;
using System.Collections.Generic;

namespace RPGModel
{
    public class PlayerEntity : CharacterEntity
    {
        public int armor { get; set; }
        public int boots { get; set; }
        public int gauntlets { get; set; }
        public Point Center { get { return new Point(position.X + 125, position.Y + 125); } }

        public int defence { get { return (armor + boots + gauntlets); } private set { defence = value; } }
        

        public PlayerEntity(String imgPath, int x, int y, int hp, int minDp, int maxDp, int armor, int boots, int gauntlets)
            : base(imgPath, x, y, hp, 1, 2 )
        {
            this.armor = 0;
            this.boots = 0;
            this.gauntlets = 0;
        }

        public PlayerEntity(PlayerEntity player)
            : this(player.imgPath, player.x, player.y, player.hp, player.minDp, player.maxDp, player.armor,player.boots,player.gauntlets)
        {

        }
        public new void affectHealth(int amount)
        {
            hp +=  (amount*(100 - defence) / 100);
            NotifyPropertyChanged("hp");
        }
    }
}
