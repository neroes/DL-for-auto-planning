using System;
using System.Runtime.Serialization;

namespace RPGModel
{
    [Serializable()]	//Set this attribute to all the classes that you define to be serialized
    public class MapData : ISerializable
    {
        public int[] Mapst; // level structure in integer array
        public int[,] Monstats; // monsters stats and possition
        public int[] StartnEnd; // player position
        public int[,] itemstats; // items

        //Default constructor
        public MapData()
        {
            Mapst = null;
            Monstats = null;
            StartnEnd = null;
            itemstats = null;
        }
        public MapData(SerializationInfo info, StreamingContext ctxt)
        {
            //Get the values from info and assign them to the appropriate properties
            Mapst = (int[])info.GetValue("Map structure", typeof(int[]));
            Monstats = (int[,])info.GetValue("Monster stats & pos.", typeof(int[,]));
            StartnEnd = (int[])info.GetValue("Player start and end pos.", typeof(int[]));
            itemstats = (int[,])info.GetValue("item stats", typeof(int[,]));
        }


        //Serialization function.
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("Map structure", Mapst);
            info.AddValue("Monster stats & pos.", Monstats);
            info.AddValue("Player start and end pos.", StartnEnd);
            info.AddValue("item stats", itemstats);
        }
    }
    [Serializable()]	//Set this attribute to all the classes that you define to be serialized
    public class SaveData : ISerializable
    {
        public int Level; // level
        public int[] PlayerStats; // player position
        public int[,] Monstats; // monsters stats and possition
        public int[] itemused; // item used


        //Default constructor
        public SaveData()
        {
            Level = 0;
            PlayerStats = null;
            Monstats = null;
            itemused = null;
        }
        public SaveData(SerializationInfo info, StreamingContext ctxt)
        {
            //Get the values from info and assign them to the appropriate properties
            Level = (int)info.GetValue("Level", typeof(int));
            PlayerStats = (int[])info.GetValue("Player stasts and pos.", typeof(int[]));
            Monstats = (int[,])info.GetValue("Monster stats & pos.", typeof(int[,]));
            itemused = (int[])info.GetValue("item used", typeof(int[]));
        }


        //Serialization function.
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("Level", Level);
            info.AddValue("Player stasts and pos.", PlayerStats);
            info.AddValue("Monster stats & pos.", Monstats);
            info.AddValue("item used", itemused);
        }
    }
}