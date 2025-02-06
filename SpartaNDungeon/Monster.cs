using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaNDungeon
{
    public class Monster
    {
        Dungeon dungeon = new Dungeon();

        int levelScale = (dungeon.stageClear / 3);

        public int Level { get; set; }
        public string Name { get; set; }
        public int Hp { get; set; }
        public int Atk { get; set; }
        public int Killcount { get; set; }
        public bool IsDead { get; set; }

        public Monster(int level, string name, int hp, int atk, int killcount,bool isDead)
        {
            Level = (level + levelScale);
            Name = name;
            Hp = (hp + (levelScale * 2));
            Atk = (atk + (levelScale));
            Killcount = killcount;
            IsDead = isDead;
        }

        public string MonsterDisplay()
        {
            if (IsDead == true)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                string mon = $"Lv.{Level} {Name} {GetIsDead()}";
                Console.ResetColor();
                return mon;
            }
            else
            {
                string mon = $"Lv.{Level} {Name} {GetIsDead()}";
                return mon;
            }
            
        }

        public string GetIsDead()
        {
            string str = IsDead == true ? "Dead" : $"HP {Hp}";
            return str;
        }

    }

    public class MonsterManager
    {

        List<Monster> monsterList;

        public MonsterManager()
        {
            monsterList = new List<Monster>
            {
                new Monster(2, "미니언", 15, 6, 0,false),
                new Monster(5, "대포미니언", 25, 7, 0, false),
                new Monster(3, "공허충", 10, 10, 0, false),
                new Monster(10, "슈퍼 미니언", 30, 15, 0, false)
            };
        }

    }
}
