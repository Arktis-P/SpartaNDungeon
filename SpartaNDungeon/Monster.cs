using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SpartaNDungeon
{

    public enum MonsterType // 몬스터 타입. 노말과 네임드가 존재. 네임드는 추가 스탯과 칭호가 생긴다
    {
        Normal,
        Named
    }

    public class Monster
    {
        Random random = new Random();

        Player player;
        Dungeon dungeon;

        public int Level { get; set; } // 몬스터 레벨
        public MonsterType Type { get; set; } // 몬스터 타입
        public string Name { get; set; } // 몬스터 이름
        public int Hp { get; set; } //  몬스터 체력
        public int Atk { get; set; } // 몬스터 공격력
        public bool IsDead { get; set; } // 사망했는지 판단하는 횟수


        
        public Monster(int level, string name, int hp, int atk,bool isDead) 
        {
            Level = level; 
            Name = name;
            Hp = hp;  
            Atk = atk; 
            IsDead = isDead;

            Type = GetType(); // 몬스터 타입 판단

            LevelScale();
        }

        public void LevelScale() // 몬스터는 플레이어의 레벨에 따라 레벨 및 각종 스탯이 상승한다.
        {
            int levelScale = (player.Level / 3); // 플레이어의 레벨 3업에 맟춰 레벨스케일링 발생

            Level = (Level + levelScale); // 레벨스케일링마다 1 증가
            Hp = (Hp + (levelScale * 2));  // 레벨스케일링마다 2 증가
            Atk = (Atk + (levelScale)); // 레벨스케일링마다 1 증가

            if (Type == MonsterType.Named) // 몬스터 타입이 Named로 결정된다면 추가 레벨 및 스탯 + 이름 앞에 "[변이]" 가 붙는다
            {
                Name = "[변이] " + Name;
                Level += 3 + (3 * levelScale); // 레벨이 2이하일 경우에도 네임드가 나와도 추가 스탯을 부여하기 위해 추가값을 따로 분리해서 더해준다
                Hp += 5 + (5 * levelScale);
                Atk += 2 + (2 * levelScale);
            }
        }

        public string MonsterDisplay() // 몬스터의 정보 출력
        {
            if (IsDead == true)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray; // 몬스터 사망 시 몬스터의 글자 색깔을 DarkGray로 변경
                string mon = $"Lv.{Level} {Name} {GetIsDead()}";
                Console.ResetColor();
                return mon;
            }
            else  // 몬스터가 살아있을 시 일반적인 글자 색 출력
            {
                if(Type == MonsterType.Named) // 네임드라면 노란색으로 글자 출력
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    string mon = $"Lv.{Level} {Name} {GetIsDead()}"; 
                    Console.ResetColor();
                    return mon;
                }
                else
                {
                    string mon = $"Lv.{Level} {Name} {GetIsDead()}"; // 노말이면 일반적인 글자 색 출력
                    return mon;
                }
            } 
        }

        public string GetIsDead() // 몬스터 사망 시 HP를 출력하지 않고 대신 Dead 출력
        {
            string str = IsDead == true ? "Dead" : $"HP {Hp}";
            return str;
        }

        // 기본적으로 10%의 확률로 네임드 몬스터 탄생. 스테이지가 3층 이상일 경우 네임드 확률이 30%로 변경
        private MonsterType GetType() 
        {
            if(dungeon.Stage < 3)
            {
                return (random.Next(0, 10) == 0) ? MonsterType.Named : MonsterType.Normal;
            }
            else
            {
                return (random.Next(0, 10) < 3) ? MonsterType.Named : MonsterType.Normal;
            }
        }
    }


    public class MonsterManager
    {
        Random random = new Random();
        Dungeon dungeon;

        List<Monster> monsterList; // 몬스터 리스트
        List<Monster> towerList; // 타워 리스트

        public MonsterManager()
        {
            monsterList = new List<Monster>
            {
                new Monster(2, "미니언", 15, 6, false),
                new Monster(3, "공허충", 10, 10, false),
                new Monster(5, "대포미니언", 25, 7, false),
                new Monster(10, "슈퍼 미니언", 30, 12, false)
            };

            towerList = new List<Monster>
            {
                new Monster(10, "외곽 포탑", 40, 10, false),
                new Monster(15, "내부 포탑", 50, 15, false),
                new Monster(20, "억제기 포탑", 60, 20, false),
                new Monster(30, "넥서스 포탑", 100, 25, false)
            };
        }

        public List<Monster> RandomMonster() // 전투에서 랜덤하게 등장할 몬스터를 정한다
        {
            List<Monster> summonMonster = new List<Monster>();
            int randomCount = random.Next(1, 5); // 한 전투에 나타나는 몬스터의 개체 수. 1마리 부터 4마리까지 등장
            int randomMon; // 랜덤한 몬스터를 지정할 변수


            if(dungeon.Stage == 6) // 스테이지 6에 보스 넥서스 포탑 등장
            {
                for (int i = 0; i < randomCount; i++)
                {
                    summonMonster.Add(monsterList[3]); // 슈퍼미니언만 등장
                }
                summonMonster.Add(towerList[3]);
                summonMonster.Add(towerList[3]);
            }
            else if(dungeon.Stage == 1) // 스테이지 1은 미니언, 대포미니언 및 공허충만 등장
            {
                for (int i = 0; i < randomCount; i++) // 랜덤하게 1마리에서 4마리 생성
                {
                    randomMon = random.Next(0,3); 
                    summonMonster.Add(monsterList[randomMon]); 
                }
            }
            else if (dungeon.Stage == 2) // 스테이지 2는 미니언 + 외곽 포탑 등장
            {
                for (int i = 0; i < randomCount; i++)
                {
                    randomMon = random.Next(0, 3); // 미니언, 대포미니언 및 공허충만 등장
                    summonMonster.Add(monsterList[randomMon]); 
                }
                summonMonster.Add(towerList[0]); // 타워는 고정적으로 등장해야하기 때문에 for문 밖에서 생성
            }
            else if (dungeon.Stage == 3) // 스테이지 3는 미니언 + 내부 포탑
            {
                for (int i = 0; i < randomCount; i++)
                {
                    randomMon = random.Next(0, 3); // 미니언, 대포미니언 및 공허충만 등장
                    summonMonster.Add(monsterList[randomMon]); 
                }
                summonMonster.Add(towerList[1]); 
            }
            else if (dungeon.Stage == 4) // 스테이지 4는 미니언 + 억제기 포탑 등장
            {
                for (int i = 0; i < randomCount; i++)
                {
                    randomMon = random.Next(0, 3); // 미니언, 대포미니언 및 공허충만 등장
                    summonMonster.Add(monsterList[randomMon]); 
                }
                summonMonster.Add(towerList[2]); 
            }
            else if (dungeon.Stage == 5) // 스테이지 5는 슈퍼미니언 + 대포미니언만 등장
            {
                for (int i = 0; i < randomCount; i++)
                {
                    randomMon = random.Next(2, 4); // 슈퍼미니언, 대포미니언만 등장
                    summonMonster.Add(monsterList[randomMon]); 
                }
            }

            return summonMonster;
        }
    }
}
