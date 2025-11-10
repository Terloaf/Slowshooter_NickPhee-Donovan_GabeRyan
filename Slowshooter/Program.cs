using System;
using System.Linq;

namespace Slowshooter
{
    internal class Program
    {

        static string playField = 
@"+---+   +---+
|   |   |   |
|   |   |   |
|   |   |   |
+---+   +---+";

        static bool isPlaying = true;

        // player input 
        static int p1_x_input;
        static int p1_y_input;

        static bool spacePress = false;
        static bool Bullet = false;
        static bool Bullet2 = false;
        

        static int p2_x_input;
        static int p2_y_input;
        

        // player 1 pos
        static int p1_x_pos = 2;
        static int p1_y_pos = 2;
        static int p1_BulletPosX = 0;
        static int p1_BulletPosY = 0;

        // player 2 pos
        static int p2_x_pos = 10;
        static int p2_y_pos = 2;
        static int p2_BulletPosX = 0;
        static int p2_BulletPosY = 0;

        // bounds for player movement
        static (int, int) p1_min_max_x = (1, 3);
        static (int, int) p1_min_max_y = (1, 3);
        static (int, int) p2_min_max_x = (9, 11);
        static (int, int) p2_min_max_y = (1, 3);
        static int p1_min_max_Bullet = 11;
        static int p2_min_max_Bullet = 1;

        // what turn is it? will be 0 after game is drawn the first time
        static int turn = -1;

        // contains the keys that player 1 and player 2 are allowed to press
        static (char[], char[]) allKeybindings = (new char[]{ 'W', 'A', 'S', 'D', ' '}, new char[]{ 'J', 'I', 'L', 'K', ' '});
        static ConsoleColor[] playerColors = { ConsoleColor.Red, ConsoleColor.Blue };

        static void Main(string[] args)
        {
            Console.CursorVisible = false;

            while(isPlaying)
            {
                ProcessInput();
                Update();
                Draw();
                
            }
        }

        static void ProcessInput()
        {
            // if this isn't here, input will block the game before drawing for the first time
            if (turn == -1) return;

            // reset input
            p1_x_input = 0;
            p1_y_input = 0;
            p2_x_input = 0;
            p2_y_input = 0;

            char[] allowedKeysThisTurn; // different keys allowed on p1 vs. p2 turn

            // choose which keybindings to use
            if (turn % 2 == 0) allowedKeysThisTurn = allKeybindings.Item1;
            else allowedKeysThisTurn = allKeybindings.Item2;

            // get the current player's input
            ConsoleKey input = ConsoleKey.NoName;
            while (!allowedKeysThisTurn.Contains(((char)input)))
            {
                input = Console.ReadKey(true).Key;
            }

            // check all input keys 
            if (input == ConsoleKey.A) p1_x_input = -1;
            if (input == ConsoleKey.D) p1_x_input = 1;
            if (input == ConsoleKey.W) p1_y_input = -1;
            if (input == ConsoleKey.S) p1_y_input = 1;
            if (input == ConsoleKey.Spacebar) spacePress = true; 

            if (input == ConsoleKey.J) p2_x_input = -1;
            if (input == ConsoleKey.L) p2_x_input = 1;
            if (input == ConsoleKey.I) p2_y_input = -1;
            if (input == ConsoleKey.K) p2_y_input = 1;

        }

        static void Update()
        {
            // update players' positions based on input
            p1_x_pos += p1_x_input;
            p1_x_pos = p1_x_pos.Clamp(p1_min_max_x.Item1, p1_min_max_x.Item2);

            p1_y_pos += p1_y_input;
            p1_y_pos = p1_y_pos.Clamp(p1_min_max_y.Item1, p1_min_max_y.Item2);

            p2_x_pos += p2_x_input;
            p2_x_pos = p2_x_pos.Clamp(p2_min_max_x.Item1, p2_min_max_x.Item2);

            p2_y_pos += p2_y_input;
            p2_y_pos = p2_y_pos.Clamp(p2_min_max_y.Item1, p2_min_max_y.Item2);

            if (spacePress == true && turn % 2 == 0 && Bullet == false)
            {
                p1_BulletPosX = p1_x_pos + 1;
                p1_BulletPosY = p1_y_pos;

                Bullet = true;
                spacePress = false;
            }

            if (spacePress == true && turn % 2 != 0 && Bullet2 == false)
            {
                p2_BulletPosX = p2_x_pos - 1;
                p2_BulletPosY = p2_y_pos;

                Bullet2 = true;
                spacePress = false;
            }
            turn += 1;
            

        }

        static void Draw()
        {
            // draw the background (playfield)
            Console.SetCursorPosition(0, 0);
            Console.Write(playField);

            // draw player 1
            Console.SetCursorPosition(p1_x_pos, p1_y_pos);
            Console.ForegroundColor = playerColors[0];
            Console.Write("O");
            if (Bullet == true)
            {
                Console.SetCursorPosition(p1_BulletPosX, p1_BulletPosY);
                Console.Write('x');
                p1_BulletPosX += 1;
                if(p1_BulletPosX == 12)
                {
                    Bullet = false;
                }
                else if (p1_BulletPosX == p2_x_pos && p1_BulletPosY == p2_y_pos)
                {
                    Console.Clear();
                    Console.WriteLine("GameOver Player 1 Wins");
                    isPlaying = false;
                    return;
                }
            }
            // draw player 2
            Console.SetCursorPosition(p2_x_pos, p2_y_pos);
            Console.ForegroundColor = playerColors[1];
            Console.Write("O");
            if (Bullet2 == true)
            {
                Console.SetCursorPosition(p2_BulletPosX, p2_BulletPosY);
                Console.Write('x');
                p2_BulletPosX -= 1;

                if (p2_BulletPosX == 0)
                {
                    Bullet2 = false;
                }
                else if (p2_BulletPosX == p1_x_pos && p2_BulletPosY == p1_BulletPosY)
                {
                    Console.Clear();
                    Console.WriteLine("GameOver Player 2 Wins");
                    isPlaying = false;
                    return;
                }
            }

            // draw the Turn Indicator
            Console.SetCursorPosition(3, 5);
            Console.ForegroundColor = playerColors[turn % 2];

            Console.Write($"PLAYER {turn % 2 + 1}'S TURN!");


            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\nUSE WASD or IJKL to move");
            Console.ForegroundColor = ConsoleColor.White;
            
            


        }
    }
}
