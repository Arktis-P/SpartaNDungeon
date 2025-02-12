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
        Named,
        Boss
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


        
        public Monster(int level, string name, int hp, int atk, int stage, int playerLevel) 
        {
            Level = level; 
            Name = name;
            Hp = hp;  
            Atk = atk; 
            IsDead = false;

            Type = GetMonsterType(stage); // 몬스터 타입 판단

            
        }

        // 같은 종류의 몬스터가 같은 값을 참조해 스탯(체력, 사망상태 등)을 공유하는 걸 막기 위해 생성할 때 클론으로 만든다.
        public Monster Clone(int stage, int playerLevel) 
        {
            return new Monster(Level, Name, Hp, Atk, stage, playerLevel);
        }

        public void LevelScale(int playerLevel) // 몬스터는 플레이어의 레벨에 따라 레벨 및 각종 스탯이 상승한다.
        {
            int levelScale = (playerLevel/ 3); // 플레이어의 레벨 3업에 맟춰 레벨스케일링 발생

            Level = (Level + levelScale); // 레벨스케일링마다 1 증가
            Hp = (Hp + (levelScale * 2));  // 레벨스케일링마다 2 증가
            Atk = (Atk + (levelScale)); // 레벨스케일링마다 1 증가
        }

        public void Mutation(int playerLevel) // 변이종 몬스터 판단
        {
            int levelScale = (playerLevel / 3); // 레벨스케일 반영

            if (this.Type == MonsterType.Named) // 몬스터 타입이 Named로 결정된다면 추가 레벨 및 스탯 + 이름 앞에 "[변이]" 가 붙는다
            {
                this.Name += "[변이] ";
                this.Level += 3 + (3 * levelScale); // 레벨이 2이하일 경우에도 네임드가 나와도 추가 스탯을 부여하기 위해 추가값을 따로 분리해서 더해준다
                this.Hp += 5 + (5 * levelScale);
                this.Atk += 2 + (2 * levelScale);
            }
        }

        public string MonsterDisplay() // 몬스터의 정보 출력
        {
                string mon = $"Lv.{Level} {Name} {GetIsDead()}";
                return mon;
        }

        public string GetIsDead() // 몬스터 사망 시 HP를 출력하지 않고 대신 Dead 출력
        {
            string str = IsDead == true ? "Dead" : $"HP {Hp}";
            return str;
        }

        // 기본적으로 10%의 확률로 네임드 몬스터 탄생. 스테이지가 3층 이상일 경우 네임드 확률이 30%로 변경
        private MonsterType GetMonsterType(int stage) 
        {
            if(stage < 3)
            {
                return (random.Next(0, 10) == 0) ? MonsterType.Named : MonsterType.Normal;
            }
            else if(stage == 7) // 보스 스테이지에서는 변이 발생하지 않음
            {
                return (random.Next(0, 10) < 0) ? MonsterType.Named : MonsterType.Normal;
            }
            else
            {
                return (random.Next(0, 10) < 3) ? MonsterType.Named : MonsterType.Normal;
            }
        }

        public void BossPassive()
        {
            Hp += 2;
            Console.WriteLine($"나는 관대하다 (보스의 패시브 스킬) : {Name}의 체력이 2 증가했습니다!");
        }

        
    }


    public class MonsterManager
    {
        Random random = new Random();
        Dungeon dungeon;
        UI ui;

        public List<Monster> monsterList; // 몬스터 리스트
        public List<Monster> towerList; // 타워 리스트
        public List<Monster> eliteList; // 엘리트 몬스터 리스트
        public List<Monster> boss; // 보스

        public int stage;
        public int playerLevel;

        public MonsterManager(int stage, int playerLevel)
        {
            this.stage = stage;
            this.playerLevel = playerLevel;

            monsterList = new List<Monster>
            {
                new Monster(2, "미니언", 12, 6, stage, playerLevel),
                new Monster(3, "공허충", 10, 10, stage, playerLevel),
                new Monster(5, "대포미니언", 15, 7, stage, playerLevel),
                new Monster(10, "슈퍼 미니언", 30, 12, stage, playerLevel)
            };

            eliteList = new List<Monster>
            {
                new Monster(8, "칼날부리", 20, 15, stage, playerLevel),
                new Monster(9, "큰 어스름 늑대", 25, 12, stage, playerLevel),
                new Monster(11, "고대 돌거북", 35, 6, stage,playerLevel),
                new Monster(10, "심술 두꺼비", 28, 10, stage, playerLevel)
            };

            towerList = new List<Monster>
            {
                new Monster(10, "외곽 포탑", 40, 10, stage, playerLevel),
                new Monster(15, "내부 포탑", 45, 15, stage, playerLevel),
                new Monster(20, "억제기 포탑", 45, 18, stage, playerLevel),
                new Monster(30, "넥서스 포탑", 60, 15, stage, playerLevel)
            };

            boss = new List<Monster>
            {
                new Monster(10, "크세르크세스 1세", 120, 25, stage, playerLevel)
            };
        }

        public List<Monster> RandomMonster() // 전투에서 랜덤하게 등장할 몬스터를 정한다
        {
            List<Monster> summonMonster = new List<Monster>();
            int min = 1 , max = 1; // 스테이지마다 등장 몬스터를 조절할 변수
            int randomCount = random.Next(min, max); // 한 전투에 나타나는 몬스터의 개체 수.
            int randomMon; // 랜덤한 일반 몬스터를 지정할 변수
            int randomElite; // 랜덤한 엘리트 몬스터를 지정할 변수

            if(stage == 6) // 스테이지 6에 넥서스 포탑 등장
            {
                min = 1; max = 4;
                for (int i = 0; i < randomCount; i++)
                {
                    summonMonster.Add(monsterList[3].Clone(stage, playerLevel)); // 슈퍼미니언만 등장
                }
                summonMonster.Add(towerList[3].Clone(stage, playerLevel));
                summonMonster.Add(towerList[3].Clone(stage, playerLevel));
            }
            else if(stage == 1) // 스테이지 1은 미니언, 대포미니언 및 공허충만 등장
            {
                min = 1; max = 4;
                for (int i = 0; i < randomCount; i++)
                {
                    randomMon = random.Next(0, 3);
                    summonMonster.Add(monsterList[randomMon].Clone(stage, playerLevel));
                }
            }
            else if (stage == 2) // 스테이지 2는 미니언 + 외곽 포탑 등장
            {
                min = 1; max = 4;
                for (int i = 0; i < randomCount; i++)
                {
                    randomMon = random.Next(0, 3); // 미니언, 대포미니언 및 공허충만 등장
                    summonMonster.Add(monsterList[randomMon].Clone(stage, playerLevel)); 
                }
                summonMonster.Add(towerList[0].Clone(stage, playerLevel)); // 타워는 고정적으로 등장해야하기 때문에 for문 밖에서 생성
            }
            else if (stage == 3) // 스테이지 3는 미니언 + 엘리트 몬스터 + 내부 포탑
            {
                min = 1; max = 4;
                randomMon = random.Next(0, 3);
                randomElite = random.Next(0, 4);
                for (int i = 0; i < randomCount; i++)
                {
                    randomMon = random.Next(0, 3); 
                    summonMonster.Add(monsterList[randomMon].Clone(stage, playerLevel)); 
                }
                summonMonster.Add(eliteList[randomElite].Clone(stage, playerLevel));
                summonMonster.Add(towerList[1].Clone(stage, playerLevel)); 
            }
            else if (stage == 4) // 스테이지 4는 미니언 + 엘리트 몬스터 2마리 + 억제기 포탑 등장
            {
                min = 1; max = 3;
                randomMon = random.Next(0, 3);
                randomElite = random.Next(0, 4);
                for (int i = 0; i < randomCount; i++)
                {
                    summonMonster.Add(monsterList[randomMon].Clone(stage, playerLevel)); 
                }
                summonMonster.Add(eliteList[randomElite].Clone(stage, playerLevel));
                summonMonster.Add(eliteList[randomElite].Clone(stage, playerLevel));
                summonMonster.Add(towerList[2].Clone(stage, playerLevel)); 
            }
            else if (stage == 5) // 스테이지 5는 슈퍼미니언 + 대포미니언만 등장
            {
                min = 3; max = 6;
                for (int i = 0; i < randomCount; i++)
                {
                    randomMon = random.Next(2, 4); // 슈퍼미니언, 대포미니언만 등장
                    summonMonster.Add(monsterList[randomMon].Clone(stage, playerLevel)); 
                }
            }
            else if (stage == 7) // 보스 스테이지. 보스 + 엘리트 몬스터 두마리 등장
            {
                randomElite = random.Next(0, 4);
                summonMonster.Add(eliteList[randomElite].Clone(stage, playerLevel));
                summonMonster.Add(eliteList[randomElite].Clone(stage, playerLevel));
                summonMonster.Add(boss[0].Clone(stage, playerLevel));
            }

            return summonMonster;
        }

        public void MonsterWiki()
        {
            foreach (var monster in monsterList)
            {
                Console.WriteLine($" 이름 : {monster.Name} |  체력 : {monster.Hp} |  공격력 : {monster.Atk} ");
                Console.WriteLine();
            }

            foreach (var monster in eliteList)
            {
                Console.WriteLine($" 이름 : {monster.Name} |  체력 : {monster.Hp} |  공격력 : {monster.Atk} ");
                Console.WriteLine();
            }
        }
    }
}
