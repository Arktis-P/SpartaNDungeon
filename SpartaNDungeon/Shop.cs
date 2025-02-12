﻿using System;
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
            List<string> names = new List<string>();
            List<string> descrips = new List<string>();
            foreach (Item iitem in ItemSale)
            {
                names.Add(iitem.Name); descrips.Add(iitem.Descrip);
            }
            int nameMax = ConsoleUtil.CalcuatedMaxNumber(names);
            int descripMax = ConsoleUtil.CalcuatedMaxNumber(descrips);

            int index = 1;
            foreach (var saleItem in ItemSale)
            {
                if (saleItem.GetPriceString() == "구매완료") { Console.ForegroundColor = ConsoleColor.DarkGray; }
                string indexStr = String.Format("{0,2}", index);
                Console.WriteLine($"{indexStr}.  {ConsoleUtil.WriteSpace(saleItem.Name, nameMax)}| {ConsoleUtil.WriteSpace(saleItem.Descrip, descripMax)}| {saleItem.GetType()}\t| {saleItem.GetPriceString()}");
                index++;
                Console.ResetColor();
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
                ConsoleUtil.ColorWrite(item.Name, ConsoleColor.Gray); //구매한 아이템은 회색 글씨로
                Console.WriteLine($"  이미 {item.Name}을 구매하였습니다.");
            }





            else if (Player.Gold >= item.Cost)
            {
                Player.Gold -= item.Cost;
                item.IsPurchase = true;
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
                Console.WriteLine("  골드가 충분하지 않습니다.");
            }
            ConsoleUtil.GetAnyKey();
        }
        public void SellItem(Player player,Item item)
        {
            int sellPrce = item.Cost / 2;
            Player.Gold += sellPrce;
            player.RemoveItem(item); //판매한 아이템 인벤토리에서 제거
            item.IsPurchase = false;
            // to sell complete page
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\t==== 상점 - 판매하기 ====");
            Console.WriteLine();
            Console.WriteLine($"  {item.Name}을/를 판매하고, {item.Cost/2} G를 받았습니다.");
            ConsoleUtil.ColorWrite(item.Name, ConsoleColor.White);// 다시 흰색 글씨로 변환
            ConsoleUtil.GetAnyKey();
        }
            
    }

}
