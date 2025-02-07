using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaNDungeon
{
    public class UI
    {
        Player player;
        Shop shop;
        Dungeon dungeon;
        
        public void IntroductionPage()  // 게임 시작 시 소개 화면
        {
            Console.Clear();
            Console.WriteLine("이곳은 스파르타 던전으로 향하는 길입니다.");
            Console.WriteLine("이곳을 지나가기 위해서는 당신에 대한 정보가 필요합니다.");
            GeneratePage();
        }
        private void GeneratePage()  // 캐릭터 생성 화면
        {
            string name;
            int jobId;
            Console.WriteLine();
            // get player name
            Console.WriteLine("당신의 이름을 알려주세요.");
            name = Console.ReadLine();
            name = name == null ? "르탄" : name;
            // get player job
            while (true)
            {
                Console.WriteLine("당신의 직업을 알려주세요.");
                Console.WriteLine();
                Console.WriteLine("1. 전사\n2. 법사\n3. 도적\n4. 궁수");
                // get player input
                if(int.TryParse(Console.ReadLine(), out int input) && input >= 1 && input <= 4)
                {
                    jobId = input;
                    InitializePlayer(name, jobId);
                    return;
                }
                else { Console.WriteLine("잘못된 입력입니다."); break; }
            }
        }

        private void InitializePlayer(string name, int jobId)
        {
            // instantiate player
            player = new Player(name, jobId);

            // show welcome 
            Console.Clear();
            Console.WriteLine($"스파르타 던전 입구 마을에 오신 것을 환영합니다, {player.Name} 님!");
        }

        public void StartPage() // 메인 화면
        {
            Console.Clear();
            Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.");
            Console.WriteLine();
            Console.WriteLine("1. 상태보기");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine("3. 상점");
            Console.WriteLine("4. 던전입장");
            Console.WriteLine("5. 퀘스트");
            Console.WriteLine("6. 게임저장");
            Console.WriteLine("0. 게임종료");
            Console.WriteLine();

            int input = ConsoleUtil.GetInput(0, 6); // input의 입력 범위를 1부터 6까지 제한
            switch(input)
            {
                case 1:
                    StatusPage();  // to status page
                    break;
                case 2:
                    InventoryPage();  // to inventory page
                    break;
                case 3:
                    ShopPage();  // to shop page
                    break;
                case 4:
                    DungeonPage();  // to dungeon page
                    break;
                case 5:
                    QuestPage();  // to quest page
                    break;
                case 6:
                    SavePage();  // to save page
                    break;
                case 0:
                    EndGame();  // to end game page
                    break;
            }
        }

        // status page
        private void StatusPage()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("\t\t==== 상태보기 ====");
                Console.WriteLine("캐릭터의 현재 상태를 확인할 수 있습니다.");
                // show options
                Console.WriteLine();
                player.DisplayStatus();
                Console.WriteLine();
                Console.WriteLine("0. 나가기");
                int input = ConsoleUtil.GetInput(0, 0);
                // 0. 나가기
                StartPage(); return;
            }
        }
        // inventory page
        private void InventoryPage()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("\t\t==== 인벤토리 ====");
                Console.WriteLine("캐릭터의 인벤토리를 확인하고, 장비를 관리할 수 있습니다.");

                Console.WriteLine();
                Console.WriteLine("\t[보유한 아이템 목록]");
                // display inventory
                Console.WriteLine();
                player.DisplayInventory();
                // show options
                Console.WriteLine();
                Console.WriteLine("1. 장착관리\n\n0. 나가기");

                // get player's input
                int input = ConsoleUtil.GetInput(0, 1);
                switch (input)
                {
                    case 1:  // 1. 장착관리
                        InventoryManagePage(); break;
                    case 0:  // 0. 나가기
                        StartPage(); return;
                }
            }
        }
        // inventory manage page
        private void InventoryManagePage()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("\t\t==== 인벤토리 - 장착관리 ====");
                Console.WriteLine("아이템을 장비하거나 해제할 수 있습니다.");

                Console.WriteLine();
                Console.WriteLine("\t[보유한 아이템 목록]");
                // show options & display inventory
                Console.WriteLine();
                player.DisplayInventory(true, false);
                Console.WriteLine();
                Console.WriteLine("0. 나가기");

                // get player's input
                int input = ConsoleUtil.GetInput(0, player.inventory.Count());
                if (input == 0) { InventoryPage(); return; }
                else
                {
                    Item item = player.inventory[input - 1];
                    // check if item is equipable (type == weapon or armor)
                    if (item.Type == ItemType.Weapon || item.Type == ItemType.Armor)
                    {
                        item.EquipItem(player);
                    }
                    else
                    {
                        Console.WriteLine($"{item.Name}은(는) 장비할 수 있는 아이템이 아닙니다.");
                    }
                    break;
                }
            }
        }

        // shop page
        private void ShopPage()
        {
            shop = new Shop();  // initiate shop

            while (true)
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("\t\t==== 상점 ====");
                Console.WriteLine("필요한 아이템을 구매하고 필요 없는 아이템을 판매할 수 있습니다.");
                // show options
                Console.WriteLine();
                Console.WriteLine("1. 구매하기\n2. 판매하기\n0. 나가기");

                // get player's input
                int input = ConsoleUtil.GetInput(0, 2);
                switch (input)
                {
                    case 1:  // 1. 구매하기
                        BuyItemPage();  break;
                    case 2:  // 2. 판매하기
                        SellItemPage();  break;
                    case 0:  // 0. 나가기
                        StartPage();  return;
                }
            }
        }
        // shop under - buy item page
        private void BuyItemPage()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("\t\t==== 상점 - 구매하기 ====");
                Console.WriteLine("필요한 아이템을 구매할 수 있습니다.");
                // show options
                Console.WriteLine();
                Console.WriteLine("\t[구매 가능한 아이템 목록]");
                shop.DisplayItem();
                Console.WriteLine();
                Console.WriteLine("0. 나가기");

                // get player's input
                int input = ConsoleUtil.GetInput(0, shop.ItemSale.Count);
                if (input == 0) { ShopPage(); return; }
                else { return; }
            }
        }
        // shop under - sell item page
        private void SellItemPage()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("\t\t==== 상점 - 판매하기 ====");
                Console.WriteLine("필요 없는 아이템을 판매할 수 있습니다.");
                // show options
                Console.WriteLine();
                Console.WriteLine("\t[판매 가능한 아이템 목록]");
                player.DisplayInventory(true, true);
                Console.WriteLine();
                Console.WriteLine("0. 나가기");

                // get player's input
                int input = ConsoleUtil.GetInput(0, player.inventory.Count);
                if (input == 0) { ShopPage(); return; }
                else
                {
                    input--;
                    shop.SellItem(player, player.inventory[input]);
                    break;
                }
            }
        }

        // dungeon page
        private void DungeonPage()
        {
            dungeon.DungeonPage();
        }
        // quest page
        private void QuestPage()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("");
                Console.WriteLine("\t\t==== 퀘스트 ====");
                Console.WriteLine("퀘스트 기능은 현재 구현되지 않았습니다.");
                // show option
                Console.WriteLine("");
                // 1~99. (퀘스트 목록 확인)
                // 0. 나가기
                Console.WriteLine("0. 나가기");

                // get player's input
                int input = ConsoleUtil.GetInput(0, 0);
                // back to startpage
                StartPage(); return;
            }
        }
        // save page
        private void SavePage()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("");
                Console.WriteLine("\t\t==== 저장하기 ====");
                Console.WriteLine("저장하기 기능은 현재 구현되지 않았습니다.");
                // show option
                Console.WriteLine("");
                // 1. 저장하기
                // 0. 나가기
                Console.WriteLine("0. 나가기");

                // get player's input
                int input = ConsoleUtil.GetInput(0, 0);
                // back to startpage
                StartPage(); return;
            }
        }

        // end game
        public void EndGame()
        {
            Console.Clear();
            Console.WriteLine("게임을 종료합니다.");
            Thread.Sleep(1000);  // wait for 1 second
            Environment.Exit(0);  // normal exit
        }
    }
}
