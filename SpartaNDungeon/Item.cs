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
            new Item("수련자의 갑옷", ItemType.Armor, 4, "수련에 도움을 주는 갑옷입니다.", 1000, 0),
            new Item("무쇠갑옷", ItemType.Armor, 9, "무쇠로 만들어져 튼튼한 갑옷입니다.", 2000, 0),
            new Item("스파르타의 갑옷", ItemType.Armor, 15, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 3500, 0),
            new Item("낡은 검", ItemType.Weapon, 5, "쉽게 볼 수 있는 낡은 검입니다.", 600, 0),
            new Item("청동 도끼", ItemType.Weapon, 10, "어디선가 사용됐던 것 같은 도끼입니다.", 1500, 0),
            new Item("스파르타의 창", ItemType.Weapon, 15, "스파르타의 전사들이 사용했다는 전설의 창입니다.", 2500, 0),
            new Item("회복 물약", ItemType.Potion, 30, "체력을 30 회복 할 수 있습니다.", 1000, 3)
        };
        public static List<Item> GetItemList()
        {
            return itemList;
        }


        public Item(string name, ItemType type, int value, string descrip, int cost, int count)
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
                //if(IsEquip==true)
                //{
                //    Count--;

                //}
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
                IsEquip = false;
                if (Type == ItemType.Weapon) player.Attack -= Value;
                if (Type == ItemType.Armor) player.Defense -= Value;
                Console.WriteLine($"{Name}을(를) 해제했습니다.");
            }
            else
            {
                IsEquip = true;
                if (Type == ItemType.Weapon) player.Attack += Value;
                if (Type == ItemType.Armor) player.Defense += Value;
                Console.WriteLine($"{Name}을(를) 장착했습니다.");
            }
        }

    }

   
}