﻿using System;
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

        public Battle(Dungeon dungeon)
        {
            this.dungeon = dungeon;
            prevHp = dungeon.player.Health;
            prevMp = dungeon.player.Mana;
            prevExp = dungeon.player.Exp;
        }
        public void EnterDungeon(UI ui)
        {
            Console.Clear();
            Shuffle(dungeon.monsters);

            StartBattle(ui);

        }
        public void StartBattle(UI ui)
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
                if(!dungeon.monsters.Any(mon => !mon.IsDead)|| dungeon.player.Health == 0)
                {
                    BattleResult();
                    break;
                }

            }
            Console.WriteLine("\n0. 다음");
            if(ConsoleUtil.GetInput(0,0) == 0) dungeon.DungeonPage(ui);

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
                Console.WriteLine($"{(i + 1)}. {dungeon.monsters[i].MonsterDisplay()}");
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
            Monster selectedMonster = null;
            int playerAtk = 0;

            Console.WriteLine($"{dungeon.player.Name}의 턴!");
            MonsterStatus(); //공격할 몬스터 출력
            Console.WriteLine(); 
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

                if (1 >input ||input > dungeon.monsters.Count) // 몬스터 외 숫자 선택 시
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    continue;
                }
                selectedMonster = dungeon.monsters[input - 1];


                if (selectedMonster.Hp <= 0) // 이미 죽은 몬스터 선택 시
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    continue;
                }

                int prevHp = selectedMonster.Hp;

                Console.WriteLine("\n1. 기본 공격");
                dungeon.player.DisplaySkills(); // 스킬 출력

                int inputSkill = ConsoleUtil.GetInput(1, dungeon.player.skills.Count + 1);
                if (inputSkill == 1) // 공격 선택 시
                {
                    playerAtk = CalAttack();
                }
                else // 스킬 선택 시
                {
                    if (dungeon.player.skills.Count == 0) return;

                    if (inputSkill != 0)
                    {
                        playerAtk = dungeon.player.skills[inputSkill -2].UseSkill(dungeon.player);
                        Console.WriteLine($"{dungeon.player.skills[inputSkill -2].Name} 사용!");
                    }
                }
                Console.WriteLine($"{selectedMonster.Name}에게 {playerAtk}의 피해를 줬습니다.");
                selectedMonster.Hp -= playerAtk;

                if (selectedMonster.Hp <= 0)
                {
                    selectedMonster.IsDead = true;
                    monsterCnt++;
                }
                playerTurn = false;
                break;
            }
            PhaseResult(true, selectedMonster, playerAtk);
        }

        public void MonsterAttack()
        {
            foreach(Monster mon in dungeon.monsters)
            {
                if (mon.IsDead) continue; // 몬스터 죽어있으면 넘어가기

                Console.WriteLine($"Lv. {mon.Level} {mon.Name}의 공격!");
                int prevHp = dungeon.player.Health;
                dungeon.player.Health -= mon.Atk;

                dungeon.player.CheckDead();
                
                PhaseResult(false, mon, mon.Atk);
                playerTurn = true;
            }
        }
        public void PhaseResult(bool playerTurn, Monster select, int atk)
        {
            Thread.Sleep(2000);
            Console.Clear();
            Console.WriteLine("Battle!!");
            if (playerTurn)
            {
                Console.WriteLine($"{dungeon.player.Name}의 공격!");
                Console.WriteLine($"Lv.{select.Level} {select.Name} 을(를) 맞췄습니다. [데미지: {atk}]");
                Console.WriteLine();
                Console.WriteLine($"Lv.{select.Level} {select.Name}");
                Console.Write($"HP {select.Hp + atk} -> {select.GetIsDead()}");
                if (select.IsDead) select.Hp = 0;
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
                dungeon.NextStage();
                Console.WriteLine("Victory");
                Console.WriteLine($"던전에서 몬스터 {monsterCnt}마리를 잡았습니다.");
                dungeon.player.Exp += random.Next(dungeon.Stage, dungeon.Stage * 10 + 1);
            }
            else if (dungeon.player.Health == 0) Console.WriteLine("You Lose");

            Console.WriteLine("[캐릭터 정보]");
            Console.WriteLine($"\nLv.{dungeon.player.Level} {dungeon.player.Name}");
            Console.WriteLine($"HP {prevHp} -> {dungeon.player.Health}");
            Console.WriteLine($"exp {prevExp} -> {dungeon.player.Exp}");

            dungeon.Reward(dungeon.Stage);

        }
    }
}
