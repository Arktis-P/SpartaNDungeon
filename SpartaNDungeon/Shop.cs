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
            Console.WriteLine("상점:");
            foreach (var item in ItemSale)
            {
                Console.WriteLine($"{item.Name}(itme.Price) gold");
            }
        }
    }

}
