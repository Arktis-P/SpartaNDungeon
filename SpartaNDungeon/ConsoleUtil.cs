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
                Console.WriteLine("원하시는 행동을 입력해 주십시오. >> ");
                if (int.TryParse(Console.ReadLine(), out int inputNumber) && (inputNumber >= min) && (inputNumber <= max))
                    return inputNumber;

                Console.WriteLine("잘못된 입력입니다.");
            }
        }
        // get any key of player
        public static bool GetAnyKey()
        {
            while (true)
            {
                Console.WriteLine("계속하려면 아무 키나 누르세요. >>");
                Console.ReadKey();
                return true;
            }
        }
    }
}
