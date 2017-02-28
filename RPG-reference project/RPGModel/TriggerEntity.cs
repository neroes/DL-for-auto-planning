using System;
using System.Windows.Media;

namespace RPGModel
{
    public class TriggerEntity : Entity
    {
        private Action triggerEffect;
        private Action<object> triggerEffect2;
        private int param;
        public string text { get; set; }
        public Brush col { get; set; }
        private bool canExecute;

        public TriggerEntity(String imgPath, int x, int y, Action triggerEffect)
            : base(imgPath, x, y)
        {
            this.triggerEffect = triggerEffect;
            canExecute = true;
        }

        public TriggerEntity(String imgPath, int x, int y, Action<object> triggerEffect, int param, string text, bool canExecute)
            : base(imgPath, x, y)
        {
            this.triggerEffect2 = triggerEffect;
            this.param = param;
            this.text = text;
            this.canExecute = canExecute;
            if (canExecute)
                col = new SolidColorBrush(Colors.White);
            else
                col = new SolidColorBrush(Colors.Red);
        }

        public void trigger()
        {
            if (canExecute)
            {
                if (triggerEffect != null)
                    triggerEffect();
                else if (triggerEffect2 != null)
                    triggerEffect2(param);
            }
            
        }
    }    
}
