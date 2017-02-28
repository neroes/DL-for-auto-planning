using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace RPGModel
{
    public class CharacterEntity : Entity
    {
        public int hp { get; set; }
        public int minDp { get; set; }
        public int maxDp { get; set; }


        public CharacterEntity(String imgPath, int x, int y, int hp, int minDp, int maxDp)
            : base(imgPath, x, y)
        {
            this.hp = hp;
            this.minDp = minDp;
            this.maxDp = maxDp;
        }

        public CharacterEntity(CharacterEntity character)
            : this(character.imgPath, character.x, character.y, character.hp, character.minDp, character.maxDp)
        {
        }

        public void Move(Direction direction)
        {
            int oldX = x;
            int oldY = y;
            switch (direction)
            {
                case Direction.DOWN:
                    y++;
                    break;
                case Direction.UP:
                    y--;
                    break;
                case Direction.LEFT:
                    x--;
                    break;
                case Direction.RIGHT:
                    x++;
                    break;
            }
            Duration duration = new Duration(TimeSpan.FromSeconds(0.2));
            DoubleAnimation xAnimation = new DoubleAnimation();
            DoubleAnimation yAnimation = new DoubleAnimation();
            xAnimation.From = position.X;
            xAnimation.To = x * img.Width;
            xAnimation.Duration = duration;

            yAnimation.From = position.Y;
            yAnimation.To = y * img.Height;
            yAnimation.Duration = duration;
            
            position.BeginAnimation(TranslateTransform.XProperty, xAnimation);
            position.BeginAnimation(TranslateTransform.YProperty, yAnimation);
            
        }

        public void affectHealth(int amount)
        {
            hp += amount;
            NotifyPropertyChanged("hp");
        }

        public int doDamage()
        {
            return -rnd.Next(minDp, maxDp+1);
        }

        public bool isDead()
        {
            return hp <= 0;
        }


        //this interacts with character
        public void interact(CharacterEntity character, Direction direction)
        {
            character.affectHealth(this.doDamage());
            animateInteraction(direction);
        }
        //this interacts with character
        public void interact(PlayerEntity character, Direction direction)
        {
            character.affectHealth(this.doDamage());
            animateInteraction(direction);
        }

        private void animateInteraction(Direction direction)
        {
            System.Drawing.Point point = getCoordinatesFromDirection(this, direction);
            point.X -= x;
            point.Y -= y;

            DoubleAnimation xAnimation = new DoubleAnimation();
            DoubleAnimation yAnimation = new DoubleAnimation();

            xAnimation.From = position.X;
            xAnimation.To = position.X + (point.X * img.Width / 3);
            xAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.1));
            xAnimation.AutoReverse = true;
            xAnimation.RepeatBehavior = new RepeatBehavior(1);

            yAnimation.From = position.Y;
            yAnimation.To = position.Y + (point.Y * img.Height/3);
            yAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.1));
            yAnimation.AutoReverse = true;
            yAnimation.RepeatBehavior = new RepeatBehavior(1);

            position.BeginAnimation(TranslateTransform.XProperty, xAnimation);
            position.BeginAnimation(TranslateTransform.YProperty, yAnimation);
        }
    }
}
