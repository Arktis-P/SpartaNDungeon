using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaNDungeon
{
    internal class Battle
    {
        Random random = new Random();
        Dungeon dungeon;

        
        public Battle(Dungeon dungeon)
        {
            this.dungeon = dungeon;
        }
        public void EnterDungeon()
        {
            Console.Clear();
            Console.WriteLine("Battle!!\n");

            //Shuffle(dungeon.monsters);
            //foreach (string info in dungeon.monsters)
            //{
            //    Console.WriteLine(info);
            //}

            Console.WriteLine("[내정보]");
            Console.WriteLine();
        }

        public void Shuffle<T>(T[] array)
        {
            int n = array.Length;
            for(int i = n-1; i >0; i--)
            {
                int j = random.Next(0, i + 1);
                (array[i], array[j]) = (array[j], array[i]);
            }
        }

    }
}
