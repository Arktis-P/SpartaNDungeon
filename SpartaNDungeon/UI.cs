using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaNDungeon
{
    public class UI
    {
        Player player = new Player();
        Item item = new Item();
        Dungeon dungeon = new Dungeon();

        public void StartPage() // 메인 화면
        {
            Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.");
            Console.WriteLine();
            Console.WriteLine("1. 상태보기");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine("3. 상점");
            Console.WriteLine("4. 던전입장");
            Console.WriteLine("5. 퀘스트");
            Console.WriteLine("6. 게임저장");
            Console.WriteLine();

            int input = ConsoleUtil.GetInput(1, 6); // input의 입력 범위를 1부터 6까지 제한
            switch(input)
            {
                case 1:
                    player.DisplayStatus(); 
                    break;
                case 2:
                    player.DisplayInventory();
                    break;
                case 3:
                    item.DisplayItem();
                    break;
                case 4:
                    dungeon.EnterDungeon();
                    break;
                case 5:
                    break;
                case 6:
                    break;
            }

        }


    }
}
