using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaNDungeon
{
    internal class Shop
    {
        public List<Item> ItemSale { get; set; }
        
        public Shop()
        {
            ItemSale = new List<Item>();

        }

        public void AddItem(Item item)
        {
            ItemSale.Add(item);

        }
        public void RemoveItem(Item item)
        {
            ItemSale.Remove(item);
        }

        public void DisplayItem(Item item)
        {
            Console.WriteLine("--- 상점 --- ");
            foreach (var item in ItemSale)
            {
                Console.WriteLine($"{item.Name}({item.Price}) gold");
            }
        }
        
        public void BuyItem(Player player, Item item)
        {
            if(player.Gold >= selectedItem.Price)
            {
                player.Gold -= selectedItem.Price;
                Console.WriteLine($"{selectedItem.Name} 구매완료!");
                player.DisplayInventory.Add(selectedItem); //구매한 아이템 인벤토리로
            }

            else
            {
                Console.WriteLine("골드가 충분하지 않습니다.");
            }
        }
        public void SellItem(Player player, int Item item)
        {
            int sellPrce = Item.Price / 2;
            player.Gold += sellPrce;
            Console.WriteLine($"{Item.Name}이/가 {sellPrce}gold 로 판매되었습니다!");
            player.DisplayInventory().Remove(Item); //판매한 아이템 인벤토리에서 제거
        }
            
    }

}
