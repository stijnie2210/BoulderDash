using System;
namespace BoulderDash.Views {
    public class OutputView {

        public void PrintGame(string[,] level, bool IsPlaying, int diamonds, int timelimit, int score) {

            Console.Clear();

            if(IsPlaying) {
                Console.WriteLine("   ----------------------------------");
                Console.WriteLine("   |                                |");
                Console.WriteLine("   |          Boulder Dash          |");
                Console.WriteLine("   |                                |");
                Console.WriteLine("   ----------------------------------");
                Console.WriteLine();

                for (int y = 0; y < level.GetLength(1); y++) {
                    for (int x = 0; x < level.GetLength(0); x++) {
                        Console.Write(level[x, y]);
                    }
                    Console.WriteLine();
                }

                Console.WriteLine();
                Console.WriteLine("   ----------------------------------");
                Console.WriteLine("    Time Left: {0} seconds", timelimit);
                Console.WriteLine("    Score: {0} ", score);
                Console.WriteLine("    Diamonds: {0}", diamonds);
                Console.WriteLine("    Controls: W, A, S, D");
            }

            else {
                PrintGameOver(level, score);
            }
        }

        public void PrintGameOver(string[,] level, int score) {
            Console.WriteLine("   ----------------------------------");
            Console.WriteLine("   |                                |");
            Console.WriteLine("   |          Boulder Dash          |");
            Console.WriteLine("   |                                |");
            Console.WriteLine("   ----------------------------------");
            Console.WriteLine();

            for (int y = 0; y < level.GetLength(1); y++) {
                for (int x = 0; x < level.GetLength(0); x++) {
                    Console.Write(level[x, y]);
                }
                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine("   ----------------------------------");
            Console.WriteLine("                                     ");
            Console.WriteLine("               Game Over!            ");
            Console.WriteLine("               Score: {0}            ", score);
            Console.WriteLine("                                     ");
            Console.WriteLine("   ----------------------------------");
        }
    }
}
