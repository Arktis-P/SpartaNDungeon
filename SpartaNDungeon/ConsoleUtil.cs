using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaNDungeon
{
    static class ConsoleUtil
    {
        // min부터 max까지만 입력 가능. 그 외의 값을 입력할 경우 잘못된 입력입니다 출력 후 다시 입력
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
