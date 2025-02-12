using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SpartaNDungeon
{
    public class UI
    {
        Player player;
        Shop shop;
        Dungeon dungeon;
        MonsterManager manager;
        public void LoadingPage()  // loading page before game starts (no practical function)
        {
            Console.Clear();
            Console.WriteLine();
            Console.Write($"\t\t게임 로딩 중");

            string dots = "";
            for (int i = 0; i < 5; i++)
            {
                dots = " .";
                Console.Write(dots);
                Thread.Sleep(200);
            }
            Thread.Sleep(300);
        }

        private TitleScreen titleScreen = new TitleScreen();
        public void ShowTitleScreen()
        {
            titleScreen.Show(); // TitleScreen의 Show() 메서드 호출
        }

        private void TitlePage()  // show game title page
        {
            Console.Clear();
            Console.WriteLine();
            ConsoleUtil.ColorWrite("\t\t이곳이!", ConsoleColor.Red);
            Thread.Sleep(500);
            Console.WriteLine();
            ConsoleUtil.ColorWrite("\t\t바로!", ConsoleColor.Red);
            Thread.Sleep(500);
            Console.WriteLine();
            ConsoleUtil.ColorWrite("\t\t스파르타다!", ConsoleColor.Red);
            if (ConsoleUtil.GetAnyKey() == true) { IntroductionPage(); }
        }

        public void IntroductionPage()  // 게임 시작 시 소개 화면  // connected from game over or ending page
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t스파르타의 협곡에 오신 것을 환영합니다.");
            Thread.Sleep(1500);
            if (DataManager.CheckLoadData())
            {
                LoadPage();
                return;
            }
            Console.WriteLine();
            Console.WriteLine("\t이곳에 입장하기 위해서는 당신에 대한 정보가 필요합니다.");
            if (ConsoleUtil.GetAnyKey() == true) { GeneratePage(); }
        }
        // ask if load saved data
        private void LoadPage()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t저장된 데이터가 있습니다. 불러오시겠습니까?");
            Console.WriteLine();
            Console.WriteLine("1. 예\n2. 아니오");
            // get player's input
            int input = ConsoleUtil.GetInput(1, 2);
            switch (input)
            {
                case 1:  // load saved data
                    // load data from save file
                    GameData gameData = DataManager.LoadData();
                    player = gameData.PlayerData;
                    Player.Gold = gameData.Gold;
                    Dungeon.Stage = gameData.Stage;
                    Item.LoadItemList(gameData.Items);  // override item list
                    // show complete msg
                    Console.Clear();
                    Console.WriteLine();
                    Console.WriteLine("\t저장된 데이터를 불러왔습니다.");
                    Thread.Sleep(500);
                    Console.WriteLine();
                    Console.Write($"\t스파르타의 협곡에 다시 오신 것을 환영합니다, ");
                    ConsoleUtil.ColorWritePart(player.Name, ConsoleColor.DarkCyan);
                    Console.WriteLine(" 님!");
                    if (ConsoleUtil.GetAnyKey()) { StartPage(); return; }
                    return;
                case 2:  // generate new player
                    GeneratePage();
                    return;
            }
        }
        private void GeneratePage()  // 캐릭터 생성 화면
        {
            string name;
            int jobId;
            Console.Clear();
            Console.WriteLine();
            Console.Write("\t당신의 이름을 알려주시겠습니까?\n\n>>  ");  // get player name
            name = Console.ReadLine();
            name = (name == null || name == "") ? "레오니르탄" : name;
            // get player job
            while (true)
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("\t당신의 직업을 선택해주십시오.");
                Console.WriteLine();
                Console.WriteLine("1. 전사\n2. 법사\n3. 도적\n4. 궁수");
                // get player input
                jobId = ConsoleUtil.GetInput(1, 4);
                // initialize player
                InitializePlayer(name, jobId); break;
            }
        }

        private void InitializePlayer(string name, int jobId)
        {
            // instantiate player
            player = new Player(name, jobId);

            // show welcome 
            Console.Clear();
            Console.WriteLine();
            ConsoleUtil.ColorWritePart("\t페르시아", ConsoleColor.Red);
            Console.Write($"의 넥서스를 파괴해 스파르타의 협곡에 평화를 이끌어 주십시오, ");
            ConsoleUtil.ColorWritePart(player.Name, ConsoleColor.DarkCyan);
            Console.WriteLine(" 님.");
            Thread.Sleep(500);
            Console.WriteLine();
            Console.WriteLine("\t그곳까지 향하는 길이 순탄하지만은 않을 것입니다.");
            Thread.Sleep(500);
            if (ConsoleUtil.GetAnyKey()) { StartPage(); }
        }

        public void StartPage() // 메인 화면
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t협곡으로 떠나기 전 철저히 준비해 주시길 바랍니다.");
            Console.WriteLine();
            Console.WriteLine("1. 상태보기");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine("3. 상점");
            Console.WriteLine("4. 던전입장");
            if (player.CheckAllClear()) { Console.WriteLine("5. 퀘스트"); }
            Console.WriteLine("6. 게임저장");
            Console.WriteLine("7. 몬스터 도감");
            Console.WriteLine();
            Console.WriteLine("0. 게임종료");
            Console.WriteLine();

            int input = ConsoleUtil.GetInput(0, 7); // input의 입력 범위를 1부터 6까지 제한
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
                    if (!player.CheckAllClear()) { NotQuestPage(); break; }
                    QuestPage();  // to quest page
                    break;
                case 6:
                    SavePage();  // to save page
                    break;
                case 7:
                    MonsterWikiPage();  // to save page
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
                Console.WriteLine("  당신의 현재 상태를 확인할 수 있습니다.");
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
                Console.WriteLine("  당신의 인벤토리를 확인하고, 장비를 관리할 수 있습니다.");

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
                Console.WriteLine("  아이템을 장비하거나 해제할 수 있습니다.");

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
                    EquipItemPage(item);
                    InventoryManagePage(); return;
                }
            }
        }
        private void EquipItemPage(Item item)
        {
            StringBuilder equipMessage = new StringBuilder();

            // 아이템 장착/해제 후 메시지 받아오기
            string resultMessage = item.EquipItem(player);

            // 메시지 추가
            equipMessage.AppendLine(resultMessage);

            // 화면 정리 후 메시지 출력
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\t==== 인벤토리 - 장착관리 ====");
            Console.WriteLine();
            Console.WriteLine(equipMessage.ToString());
            ConsoleUtil.GetAnyKey();
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
                Console.WriteLine("  상점에 방문하신 것을 환영합니다.");
                Console.WriteLine("  필요한 아이템을 구매하고, 필요 없는 아이템을 판매할 수 있습니다.");
                // show options
                Console.WriteLine();
                Console.WriteLine("1. 구매하기\n2. 판매하기\n\n0. 나가기");

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
                Console.WriteLine("  필요한 아이템을 구매할 수 있습니다.");
                // display player Gold
                Console.WriteLine();
                Console.Write($"  보유 골드: ");
                ConsoleUtil.ColorWritePart(Player.Gold.ToString(), ConsoleColor.DarkYellow);
                Console.WriteLine(" G");
                // show options
                Console.WriteLine();
                Console.WriteLine("\t[구매 가능한 아이템 목록]");
                shop.DisplayItem();
                Console.WriteLine();
                Console.WriteLine("0. 나가기");

                // get player's input
                int input = ConsoleUtil.GetInput(0, shop.ItemSale.Count);
                if (input == 0) { ShopPage(); return; }
                else
                {
                    input--;
                    shop.BuyItem(player, shop.ItemSale[input]);
                }
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
                Console.WriteLine("  필요 없는 아이템을 판매할 수 있습니다.");
                // show options
                Console.WriteLine();
                Console.WriteLine("\t[판매 가능한 아이템 목록]");
                Console.WriteLine();
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
            if (Dungeon.Stage == 0) { Dungeon.Stage = 1; }
            dungeon = new Dungeon(Dungeon.Stage, player, manager, this);
            dungeon.DungeonPage();
        }
        // when quest is limited
        private void NotQuestPage()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\t\t==== 퀘스트 ====");
                Console.WriteLine("  퀘스트 기능은 아직 해금되지 않았습니다.");
                // show option
                Console.WriteLine();
                Console.WriteLine("0. 나가기");
                // get player's input
                int input = ConsoleUtil.GetInput(0, 0);
                StartPage(); return;
            }
        }

        // quest page
        private void QuestPage()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("\t\t==== 퀘스트 ====");
                Console.WriteLine("  진행할 수 있는 퀘스트를 확인하고, 퀘스트를 수주할 수 있습니다.");
                // show option
                Console.WriteLine();
                // 1~99. (퀘스트 목록 확인)
                // 0. 나가기
                Console.WriteLine("0. 나가기");

                // get player's input
                int input = ConsoleUtil.GetInput(0, 0);
                // back to startpage
                StartPage(); return;
            }
        }

        // Monster Wiki page
        private void MonsterWikiPage()
        {
            Console.Clear();
            manager = new MonsterManager(1);
            Console.WriteLine();
            Console.WriteLine("\t\t==== 몬스터 위키 ====");
            Console.WriteLine("  이곳에서 몬스터의 정보를 확인할 수 있습니다.");
            Console.WriteLine();
            string str = "\t[몬스터 목록]";
            ConsoleUtil.ColorWrite(str, ConsoleColor.Green);
            manager.MonsterWiki();
            Console.WriteLine();

            Console.WriteLine("1. 다음 페이지");
            Console.WriteLine("0. 돌아가기");
            int input = ConsoleUtil.GetInput(0, 1);

            switch (input)
            {
                case 0:
                    Console.Clear();
                    Console.WriteLine("\x1b[3J");
                    StartPage(); return;
                case 1:
                    Console.Clear();
                    Console.WriteLine("\x1b[3J");
                    MonsterWikiPageSecond();
                    break;
            }
        }

        private void MonsterWikiPageSecond()
        {
            Console.Clear();
            manager = new MonsterManager(1);
            Console.WriteLine("\n\t\t==== 몬스터 위키 ====");
            Console.WriteLine("  이곳에서 몬스터의 정보를 확인할 수 있습니다.");
            Console.WriteLine();
            string str = "\t[몬스터 목록]";
            ConsoleUtil.ColorWrite(str, ConsoleColor.Green);
            manager.MonsterWikiSecond();
            Console.WriteLine();

            Console.WriteLine("1. 다음 페이지");
            Console.WriteLine("2. 이전 페이지");
            Console.WriteLine("0. 돌아가기");
            int input = ConsoleUtil.GetInput(0, 2);

            switch (input)
            {
                case 0:
                    Console.Clear();
                    Console.WriteLine("\x1b[3J");
                    StartPage(); return;
                case 1:
                    Console.Clear();
                    Console.WriteLine("\x1b[3J");
                    MonsterWikiPageThird();
                    break;
                case 2:
                    Console.Clear();
                    Console.WriteLine("\x1b[3J");
                    MonsterWikiPage();
                    break;
            }
        }

        private void MonsterWikiPageThird()
        {
            Console.Clear();
            manager = new MonsterManager(1);
            Console.WriteLine("\n\t\t==== 몬스터 위키 ====");
            Console.WriteLine("  이곳에서 몬스터의 정보를 확인할 수 있습니다.");
            Console.WriteLine();
            string str = "\t[몬스터 목록]";
            ConsoleUtil.ColorWrite(str, ConsoleColor.Green);
            manager.MonsterWikiThird();
            Console.WriteLine();

            Console.WriteLine("1. 이전 페이지");
            Console.WriteLine("0. 돌아가기");
            int input = ConsoleUtil.GetInput(0, 1);

            switch (input)
            {
                case 0:
                    Console.Clear();
                    Console.WriteLine("\x1b[3J");
                    StartPage(); return;
                case 1:
                    Console.Clear();
                    Console.WriteLine("\x1b[3J");
                    MonsterWikiPageSecond();
                    break;
            }
        }

        // game clear page (when defeated Nexus tower)
        public void EndingPage()
        {
            Console.Clear();
            Console.WriteLine();
            ConsoleUtil.ColorWrite("\t\t==== 승리 ====", ConsoleColor.Green);
            Thread.Sleep(500);
            Console.WriteLine();
            ConsoleUtil.ColorWritePart("  페르시아", ConsoleColor.Red);
            Console.WriteLine("의 넥서스가 파괴되었습니다!");
            Thread.Sleep(500);
            Console.Write($"  스파르타의 협곡에 평화가 찾아왔습니다. 감사합니다, ");
            ConsoleUtil.ColorWritePart(player.Name, ConsoleColor.DarkCyan);
            Console.WriteLine(" 님!");
            Thread.Sleep(500);
            Console.WriteLine();
            // check player's job and convert its value
            // show claer status ex) ■ 전사    □ 마법사   □ 도적    □ 궁수
            player.DisplayClearStatus();
            if (ConsoleUtil.GetAnyKey()) { CreditPage(); }
        }

        // ending page (when cleared game)
        public void CreditPage()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t      ==== 제작 ====");
            Thread.Sleep(500);
            Console.WriteLine();
            Console.WriteLine("\t스파르타의 협곡에 오신 것을 환영합니다.");
            Thread.Sleep(500);
            Console.WriteLine();
            Console.WriteLine("\t 기획 및 구상 : 손치완 ");
            Thread.Sleep(500);
            Console.WriteLine("\t      UI 구성 : 박규태 손치완");
            Thread.Sleep(500);
            Console.WriteLine("\t플레이어 기능 : 손치완");
            Thread.Sleep(500);
            Console.WriteLine("\t  아이템 기능 : 진희원");
            Thread.Sleep(500);
            Console.WriteLine("\t    상점 기능 : 이정구");
            Thread.Sleep(500);
            Console.WriteLine("\t  몬스터 기능 : 박규태");
            Thread.Sleep(500);
            Console.WriteLine("\t    전투 기능 : 박소희");
            Thread.Sleep(500);
            Console.WriteLine("\t  퀘스트 기능 : 이정구");
            Thread.Sleep(500);
            Console.WriteLine("\t게임을 플레이해주셔서 감사합니다.");
            // check if all clear // if all clear, pop all clear msg up
            if (player.CheckAllClear()) { AllClearMessage(); return; }
            // get any key to continue
            if (ConsoleUtil.GetAnyKey()) { LoadingPage(); }
        }
        // msg page for all clear page
        private void AllClearMessage()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("  퀘스트 기능이 해금되었습니다.");
            Thread.Sleep(500);
            Console.WriteLine();
            Console.WriteLine("  무한 모드가 해금되었습니다.");
            ConsoleUtil.GetAnyKey();
            LoadingPage();
        }

        // save page
        private void SavePage()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("");
                Console.WriteLine("\t\t==== 저장하기 ====");
                Console.WriteLine("  현재 플레이 데이터를 파일에 저장할 수 있습니다.");
                // show option
                Console.WriteLine("");
                // 1. 저장하기
                // 0. 나가기
                Console.WriteLine("1. 저장하기\n\n0. 나가기");

                // get player's input
                int input = ConsoleUtil.GetInput(0, 1);
                switch (input)
                {
                    case 1:  // save data
                        GameData gameData = new GameData(player, Player.Gold, Dungeon.Stage, Item.GetItemList());
                        DataManager.SaveData(gameData);
                        SaveCompletePage();
                        return;
                    case 0:  // back to startpage
                        StartPage(); return;
                }
            }
        }
        private void SaveCompletePage()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\t==== 저장하기 ====");
            Console.WriteLine();
            Console.WriteLine("  데이터를 저장했습니다!");
            if (ConsoleUtil.GetAnyKey()) { SavePage(); }
        }

        // end game
        public void EndGame()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("  게임을 종료합니다.");
            Thread.Sleep(1000);  // wait for 1 second
            Environment.Exit(0);  // normal exit
        }
    }
}
