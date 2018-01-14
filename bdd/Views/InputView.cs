using System;
using System.Threading;
using BoulderDash.Controllers;
using BoulderDash.Models;

namespace BoulderDash.Views {
    public class InputView {

        GameController GameController;

        public InputView(GameController gameController) {

            this.GameController = gameController;
            Thread t = new Thread(GetUserInput);
            t.Start();

        }

        public void GetUserInput() {

            while(GameController.Game.IsPlaying) {
                ConsoleKey key = Console.ReadKey(true).Key;

                switch (key) {
                    case ConsoleKey.UpArrow:
                        GameController.Game.MoveHero(Direction.UP);
                        break;

                    case ConsoleKey.LeftArrow:
                        GameController.Game.MoveHero(Direction.LEFT);
                        break;

                    case ConsoleKey.DownArrow:
                        GameController.Game.MoveHero(Direction.DOWN);
                        break;

                    case ConsoleKey.RightArrow:
                        GameController.Game.MoveHero(Direction.RIGHT);
                        break;
                }
            }
        }
    }
}
