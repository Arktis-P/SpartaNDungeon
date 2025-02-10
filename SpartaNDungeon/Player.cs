using System;
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
        public JobType Job { get; }
        public int Level { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int Intelligence { get; set; }
        public int Luck { get; set; }
        public int Dexterity { get; set; }
        public int Health { get; set; }
        public int Mana { get; set; }
        static public int Gold { get; set; }
        public int Exp { get; set; }

        // intermediate stats
        // max health  // level exp 
        public int MaxHealth { get; }
        public int MaxMana { get; }
        public int LevelExp { get; private set; }
        public int SkillDamage { get; set; }

        // complex stats  
        public List<Item> inventory;
        // public List<Item> inventory;
        public List<ISkill> skills;

        // clear variables
        public bool WarriorClear { get; private set; }
        public bool MageClear { get; private set; }
        public bool RogueClear { get; private set; }
        public bool ArcherClear { get; private set; }

        // player class initiate
        public Player() { }
        public Player(string name, int jobId)
        {
            Name = name; Level = 1; Job = (JobType)jobId;
            Attack = 5; Defense = 5; Intelligence = 5; Luck = 5; Dexterity = 5;
            Health = 100; Mana = 100;
            Gold = 100000; Exp = 0;

            MaxHealth = 100;  // may change dynamically with player's other stats (ex. level, attack, etc.)
            MaxMana = 100;  // may change dynamically with player's other stats (ex. level, intelligence, etc.)
            LevelExp = 100 * Level;  // requied exp increases as level increases
            SkillDamage = 0;

            // player's inventoy list
            inventory = new List<Item>();

            // player's skill set
            skills = new List<ISkill>();
            // add skills to player's skill set according to player's job
            AddSkillsByJob(Job.ToString());

            // give player additional stat according to player's job
            AddStatus();
        }

        // job gives additional stats
        private void AddStatus()
        {
            switch (Job)
            {
                case JobType.Warrior:  // warrior
                    Attack += 5; Defense += 5;
                    break;
                case JobType.Mage:  // mage
                    Attack += 5; Intelligence += 5;
                    break;
                case JobType.Rogue:  // rogue
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
        // if player use skill, returns damage (int) value 
        public void UseSkill(string name)
        {
            ISkill usedSkill = SkillDatabase.GetSkill(name);

            // check if player has used skill in its skill set
            // check if player has enough mana
            if (Mana >= usedSkill.ManaCost)
            {
                // take mana off
                Mana -= usedSkill.ManaCost;
                // calculate damage
                SkillDamage = usedSkill.UseSkill(this);

                // show use log
                Console.WriteLine();
                Console.WriteLine($"{usedSkill.Name} 스킬을 사용했습니다. {SkillDamage} 만큼의 피해를 주었습니다.");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine($"{usedSkill.Name} 스킬을 사용하기에 마나가 충분하지 않습니다. (현재 마나: {Mana})");
                return;
            }
        }

        // display player's status
        public void DisplayStatus()
        {
            // translate job
            string jobName;
            switch (Job)
            {
                case JobType.Warrior: jobName = "전사"; break;
                case JobType.Mage: jobName = "법사"; break;
                case JobType.Rogue: jobName = "도적"; break;
                case JobType.Archer: jobName = "궁수"; break;
                default: jobName = "무직"; break;
            }
            // show status
            Console.WriteLine($"LV. {Level}");  // Lv. 01
            Console.WriteLine($"{Name} ( {jobName} )");  // Chad ( 전사 )
            Console.WriteLine($"ATK : {Attack}\tDEF : {Defense}\tLUK : {Luck}\tDEX : {Dexterity}");  // ATK : 10    DEF : 10    LUK : 10    DEX : 10
            Console.WriteLine($"HP : {Health} / {MaxHealth}\tMP : {Mana} / {MaxMana}"); // HP : 100 / 100    MP : 100 / 100
            Console.WriteLine($"Gold : {Gold} G");  // Gold : 1000 G
        }

        public enum JobType { Warrior = 1, Mage, Rogue, Archer }

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
                item += inventory[i].IsEquip ? "(E)" : "   ";
                item += inventory[i].Name + "\t| " + inventory[i].Descrip;
                item += "\t| " + inventory[i].GetType();
                item += isSelling ? $"\t| {inventory[i].Cost} G" : "";
                // show on console
                Console.WriteLine(item);
            }

            return;
        }

        public void AddItem(Item item) { inventory.Add(item); return; }

        public void RemoveItem(Item item) { inventory.Remove(item); return; }

        // display player's skill set
        public void DisplaySkills()
        {
            // if skill set is empty, out empty msg
            if (skills.Count == 0) { Console.WriteLine("스킬셋이 비어 있습니다."); return; }

            string item;
            for (int i = 0; i < skills.Count; i++)
            {
                ISkill skill = skills[i];
                item = "";  // initializze entire string for each item
                // 1. 기본 공격
                item = $"{i + 2}. {skill.Name}\t| {skill.Desc}\t| 필요 마나: {skill.ManaCost}";
                // show on console
                Console.WriteLine(item);
            }
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
            Console.WriteLine($"{Name}이(가) 던전을 탐험하다 죽었습니다.");
            // ask if retry
            // [later] insert on UI class
            Console.WriteLine();
            Console.WriteLine("다시 시작하시겠습니까?");
            Console.WriteLine();
            Console.WriteLine("1. 예");
            Console.WriteLine("2. 아니오");

            // get player's input
            UI ui = new UI();  // initiate new UI object
            int input;
            int.TryParse(Console.ReadLine(), out input);
            switch (input)
            {
                case 1:  // to generation page
                    Console.Clear();
                    Console.WriteLine("게임을 다시 시작합니다.");
                    Thread.Sleep(1000);

                    ui.IntroductionPage();
                    return;
                case 2:  // to endgame page
                    Console.Clear();

                    ui.EndGame();
                    return;
            }
        }
        //
        private void CheckClearStatus()
        {
            // check which job player has cleared the dungeon
            switch (Job)
            {
                case JobType.Warrior:
                    if (!WarriorClear) { WarriorClear = !WarriorClear; }
                    break;
                case JobType.Mage:
                    if (!MageClear) { MageClear = !MageClear; }
                    break;
                case JobType.Rogue:
                    if (!RogueClear) { RogueClear = !RogueClear; }
                    break;
                case JobType.Archer:
                    if (!ArcherClear) { ArcherClear = !ArcherClear; }
                    break;
            }
        }
        public void DisplayClearStatus()
        {
            CheckClearStatus();
            string str = "  ";
            str += WarriorClear ? "■" : "□";
            str += " 전사\t";
            str += MageClear ? "■" : "□";
            str += " 마법사\t";
            str += RogueClear ? "■" : "□";
            str += " 도적\t";
            str += ArcherClear ? "■" : "□";
            str += " 궁수\t";
            Console.WriteLine();
            Console.WriteLine(str);
        }
    }
}
