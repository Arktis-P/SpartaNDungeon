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
                names.Add(iitem.Name);
                descrips.Add(iitem.Descrip);
            }
            int nameMax = ConsoleUtil.CalcuatedMaxNumber(names);
            int descripMax = ConsoleUtil.CalcuatedMaxNumber(descrips);

            int index = 1;
            foreach (var saleItem in ItemSale)
            {
                // 아이템이 구매된 상태일 때 회색으로 표시
                if (saleItem.IsPurchase)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White; // 기본 흰색으로 표시
                }

                string indexStr = String.Format("{0,2}", index);
                Console.WriteLine($"{indexStr}.  {ConsoleUtil.WriteSpace(saleItem.Name, nameMax)}| {ConsoleUtil.WriteSpace(saleItem.Descrip, descripMax)}| {saleItem.GetType()}\t| {saleItem.GetPriceString()}");
                index++;
                Console.ResetColor();
            }
        }


        public void BuyItem(Player player, Item item)
        {
            Console.Clear();
            Console.WriteLine("\t\t==== 상점 - 구매하기 ====");
            Console.WriteLine();

            // 이미 구매한 아이템인지 확인 (포션 제외)
            if (item.Type != ItemType.Potion && player.HasItem(item))
            {
                Console.WriteLine($"  이미 {item.Name}을(를) 구매하였습니다.");
            }
            else if (Player.Gold < item.Cost)
            {
                Console.WriteLine("  골드가 충분하지 않습니다.");
            }
            else
            {
                Player.Gold -= item.Cost; // 골드 차감

                if (item.Type == ItemType.Potion)
                {
                    // 포션 개수 증가
                    item.Count++;
                    Console.WriteLine($"  {item.Name}을(를) 구매해주셔서 감사합니다. (보유량: {player.inventory.Find(i => i.Name == item.Name)!.Count}개)");
                }
                else
                {
                    item.IsPurchase = true;
                    player.AddItem(item); // 인벤토리에 추가
                    Console.WriteLine($"  {item.Name}을(를) 구매해주셔서 감사합니다.");
                }
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
            ConsoleUtil.GetAnyKey();
        }
            
    }

}
