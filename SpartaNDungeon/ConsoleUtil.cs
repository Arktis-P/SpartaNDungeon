using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
                Console.WriteLine();
                Console.WriteLine();
                Console.Write("원하시는 행동을 입력해 주십시오. >>  ");
                if (int.TryParse(Console.ReadLine(), out int inputNumber) && (inputNumber >= min) && (inputNumber <= max))
                    return inputNumber;

                Console.WriteLine("\n\n잘못된 입력입니다.");
            }
        }
        // get any key of player
        public static bool GetAnyKey()
        {
            Thread.Sleep(1000);
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine();
                Console.Write("계속하려면 아무 키나 누르세요. >>  ");
                Console.ReadKey();
                return true;
            }
        }

        public static void ColorWrite(string str, ConsoleColor color) // 텍스트 칼라 변경
        {
            Console.ForegroundColor = color;
            Console.WriteLine(str);
            Console.ResetColor();
            // Black DarkBle DarkGreen DarkCyan DarkRed DarkMagenta DarkYellow
            // Gray Blue Green Cyan Red Magenta Yellow White
        }
        public static void ColorWritePart(string str, ConsoleColor color)  // change color of word by word
        {
            Console.ForegroundColor = color;
            Console.Write(str);
            Console.ResetColor();
        }

        public static void Loading() // 로딩
        {
            Console.Clear();
            Console.Write("Loading");
            string str = ".";

            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(80);
                Console.Write(str);
            }
        }
        
        // calculate length of string
        private static int CalculateMixedString(string str)
        {
            int len = 0;// total length of str (hangeul == 2)
            foreach (char c in str)
            {
                if (c >= '\uAC00' && c <= '\uD7A3') { len += 2; }  // if hangeul += 2
                else { len += 1; }
            }
            return len;
        }
        // calculate tabNumber for entire list
        public static int CalcuatedMaxNumber(List<string> list)
        {
            int MaxNumber = 0;
            foreach (string str in list)
            {
                int mixedLength = CalculateMixedString(str);
                if (mixedLength > MaxNumber) { MaxNumber = mixedLength; }
            }
            MaxNumber++; return MaxNumber;
        }
        // wirte string in particular spacing (using tabs)
        public static string WriteSpace(string str, int maxNumber)
        {
            int len = CalculateMixedString(str);
            int spaceRepeat = maxNumber - len;  // how may tabs needed
            for (int i = 0; i < spaceRepeat; i++) { str += " "; }

            return str;
        }
    }
}
