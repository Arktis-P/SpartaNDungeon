using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaNDungeon
{
    public interface ISkill
    {
        string Name { get; set; }
        string Desc { get; set; }
        int ManaCost { get; set; }
        int Multiplier { get; set; }  // basically multiple operation

        // how to calcualte damage done by player
        int CalculateDamage(Player player);

        void UseSkill();
    }

    // warrior class skills
    public class WarriorSkill : ISkill
    {
        public string Name { get; set; }
        public string Desc { get; set; }
        public int ManaCost { get; set; }
        public int Multiplier { get; set; }

        public WarriorSkill(string name, string desc, int manaCost, int multiplier)
        {
            Name = name; Desc = desc; ManaCost = manaCost; Multiplier = multiplier;
        }

        public int CalculateDamage(Player player) { return player.Attack * Multiplier; }
        
        public void UseSkill()
        {
            // real logic of skill
        }
    }

    // mage class skills
    public class MageSkill : ISkill
    {
        public string Name { get; set; }
        public string Desc { get; set; }
        public int ManaCost { get; set; }
        public int Multiplier { get; set; }

        public MageSkill(string name, string desc, int manaCost, int multiplier)
        {
            Name = name; Desc = desc; ManaCost = manaCost; Multiplier = multiplier;
        }

        public int CalculateDamage(Player player) { return player.Intelligence * Multiplier; }

        public void UseSkill()
        {
            // real logic of skill
        }
    }

    // logue class skills
    public class LogueSkill : ISkill
    {
        public string Name { get; set; }
        public string Desc { get; set; }
        public int ManaCost { get; set; }
        public int Multiplier { get; set; }

        public LogueSkill(string name, string desc, int manaCost, int multiplier)
        {
            Name = name; Desc = desc; ManaCost = manaCost; Multiplier = multiplier;
        }

        public int CalculateDamage(Player player) { return player.Luck * Multiplier; }

        public void UseSkill()
        {
            // real logic of skill
        }
    }

    // archer class skills
    public class ArcherSkill : ISkill
    {
        public string Name { get; set; }
        public string Desc { get; set; }
        public int ManaCost { get; set; }
        public int Multiplier { get; set; }

        public ArcherSkill(string name, string desc, int manaCost, int multiplier)
        {
            Name = name; Desc = desc; ManaCost = manaCost; Multiplier = multiplier;
        }

        public int CalculateDamage(Player player) { return player.Dexterity * Multiplier; }

        public void UseSkill()
        {
            // real logic of skill
        }
    }

    // skill database
    static class SkillDatabase
    {
        static Dictionary<string, ISkill> AllSkills { get; set; }

        static SkillDatabase()
        {
            // database for all skills
            AllSkills = new Dictionary<string, ISkill>();
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

        static void AddSkill(ISkill skill)
        {
            AllSkills.Add(skill.Name, skill);
        }

        // get skill object with its name
        static public ISkill GetSkill(string name)
        {
            // if there's name on keys, return ISkill object
            if (AllSkills.ContainsKey(name)) { return AllSkills[name]; }
            // if not, return null
            return null;
        }

        static public List<ISkill> GetSkillsByJob(string job)
        {
            List<ISkill> skills = new List<ISkill>();

            // check if skill is for the job and add to skills list
            foreach (var skill in AllSkills.Values)
            {
                if (skill.GetType().Name == job + "Skill") { skills.Add(skill); }
            }
            return skills;
        }
    }
}
