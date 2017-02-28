using RPGModel;
using System;
using System.IO;
using System.Collections.ObjectModel;
using System.Runtime.Serialization.Formatters.Soap;

namespace RPG.ViewModel
{
    public static class SaveGame
    {
        private static MainViewModel MVM;
        private static SaveData SD;
        private static ObservableCollection<ItemEntity> item = new ObservableCollection<ItemEntity>();
        private static int pos;

        public static void initialise(MainViewModel MVM)
        {
            SaveGame.MVM = MVM;
        }

        public static void setSavePosition(int pos)
        {
            SaveGame.pos = pos;
        }

        // This event handler is where the time-consuming work is done. 
        public static void Execute()
        {

            SaveData SD = new SaveData();
            SD.Level = MVM.level;
            int i = 0;
            SD.Monstats = new int[MVM.monsters.Count, 6];
            foreach (MonsterEntity creature in MVM.monsters)
            {
                SD.Monstats[i, 0] = Convert.ToInt32(creature.imgPath.Substring(8));
                SD.Monstats[i, 1] = creature.x;
                SD.Monstats[i, 2] = creature.y;
                SD.Monstats[i, 3] = creature.hp;
                SD.Monstats[i, 4] = creature.minDp;
                SD.Monstats[i, 5] = creature.maxDp;
                i++;
            }
            i = 0;
            SD.itemused = new int[MVM.items.Count];
            foreach (ItemEntity item in MVM.items)
            {
                SD.itemused[i] = item.ID;
                i++;
            }
            int[] tmpar = { MVM.player.x, MVM.player.y, MVM.player.hp, MVM.player.minDp, MVM.player.maxDp, MVM.player.armor, MVM.player.boots, MVM.player.gauntlets };
            SD.PlayerStats = tmpar;
            
            SaveGame.SD = SD;
            System.IO.Directory.CreateDirectory("savegames");
            string filename = ("savegames\\save " + pos + ".xml");
            Stream stream = File.Open(filename, FileMode.Create);
            SoapFormatter sformatter = new SoapFormatter();
            sformatter.Serialize(stream, SD);
            stream.Close();
        }
    }




    public class LoadGame
    {
        private SaveData SD;
        private int pos;
        //Clear sav to avoid size issues
        public LoadGame(int pos)
        {
            this.pos = pos;

            SD = null;
            string filename = ("savegames\\save " + pos + ".xml");
            if (!File.Exists(filename)) { filename = ("save\\default.xml"); }
            //Making sure the file exists


            //Open the file.
            Stream stream = File.Open(filename, FileMode.Open);
            SoapFormatter sformatter = new SoapFormatter();
            SD = (SaveData)sformatter.Deserialize(stream);
            stream.Close();

        }
        public SaveData LoadSave()
        {
            return SD;
        }
        public bool CanLoad()
        {
            string filename = ("savegames\\save " + pos + ".xml");
            return File.Exists(filename);
        }
    }
    public class MapLoad
    {
        private MapData mp;
        private int level;

        public MapLoad(int level)
        {
            this.level = level;

            
            //Clear mp for further usage.
            mp = null;
            if (level == 0)
            {
                mp = MainMenu.mapData();
            }
            else
            {
                string filename = ("data\\level " + level + ".xml");
                //Open the file.
                if (level == Int32.MaxValue) { filename = ("data\\level Dead.xml"); }
                if (!File.Exists(filename)) { filename = ("data\\level Win.xml"); }

                Stream stream = File.Open(filename, FileMode.Open);
                SoapFormatter sformatter = new SoapFormatter();
                mp = (MapData)sformatter.Deserialize(stream);
                stream.Close();
            }
        }
        public MapData GetMap()
        {
            return mp;
        }
    }
}