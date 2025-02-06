using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace SpartaNDungeon
{
    class Player
    {
        // basic stats
        // name, job, level  // atk, def, luk, dex  // hp, mp  // gold
        public string Name { get; set; }
        public string Job { get; }
        public int Level { get; }
        public int Attack { get; }
        public int Defense { get; }
        public int Intelligence { get; }
        public int Luck { get; }
        public int Dexterity { get; }
        public int Health { get; }
        public int Mana { get; }
        public int Gold { get; }
        public int Exp { get; }

        // intermediate stats
        // max health  // level exp 
        public int MaxHealth { get; }
        public int MaxMana { get; }
        public int LevelExp { get; }

        // complex stats  
        // inventory
        //public List<Item> inventory;

        // player class initiate
        public Player(string name, int jobId)
        {
            Name = name; Level = 1; Job = Enum.GetName(typeof(JobType), jobId);
            Attack = 10; Defense = 10; Intelligence = 10; Luck = 10; Dexterity = 10;
            Health = 100; Mana = 100;
            Gold = 1000; Exp = 0;

            MaxHealth = 100;  // may change dynamically with player's other stats (ex. level, attack, etc.)
            MaxMana = 100;  // may change dynamically with player's other stats (ex. level, intelligence, etc.)
            LevelExp = 100;  // may change dynamically with player's level

            //inventory = new List<Item>();
        }

        // display player's status
        public void DisplayStatus()
        {
            Console.WriteLine($"LV. {Level}");  // Lv. 01
            Console.WriteLine($"{Name} ( {Job} )");  // Chad ( 전사 )
            Console.WriteLine($"ATK : {Attack}\tDEF : {Defense}\tLUK : {Luck}\tDEX : {Dexterity}");  // ATK : 10    DEF : 10    LUK : 10    DEX : 10
            Console.WriteLine($"HP : {Health} / {MaxHealth}\tMP : {Mana} / {MaxMana}"); // HP : 100 / 100    MP : 100 / 100
            Console.WriteLine($"Gold : {Gold} G");  // Gold : 1000 G
        }

        enum JobType { 전사 = 1, 마법사, 도적, 궁수 }

        public void DisplayInventory()
        {

        }
        
    }
}
