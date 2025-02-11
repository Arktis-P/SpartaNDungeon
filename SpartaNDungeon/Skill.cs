using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SpartaNDungeon
{
    public interface ISkill
    {
        // how to calcualte damage done by player
        int CalculateDamage(Player player) { return 0; }

        int UseSkill(Player player) { return CalculateDamage(player); }
    }

    public class CSkill : ISkill
    {
        public string Name { get; set; }
        public string Desc { get; set; }
        public int ManaCost { get; set; }
        public int Multiplier { get; set; }

        public CSkill(string name, string desc, int manaCost, int multiplier)
        {
            Name = name; Desc = desc; ManaCost = manaCost; Multiplier = multiplier;
        }

        private int CalculateDamage(Player player)
        {
            int[] bases = { player.Attack, player.Intelligence, player.Luck, player.Dexterity };
            int damage = bases[(int)player.Job - 1] * Multiplier;
            return damage;
        }

        public int UseSkill(Player player) { player.Mana -= this.ManaCost; return CalculateDamage(player); }
    }

    // warrior class skills
    public class WarriorSkill : CSkill
    {
        public string Name { get; set; }
        public string Desc { get; set; }
        public int ManaCost { get; set; }
        public int Multiplier { get; set; }

        public WarriorSkill(string name, string desc, int manaCost, int multiplier) : base(name, desc, manaCost, multiplier)
        {
            Name = name; Desc = desc; ManaCost = manaCost; Multiplier = multiplier;
        }

        private int CalculateDamage(Player player)
        {
            int[] bases = { player.Attack, player.Intelligence, player.Luck, player.Dexterity };
            int damage = bases[(int)player.Job - 1] * Multiplier;
            return damage;
        }

        public int UseSkill(Player player) { player.Mana -= this.ManaCost; return CalculateDamage(player); }
    }

    // mage class skills
    public class MageSkill : CSkill
    {
        public string Name { get; set; }
        public string Desc { get; set; }
        public int ManaCost { get; set; }
        public int Multiplier { get; set; }

        public MageSkill(string name, string desc, int manaCost, int multiplier) : base(name, desc, manaCost, multiplier)
        {
            Name = name; Desc = desc; ManaCost = manaCost; Multiplier = multiplier;
        }

        private int CalculateDamage(Player player)
        {
            int[] bases = { player.Attack, player.Intelligence, player.Luck, player.Dexterity };
            int damage = bases[(int)player.Job - 1] * Multiplier;
            return damage;
        }

        public int UseSkill(Player player) { player.Mana -= this.ManaCost; return CalculateDamage(player); }
    }

    // logue class skills
    public class LogueSkill : CSkill
    {
        public string Name { get; set; }
        public string Desc { get; set; }
        public int ManaCost { get; set; }
        public int Multiplier { get; set; }

        public LogueSkill(string name, string desc, int manaCost, int multiplier) : base(name, desc, manaCost, multiplier)
        {
            Name = name; Desc = desc; ManaCost = manaCost; Multiplier = multiplier;
        }

        private int CalculateDamage(Player player)
        {
            int[] bases = { player.Attack, player.Intelligence, player.Luck, player.Dexterity };
            int damage = bases[(int)player.Job - 1] * Multiplier;
            return damage;
        }

        public int UseSkill(Player player) { player.Mana -= this.ManaCost; return CalculateDamage(player); }
    }

    // archer class skills
    public class ArcherSkill : CSkill
    {
        public string Name { get; set; }
        public string Desc { get; set; }
        public int ManaCost { get; set; }
        public int Multiplier { get; set; }

        public ArcherSkill(string name, string desc, int manaCost, int multiplier) : base(name, desc, manaCost, multiplier)
        {
            Name = name; Desc = desc; ManaCost = manaCost; Multiplier = multiplier;
        }

        private int CalculateDamage(Player player)
        {
            int[] bases = { player.Attack, player.Intelligence, player.Luck, player.Dexterity };
            int damage = bases[(int)player.Job - 1] * Multiplier;
            return damage;
        }

        public int UseSkill(Player player) { player.Mana -= this.ManaCost; return CalculateDamage(player); }
    }

    // skill database
    static class SkillDatabase
    {
        static Dictionary<string, CSkill> AllSkills { get; set; }

        static SkillDatabase()
        {
            // database for all skills
            AllSkills = new Dictionary<string, CSkill>();
            InitializeSkills();
        }

        static void InitializeSkills()
        {
            // warrior class skills
            AddSkill(new WarriorSkill("돌진", "빠르게 적에게 돌진하여 ATT x 2의 피해를 입힙니다.", 50, 2));
            AddSkill(new WarriorSkill("연속베기", "하나의 적을 연속으로 ATT x 5의 피해를 입힙니다.", 100, 4));
            AddSkill(new MageSkill("파이어볼", "불덩이를 발사하여 적에게 INT x 2의 피해를 입힙니다.", 50, 2));
            AddSkill(new MageSkill("메테오", "하늘에서 커다란 불덩이를 떨어뜨려 적에게 INT x 4의 피해를 입힙니다.", 100, 4));
            AddSkill(new LogueSkill("암습", "조용하게 적에게 접근해 LUK x 2의 피해를 입힙니다.", 50, 2));
            AddSkill(new LogueSkill("급습", "빠르게 적에게 접근해 LUK x 4의 피해를 입힙니다.", 100, 4));
            AddSkill(new ArcherSkill("동시사격", "한 번에 두 개의 화살을 쏘아 DEX x 2의 피해를 입힙니다.", 50, 2));
            AddSkill(new ArcherSkill("연속사격", "여러 발의 화살을 연속으로 쏘아 DEX x 4의 피해를 입힙니다.", 100, 4));
        }

        static void AddSkill(CSkill skill)
        {
            AllSkills.Add(skill.Name, skill);
        }

        // get skill object with its name
        static public CSkill GetSkill(string name)
        {
            // if there's name on keys, return CSkill object
            if (AllSkills.ContainsKey(name)) { return AllSkills[name]; }
            // if not, return null
            return null;
        }

        static public List<CSkill> GetSkillsByJob(string job)
        {
            List<CSkill> skills = new List<CSkill>();

            // check if skill is for the job and add to skills list
            foreach (var skill in AllSkills.Values)
            {
                if (skill.GetType().Name == job + "Skill") { skills.Add(skill); }
            }
            return skills;
        }
    }
}
