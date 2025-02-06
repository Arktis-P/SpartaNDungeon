using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
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
        public int Level { get; private set; }
        public int Attack { get; private set; }
        public int Defense { get; private set; }
        public int Intelligence { get; private set; }
        public int Luck { get; private set; }
        public int Dexterity { get; private set; }
        public int Health { get; }
        public int Mana { get; }
        public int Gold { get; }
        public int Exp { get; private set; }

        // intermediate stats
        // max health  // level exp 
        public int MaxHealth { get; }
        public int MaxMana { get; }
        public int LevelExp { get; private set; }

        // complex stats  
        // inventory
        public List<Item> inventory;

        // player class initiate
        public Player(string name, int jobId)
        {
            Name = name; Level = 1; Job = Enum.GetName(typeof(JobType), jobId);
            Attack = 5; Defense = 5; Intelligence = 5; Luck = 5; Dexterity = 5;
            Health = 100; Mana = 100;
            Gold = 1000; Exp = 0;

            MaxHealth = 100;  // may change dynamically with player's other stats (ex. level, attack, etc.)
            MaxMana = 100;  // may change dynamically with player's other stats (ex. level, intelligence, etc.)
            LevelExp = 100 * Level;  // requied exp increases as level increases

            // player's inventoy list
            // write after item class is made

            // give player additional stat according to its job
            AddStatus(Job);
        }

        // job gives additional stats
        private void AddStatus(string job)
        {
            JobType jobType;
            JobType.TryParse(job, out jobType);

            switch (jobType)
            {
                case JobType.전사:  // warrior
                    Attack += 5; Defense += 5;
                    break;
                case JobType.마법사:  // magician
                    Attack += 5; Intelligence += 5;
                    break;
                case JobType.도적:  // logue
                    Luck += 5; Dexterity += 5;
                    break;
                case JobType.궁수:  // archer
                    Attack += 5; Dexterity += 5;
                    break;
                default: break;  // default, no additional stats
            }
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
            return;
        }

        // check if player can level up
        public void CheckLevelUp()
        {
            if (Exp >= LevelExp) { Levelup(); }
        }
        // increase player's level and initiate related values (exp, level exp)
        private void Levelup()
        {
            // increase level
            Level++;
            // initiate exp to 0
            Exp = 0;
            // set new level exp
            LevelExp = 100 * Level;
        }

        // check if player is dead
        public void CheckDead()
        {
            if (Health <= 0) { PlayerDead(); }
        }
        // features done when player is dead
        private void PlayerDead()
        {
            // when player is dead
            // show death msg
            Console.WriteLine();
            Console.WriteLine($"{Name}이(가) 죽었습니다.");
            // ask if retry
            // [later] insert on UI class
            Console.WriteLine();
            Console.WriteLine("다시 시작하시겠습니까?");
            Console.WriteLine("1. 예");
            Console.WriteLine("2. 아니오");

            // get player's input
            int input;
            int.TryParse(Console.ReadLine(), out input);
            switch (input)
            {
                case 1:  // to generation page
                    break;
                case 2:  // to endgame page
                    break;
            }
        }
    }
}
