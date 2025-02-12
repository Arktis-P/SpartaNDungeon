using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SpartaNDungeon
{
    internal class Battle
    {
        Random random = new Random();
        Dungeon dungeon;
        int damage;
        bool playerTurn = true;
        int monsterCnt = 0; // 잡은 몬스터 수
        int prevHp = 0; // 입장 플레이어 체력
        int prevMp = 0;
        int prevExp = 0;
        bool critical = false;

        public Battle(Dungeon dungeon)
        {
            this.dungeon = dungeon;
            prevHp = dungeon.player.Health;
            prevMp = dungeon.player.Mana;
            prevExp = dungeon.player.Exp;
        }
        public void EnterDungeon()
        {
            Console.Clear();
            Shuffle(dungeon.monsters);

            StartBattle();

        }
        public void StartBattle()
        {
            while (true)
            {
                Console.Clear();

                if (playerTurn)
                {
                    Attack();
                }
                else
                {
                    MonsterAttack();
                }
                if(!dungeon.monsters.Any(mon => !mon.IsDead)|| dungeon.player.Health == 0)
                {
                    BattleResult();
                    break;
                }

            }
            Console.WriteLine("\n0. 다음");
            if(ConsoleUtil.GetInput(0,0) == 0) dungeon.DungeonPage();

        }
        public void Shuffle(List<Monster> list) // 배열 섞기
        {
            int n = list.Count;
            for(int i = n-1; i >0; i--)
            {
                int j = random.Next(0, i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }
        
        public void PlayerStatus() // 플레이어 상태 출력
        {
            Console.WriteLine("[내정보]");
            Console.WriteLine($"Lv.{dungeon.player.Level} {dungeon.player.Name} ({dungeon.player.Job})");
            Console.WriteLine($"HP {dungeon.player.Health}/{dungeon.player.MaxHealth}");
            Console.WriteLine($"MP {dungeon.player.Mana}/{dungeon.player.MaxMana}");
        }

        // 레벨 스케일과 변이 판단을 위한 HashSet. 스케일링 및 변이 판단 후 여기에 저장되어 중복을 확인하는 데 사용한다
        private HashSet<Monster> scaledMon = new HashSet<Monster>(); // 레벨 스케일과 변이 중복 방지용 HashSet

        public void MonsterStatus() // 몬스터 상태 출력
        {
            for (int i = 0; i < dungeon.monsters.Count; i++)
            {
                Monster currentMon = dungeon.monsters[i];

                // 아직 레벨스케일링과 변이 판단이 되지않은 몬스터들만 LevelScale(), Mutation() 실행
                if (!scaledMon.Contains(currentMon))
                {
                    currentMon.LevelScale(dungeon.player.Level);
                    currentMon.Mutation(dungeon.player.Level);
                    scaledMon.Add(currentMon); // HashSet에 저장 후 비교해서 중복을 방지한다.
                }

                if (Dungeon.Stage == 7) // 보스 스테이지일때 보스 패시브 발동
                {
                    if (currentMon.IsDead == false)
                        currentMon.BossPassive();
                }

                if (currentMon.IsDead == true) // 몬스터가 사망할 경우 텍스트 색을 회색으로 변경
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"{(i + 1)}. {dungeon.monsters[i].MonsterDisplay()}");
                    Console.ResetColor();
                }
                else
                {
                    if (currentMon.Type == MonsterType.Named) // 몬스터가 변이종일 경우 텍스트 색을 노란색으로 변경
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"{(i + 1)}. {dungeon.monsters[i].MonsterDisplay()}");
                        Console.ResetColor();
                    }
                    else
                        Console.WriteLine($"{(i + 1)}. {dungeon.monsters[i].MonsterDisplay()}");

                }
            }
        }

        public int CalAttack()
        {
            int randomAtk = (int)Math.Ceiling(dungeon.player.Attack * 0.1);
            int playerAtk = dungeon.player.Attack + random.Next(-randomAtk, randomAtk + 1);

            return playerAtk;
        }
        public void Attack()
        {
            while (true)
            {
                Monster selectedMonster = SelectMonster();
                if (selectedMonster == null) return; // 턴 종료

                bool success = SelectSkill(selectedMonster); // 스킬 선택 후 실행
                if (success) return;
            }
        }
        public Monster SelectMonster()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Battle!\n");
                MonsterStatus();
                Console.WriteLine();
                PlayerStatus();
                Console.WriteLine("\n대상을 선택해주세요.");
                Console.WriteLine("\n0. 턴 넘기기");

                string inputStr = Console.ReadLine();
                int input;

                if (!int.TryParse(inputStr, out input))
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    continue;
                }
                if (input == 0)
                {
                    //턴 넘기기
                    playerTurn = false;
                    return null;
                }

                else if (1 > input || input > dungeon.monsters.Count) // 몬스터 외 숫자 선택 시
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    continue;
                }

                Monster selectedMonster = dungeon.monsters[input - 1];

                if (selectedMonster.Hp <= 0) // 이미 죽은 몬스터 선택 시
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    continue;
                }
                return selectedMonster;
            }
        }

        public bool SelectSkill(Monster monster)
        {
            while (true)
            {
                Console.WriteLine("\n0. 대상 다시 선택");
                Console.WriteLine("\n1. 기본 공격");
                dungeon.player.DisplaySkills(); // 스킬 출력

                int inputSkill = ConsoleUtil.GetInput(0, dungeon.player.skills.Count + 1);

                if (inputSkill == 0) return false;

                int damage = (inputSkill == 1) ? CalAttack() : UseSkill(inputSkill);
                if (damage == -1) continue;
                if (random.NextDouble() < dungeon.player.Dexterity/100)
                {
                    damage *= 2;
                    critical = true;
                }
                ExecuteAttack(monster, damage);
                return true;
            }
        }

        public int UseSkill(int input)
        {
            if (dungeon.player.skills.Count == 0) return -1;

            int skillIndex = input - 2;

            Console.WriteLine($"{dungeon.player.skills[skillIndex].Name} 사용!");
            return dungeon.player.skills[skillIndex].UseSkill(dungeon.player);
        }

        public void ExecuteAttack(Monster monster, int damage)
        {
            if(random.NextDouble() < 0.1) // 몬스터 회피 10%
            {
                Console.WriteLine($"Lv.{monster.Level} {monster.Name} 을(를) 공격했지만 아무일도 일어나지 않았습니다.");
                damage = 0;
            }
            else Console.WriteLine($"{monster.Name}에게 {damage}의 피해를 줬습니다.");
            monster.Hp -= damage;
            if (monster.Hp <= 0)
            {
                monster.IsDead = true;
                monsterCnt++;
            }
            playerTurn = false;

            PhaseResult(true, monster, damage);
        }
        public void MonsterAttack()
        {
            Console.WriteLine("Battle!");
            foreach (Monster mon in dungeon.monsters)
            {
                int damage = 0;
                if (mon.IsDead) continue; // 몬스터 죽어있으면 넘어가기
                if (random.NextDouble() < dungeon.player.Luck / 100.0) //회피 시
                {
                    Console.WriteLine("회피 성공!");
                }
                else
                { //방어력의 절반만큼 빼고 데미지 계산
                    damage = (int)Math.Ceiling(mon.Atk - dungeon.player.Defense * .5);
                    if (damage < 0) damage = 1;
                    dungeon.player.Health -= damage; //데미지가 음수일 때 0으로 처리
                }
                dungeon.player.CheckDead();
                
                PhaseResult(false, mon, damage);
                prevHp -= damage;
                playerTurn = true;
            }

        }
        public void PhaseResult(bool playerTurn, Monster select, int atk)
        {
            Console.Clear();
            Console.WriteLine("Battle!!\n");
            if (playerTurn)
            {
                Console.WriteLine($"{dungeon.player.Name}의 공격!");
                Console.Write($"Lv.{select.Level} {select.Name} 을(를) 맞췄습니다. [데미지: {atk}]");
                if (critical)
                {
                    Console.WriteLine(" - 치명타 공격!!");
                    critical = false;
                }
                else Console.WriteLine();

                Console.WriteLine();
                Console.WriteLine($"Lv.{select.Level} {select.Name}");
                Console.Write($"HP {select.Hp + atk} -> {select.GetIsDead()}");
                if (select.IsDead) select.Hp = 0;
                Thread.Sleep(2000);
            }
            else
            {
                Console.WriteLine($"Lv.{select.Level} {select.Name}의 공격!");
                Console.WriteLine($"{dungeon.player.Name} 을(를) 맞췄습니다. [데미지: {atk}]");
                Console.WriteLine($"\nLv.{dungeon.player.Level} {dungeon.player.Name}");
                Console.WriteLine($"HP {prevHp} -> {dungeon.player.Health}");
                Thread.Sleep(2000);
            }
        }

        public void BattleResult()
        {
            Console.Clear();
            Console.WriteLine("Battle!! - Result\n");
            if (dungeon.monsters.Count == monsterCnt)
            {
                Console.WriteLine("Victory");
                Console.WriteLine($"던전에서 몬스터 {monsterCnt}마리를 잡았습니다.");
                dungeon.player.Exp += random.Next(Dungeon.Stage * 50 , Dungeon.Stage * 100);
                dungeon.NextStage();
            }
            else if (dungeon.player.Health == 0) Console.WriteLine("You Lose");

            Console.WriteLine("[캐릭터 정보]");
            Console.WriteLine($"\nLv.{dungeon.player.Level} {dungeon.player.Name}");
            Console.WriteLine($"HP {dungeon.player.Health}");
            Console.WriteLine($"exp {prevExp} -> {dungeon.player.Exp}");

            dungeon.Reward(Dungeon.Stage);
            dungeon.player.Mana += (10 + dungeon.player.Intelligence); // 스테이지 종료 시 마나 회복
            dungeon.player.CheckLevelUp();  // check if level up possible
        }
    }
}
