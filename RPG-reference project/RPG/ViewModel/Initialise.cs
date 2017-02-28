using RPGModel;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Media;

namespace RPG.ViewModel
{
    public class Initialise
    {
        static Collections collection = new Collections();

        public Initialise(int load, Action LoadNextLevel)
        {
            //Things that need to be done before old level data is wiped      
            SaveData sav = new SaveData();

            //Save player hp between levels
            int playerhp = 20;
            int[] playerarmor = {0,0,0};
            int[] playerdp = { 1, 2 };
            if (load > 1)
            {
                playerhp = collection.player.hp;
                playerarmor[0] = collection.player.armor; playerarmor[1] = collection.player.boots; playerarmor[2] = collection.player.gauntlets;
                playerdp[0] = collection.player.minDp; playerdp[1] = collection.player.maxDp;
            }
            
            
            //Wiping old level data
            collection = new Collections();

            //Initialise monsters
            collection.monsters = new ObservableCollection<MonsterEntity>();
            //Initialise items
            collection.items = new ObservableCollection<ItemEntity>();   
            //Initialise triggers
            collection.triggers = new ObservableCollection<TriggerEntity>();
            

            // loads data in case we are loading a saved game
            if (load < 0)
            {
                LoadGame Load = new LoadGame(-load);
                sav = Load.LoadSave();  
                collection.level = sav.Level;
            }
            else//if we aren't we use the input int to load the level
            { collection.level = load; }


            //Initialise map
            MapData mp = new MapData();
            MapLoad MapInfo = new MapLoad(collection.level);
            mp = MapInfo.GetMap();
            collection.map = new Map(mp.Mapst);
            


            //taking into account save game modifying data
            int monstatsLength = mp.Monstats.GetLength(0);
            if (load < 0)
            {
                mp.Monstats = sav.Monstats; 

                monstatsLength = sav.Monstats.GetLength(0);

                Array.Copy(sav.PlayerStats, mp.StartnEnd, 2);
                playerhp = sav.PlayerStats[2];
                Array.Copy(sav.PlayerStats, 3, playerdp, 0, 2);
                Array.Copy(sav.PlayerStats, 5, playerarmor, 0, 3);
            }

            //Set up the player
            collection.player = new PlayerEntity("player", mp.StartnEnd[0], mp.StartnEnd[1], playerhp, playerdp[0], playerdp[1], playerarmor[0], playerarmor[1], playerarmor[2]);

            //Set up monsters
            for (int i = 0; i < monstatsLength; i++)
            {
                string monstertype = ("monster " + mp.Monstats[i, 0]);
                MonsterEntity monster = new MonsterEntity(monstertype, mp.Monstats[i, 1], mp.Monstats[i, 2], mp.Monstats[i, 3], mp.Monstats[i, 4], mp.Monstats[i, 5]);
                collection.monsters.Add(monster);
            }

            //Set up Items
            if (load < 0)
            {
                foreach (int i in sav.itemused)
                {
                    {
                        string itemtype = ("item " + mp.itemstats[i, 0]);
                        ItemEntity item = new ItemEntity(itemtype, mp.itemstats[i, 1], mp.itemstats[i, 2], mp.itemstats[i, 0], mp.itemstats[i, 3], false, i);
                        collection.items.Add(item);
                    }

                }
            }
            else
            {
                for (int i = 0; i < mp.itemstats.GetLength(0); i++)
                {
                    string itemtype = ("item " + mp.itemstats[i, 0]);
                    ItemEntity item = new ItemEntity(itemtype, mp.itemstats[i, 1], mp.itemstats[i, 2], mp.itemstats[i, 0], mp.itemstats[i, 3], false, i);
                    collection.items.Add(item);
                }
            }

            //Set up triggers
            collection.nextLevelTrigger = new TriggerEntity("End", mp.StartnEnd[2], mp.StartnEnd[3], LoadNextLevel);
            if (collection.level == 0)
            {
                collection.triggers.Add(new TriggerEntity("trigger", 6, 7, new Action<object>(MainViewModel.StartGameCommand.Execute), 1, "New\nGame", true));
                collection.triggers.Add(new TriggerEntity("trigger", 5, 7, new Action<object>(MainViewModel.StartGameCommand.Execute), 1, "New\nGame", true));
                collection.triggers.Add(new TriggerEntity("trigger", 1, 7, new Action<object>(MainViewModel.StartGameCommand.Execute), -1, "Load\nSlot 1", MainViewModel.StartGameCommand.CanExecute(-1)));
                collection.triggers.Add(new TriggerEntity("trigger", 3, 7, new Action<object>(MainViewModel.StartGameCommand.Execute), -2, "Load\nSlot 2", MainViewModel.StartGameCommand.CanExecute(-2)));
                collection.triggers.Add(new TriggerEntity("trigger", 2, 10, new Action<object>(MainViewModel.StartGameCommand.Execute), -3, "Load\nSlot 3", MainViewModel.StartGameCommand.CanExecute(-3)));
                collection.triggers.Add(new TriggerEntity("trigger", 8, 7, new Action<object>(MainViewModel.StartGameCommand.Execute), -4, "Load\nSlot 4", MainViewModel.StartGameCommand.CanExecute(-4)));
                collection.triggers.Add(new TriggerEntity("trigger", 10, 7, new Action<object>(MainViewModel.StartGameCommand.Execute), -5, "Load\nSlot 5", MainViewModel.StartGameCommand.CanExecute(-5)));
            }
        }
        
        public Collections Ini() { return collection; }

    }
    public class Collections
    {
        public Map map { get; set; }
        public PlayerEntity player { get; set; }
        public ObservableCollection<MonsterEntity> monsters { get; set; }
        public ObservableCollection<ItemEntity> items { get; set; }
        public ObservableCollection<TriggerEntity> triggers { get; set; }
        public TriggerEntity nextLevelTrigger { get; set; }
        public int level;

        //Default constructor
        public Collections()
        {
            map = null;
            player = null;
            monsters = null;
            items = null;
            triggers = null;
            nextLevelTrigger = null;
            level = 0;
        }
    }
}