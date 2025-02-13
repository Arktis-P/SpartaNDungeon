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

    public enum MonsterClass
    {
        Monster,
        Elite,
        Tower,
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
        public string Info {  get; set; } // 몬스터 정보
        public bool IsDead { get; set; } // 사망했는지 판단하는 횟수
        public MonsterClass MonClass { get; set; } // 몬스터 등급


        
        public Monster(int level, string name, int hp, int atk, int stage, string info, int playerLevel, MonsterClass monClass) 
        {
            Level = level; 
            Name = name;
            Hp = hp;  
            Atk = atk;
            Info = info;
            IsDead = false;
            MonClass = monClass;

            Type = GetMonsterType(stage, monClass); // 몬스터 타입 판단

            
        }

        // 같은 종류의 몬스터가 같은 값을 참조해 스탯(체력, 사망상태 등)을 공유하는 걸 막기 위해 생성할 때 클론으로 만든다.
        public Monster Clone(int stage, int playerLevel) 
        {
            return new Monster(Level, Name, Hp, Atk, stage, Info, playerLevel, MonClass);
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

        // 기본적으로 20%의 확률로 네임드 몬스터 탄생. 스테이지가 3층 이상일 경우 네임드 확률이 30%로 변경
        private MonsterType GetMonsterType(int stage, MonsterClass monClass) 
        {
            if((monClass != MonsterClass.Tower) && (monClass != MonsterClass.Boss)) // 타워나 보스는 변이 발생 x
            {
                if (stage < 3)
                {
                    return (random.Next(0, 10) < 2) ? MonsterType.Named : MonsterType.Normal;
                }
                else
                {
                    return (random.Next(0, 10) < 3) ? MonsterType.Named : MonsterType.Normal;
                }
            }

            return MonsterType.Normal;
        }

        public void BossPassive(ref int playerHealth) // 보스의 패시브 스킬
        {
            Hp += 10;
            Atk += 2;
            if (MonClass == MonsterClass.Boss) // 보스몬스터만 발동
            {
                playerHealth -= 5;
                string str2 = $" 황제의 위광 (매턴 발동) : 어떠한 압박감이 플레이어를 덮칩니다... (매턴 플레이어 체력 5 감소)";
                ConsoleUtil.ColorWrite(str2, ConsoleColor.DarkYellow);
            }
            string str = $" 나는 관대하다 (매턴 발동) : {Name}의 체력이 10, 공격력이 2 증가했습니다!";
            ConsoleUtil.ColorWrite(str, ConsoleColor.Red);
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

        public int stage = Dungeon.Stage;
        public int playerLevel;

        public MonsterManager(int playerLevel)
        {
            this.playerLevel = playerLevel;

            monsterList = new List<Monster>
            {
                new Monster(2, "미니언", 12, 6, Dungeon.Stage, "일반적인 미니언 병사이다. 하나씩은 그렇게 강하지 않지만 여러마리가 모인다면 무시할 수 없다.", playerLevel, MonsterClass.Monster),
                new Monster(3, "공허충", 10, 10, Dungeon.Stage, "공허에서 튀어나온 벌레이다. 체력이 많지 않으나 공격성이 강하다.", playerLevel, MonsterClass.Monster),
                new Monster(5, "대포미니언", 15, 7, Dungeon.Stage, "대포를 조종하는 미니언 병사이다. 일반적인 미니언보다 맷집이 단단한 편이다.", playerLevel, MonsterClass.Monster),
                new Monster(10, "슈퍼 미니언", 40, 15, Dungeon.Stage, "최종 방어선을 지키는 정예병이다. 체력이 높고 공격력도 강해 방심할 수 없는 상대이다.", playerLevel, MonsterClass.Monster)
            };

            eliteList = new List<Monster>
            {
                new Monster(8, "칼날부리", 24, 15, Dungeon.Stage, "칼날부리란 이름 처럼 날카로운 부리로 공격을 하는 위험한 새다. 공격력이 높으니 주의해야한다.", playerLevel, MonsterClass.Elite),
                new Monster(9, "큰 어스름 늑대", 28, 12, Dungeon.Stage, "일반적인 늑대보다 커다란 늑대이다. 준수한 스펙을 지니고 있으니 조심하자.", playerLevel, MonsterClass.Elite),
                new Monster(11, "고대 돌거북", 40, 6, Dungeon.Stage, "아주 오랫동안 살아온 거북이다. 체력이 매우 높으나 공격성이 약해 다른 놈을 먼저 노리자.", playerLevel, MonsterClass.Elite),
                new Monster(10, "심술 두꺼비", 30, 10, Dungeon.Stage, "늪지에서 사는 독두꺼비다. 커다란 몸집에 맞게 체력이 높으니 주의.", playerLevel, MonsterClass.Elite)
            };

            towerList = new List<Monster>
            {
                new Monster(10, "외곽 포탑", 40, 10, Dungeon.Stage, "외곽 방어선에 배치되어 있는 포탑. 단단한 것 빼곤 그저 그럴 뿐이다.", playerLevel, MonsterClass.Tower),
                new Monster(15, "내부 포탑", 45, 15, Dungeon.Stage, "내부 방어선에 배치되어 있는 포탑. 외곽 포탑보다 조금 더 강하다.", playerLevel, MonsterClass.Tower),
                new Monster(20, "억제기 포탑", 45, 18, Dungeon.Stage, "억제기 바로 앞에 배치되어 있는 포탑. 포탄의 위력이 조금 더 올랐으니 주의하자.", playerLevel, MonsterClass.Tower),
                new Monster(30, "넥서스 포탑", 60, 15, Dungeon.Stage, "황제에게 가기 직전에 배치되어 있는 수호자 포탑. \n 쌍둥이 포탑이라고도 부르며 두개가 같이 배치되어 있어 계속해서 공격을 퍼붓는다.", playerLevel, MonsterClass.Tower)
            };

            boss = new List<Monster>
            {
                new Monster(10, "크세르크세스 1세", 120, 30, Dungeon.Stage, "관대하지 않은 자. 스파르타를 위해 죽여야한다", playerLevel, MonsterClass.Boss)
            };
        }

        public List<Monster> RandomMonster() // 전투에서 랜덤하게 등장할 몬스터를 정한다
        {
            List<Monster> summonMonster = new List<Monster>();
            int randomCount = random.Next(1, 5); // 한 전투에 나타나는 몬스터의 개체 수.
            int randomMon; // 랜덤한 일반 몬스터를 지정할 변수
            int randomElite; // 랜덤한 엘리트 몬스터를 지정할 변수

            if(Dungeon.Stage == 6) // 스테이지 6에 넥서스 포탑 등장
            {
                for (int i = 0; i < randomCount; i++)
                {
                    summonMonster.Add(monsterList[3].Clone(Dungeon.Stage, playerLevel)); // 슈퍼미니언만 등장
                }
                summonMonster.Add(towerList[3].Clone(Dungeon.Stage, playerLevel));
                summonMonster.Add(towerList[3].Clone(Dungeon.Stage, playerLevel));
            }
            else if(Dungeon.Stage == 1) // 스테이지 1은 미니언, 대포미니언 및 공허충만 등장
            {
                for (int i = 0; i < randomCount; i++)
                {
                    randomMon = random.Next(0, 3);
                    summonMonster.Add(monsterList[randomMon].Clone(Dungeon.Stage, playerLevel));
                }
            }
            else if (Dungeon.Stage == 2) // 스테이지 2는 미니언 + 외곽 포탑 등장
            {
                for (int i = 0; i < randomCount; i++)
                {
                    randomMon = random.Next(0, 3); // 미니언, 대포미니언 및 공허충만 등장
                    summonMonster.Add(monsterList[randomMon].Clone(Dungeon.Stage, playerLevel)); 
                }
                summonMonster.Add(towerList[0].Clone(Dungeon.Stage, playerLevel)); // 스테이지에 고정적으로 등장해야하는 몬스터는 for문 밖에서 생성
            }
            else if (Dungeon.Stage == 3) // 스테이지 3는 미니언 + 엘리트 몬스터 + 내부 포탑
            {
                randomElite = random.Next(0, 4);
                for (int i = 0; i < randomCount; i++)
                {
                    randomMon = random.Next(0, 3); 
                    summonMonster.Add(monsterList[randomMon].Clone(Dungeon.Stage, playerLevel)); 
                }
                summonMonster.Add(eliteList[randomElite].Clone(Dungeon.Stage, playerLevel));
                summonMonster.Add(towerList[1].Clone(Dungeon.Stage, playerLevel)); 
            }
            else if (Dungeon.Stage == 4) // 스테이지 4는 미니언 + 엘리트 몬스터 2마리 + 억제기 포탑 등장
            {
                
                randomElite = random.Next(0, 4);
                for (int i = 0; i < randomCount; i++)
                {
                    randomMon = random.Next(0, 3);
                    summonMonster.Add(monsterList[randomMon].Clone(Dungeon.Stage, playerLevel)); 
                }
                summonMonster.Add(eliteList[randomElite].Clone(Dungeon.Stage, playerLevel));
                summonMonster.Add(eliteList[randomElite].Clone(Dungeon.Stage, playerLevel));
                summonMonster.Add(towerList[2].Clone(Dungeon.Stage, playerLevel)); 
            }
            else if (Dungeon.Stage == 5) // 스테이지 5는 슈퍼미니언만 등장
            {
                randomCount = random.Next(3, 5);
                for (int i = 0; i < randomCount; i++)
                {
                    randomMon = random.Next(3, 4); // 슈퍼미니언만 등장
                    summonMonster.Add(monsterList[randomMon].Clone(Dungeon.Stage, playerLevel)); 
                }
            }
            else if (Dungeon.Stage == 7) // 보스 스테이지. 보스 + 엘리트 몬스터 두마리 등장
            {
                randomElite = random.Next(0, 4);
                summonMonster.Add(eliteList[randomElite].Clone(Dungeon.Stage, playerLevel));
                summonMonster.Add(eliteList[randomElite].Clone(Dungeon.Stage, playerLevel));
                summonMonster.Add(eliteList[randomElite].Clone(Dungeon.Stage, playerLevel));
                summonMonster.Add(boss[0].Clone(Dungeon.Stage, playerLevel));
            }

            return summonMonster;
        }

        public void MonsterWiki()
        {
            foreach (var monster in monsterList)
            {
                Console.WriteLine($" 이름 : {monster.Name} |  체력 : {monster.Hp} |  공격력 : {monster.Atk} \n {monster.Info}");
                Console.WriteLine();
            }
        }

        public void MonsterWikiSecond()
        {
            foreach (var monster in eliteList)
            {
                Console.WriteLine($" 이름 : {monster.Name} |  체력 : {monster.Hp} |  공격력 : {monster.Atk} \n {monster.Info}");
                Console.WriteLine();
            }
        }

        public void MonsterWikiThird()
        {
            foreach (var monster in towerList)
            {
                Console.WriteLine($" 이름 : {monster.Name} |  체력 : {monster.Hp} |  공격력 : {monster.Atk} \n {monster.Info}");
                Console.WriteLine();
            }
        }
    }
}
