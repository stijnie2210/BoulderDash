using System;
using System.Threading;
using BoulderDash.Models;
using BoulderDash.Views;

namespace BoulderDash.Controllers {
    public class GameController {

        public OutputView OutputView { get; set; }
        public Game Game { get; set; } = new Game();

        public GameController() {

            new InputView(this);
            OutputView = new OutputView();
            Thread t = new Thread(Play);
            t.Start();

        }

        public void Play() {

            while(Game.IsPlaying) {
                PrintGame(GetLevel());
                Game.CheckObjects();
                Game.MoveEnemies();
                if (Game.CurrentLevel.IsCompleted) {
                    Game.CurrentLevel = Game.CurrentLevel.Next;
                }
                Thread.Sleep(400);
                PrintGame(GetLevel());
            }

            PrintGame(GetLevel());

        }

        public string[,] GetLevel() {

            string[,] level = new string[Game.CurrentLevel.Fields.GetLength(0), Game.CurrentLevel.Fields.GetLength(1)];

            for (int y = 0; y < Game.CurrentLevel.Fields.GetLength(1); y++) {
                for (int x = 0; x < Game.CurrentLevel.Fields.GetLength(0); x++) {
                    if(Game.CurrentLevel.Fields[x, y].Character != null) {
                        level[x, y] = Game.CurrentLevel.Fields[x, y].Character.ToString();    
                    }

                    else if (Game.CurrentLevel.Fields[x, y].Object != null) {
                        level[x, y] = Game.CurrentLevel.Fields[x, y].Object.ToString();    
                    }

                    else {
                        level[x, y] = Game.CurrentLevel.Fields[x, y].ToString();    
                    }

                }
            }

            return level;

        }

        public void PrintGame(string[,] level) {
            OutputView.PrintGame(level, Game.IsPlaying, Game.DiamondsLeft, Game.TimeLimit, Game.Score);
        }



    }
}
