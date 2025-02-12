using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SpartaNDungeon
{
    public class Player
    {
        // basic stats
        // name, job, level  // atk, def, luk, dex  // hp, mp  // gold
        public string Name { get; set; }
        public JobType Job { get; set; }
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
        public int MaxHealth { get; set; }
        public int MaxMana { get; set; }
        public int LevelExp { get; private set; }
        public int SkillDamage { get; set; }

        // complex stats  
        public List<Item> inventory { get; set; }
        // public List<Item> inventory;
        public List<CSkill> skills { get; set; }

        // clear variables
        public bool WarriorClear { get; private set; }
        public bool MageClear { get; private set; }
        public bool RogueClear { get; private set; }
        public bool ArcherClear { get; private set; }

        //Item SetBonus
        public int CurrentSetBonus { get; set; } = 0;

        // player class initiate
        public Player() { }
        public Player(string name, int jobId)
        {
            Name = name; Level = 1; Job = (JobType)jobId;
            Attack = 5; Defense = 5; Intelligence = 5; Luck = 5; Dexterity = 5;
            Health = 100; Mana = 50;
            Gold = 100000; Exp = 0;

            MaxHealth = 90 + Level * 10;  // may change dynamically with player's other stats (ex. level, attack, etc.)
            MaxMana = 50 + Intelligence * 10;  // may change dynamically with player's other stats (ex. level, intelligence, etc.)
            LevelExp = 100 * Level;  // requied exp increases as level increases
            SkillDamage = 0;

            // player's inventoy list
            inventory = new List<Item>();

            // player's skill set
            skills = new List<CSkill>();
            // add skills to player's skill set according to player's job
            AddSkillsByJob(Job);
            // give player additional stat according to player's job
            AddStatus();
            // give player default 3 potion
            AddDefaultItem();
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
            UpdateStatus();
        }
        // update changed stats after add stat
        private void UpdateStatus()
        {
            MaxHealth = 90 + Level * 10;
            MaxMana = 50 + Intelligence * 10;
            Health = MaxHealth; Mana = MaxMana;
        }

        // give player default items (3 potions)
        private void AddDefaultItem()
        {
            List<Item> itemList = Item.GetItemList();
            Item item = itemList[itemList.Count() - 1];
            AddItem(item);
        }

        // skill related methods
        // add only skills for each job of player
        public void AddSkillsByJob(JobType jobType)
        {
            string job = jobType.ToString();
            skills.AddRange(SkillDatabase.GetSkillsByJob(job));
        }
        // if player use skill, returns damage (int) value 
        public void UseSkill(string name)
        {
            CSkill usedSkill = SkillDatabase.GetSkill(name);

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
                Console.WriteLine($"{usedSkill.Name} 스킬을 사용!");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine($"{usedSkill.Name} 스킬을 사용하기에 마나가 충분하지 않습니다. (현재 마나: {Mana})");
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
            ConsoleUtil.ColorWritePart(Name, ConsoleColor.DarkCyan);
            Console.WriteLine($" ( {jobName} )");  // Chad ( 전사 )
            Console.WriteLine("ATK : {0,3}  DEF : {1,3}  INT : {2,3}  LUK : {3,3}  DEX : {4,3}", Attack, Defense, Intelligence, Luck, Dexterity);  // ATK : 10    DEF : 10    LUK : 10    DEX : 10
            Console.WriteLine($"HP : {Health} / {MaxHealth}\tMP : {Mana} / {MaxMana}"); // HP : 100 / 100    MP : 50 / 50
            Console.Write($"Gold : ");  // Gold : 1000 G
            ConsoleUtil.ColorWritePart(Gold.ToString(), ConsoleColor.DarkYellow);
            Console.WriteLine(" G");
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
            if (inventory.Count == 0) { Console.WriteLine("  인벤토리가 비어 있습니다."); return; }

            List<string> itemNames = new List<string>();
            List<string> itemDescrips = new List<string>();
            foreach (Item iitem in inventory)
            {
                if (iitem.Type == ItemType.Potion)
                {
                    string tempName = $"{iitem.Name} ({iitem.Count}개)";
                    itemNames.Add(tempName); itemDescrips.Add(iitem.Descrip); break;
                }
                itemNames.Add(iitem.Name); itemDescrips.Add(iitem.Descrip);
            }

            int nameMax = ConsoleUtil.CalcuatedMaxNumber(itemNames);
            int descripMax = ConsoleUtil.CalcuatedMaxNumber(itemDescrips);

            string item;
            for (int i = 0; i < inventory.Count; i++)
            {
                item = "";  // initialize entire string for each item
                item += isManaging ? String.Format("{0,2}. ", i+1) : "-  ";
                item += inventory[i].IsEquip ? "(E)" : "   ";
                item += inventory[i].Type == ItemType.Potion ? ConsoleUtil.WriteSpace($"{inventory[i].Name} ({inventory[i].Count}개)", nameMax) : ConsoleUtil.WriteSpace(inventory[i].Name, nameMax);
                item += "\t| " + ConsoleUtil.WriteSpace(inventory[i].Descrip, descripMax);
                item += "\t| " + inventory[i].GetType();
                item += isSelling ? $"\t| {inventory[i].Cost} G" : "";
                // 장착한 아이템은 초록색으로 변경
                if (inventory[i].IsEquip)
                {
                    ConsoleUtil.ColorWrite(item, ConsoleColor.Green);
                }
                else
                {
                    Console.WriteLine(item);
                }
            }


            return;
        }

        public void AddItem(Item item) { inventory.Add(item); return; }

        public void RemoveItem(Item item) { inventory.Remove(item); return; }

        public bool HasItem(Item item)
        {
            return inventory.Contains(item); //인벤토리에 아이템보유여부 확인
        }

        // display player's skill set
        public void DisplaySkills()
        {
            // if skill set is empty, out empty msg
            if (skills.Count == 0) { Console.WriteLine("  스킬셋이 비어 있습니다."); return; }

            string item;
            for (int i = 0; i < skills.Count; i++)
            {
                CSkill skill = skills[i];
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
            Level++;  // increase level
            Exp = 0;  // initiate exp to 0
            LevelExp = 100 * Level;  // set new level exp
            Attack++; Defense++; Intelligence++; Luck++; Dexterity++;
            switch (Job)  // additional stat increase for each job
            {
                case JobType.Warrior: Attack++; break;
                case JobType.Mage: Intelligence++; break;
                case JobType.Rogue: Luck++; break;
                case JobType.Archer: Dexterity++; break;
            }
            UpdateStatus();  // update status change
            // level up msg
            Console.WriteLine();
            ConsoleUtil.ColorWritePart(Name, ConsoleColor.DarkCyan);
            Console.Write("의 레벨이 ");
            ConsoleUtil.ColorWritePart(Level.ToString(), ConsoleColor.Green);
            Console.WriteLine("(으)로 올랐습니다.");
            Console.WriteLine("  당신은 더욱 강력해지는 것을 느낍니다.");
            Console.WriteLine("  체력과 마나가 회복되었습니다.");
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
            Console.Clear();
            Console.WriteLine();
            ConsoleUtil.ColorWrite("\t\t==== 패배 ====", ConsoleColor.Red);
            ConsoleUtil.ColorWritePart(Name, ConsoleColor.DarkCyan);
            Console.WriteLine("이(가) 협곡에서 죽었습니다.");
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
            str += (WarriorClear ? "■" : "□") + " 전사\t";
            str += (MageClear ? "■" : "□") +" 마법사\t";
            str += (RogueClear ? "■" : "□") + " 도적\t";
            str += (ArcherClear ? "■" : "□") + " 궁수\t";
            Console.WriteLine();
            Console.WriteLine(str);
        }
        // check if player has cleared every 
        public bool CheckAllClear()
        {
            bool isAllClear = false;
            if (WarriorClear && MageClear && RogueClear && ArcherClear) { isAllClear = true; }
            return isAllClear;
        }

    }
}
