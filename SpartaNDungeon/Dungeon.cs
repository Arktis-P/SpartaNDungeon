using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaNDungeon
{
    internal class Dungeon
    {
        //public string[] dungeonMenu = { "상태 보기", "전투 시작", "포션 사용" };
        public List<Monster> monsters = new List<Monster>(); // 출현 몬스터 지정
        public static int Stage { get;  set; }
        public Player player;
        public MonsterManager manager;
        public UI ui;
        public Dungeon(int stage, Player player, MonsterManager manager, UI ui)
        {
            this.player = player;
            Dungeon.Stage = stage;
            this.manager = new MonsterManager(player.Level);
            this.ui = ui;
            SetMonster(stage);

        }
        public void DungeonPage()
        {
            Console.Clear();
            Console.WriteLine("\x1b[3J");
            Console.WriteLine();
            Console.WriteLine("\t\t==== 협곡입장 ====");
            Console.Write("  미니언 생성까지 한 발자국 남았습니다.\n  협곡에는 ");
            ConsoleUtil.ColorWritePart("페르시아", ConsoleColor.Red);
            Console.WriteLine("가 보낸 몬스터가 가득합니다.\n  입장하기 전에 만반의 준비를 갖춰주십시오.\n");
            Console.WriteLine("\n1. 상태 보기");
            Console.Write($"2. 전투 시작 (현재 스테이지: ");
            ConsoleUtil.ColorWritePart(Stage.ToString(), ConsoleColor.Green);
            Console.WriteLine(")");
            Console.WriteLine("3. 포션 사용\n");
            Console.WriteLine("0. 나가기");

            switch (ConsoleUtil.GetInput(0, 3))
            {
                case 0: // 나가기
                    ui.StartPage();
                    break;
                case 1: // 상태 보기
                    BattleStatusPage();
                    break;
                case 2: // 전투 시작
                    Battle battle = new Battle(this);
                    battle.EnterDungeon();
                    break;
                case 3: // 포션 사용
                    UsePotionPage();
                    break;
                default:
                    break;
            }
        }

        public void BattleStatusPage() // 상태 보기
        {
            Console.Clear();
            Console.WriteLine("\x1b[3J");
            Console.WriteLine("\t\t==== 상태 보기 ====");
            Console.WriteLine("  던전에 입장할 캐릭터의 정보가 표시됩니다.\n");

            //플레이어 정보 출력
            player.DisplayStatus();

            Console.WriteLine("0. 나가기");
            if (ConsoleUtil.GetInput(0, 0) == 0) DungeonPage();
        }
        
        public void UsePotionPage() // 포션 사용
        {
            Console.Clear();
            Item potion = player.inventory.FirstOrDefault(x => x.Type == ItemType.Potion);
            Console.WriteLine("회복");
            Console.Write($"포션을 사용하면 체력을 30 회복할 수 있습니다. ");
            if (potion == null) Console.WriteLine("남은 포션: 0개");
            else Console.WriteLine($"(남은 포션: {potion.Count})");
            Console.WriteLine();
            Console.WriteLine("1. 사용하기\n0. 나가기");

            switch (ConsoleUtil.GetInput(0, 1))
            {
                case 0:
                    DungeonPage();
                    break;
                case 1:
                    UsePotion(potion);
                    break;
                default:
                    break;
            }
        }
        
        public void UsePotion(Item potion)
        {
            if (potion != null)
            {
                Console.WriteLine($"\n{potion.Name}을 사용했습니다.");
                Console.Write($"체력 30 회복.\nHP {player.Health} ");
                Item.UseItem(player, potion);
                if(player.Health > player.MaxHealth) player.Health = player.MaxHealth;
                Console.WriteLine($"-> {player.Health}");
                if (potion.Count <= 0)
                {
                    player.RemoveItem(potion);
                }
                if (potion == null) Console.WriteLine("남은 포션: 0개");
                else Console.WriteLine($"남은 포션: {potion.Count}");
            }
            else
            {
                Console.WriteLine("사용할 포션이 없습니다.");
            }
            Thread.Sleep(1000);
            UsePotionPage();
        }
        
        
        public void SetMonster(int stage) // 해당 던전에서 출현하는 몬스터 저장
        {
            List<Monster> randomMonsters = manager.RandomMonster();
            monsters.AddRange(randomMonsters);
        }

        public void Reward(int stage) // 1~stage 개수 만큼 보상 랜덤 지급
        {
            List<Item> reward = new List<Item>();
            Item potion = Item.GetItemList().FirstOrDefault(x => x.Type == ItemType.Potion);
            List<Item> items = Item.GetItemList().Where(x => x.Type == ItemType.Armor || x.Type == ItemType.Weapon).ToList();
            items = items.Where(i => i != null).ToList(); //null 제거

            Random random = new Random();
            int randomItem = random.Next(1, stage); // 아이템 보상 개수
            int randomPotion = random.Next(1, stage+1); // 포션 보상 개수

            //포션 
            Item potionInven = player.inventory.FirstOrDefault(x=>x.Type == ItemType.Potion);
            if (potionInven != null) potionInven.Count += randomPotion;
            else
            {
                potion.Count = randomPotion;
                player.AddItem(potion);
            }
            reward.Add(potion);

            //장비 아이템
            for (int i = 0; i < randomItem; i++)
            {
                Item item = items[random.Next(items.Count)];
                if (player.inventory.Contains(item))
                {
                    item.Count++;
                }
                else
                {
                    player.AddItem(item);
                }
                reward.Add(item);
            }

            //골드
            int rewardGold = 500 * stage;
            Player.Gold += rewardGold;

            DisplayReward(reward, randomPotion, rewardGold); // 보상 출력
        }

        public void DisplayReward(List<Item> reward,int randomPotion, int rewardGold)
        {
            Console.WriteLine();
            ConsoleUtil.ColorWrite("[획득 아이템]", ConsoleColor.Yellow);
            Console.WriteLine($"{rewardGold} G");
            foreach (Item item in reward)
            {
                if(item.Type == ItemType.Potion)
                {
                    Console.WriteLine($"{item.Name} - {randomPotion}개");
                }
                else Console.WriteLine($"{item.Name} - 1개");
            }

        }

        public void NextStage()
        {
            if (Stage < 7)
            {
                Stage++;
                monsters.Clear();
                SetMonster(Stage);
            }
            else {
                Stage = 1; //Stage 1로
                ui.EndingPage();
            }
        }
    }
}
