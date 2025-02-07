using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SpartaNDungeon
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.Start();
        }
    }

    class Game
    {
        private Player player;
        private UI ui;

        public void Start()
        {
            ui = new UI();
            ui.IntroductionPage();
            ui.StartPage();
        }
    }
}
