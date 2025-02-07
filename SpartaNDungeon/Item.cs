using System.Numerics;

namespace SpartaNDungeon
{
    internal class Program
    {

        public enum ItemType
        {
            Weapon,
            Armor,
            Potion
        }

        static List<Item> itemList = new List<Item>
            {
                new Item("수련자의 갑옷", ItemType.Armor, 4, "수련에 도움을 주는 갑옷입니다.", 1000,0),
                new Item("무쇠갑옷", ItemType.Armor, 9, "무쇠로 만들어져 튼튼한 갑옷입니다.", 2000,0),
                new Item("스파르타의 갑옷", ItemType.Armor, 15, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 3500,0),
                new Item("낡은 검", ItemType.Weapon, 5, "쉽게 볼 수 있는 낡은 검입니다.", 600,0),
                new Item("청동 도끼", ItemType.Weapon, 10, "어디선가 사용됐던 것 같은 도끼입니다.", 1500,0),
                new Item("스파르타의 창", ItemType.Weapon, 15, "스파르타의 전사들이 사용했다는 전설의 창입니다.", 2500,0),
                new Item("회복 물약", ItemType.Potion, 30, "체력을 30 회복 할 수 있습니다.", 1000,3)
            };

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
                string str = IsEquip ? "[E] " : "";
                str += $"- {Name} | {GetType()} | {Descrip} | {GetPriceString()}";
                if (Type == ItemType.Potion)
                {
                    str += $"  (보유량: {Count})";
                    if(IsEquip==true)
                    {
                        Count--;

                    }
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

            public int UseItem()
            {
                // 아이템 사용 로직 추가 
                //if (Type == ItemType.Potion)
                //{
                //    if (Count > 0)
                //    {
                //        Count--;
                //        return player.Health+=30; // 사용 성공
                //    }
                //    else
                //    {
                //        Console.WriteLine("포션이 부족합니다!");
                //        return player.Health; // 사용 실패
                //    }
                //}

                return 0;
            }
        }

        static void Main(string[] args)
        {
            // 출력 테스트
            foreach (var item in itemList)
            {
                Console.WriteLine(item.DisplayItem());

                Console.WriteLine();
            }
        }
    }
}