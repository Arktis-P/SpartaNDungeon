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

 ::::::::  :::::::::  :::::::::  :::::::::::     :::     ::::    ::: :::::::::  :::    ::: ::::    :::  ::::::::  ::::::::::  ::::::::  ::::    ::: 
:+:    :+: :+:    :+: :+:    :+:     :+:       :+: :+:   :+:+:   :+: :+:    :+: :+:    :+: :+:+:   :+: :+:    :+: :+:        :+:    :+: :+:+:   :+: 
+:+        +:+    +:+ +:+    +:+     +:+      +:+   +:+  :+:+:+  +:+ +:+    +:+ +:+    +:+ :+:+:+  +:+ +:+        +:+        +:+    +:+ :+:+:+  +:+ 
+#++:++#++ +#++:++#+  +#++:++#:      +#+     +#++:++#++: +#+ +:+ +#+ +#+    +:+ +#+    +:+ +#+ +:+ +#+ :#:        +#++:++#   +#+    +:+ +#+ +:+ +#+ 
       +#+ +#+        +#+    +#+     +#+     +#+     +#+ +#+  +#+#+# +#+    +#+ +#+    +#+ +#+  +#+#+# +#+   +#+# +#+        +#+    +#+ +#+  +#+#+# 
#+#    #+# #+#        #+#    #+#     #+#     #+#     #+# #+#   #+#+# #+#    #+# #+#    #+# #+#   #+#+# #+#    #+# #+#        #+#    #+# #+#   #+#+# 
 ########  ###        ###    ###     ###     ###     ### ###    #### #########   ########  ###    ####  ########  ##########  ########  ###    #### 
";

            // 타이틀 색상은 고정
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White; 
            Console.WriteLine(title); 

            // "Press any key to start the game..." 문구를 한 번만 출력하고 색상만 변경
            Console.SetCursorPosition(0, Console.CursorTop + 1);
            Console.Write("Press any key to start the game...");

            // 색상만 변경
            while (true)
            {
                // 빨간색 출력
                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(0, Console.CursorTop); // 이전 줄로 커서 이동
                Console.Write("Press any key to start the game...");
                Thread.Sleep(500); // 0.5초 대기

                // 흰색 출력
                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(0, Console.CursorTop); // 이전 줄로 커서 이동
                Console.Write("Press any key to start the game...");
                Thread.Sleep(500); // 0.5초 대기

                if (Console.KeyAvailable) // 키가 눌렸다면
                {
                    Console.ReadKey(true); 
                    break; // 루프 종료
                }
            }

            Console.ResetColor(); // 기본 색상으로 리셋
            Console.Clear(); 
        }
    }
}
