using System;

namespace RPGModel
{
    public class ItemEntity : Entity
    {
        public int itemtype { get; set; }
        public int statmodifier { get; set; }
        public bool used { get; set; }
        public int ID { get; set; }
        public ItemEntity(String imgPath, int x, int y, int itemtype, int statmodifier, bool used, int ID)
            : base(imgPath, x, y)
        {
            this.statmodifier = statmodifier;
            this.itemtype = itemtype;
            this.used = used;
            this.ID = ID;
        }

        public ItemEntity(ItemEntity item)
            : this(item.imgPath, item.x, item.y, item.statmodifier, item.itemtype, item.used, item.ID)
        {
        }
        public void Use(PlayerEntity player)
        {
            switch (this.itemtype)
            {
                case 0://healthpotions
                    player.hp += this.statmodifier;
                    if (player.hp > 100) { player.hp = 100; }
                    break;
                case 1://manapotions (Spells doesn't exists so no function)
                    //player.mp += this.statmodifier; // nor does manapoints
                    break;
                case 2://armor
                    if (this.statmodifier > player.armor) { player.armor = this.statmodifier; }
                    break;
                case 3://boots
                    if (this.statmodifier > player.boots) { player.boots = this.statmodifier; }
                    break;
                case 4://gauntlets
                    if (this.statmodifier > player.gauntlets) { player.gauntlets = this.statmodifier; }
                    break;
                case 5://Weapons
                    player.minDp = this.statmodifier % 100;
                    player.maxDp = this.statmodifier / 100;
                    break;
                default:
                    break;
            }
            this.used = true;
        }
    }

}
