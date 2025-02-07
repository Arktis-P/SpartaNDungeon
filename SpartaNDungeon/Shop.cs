using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SpartaNDungeon
{
    internal class Shop
    {
        public List<Item> ItemSale { get; private set; }
        
        public Shop()
        {
            ItemSale = Item.GetItemList(); //목록 가져오기

        }



        public void AddItem(Item item)
        {
            ItemSale.Add(item);

        }
        public void RemoveItem(Item item)
        {
            ItemSale.Remove(item);
        }

        public void DisplayItem()
        {
            Console.WriteLine("====== 상점 ====== ");
            Console.WriteLine($"보유 골드 : {Player.Gold}G ");
            Console.WriteLine();
            int index = 1;

            foreach (var saleItem in ItemSale)
            {
                Console.WriteLine($"{index}. {saleItem.Name} | {saleItem.Descrip} |  {saleItem.Cost}G");
                index++;
            }
        }
        
        public void BuyItem(Item item)
        {
            if(Player.Gold >= item.Cost)
            {
                Player.Gold -= item.Cost;
                Console.WriteLine($"{item.Name} 구매완료!");
                Player.AddItem(item); //구매한 아이템 인벤토리로
            }

            else
            {
                Console.WriteLine("골드가 충분하지 않습니다.");
            }
        }
        public void SellItem(Item item)
        {
            int sellPrce = item.Cost / 2;
            Player.Gold += sellPrce;
            Console.WriteLine($"{item.Name}이/가 {sellPrce}gold 로 판매되었습니다!");
            Player.RemoveItem(item); //판매한 아이템 인벤토리에서 제거
        }
            
    }

}
