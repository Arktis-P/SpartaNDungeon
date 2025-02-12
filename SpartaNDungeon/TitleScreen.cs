using System;
using System.Text;
using System.Threading;

namespace SpartaNDungeon
{
    public class TitleScreen
    {
        public void Show()
        {
            Console.OutputEncoding = Encoding.UTF8;
            string title = @"

 _____                      _             _   _   ______  _   __  _   
/  ___|                    | |           | \ | |  | ___ \(_) / _|| |  
\ `--.  _ __    __ _  _ __ | |_   __ _   |  \| |  | |_/ / _ | |_ | |_ 
 `--. \| '_ \  / _` || '__|| __| / _` |  | . ` |  |    / | ||  _|| __|
/\__/ /| |_) || (_| || |   | |_ | (_| |  | |\  |  | |\ \ | || |  | |_ 
\____/ | .__/  \__,_||_|    \__| \__,_|  \_| \_/  \_| \_||_||_|   \__|
       | |                                                            
       |_|                                                            
                                                       
";

            // 타이틀 색상은 고정
            Console.Clear();
            ConsoleUtil.ColorWrite(title, ConsoleColor.White);

            // show sub title
            SubTitlePage();

            // "Press any key to start the game..." 문구를 한 번만 출력하고 색상만 변경
            Console.SetCursorPosition(0, Console.CursorTop + 1);
            Console.Write("Press any key to start the game...");

            // 색상만 변경
            while (true)
            {
                // 빨간색 출력
                Console.SetCursorPosition(0, Console.CursorTop); // 이전 줄로 커서 이동
                ConsoleUtil.ColorWritePart("계속하려면 아무 키나 누르세요. >>  ", ConsoleColor.Red);
                Thread.Sleep(500); // 0.5초 대기

                // 흰색 출력
                Console.SetCursorPosition(0, Console.CursorTop); // 이전 줄로 커서 이동
                ConsoleUtil.ColorWritePart("계속하려면 아무 키나 누르세요. >>  ", ConsoleColor.White);
                Thread.Sleep(500); // 0.5초 대기

                if (Console.KeyAvailable) // 키가 눌렸다면
                {
                    Console.ReadKey(true); break; // 루프 종료
                }
            }

            Console.ResetColor(); // 기본 색상으로 리셋
            Console.Clear();
        }

        private void SubTitlePage()  // show game title page
        {
            ConsoleUtil.ColorWrite("\n\n\n\t이곳이!", ConsoleColor.Red);
            Thread.Sleep(500);
            ConsoleUtil.ColorWrite("\n\t바로!", ConsoleColor.Red);
            Thread.Sleep(500);
            ConsoleUtil.ColorWrite("\n\t스파르타다!\n\n", ConsoleColor.Red);
            Thread.Sleep(500);
        }
    }
}