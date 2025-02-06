using System;
using System.Collections.Generic;
using System.Linq;
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
        int prevHp = 0;

        public Battle(Dungeon dungeon)
        {
            this.dungeon = dungeon;
            prevHp = dungeon.player.Health;
        }
        public void EnterDungeon()
        {
            Console.Clear();
            Console.WriteLine("Battle!!\n");

            //Shuffle(dungeon.monsters);
            //foreach (string info in dungeon.monsters)
            //{
            //    Console.WriteLine(info);
            //}

            PlayerStatus();
            Console.WriteLine("\n1. 공격\n");
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            if (Console.ReadLine() == "1") StartBattle();
            else Console.WriteLine("다시 입력해주세요.");

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
                if(dungeon.monsters.Count == monsterCnt || dungeon.player.Hp == 0)
                {

                }
            }
        }
        public void Shuffle<T>(T[] array) // 배열 섞기
        {
            int n = array.Length;
            for(int i = n-1; i >0; i--)
            {
                int j = random.Next(0, i + 1);
                (array[i], array[j]) = (array[j], array[i]);
            }
        }
        
        public void PlayerStatus() // 플레이어 상태 출력
        {
            Console.WriteLine("[내정보]");
            Console.WriteLine($"Lv.{dungeon.player.Level} {dungeon.player.Name} ({dungeon.player.Job})");
            Console.WriteLine($"HP {dungeon.player.Health - damage}/{dungeon.player.Health}");
        }

        public void MonsterStatus() // 몬스터 상태 출력
        {
            for (int i = 0; i < dungeon.monsters.Count; i++)
            {
                //if (dungeon.monsters[i].hp == 0) // 어둡게 출력
                Console.WriteLine((i + 1) + dungeon.monsters[i].GetInfo());
            }
        }

        public int CalAttack()
        {
            int randomAtk = (int)Math.Ceiling(dungeon.player.Attack * 0.1);
            return dungeon.player.Attack + random.Next(-randomAtk, randomAtk + 1);
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
                    // 몬스터 사망 상태로 바꾸기 -> 텍스트 어두운 색
                    selectedMonster.Hp = 0;
                }
                PhaseResult(false, selectedMonster, playerAtk);
            }
        }

        public void MonsterAttack()
        {
            foreach(Monster mon in dungeon.monsters)
            {
                if (mon.HP <= 0) continue;

                Console.WriteLine($"Lv. {mon.Level} {mon.Name}의 공격!");
                int prevHp = dungeon.player.Health;
                dungeon.player.Health -= mon.Attack;

                if(dungeon.player.Health <= 0)
                {
                    dungeon.player.Health = 0;
                    PhaseResult(false, mon, mon.Attack);
                    break;
                }
                PhaseResult(false, mon, mon.Attack);
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
                Console.Write($"HP {select.Hp + atk} -> ");
                if (select.Hp == 0) Console.WriteLine("Dead");
                else Console.WriteLine(select.Hp);
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
