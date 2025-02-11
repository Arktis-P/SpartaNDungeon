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
            // Console.WriteLine("====== 상점 ====== ");
            // Console.WriteLine($"보유 골드 : {Player.Gold} G ");
            Console.WriteLine();
            int index = 1;

            foreach (var saleItem in ItemSale)
            {
                Console.WriteLine($"{index}.  {saleItem.Name}\t| {saleItem.Descrip}\t|  {saleItem.Cost} G");
                index++;
            }
        }
        
        public void BuyItem(Player player,Item item)
        {
            if (player.HasItem(item))
            {
                Console.Clear();
                Console.WriteLine();    
                Console.WriteLine("\t\t==== 상점 - 구매하기 ====");
                Console.WriteLine();
                Console.WriteLine($"이미 {item.Name}을 구매하였습니다.");
            }





            else if (Player.Gold >= item.Cost)
            {
                Player.Gold -= item.Cost;
                player.AddItem(item); //구매한 아이템 인벤토리로
                // to buy complete page
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("\t\t==== 상점 - 구매하기 ====");
                Console.WriteLine();
                Console.WriteLine($"  {item.Name}을(를) 구매해주셔서 감사합니다.");
            }

            else
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("\t\t==== 상점 - 구매하기 ====");
                Console.WriteLine();
                Console.WriteLine("골드가 충분하지 않습니다.");
            }
            ConsoleUtil.GetAnyKey();
        }
        public void SellItem(Player player,Item item)
        {
            int sellPrce = item.Cost / 2;
            Player.Gold += sellPrce;
            player.RemoveItem(item); //판매한 아이템 인벤토리에서 제거
            // to sell complete page
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\t==== 상점 - 판매하기 ====");
            Console.WriteLine();
            Console.WriteLine($"{item.Name}을/를 판매하고, {item.Cost/2} G를 받았습니다.");
            ConsoleUtil.GetAnyKey();
        }
            
    }

}
