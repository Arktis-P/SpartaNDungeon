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
        public int stage = 1;
        public Player player;
        public MonsterManager manager;
        public Dungeon(int stage, Player player, MonsterManager manager)
        {
            this.player = player;
            this.stage = stage;
            SetMonster(stage);
            this.manager = manager;
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
            switch (ConsoleUtil.GetInput(0,3))
            {
                case 0: // 나가기
                    // startPage();
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
            Console.WriteLine($"포션을 사용하면 체력을 30 회복할 수 있습니다. (남은 포션: )");
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
        
        public void UsePotion() // 포션 사용
        {
            //개수 검사 후 사용
            //Item에서 가져오기
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
            List<Item> potions = Item.GetItemList().Where(x => x.Type == ItemType.Potion).ToList();
            List<Item> items = Item.GetItemList().Where(x => x.Type == ItemType.Armor || x.Type == ItemType.Weapon).ToList();
            Random random = new Random();
            int randomItem = random.Next(1, stage);
            for (int i = 0; i < randomItem; i++)
            {
                Item potion = potions[random.Next(potions.Count)];
                reward.Add(potion);
                Item item = items[random.Next(items.Count)];
                reward.Add(item);
                
            }
            DisplayReward(reward, randomItem); // 보상 출력
            player.inventory.AddRange(reward); // 보상 인벤토리에 추가
        }

        public void DisplayReward(List<Item> reward, int random)
        {
            Console.WriteLine("[획득 아이템]");
            foreach (Item item in reward)
            {
                Console.WriteLine($"{item.Name} - {random}");
            }
        }
    }
}
