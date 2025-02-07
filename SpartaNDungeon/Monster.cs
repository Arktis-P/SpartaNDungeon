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

        public int Level { get; set; } // 몬스터 레벨
        public string Name { get; set; } // 몬스터 이름
        public int Hp { get; set; } //  몬스터 체력
        public int Atk { get; set; } // 몬스터 공격력
        public bool IsDead { get; set; } // 사망했는지 판단하는 횟수

        // 몬스터는 플레이어의 스테이지 클리어 횟수에 따라 레벨 및 각종 스탯이 상승한다.
        public Monster(int level, string name, int hp, int atk,bool isDead) 
        {
            Level = (level + levelScale); // 스테이지 클리어 3회당 레벨 1 증가
            Name = name;
            Hp = (hp + (levelScale * 2)); // 스테이지 클리어 3회당 체력 2 증가
            Atk = (atk + (levelScale)); // 스테이지 클리어 3회당 공격력 1 증가
            IsDead = isDead;
        }

        public string MonsterDisplay() // 몬스터의 정보 출력
        {
            if (IsDead == true)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray; // 몬스터 사망 시 몬스터의 텍스트 색깔을 DarkGray로 변경
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

        public string GetIsDead() // 몬스터 사망 시 HP를 출력하지 않고 대신 Dead 출력
        {
            string str = IsDead == true ? "Dead" : $"HP {Hp}";
            return str;
        }

    }

    public class MonsterManager
    {

        List<Monster> monsterList; // 몬스터 리스트

        public MonsterManager()
        {
            monsterList = new List<Monster>
            {
                new Monster(2, "미니언", 15, 6, false),
                new Monster(5, "대포미니언", 25, 7, false),
                new Monster(3, "공허충", 10, 10, false),
                new Monster(10, "슈퍼 미니언", 30, 15, false)
            };
        }

    }
}
