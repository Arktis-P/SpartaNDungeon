using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaNDungeon
{
    internal class Dungeon
    {
        private string[] dungeonMenu = { "상태 보기", "전투 시작", "포션 사용" };
        public List<Monster> monsters = new List<Monster>(); // 출현 몬스터 지정
        public int Stage { get;  set; }
        public Player player;
        public MonsterManager manager;
        public UI ui;
        public Dungeon(int stage, Player player, MonsterManager manager)
        {
            this.player = player;
            this.Stage = stage;
            this.manager = new MonsterManager();
            SetMonster(stage);

        }
        public void DungeonPage()
        {
            Console.Clear();
            Console.WriteLine("협곡입장");
            Console.WriteLine("미니언 생성까지 한 발자국 남았습니다.\n협곡에는 페르시아가 보낸 몬스터가 가득합니다.\n입장하기 전에 만반의 준비를 갖춰주십시오.");
            for(int i = 0; i < dungeonMenu.Length; i++)
            {
                Console.WriteLine($"{i+1}. {dungeonMenu[i]}");
            }
            Console.WriteLine("0. 나가기");

            Console.WriteLine("원하시는 행동을 입력해주세요");
            switch (ConsoleUtil.GetInput(0,3))
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
            Console.WriteLine("상태 보기");
            Console.WriteLine("던전에 입장할 캐릭터의 정보가 표시됩니다.");

            //플레이어 정보 출력
            player.DisplayStatus();

            Console.WriteLine("0. 나가기");
            ConsoleUtil.GetInput(0, 0);
        }
        
        public void UsePotionPage() // 포션 사용
        {
            Console.WriteLine("회복");
            Console.WriteLine($"포션을 사용하면 체력을 30 회복할 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("1. 사용하기\n0. 나가기");
            
            switch(ConsoleUtil.GetInput(0, 1))
            {
                case 0:
                    DungeonPage();
                    break;
                case 1:
                    UsePotion();
                    break;
                default:
                    break;
            }
        }
        
        public void UsePotion()
        {
            // 개수 검사
            Item potion = player.inventory.FirstOrDefault(x => x.Type == ItemType.Potion);
            if(potion != null)
            {
                Console.WriteLine($"{potion.Name}을 사용했습니다.");
                int newHp = Item.UseItem(player, potion);
                Console.WriteLine($"체력 30 회복.\n HP {newHp-30} -> {newHp}");
                potion.Count--;
                if(potion.Count <= 0)
                {
                    player.inventory.Remove(potion);
                }
                Console.WriteLine($"남은 포션: {potion.Count}개") ;
            }
        }
        
        
        public void SetMonster(int stage) // 해당 던전에서 출현하는 몬스터 저장
        {
            List<Monster> randomMonsters = manager.RandomMonster();
            if(stage < 3)
            {
                for(int i = 0; i < 3; i++)
                {
                    monsters.Add(randomMonsters[i]);
                }
            }
            else
            {
                monsters.AddRange(randomMonsters);
            }
        }

        public void Reward(int stage) // 1~stage 개수 만큼 보상 랜덤 지급
        {
            List<Item> reward = new List<Item>();
            Item potion = Item.GetItemList().FirstOrDefault(x => x.Type == ItemType.Potion);
            List<Item> items = Item.GetItemList().Where(x => x.Type == ItemType.Armor || x.Type == ItemType.Weapon).ToList();

            Random random = new Random();
            int randomItem = random.Next(1, stage); // 아이템 보상 개수
            int randomPotion = random.Next(1, stage+1); // 포션 보상 개수

            //포션 
            if (potion != null) potion.Count += randomPotion;
            else player.inventory.Add(new Item("회복 물약", ItemType.Potion, 30, "체력을 30 회복 할 수 있습니다.", 1000, randomPotion));
            reward.Add(potion);

            //장비 아이템
            for (int i = 0; i < randomItem; i++)
            {
                Item item = items[random.Next(items.Count)];
                if (item != null) item.Count++;
                else player.inventory.Add(new Item(item.Name, item.Type, item.Value, item.Descrip, item.Cost, 1));
                reward.Add(item);
            }

            //골드
            int rewardGold = 500 * stage;
            player.Gold += rewardGold;

            DisplayReward(reward, randomPotion, rewardGold); // 보상 출력
        }

        public void DisplayReward(List<Item> reward,int randomPotion, int rewardGold)
        {
            Console.WriteLine("[획득 아이템]");
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
            Stage++;
            monsters.Clear();
            SetMonster(Stage);
        }
    }
}
