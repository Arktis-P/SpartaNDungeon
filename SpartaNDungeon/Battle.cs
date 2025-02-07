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

        public Battle(Dungeon dungeon)
        {
            this.dungeon = dungeon;
            prevHp = dungeon.player.Health;
            prevMp = dungeon.player.Mana;
        }
        public void EnterDungeon()
        {
            Console.Clear();
            Console.WriteLine("Battle!!\n");

            Shuffle(dungeon.monsters);
            foreach (Monster mon in dungeon.monsters)
            {
               Console.WriteLine(mon.MonsterDisplay());
            }

            PlayerStatus();
            Console.WriteLine("\n1. 공격\n");
            Console.WriteLine("2. 스킬\n");
            switch (ConsoleUtil.GetInput(1, 2))
            {
                case 1:
                    StartBattle();
                    break;
                case 2:

                    break;
            }

        }
        public void StartBattle()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Battle!!");
                if (playerTurn)
                {
                    Attack();
                }
                else
                {
                    MonsterAttack();
                }
                if(dungeon.monsters.Count == monsterCnt || dungeon.player.Health == 0)
                {
                    BattleResult();
                    break;
                }

            }
            dungeon.DungeonPage();
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
            Console.WriteLine($"HP {prevHp}/{dungeon.player.Health}");
            Console.WriteLine($"MP {prevMp}/{dungeon.player.Mana}");
        }

        public void MonsterStatus() // 몬스터 상태 출력
        {
            for (int i = 0; i < dungeon.monsters.Count; i++)
            {
                //if (dungeon.monsters[i].hp == 0) // 어둡게 출력
                Console.WriteLine((i + 1) + dungeon.monsters[i].MonsterDisplay());
            }
        }

        public int CalAttack()
        {
            int randomAtk = (int)Math.Ceiling(dungeon.player.Attack * 0.1);
            int playerAtk = dungeon.player.Attack + random.Next(-randomAtk, randomAtk + 1);
            if(random.Next(0, 1) < 0.15) // 크리티컬 추후 Luk에 따라 변경
            {
                playerAtk = (int)(playerAtk * 1.6);
            }
            return playerAtk;
        }
        public void Attack()
        {
            MonsterStatus(); //공격할 몬스터 출력
            Console.WriteLine(); 
            PlayerStatus(); // 내정보 출력

            Console.WriteLine("\n0. 취소");
            // 공격할 몬스터 입력받기
            while (true)
            {
                Console.WriteLine("대상을 선택해주세요.");
                int input = int.Parse(Console.ReadLine());

                if (input == 0)
                {
                    //턴 넘기기
                    playerTurn = false;
                    break;
                }

                if (1 <input ||input > dungeon.monsters.Count)
                {
                    Console.WriteLine("잘못된 입력입니다.");
                }

                Monster selectedMonster = dungeon.monsters[input - 1];

                if(selectedMonster.Hp <= 0)
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    continue;
                }

                int playerAtk = CalAttack();
                int prevHp = selectedMonster.Hp;
                selectedMonster.Hp -= playerAtk;
                if (selectedMonster.Hp <= 0)
                {
                    selectedMonster.Hp = 0;
                    selectedMonster.IsDead = true;
                }
                PhaseResult(false, selectedMonster, playerAtk);
            }
        }

        public void MonsterAttack()
        {
            foreach(Monster mon in dungeon.monsters)
            {
                if (mon.Hp <= 0) continue;

                Console.WriteLine($"Lv. {mon.Level} {mon.Name}의 공격!");
                int prevHp = dungeon.player.Health;
                dungeon.player.Health -= mon.Atk;

                if(dungeon.player.Health <= 0)
                {
                    dungeon.player.Health = 0;
                    PhaseResult(false, mon, mon.Atk);
                    break;
                }
                PhaseResult(false, mon, mon.Atk);
            }
            while (true)
            {
                Console.WriteLine("대상을 선택해주세요.");
                if (Console.ReadLine() == "0") playerTurn = false;
                else Console.WriteLine("잘못된 입력입니다.");
            }
        }
        public void PhaseResult(bool playerTurn, Monster select, int atk)
        {
            Console.WriteLine("Battle!!");
            if (playerTurn)
            {
                Console.WriteLine($"{dungeon.player.Name}의 공격!");
                Console.WriteLine($"Lv.{select.Level} 을(를) 맞췄습니다. [데미지: {atk}]");
                Console.WriteLine();
                Console.WriteLine($"Lv.{select.Level} {select.Name}");
                Console.Write($"HP {select.Hp + atk} -> {select.GetIsDead}");

            }
            else
            {
                Console.WriteLine($"Lv.{select.Level} {select.Name}의 공격!");
                Console.WriteLine($"{dungeon.player.Name} 을(를) 맞췄습니다. [데미지: {atk}]");
                Console.WriteLine($"\nLv.{dungeon.player.Level} {dungeon.player.Name}");
                Console.WriteLine($"HP {prevHp} -> {dungeon.player.Health}");
            }
            Console.WriteLine("\n0. 다음");
        }

        public void BattleResult()
        {
            Console.WriteLine("Battle!! - Result");
            if (dungeon.monsters.Count == monsterCnt)
            {
                dungeon.stage++;
                Console.WriteLine("Victory");
                Console.WriteLine($"던전에서 몬스터 {monsterCnt}마리를 잡았습니다.");
            }
            else if (dungeon.player.Health == 0) Console.WriteLine("You Lose");

            Console.WriteLine($"\nLv.{dungeon.player.Level} {dungeon.player.Name}");
            Console.WriteLine($"HP {prevHp} -> {dungeon.player.Health}");

            Console.WriteLine("\n0. 다음");
            if(Console.ReadLine() == "0")
            {
                // 리워드
            }


        }
    }
}
