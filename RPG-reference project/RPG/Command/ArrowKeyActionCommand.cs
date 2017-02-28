using RPGModel;
using RPG.ViewModel;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RPG.ViewModel
{
    // Dette er interfacet der benyttes til at implementere kommandoer, som kan bruges til undo/redo.
    public class ArrowKeyActionCommand : IUndoRedoCommand
    {
        private MainViewModel MVM;
        private CharacterEntity.Direction direction;
        private MonsterEntity monsterFocus;

        private int damagedMonster;

        private PlayerEntity player_state;
        private ObservableCollection<MonsterEntity> monsters_state;
        private ObservableCollection<ItemEntity> items_state;
        
        private int random_state;

        //Enable undo/redo by saving the game state at the time of command execution
        public ArrowKeyActionCommand(MainViewModel MVM, CharacterEntity.Direction direction)
        {
            this.MVM = MVM;
            this.direction = direction;
            this.monsterFocus = MVM.monsterFocus;
            this.items_state = new ObservableCollection<ItemEntity>();

            this.monsters_state = new ObservableCollection<MonsterEntity>();
            this.random_state = Entity.rnd.NumberOfInvokes;
            player_state = new PlayerEntity(MVM.player);

            damagedMonster = -1;

            foreach (MonsterEntity otherMonster in MVM.monsters)
            {
                monsters_state.Add(new MonsterEntity(otherMonster));
            }
            foreach (ItemEntity otherItem in MVM.items)
            {
                items_state.Add(new ItemEntity(otherItem));
            }

        }
        public void Execute()
        {
            bool canMove = true;
            //Loop through monsters to see if any of them are standing in the direction of the move
            for (int i = 0; i < MVM.monsters.Count; i++ )
            {
                //Do damage if monster is adjacent
                if (MVM.monsters[i].isInDirection(MVM.player, direction))
                {
                    MVM.player.interact(MVM.monsters[i], direction);

                    damagedMonster = i;
                    MVM.monsterFocus = MVM.monsters[i];

                    canMove = false;
               }
            }
            

            //Trigger a trigger if it is nearby
            if (MVM.nextLevelTrigger.isInDirection(MVM.player, direction))
            {
                MVM.nextLevelTrigger.trigger();
                canMove = false;
            }
            foreach (TriggerEntity trigger in MVM.triggers)
            {
                if (trigger.isInDirection(MVM.player, direction))
                {
                    trigger.trigger();
                    canMove = false;
                }
            }

            //If empty space is in the movement direction; move
            if (canMove)
            {
                MVM.player.Move(direction);
            }

            foreach (ItemEntity item in MVM.items)
            {
                if (MVM.player.x == item.x && MVM.player.y == item.y)
                {
                    item.Use(MVM.player);
                }
            }
        }

        public void UnExecute()
        {
            //Clear current collections
            MVM.player = new PlayerEntity(player_state);

            MVM.monsters.Clear();
            MVM.items.Clear();

            //Fill collections with content from previous state
            for (int i = 0; i < monsters_state.Count; i++)
            {
                MVM.monsters.Add(new MonsterEntity(monsters_state[i]));

                if (damagedMonster != -1 && i == damagedMonster)
                    MVM.monsterFocus = MVM.monsters[i];
            }
            foreach (ItemEntity otherItem in items_state)
            {
                MVM.items.Add(new ItemEntity(otherItem));
            }

            Entity.rnd = new StateRandom(Entity.rnd.Seed, random_state);
        }
    }
}