using System;
using System.Collections.Generic;

namespace tic_tac_toe
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            bool bot_flag = true, active_player_flag = true, still_playing = true;
            byte field_side = 3;
            while (still_playing)
            {
                Greetings(ref bot_flag, ref active_player_flag, ref field_side, out string playermark, out string enemymark);
                Gameplay(bot_flag, active_player_flag, field_side, playermark, enemymark);
                Console.WriteLine("\nDo you wanna play again?\n 1 Yes\n 2 No ");
                string again = Console.ReadLine();
                while (again != "1" && again != "2")
                {
                    Console.Clear();
                    Console.WriteLine("\nDO YOU WANNA PLAY AGAIN ?\n\n" +
                    "1 Yea, sure.\n" +
                    "2 No.\n");
                    again = Console.ReadLine();
                }
                switch (again)
                {
                    case "2":
                        still_playing = false;
                        Console.Clear();
                        break;
                    default:
                        Console.Clear();
                        break;
                }
            }
        }

        public static void Greetings(ref bool bot_flag, ref bool active_player_flag, ref byte field_side, out string playermark, out string enemymark)
        {
            Console.WriteLine("Hello! This is a tic-tac-toe game with scalable field.\n" +
                "The rules are slightly different from the original:The goal is to collect the biggest number of three marks in a row.\n" +
                "Game ends, when there are no more opportunities to make a combination.\n" +
                "When turns out, that one of the players will win anyway, game ends automatically too.\n" +
                "So, choose your opponent! ( Write the number of your answer and press ENTER button. )\n\n1 I want to play with a robot.\n2 I want to play with a human.\n\n\n");
            string str = Console.ReadLine();
            while (str != "1" && str != "2")
            {
                Console.Clear();
                Console.WriteLine("Well, I mean, you need to write number '1' or'2', robot or player. Try it again ");
                str = Console.ReadLine();
            }
            bot_flag = str switch
            {
                "1" => true,
                _ => false,
            };
            Console.Clear();
            Console.WriteLine("Do u wanna go first?\n\n" +
                "1 Yea, sure.\n" +
                "2 No.\n" +
                "3 As you wish. (random player will start first)\n\n\n");
            str = Console.ReadLine();
            while (str != "1" && str != "2" && str != "3")
            {
                Console.Clear();
                Console.WriteLine("Do u wanna go first ?\n\n" +
                "1 Yea, sure.\n" +
                "2 No.\n" +
                "3 As you wish. (random player will start first)\n\n\n");
                str = Console.ReadLine();
            }
            switch (str)
            {
                case "1":
                    Console.Clear();
                    active_player_flag = true;
                    break;
                case "2":
                    Console.Clear();
                    active_player_flag = false;
                    break;
                default:
                    Console.Clear();
                    Random rnd = new Random();
                    active_player_flag = Convert.ToBoolean(rnd.Next(0, 2));
                    break;
            }
            Console.WriteLine("Well, do you want to use noughts or crosses?\n\n1 Noughts\n2 Crosses\n\n");
            str = Console.ReadLine();
            while ((str != "1") && (str != "2"))
            {
                Console.Clear();
                Console.WriteLine("Noughsts. Or. Crosses. 1 or 2, its simple as a pie.\n\n");
                str = Console.ReadLine();
            }
            switch (str)
            {
                case "1":
                    playermark = "0";
                    enemymark = "X";
                    break;
                default:
                    playermark = "X";
                    enemymark = "0";
                    break;
            }
            Console.Clear();
            Console.WriteLine("Finally, what is the length 'n' of your n*n field? ( Write the odd number from 3 to 100. )\n\n");
            while ((!byte.TryParse(Console.ReadLine(), out field_side)) || (field_side < 3) || (field_side % 2 == 0) || (field_side > 30))
            {
                Console.Clear();
                Console.WriteLine("Ha-ha, no, try again. Write the odd number >= 3.");
            }
        }
        public static void MakeConstantText(bool bot_flag, byte field_side, out string signboard, string playermark, string enemymark)
        {
            string s1 = " two players!!! ", s2 = playermark, s3 = Convert.ToString(field_side) + "\n Press the Escape(Esc) key to quit: \n\n";
            if (bot_flag)
            {
                s1 = " player and robot!!! ";
            }
            signboard = "This is a game between" + s1 + "Operator's mark is " + s2 + " and lenght of a side is: " + s3;
        }

        public static void Gameplay(bool bot_flag, bool active_player_flag, byte field_side, string playermark, string enemymark)
        {
            bool turn;
            bool in_game = true;
            int timer = 0;
            string winner = "0";
            MakeConstantText(bot_flag, field_side, out string signboard, playermark, enemymark);
            int length = field_side + 2;
            string[,] map = new string[length, length];
            for (int i = 0; i < length; i++)
            {
                for (int k = 0; k < length; k++)
                {
                    if (i == 0 || i == length - 1 || k == 0 || k == length - 1)
                    {
                        map[i, k] = "#";
                        Console.Write(map[i, k]);
                    }
                    else
                    {
                        map[i, k] = " ";
                        Console.Write(map[i, k]);
                    }
                }
                Console.Write("\n");
            }

            byte[] target = new byte[2] { 1, 1 };
            string temp = map[target[0], target[1]];
            map[target[0], target[1]] = "*";
            while (in_game)
            {
                turn = true;
                while (turn)
                {
                    Console.Clear();
                    Console.WriteLine(signboard);
                    DrawMap(map, length);
                    Turn(map, playermark, length, target, ref temp, ref active_player_flag, ref in_game, enemymark, ref turn, ref bot_flag, ref timer);

                }
                Wincheck(Points(map, playermark, length, temp), Points(map, enemymark, length, temp), PotentialPoints(map, playermark, length, temp), PotentialPoints(map, enemymark, length, temp), ref in_game, ref winner);
            }

            switch (winner)
            {
                case "1":
                    Console.WriteLine("\nOperator wins!!!\n");
                    break;
                case "2":
                    Console.WriteLine("\nOperator loses!!!\n");
                    break;
                default:
                    Console.WriteLine("\nNo winner\n");
                    break;
            }
        }
        public static void DrawMap(string[,] array, int length)
        {
            for (int i = 0; i < length; i++)
            {
                for (int k = 0; k < length; k++)
                {
                    Console.Write(array[i, k]);
                }
                Console.Write("\n");
            }
        }
        public static void Turn(string[,] map, string playermark, int length, byte[] target, ref string temp, ref bool active_player_flag, ref bool in_game, string enemymark, ref bool turn, ref bool bot_flag, ref int timer)
        {
            if (bot_flag && !active_player_flag)
            {
                BotPlay(ref turn, length, map, ref in_game, ref active_player_flag, ref enemymark);
                timer++;
            }
            else
            {
                PlayerPlay(map, target, ref temp, length, ref active_player_flag, playermark, enemymark, ref turn, ref timer);
            }
        }

        public static void BotPlay(ref bool turn, int length, string[,] map, ref bool in_game, ref bool active_player_flag, ref string enemymark)
        {
            List<string> coordinates_for_random = new List<string>();
            for (int i = 0; i < length; i++)
            {
                for (int k = 0; k < length; k++)
                {
                    if (map[i, k] == " ")
                    {
                        string coordinates = Convert.ToString(i) + Convert.ToString(k);
                        coordinates_for_random.Add(coordinates);
                    }
                }
            }

            if (coordinates_for_random.Count == 0)
            {
                turn = false;
                in_game = false;
            }
            else
            {
                Random rnd = new Random();
                int r = rnd.Next(0, coordinates_for_random.Count);
                map[Convert.ToInt32(Convert.ToString(coordinates_for_random[r][0])), Convert.ToInt32(Convert.ToString(coordinates_for_random[r][1]))] = enemymark;
                active_player_flag = !active_player_flag;
                turn = false;
            }
        }

        public static void PlayerPlay(string[,] map, byte[] target, ref string temp, int length, ref bool active_player_flag, string playermark, string enemymark, ref bool turn, ref int timer)
        {
            ConsoleKeyInfo catcher;
            do // epilepltic simulator
            {
                catcher = Console.ReadKey();
                if (catcher.Key == ConsoleKey.W)
                {
                    map[target[0], target[1]] = temp;
                    if (target[0] > 1)
                    {
                        target[0]--;
                    }

                    temp = map[target[0], target[1]];
                    map[target[0], target[1]] = "*";
                    break;

                }
                if (catcher.Key == ConsoleKey.A)
                {
                    map[target[0], target[1]] = temp;
                    if (target[1] > 1)
                    {
                        target[1]--;
                    }

                    temp = map[target[0], target[1]];
                    map[target[0], target[1]] = "*";
                    break;

                }
                if (catcher.Key == ConsoleKey.S)
                {
                    map[target[0], target[1]] = temp;
                    if (target[0] < length - 2)
                    {
                        target[0]++;
                    }

                    temp = map[target[0], target[1]];
                    map[target[0], target[1]] = "*";
                    break;

                }
                if (catcher.Key == ConsoleKey.D)
                {
                    map[target[0], target[1]] = temp;
                    if (target[1] < length - 2)
                    {
                        target[1]++;
                    }

                    temp = map[target[0], target[1]];
                    map[target[0], target[1]] = "*";
                    break;
                }
                if (catcher.Key == ConsoleKey.Enter)
                {
                    map[target[0], target[1]] = temp;
                    if (temp == " ")
                    {
                        map[target[0], target[1]] = active_player_flag ? playermark : enemymark;
                        temp = map[target[0], target[1]];
                        map[target[0], target[1]] = "*";
                        active_player_flag = !active_player_flag;
                        turn = false;
                        timer++;
                        break;
                    }
                }
            } while (true);
        }

        public static int Points(string[,] map, string mark, int length, string temp)
        {
            int playercount = 0;
            int counter = 0;
            HorisontalP(map, mark, length, temp, ref counter, ref playercount);
            VerticalP(map, mark, length, temp, ref counter, ref playercount);
            DiagLefrRightPFirstPart(map, mark, length, temp, ref counter, ref playercount);
            DiagLefrRightPSecondPart(map, mark, length, temp, ref counter, ref playercount);
            DiagRightLeftPFirstPart(map, mark, length, temp, ref counter, ref playercount);
            DiagRightLeftPSecondPart(map, mark, length, temp, ref counter, ref playercount);
            return playercount;
        }

        public static void HorisontalP(string[,] map, string mark, int length, string temp, ref int counter, ref int playercount)
        {
            for (int i = 1; i < length - 1; i++)
            {
                for (int k = 1; k < length - 3; k++)
                {
                    if (map[i, k] == mark || (map[i, k] == "*" && temp == mark))
                    {
                        for (int l = k; l < k + 3; l++)
                        {
                            if (map[i, l] == mark || (map[i, l] == "*" && temp == mark))
                            {
                                counter++;
                            }
                            else
                            {
                                counter = 0;
                                break;
                            }
                        }

                        if (counter == 3)
                        {
                            counter = 0;
                            playercount++;
                            k += 2;
                        }
                    }
                }
            }
        }

        public static void VerticalP(string[,] map, string mark, int length, string temp, ref int counter, ref int playercount)
        {
            for (int i = 1; i < length - 1; i++)
            {
                for (int k = 1; k < length - 3; k++)
                {
                    if (map[k, i] == mark || (map[k, i] == "*" && temp == mark))
                    {
                        for (int l = k; l < k + 3; l++)
                        {
                            if (map[l, i] == mark || (map[l, i] == "*" && temp == mark))
                            {
                                counter++;
                            }
                            else
                            {
                                counter = 0;
                                break;
                            }
                        }

                        if (counter == 3)
                        {
                            counter = 0;
                            playercount++;
                            k += 2;
                        }
                    }
                }
            }
        }
        public static void DiagLefrRightPFirstPart(string[,] map, string mark, int length, string temp, ref int counter, ref int playercount)
        {
            int i; int k;
            for (int a = 1; a < length - 3; a++)
            {
                k = a;
                i = 1;
                while (i < length - 3 && k < length - 3)
                {
                    if (map[i, k] == mark || (map[i, k] == "*" && temp == mark))
                    {
                        for (int m = i, l = k; l < k + 3; m++, l++)
                        {
                            if (map[m, l] == mark || (map[m, l] == "*" && temp == mark))
                            {
                                counter++;
                            }
                            else
                            {
                                counter = 0;
                                break;
                            }
                        }

                        if (counter == 3)
                        {
                            counter = 0;
                            playercount++;
                            k += 2;
                            i += 2;
                        }
                    }
                    k++;
                    i++;
                }
            }
        }

        public static void DiagLefrRightPSecondPart(string[,] map, string mark, int length, string temp, ref int counter, ref int playercount)
        {
            int i; int k;
            for (int a = 2; a < length - 3; a++)
            {
                k = 1;
                i = a;
                while (i < length - 3 && k < length - 3)
                {
                    if (map[i, k] == mark || (map[i, k] == "*" && temp == mark))
                    {
                        for (int m = i, l = k; l < k + 3; m++, l++)
                        {
                            if (map[m, l] == mark || (map[m, l] == "*" && temp == mark))
                            {
                                counter++;
                            }
                            else
                            {
                                counter = 0;
                                break;
                            }
                        }

                        if (counter == 3)
                        {
                            counter = 0;
                            playercount++;
                            k += 2;
                            i += 2;
                        }
                    }
                    k++;
                    i++;
                }

            }
        }

        public static void DiagRightLeftPFirstPart(string[,] map, string mark, int length, string temp, ref int counter, ref int playercount)
        {
            int i; int k;
            for (int a = 3; a < length - 1; a++)
            {
                k = a;
                i = 1;
                while (i < length - 3 && k > 2)
                {
                    if (map[i, k] == mark || (map[i, k] == "*" && temp == mark))
                    {
                        for (int m = i, l = k; m < i + 3; m++, l--)
                        {
                            if (map[m, l] == mark || (map[m, l] == "*" && temp == mark))
                            {
                                counter++;
                            }
                            else
                            {
                                counter = 0;
                                break;
                            }
                        }

                        if (counter == 3)
                        {
                            counter = 0;
                            playercount++;
                            k += 2;
                            i += 2;
                        }
                    }
                    k--;
                    i++;
                }

            }
        }

        public static void DiagRightLeftPSecondPart(string[,] map, string mark, int length, string temp, ref int counter, ref int playercount)
        {
            int i; int k;
            for (int a = 2; a < length - 3; a++)
            {
                k = length - 2;
                i = a;
                while (i < length - 3 && k > 2)
                {
                    if (map[i, k] == mark || (map[i, k] == "*" && temp == mark))
                    {
                        for (int m = i, l = k; m < i + 3; m++, l--)
                        {
                            if (map[m, l] == mark || (map[m, l] == "*" && temp == mark))
                            {
                                counter++;
                            }
                            else
                            {
                                counter = 0;
                                break;
                            }
                        }

                        if (counter == 3)
                        {
                            counter = 0;
                            playercount++;
                            k += 2;
                            i += 2;
                        }
                    }
                    k--;
                    i++;
                }
            }
        }

        public static int PotentialPoints(string[,] map, string mark, int length, string temp)
        {
            int playercount = 0;
            int counter = 0;
            HorisontalPP(map, mark, length, temp, ref counter, ref playercount);
            VerticalPP(map, mark, length, temp, ref counter, ref playercount);
            DiagLeftRightPPFirstPart(map, mark, length, temp, ref counter, ref playercount);
            DiagLeftRightPPSecondPart(map, mark, length, temp, ref counter, ref playercount);
            DiagRightLeftPPFirstPart(map, mark, length, temp, ref counter, ref playercount);
            DiagRightLeftPPSecondPart(map, mark, length, temp, ref counter, ref playercount);
            return playercount;
        }

        public static void HorisontalPP(string[,] map, string mark, int length, string temp, ref int counter, ref int playercount)
        {
            for (int i = 1; i < length - 1; i++)
            {
                for (int k = 1; k < length - 3; k++)
                {
                    if (map[i, k] == mark || map[i, k] == " " || (map[i, k] == "*" && temp == mark))
                    {
                        for (int l = k; l < k + 3; l++)
                        {
                            if (map[i, l] == mark || map[i, l] == " " || (map[i, l] == "*" && temp == mark))
                            {
                                counter++;
                            }
                            else
                            {
                                counter = 0;
                                break;
                            }
                        }

                        if (counter == 3)
                        {
                            counter = 0;
                            playercount++;
                            k += 2;
                        }
                    }
                }
            }
        }
        public static void VerticalPP(string[,] map, string mark, int length, string temp, ref int counter, ref int playercount)
        {
            for (int i = 1; i < length - 1; i++)
            {
                for (int k = 1; k < length - 3; k++)
                {
                    if (map[k, i] == mark || map[k, i] == " " || (map[k, i] == "*" && temp == mark))
                    {
                        for (int l = k; l < k + 3; l++)
                        {
                            if (map[l, i] == mark || map[l, i] == " " || (map[l, i] == "*" && temp == mark))
                            {
                                counter++;
                            }
                            else
                            {
                                counter = 0;
                                break;
                            }
                        }

                        if (counter == 3)
                        {
                            counter = 0;
                            playercount++;
                            k += 2;
                        }
                    }
                }
            }
        }

        public static void DiagLeftRightPPFirstPart(string[,] map, string mark, int length, string temp, ref int counter, ref int playercount)
        {
            int i; int k;
            for (int a = 1; a < length - 3; a++)
            {
                k = a;
                i = 1;
                while (i < length - 3 && k < length - 3)
                {
                    if (map[i, k] == mark || map[i, k] == " " || (map[i, k] == "*" && temp == mark))
                    {
                        for (int m = i, l = k; l < k + 3; m++, l++)
                        {
                            if (map[m, l] == mark || map[m, l] == " " || (map[m, l] == "*" && temp == mark))
                            {
                                counter++;
                            }
                            else
                            {
                                counter = 0;
                                break;
                            }
                        }

                        if (counter == 3)
                        {
                            counter = 0;
                            playercount++;
                            k += 2;
                            i += 2;
                        }
                    }
                    k++;
                    i++;
                }
            }
        }

        public static void DiagLeftRightPPSecondPart(string[,] map, string mark, int length, string temp, ref int counter, ref int playercount)
        {
            int i; int k;
            for (int a = 2; a < length - 3; a++)
            {
                k = 1;
                i = a;
                while (i < length - 3 && k < length - 3)
                {
                    if (map[i, k] == mark || map[i, k] == " " || (map[i, k] == "*" && temp == mark))
                    {
                        for (int m = i, l = k; l < k + 3; m++, l++)
                        {
                            if (map[m, l] == mark || map[m, l] == " " || (map[m, l] == "*" && temp == mark))
                            {
                                counter++;
                            }
                            else
                            {
                                counter = 0;
                                break;
                            }
                        }

                        if (counter == 3)
                        {
                            counter = 0;
                            playercount++;
                            k += 2;
                            i += 2;
                        }
                    }
                    k++;
                    i++;
                }

            }
        }

        public static void DiagRightLeftPPFirstPart(string[,] map, string mark, int length, string temp, ref int counter, ref int playercount)
        {
            int i; int k;
            for (int a = 3; a < length - 1; a++)
            {
                k = a;
                i = 1;
                while (i < length - 3 && k > 2)
                {
                    if (map[i, k] == mark || map[i, k] == " " || (map[i, k] == "*" && temp == mark))
                    {
                        for (int m = i, l = k; m < i + 3; m++, l--)
                        {
                            if (map[m, l] == mark || map[m, l] == " " || (map[m, l] == "*" && temp == mark))
                            {
                                counter++;
                            }
                            else
                            {
                                counter = 0;
                                break;
                            }
                        }

                        if (counter == 3)
                        {
                            counter = 0;
                            playercount++;
                            k += 2;
                            i += 2;
                        }
                    }
                    k--;
                    i++;
                }

            }
        }

        public static void DiagRightLeftPPSecondPart(string[,] map, string mark, int length, string temp, ref int counter, ref int playercount)
        {
            int i; int k;
            for (int a = 2; a < length - 3; a++)
            {
                k = length - 2;
                i = a;
                while (i < length - 3 && k > 2)
                {
                    if (map[i, k] == mark || map[i, k] == " " || (map[i, k] == "*" && temp == mark))
                    {
                        for (int m = i, l = k; m < i + 3; m++, l--)
                        {
                            if (map[m, l] == mark || map[m, l] == " " || (map[m, l] == "*" && temp == mark))
                            {
                                counter++;
                            }
                            else
                            {
                                counter = 0;
                                break;
                            }
                        }

                        if (counter == 3)
                        {
                            counter = 0;
                            playercount++;
                            k += 2;
                            i += 2;
                        }
                    }
                    k--;
                    i++;
                }
            }
        }

        public static void Wincheck(int playercount, int enemycount, int playerpotential, int enemypotential, ref bool in_game, ref string winner)
        {
            if (playerpotential < enemycount)
            {
                in_game = false;
                winner = "2";
            }
            if (enemypotential < playercount)
            {
                in_game = false;
                winner = "1";
            }
            if (playerpotential == enemypotential && enemypotential - enemycount == 0 && playerpotential - playercount == 0)
            {
                in_game = false;
                winner = "0";
            }
        }
    }
}
