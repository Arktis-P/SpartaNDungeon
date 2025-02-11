using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SpartaNDungeon
{
    public class GameData
    {
        public Player PlayerData { get; set; }
        public int Gold { get; set; }
        // add more static variables if necessary
        // lists saved in player
        public List<Item> Inventory { get; set; }
        public List<Item> Items { get; set; }

        // initialize
        public GameData() { }
        public GameData(Player playerData, int gold, List<Item> inventory, List<Item> items)
        {
            PlayerData = playerData; Gold = gold;
            Inventory = inventory;
            Items = items;
        }
    }

    static class DataManager
    {
        // save player's data to file
        public static void SaveData(GameData gameData)
        {
            // set save file Path
            string filePath = "../../../save_data.json";
            // json serialization
            string jsonString = JsonSerializer.Serialize(gameData);
            // write in json file
            File.WriteAllText(filePath, jsonString, Encoding.UTF8);
        }

        // check if there's save file
        public static bool CheckLoadData()
        {
            // check if save file exists
            string filePath = "../../../save_data.json";
            if (File.Exists(filePath)) { return true; }
            else { return false; }
        }

        // load saved data and give out as player object?
        public static GameData LoadData()
        {
            string filePath = "../../../save_data.json";
            string jsonString = File.ReadAllText(filePath, Encoding.UTF8);
            GameData gameData = JsonSerializer.Deserialize<GameData>(jsonString);
            return gameData;
        }
    }
}
