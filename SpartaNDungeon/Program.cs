using System;
using System.Collections.Generic;
using System.Linq;
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

        public void Start()
        {
            ShowIntroduction();
            InitializePlayer();
            UI.StartPage();
        }
    }
}
