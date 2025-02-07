﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace SpartaNDungeon
{
    public class Player
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
        public int Health { get; set; }
        public int Mana { get; }
        public int Gold { get; }
        public int Exp { get; private set; }

        // intermediate stats
        // max health  // level exp 
        public int MaxHealth { get; }
        public int MaxMana { get; }
        public int LevelExp { get; private set; }

        // complex stats  
        public List<Item> inventory;
        // public List<Item> inventory;
        public List<ISkill> skills;

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
            inventory = new List<Item>();

            // player's skill set
            skills = new List<ISkill>();
            // add skills to player's skill set according to player's job
            AddSkillsByJob(Job);

            // give player additional stat according to player's job
            AddStatus(Job);
        }

        // job gives additional stats
        private void AddStatus(string job)
        {
            JobType.TryParse(job, out JobType jobType);

            switch (jobType)
            {
                case JobType.Warrior:  // warrior
                    Attack += 5; Defense += 5;
                    break;
                case JobType.Mage:  // mage
                    Attack += 5; Intelligence += 5;
                    break;
                case JobType.Logue:  // logue
                    Luck += 5; Dexterity += 5;
                    break;
                case JobType.Archer:  // archer
                    Attack += 5; Dexterity += 5;
                    break;
                default: break;  // default, no additional stats
            }
        }

        // skill related methods
        // add only skills for each job of player
        private void AddSkillsByJob(string job)
        {
            skills.AddRange(SkillDatabase.GetSkillsByJob(job));
        }
        private void UseSkill(string skillName)
        {
            foreach (ISkill skill in skills)
            {
                if (skill.Name == skillName) { skill.UseSkill(); return; }
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

        public enum JobType { Warrior = 1, Mage, Logue, Archer }

        // display player's health
        public void DisplayHealth()
        {
            // (이름)의 현재 체력: 100 / 100
            Console.WriteLine($"{Name}의 현재 체력: {Health} / {MaxHealth}");
        }

        // display player's gold
        public void DisplayGold()
        {
            Console.WriteLine($"{Name}의 현재 잔고: {Gold} G");
        }

        // display player's inventory for inventory checking and selling items, using & equipping items
        public void DisplayInventory(bool isManaging = false, bool isSelling = false)
        {
            // if inventory is empty, out empty msg
            if (inventory.Count == 0) { Console.WriteLine("인벤토리가 비어 있습니다."); return; }

            string item;
            for (int i = 0; i < inventory.Count; i++)
            {
                item = "";  // initialize entire string for each item
                item += isManaging ? $"{i + 1}. " : "-  ";
                item += inventory[i].isEquip ? "(E)" : "   ";
                item += inventory[i].Name + "\t| " + inventory[i].Descrip;
                // item += (item의 효과)  // itemType에 따라서 방어/공격 결정
                item += isSelling ? $"\t| {inventory[i].Cost} G" : "";
                // show on console
                Console.WriteLine(item);
            }

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
