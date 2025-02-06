﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaNDungeon
{
    internal class Dungeon
    {
        private string[] dungeonMenu = { "상태 보기", "전투 시작", "포션 사용" };
        // private Monster[] monsters; // 출현 몬스터 지정
        private int stage = 1; // 난이도
        
        public Dungeon(int stage)
        {
            this.stage = stage;
            SetMonster(stage);
        }
        public void DungeonPage()
        {
            Console.Clear();
            Console.WriteLine("던전입장");
            Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.");
            for(int i = 0; i < dungeonMenu.Length; i++)
            {
                Console.WriteLine($"{i+1}. {dungeonMenu[i]}");
            }
            Console.WriteLine("0. 나가기");

            Console.WriteLine("원하시는 행동을 입력해주세요");
            switch (Console.ReadLine())
            {
                case "0": // 나가기
                    // startPage();
                    break;
                case "1": // 상태 보기
                    BattleStatusPage();
                    break;
                case "2": // 전투 시작
                    // enterDungeon(); -> Battle 클래스에
                    break;
                case "3": // 포션 사용
                    UsePotionPage();
                    break;
                default:
                    // GameManager.GetInput(0, deogonMenu.Length-1);
                    break;
            }
        }

        public void BattleStatusPage() // 상태 보기
        {
            Console.Clear();
            Console.WriteLine("상태 보기");
            Console.WriteLine("던전에 입장할 캐릭터의 정보가 표시됩니다.");

            //플레이어 정보 출력

            Console.WriteLine("0. 나가기");
            //GameManager.GetInput(0, 0);
        }
        
        public void UsePotionPage() // 포션 사용
        {
            Console.WriteLine("회복");
            Console.WriteLine($"포션을 사용하면 체력을 30 회복할 수 있습니다. (남은 포션: )");
            Console.WriteLine();
            Console.WriteLine("1. 사용하기\n0. 나가기");
            
            switch(Console.ReadLine())
            {
                case "0":
                    DungeonPage();
                    break;
                case "1":
                    UsePotion();
                    break;
                default:
                    //GameManager.GetInput(0,1);
                    break;
            }
        }
        
        public void UsePotion() // 포션 사용
        {
            //개수 검사 후 사용
            //Item에서 가져오기
        }
        
        public void SetMonster(int stage) // 해당 던전에서 출현하는 몬스터 저장
        {
            switch (stage)
            {
                case 1:
                    //monsters.Add(new Monster~);
                    break;
            }
        }


    }
}
