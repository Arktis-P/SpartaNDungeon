using Microsoft.VisualBasic;
using System.Numerics;

namespace SpartaNDungeon
{

    public enum ItemType
    {
        Weapon,
        Armor,
        Potion
    }

    public class Item
    {
        public string Name { get; }
        public ItemType Type { get; }
        public int Value { get; }
        public string Descrip { get; }        
        public int Cost { get; }
        public int Count { get; set; }
        public bool IsPurchase { get; set; }
        public bool IsEquip { get; set; }

        // 아이템 리스트를 Item 클래스 내부로 이동
        private static List<Item> itemList = new List<Item>
        {
            new Item("가시 갑옷", ItemType.Armor, 3, "고대의 용사가 입었던 갑옷.", 1000, 0),
            new Item("지배자의 피갑옷", ItemType.Armor, 4, "한 시대를 피로 물들인 폭군의 갑옷.", 1500, 0),
            new Item("망자의 갑옷", ItemType.Armor, 6, "전장에서 쓰러진 병사들의 원혼이 깃든 갑옷.", 2000, 0),
            new Item("심연의 갑옷", ItemType.Armor, 8, "심연 속에서 떠오른 갑옷. 어둠을 두려워하지 않는다면 그것은 그의 것이 된다.", 2500, 0),
            new Item("정령의 형상 갑옷", ItemType.Armor, 10, "잊혀진 전사의 유산. ", 3000, 0),
            new Item("워모그의 갑옷", ItemType.Armor, 13, "워모그, 불사의 거인의 이름을 따 만든 갑옷.", 4000, 0),
            new Item("몰락한 왕의 검", ItemType.Weapon, 5, "한때 왕국을 다스리던 자의 검.", 600, 0),
            new Item("쇼진의 창", ItemType.Weapon, 10, "용을 닮고자 했던 수도원의 유산.", 1500, 0),
            new Item("어둠불꽃 횃불", ItemType.Weapon, 15, "불꽃이 꺼지지 않는 창. 하지만 그 불꽃이 어디서 왔는지는 아무도 모른다.", 800, 0),
            new Item("공허의 지팡이", ItemType.Weapon, 15, "차원을 넘어온 힘이 깃든 지팡이.", 2000, 0),
            new Item("원칙의 원형낫", ItemType.Weapon, 15, "오직 한 가지 신념만을 따르는 자를 위한 무기", 1500, 0),
            new Item("스태틱의 단검", ItemType.Weapon, 15, "하늘을 가르는 번개, 그리고 그보다 더 빠른 단검", 2500, 0),
            new Item("루난의 허리케인", ItemType.Weapon, 15, "바람을 조종하는 궁수의 유산.", 1500, 0),
            new Item("고속 연사포", ItemType.Weapon, 15, "왕국의 친위대 저격수들을 위해 제작된 무기.", 2500, 0),
            new Item("회복 물약", ItemType.Potion, 30, "체력을 30 회복 할 수 있습니다.", 1000, 3)
        };
        public static List<Item> GetItemList()
        {
            return itemList;
        }


        public Item(string name, ItemType type, int value, string descrip,  int cost, int count)
        {
            Name = name;
            Type = type;
            Value = value;
            Descrip = descrip;
            Cost = cost;
            Count = count < 0 ? 0 : count; ;
            IsPurchase = false;
            IsEquip = false;

        }

        public string DisplayItem()
        {
            //string str = IsEquip ? "[E] " : "";
            string str = $"- {Name} | {GetType()} | {Descrip} | {GetPriceString()}";
            if (Type == ItemType.Potion)
            {
                str += $"  (보유량: {Count})";
            }

            return str;
        }

        public string GetType()
        {
            if (Type == ItemType.Weapon)
                return $"공격력+{Value}";
            else if (Type == ItemType.Armor)
                return $"방어력+{Value}";
            else if (Type == ItemType.Potion)
                return $"회복력+{Value}";

            return "알 수 없는 아이템";
        }

        public string GetPriceString()
        {
           return IsPurchase ? "구매완료" : $"{Cost} G";
        }

        public static int UseItem(Player player, Item item)
        {
            if (item.Type == ItemType.Potion)
            {
                if (item.Count > 0)
                {
                    item.Count--;
                    int health = player.Health += item.Value;
                    return health >= 100 ? 100 : health;
                }
                else
                {
                    return player.Health;
                }
            }
            return 0;
        }
        public void EquipItem(Player player)
        {
            if (IsEquip)
            {
                // 장비 해제
                IsEquip = false;
                if (Type == ItemType.Weapon) player.Attack -= Value;
                if (Type == ItemType.Armor) player.Defense -= Value;
                Console.WriteLine($"{Name}을(를) 해제했습니다.");
            }
            else
            {
                // 같은 타입의 기존 장착 아이템 찾기
                Item? equippedItem = player.inventory.Find(i => i.IsEquip && i.Type == Type);

                if (equippedItem != null)
                {
                    equippedItem.IsEquip = false;
                    if (equippedItem.Type == ItemType.Weapon) player.Attack -= equippedItem.Value;
                    if (equippedItem.Type == ItemType.Armor) player.Defense -= equippedItem.Value;
                    Console.WriteLine($"{equippedItem.Name}을(를) 해제했습니다.");
                }

                // 새 아이템 장착
                IsEquip = true;
                if (Type == ItemType.Weapon) player.Attack += Value;
                if (Type == ItemType.Armor) player.Defense += Value;
                Console.WriteLine($"{Name}을(를) 장착했습니다.");
                SetBonus(player);
            }
        }
        private void SetBonus(Player player) // 아이템 세트 효과
        {
            var equippedItems = player.inventory.Where(i => i.IsEquip).ToList();

            int hasSetBonus = 0;
            if (equippedItems.Any(i => i.Name == "몰락한 왕의 검") && equippedItems.Any(i => i.Name == "지배자의 피갑옷"))  hasSetBonus = 1;
            if (equippedItems.Any(i => i.Name == "심연의 갑옷") && equippedItems.Any(i => i.Name == "어둠불꽃 횃불")) hasSetBonus = 2;

             switch (hasSetBonus)
            {
                case 1:
                    player.Attack += 2;
                    player.Defense += 1;
                    Console.WriteLine("세트 효과 발동! 공격력: 2 | 방어력: 1 추가 증가했습니다.");
                    break;
                case 2:
                    player.Attack += 1;
                    player.Defense += 2;
                    Console.WriteLine("세트 효과 발동! 공격력: 1 | 방어력: 2 추가 증가했습니다.");
                    break;
            }
            
        }
    }
}

    