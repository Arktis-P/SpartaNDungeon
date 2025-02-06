﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaNDungeon
{
    static class ConsoleUtil
    {
        public static int GetInput(int min, int max)
        {
            while (true)
            {
                Console.WriteLine("원하시는 행동을 입력해주세요\n>> ");
                if (int.TryParse(Console.ReadLine(), out int inputNumber) && (inputNumber >= min) && (inputNumber <= max))
                    return inputNumber;

                Console.WriteLine("잘못된 입력입니다.");
            }
        }
    }
}
