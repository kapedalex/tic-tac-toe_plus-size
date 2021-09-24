﻿using System;
using System.Collections.Generic;

namespace tic_tac_toe
{
    static class Program
    {
        public static void Main(string[] args)
        {
            bool bot_flag = true, active_player_flag = true, noughts_flag = true;
            byte field_side = 3;
            Greetings(ref bot_flag, ref noughts_flag, ref active_player_flag, ref field_side);
            Gameplay(bot_flag, active_player_flag, noughts_flag, field_side);

        }

        public static void Greetings(ref bool bot_flag, ref bool noughts_flag, ref bool active_player_flag, ref byte field_side)
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
                Console.WriteLine("Well, I mean, you need to write number '1' or'2', player or robot. Try it again ");
                str = Console.ReadLine();
            }
            switch (str)
            {
                case "1":
                    bot_flag = true;
                    break;
                case "2":
                    bot_flag = false;
                    break;
                default:
                    break;
            }
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
                case "3":
                    Console.Clear();
                    Random rnd = new Random();
                    active_player_flag = Convert.ToBoolean(rnd.Next(0, 2));
                    break;
                default:
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
                    noughts_flag = true;
                    break;
                case "2":
                    noughts_flag = false;
                    break;
                default:
                    break;
            }
            Console.Clear();
            Console.WriteLine("Finally, what is the length 'n' of your n*n field? ( Write the odd number from 3 to 100. )\n\n");
            while ((!byte.TryParse(Console.ReadLine(), out field_side)) || (field_side < 3) || (field_side % 2 == 0) || (field_side > 100))
            {
                Console.Clear();
                Console.WriteLine("Ha-ha, no, try again. Write the odd number >= 3.");
            }
        }
        public static void Gameplay(bool bot_flag, bool first_player_flag, bool noughts_flag, byte field_side)
        {
            bool turn;
            bool in_game = true;
            int timer = 0;
            string winner = "0";
            int maxturns = field_side * field_side;
            MakeConstantText(bot_flag, noughts_flag, field_side, out string signboard);
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
            string playermark, enemymark;

            if (noughts_flag)
            {
                playermark = "0";
                enemymark = "X";
            }
            else
            {
                playermark = "X";
                enemymark = "0";
            }

            while (in_game)
            {
                Wincheck(Points(map, playermark, length), Points(map, enemymark, length), PotentialPoints(map, playermark, length), PotentialPoints(map, enemymark, length), ref in_game, ref winner, maxturns, timer);
                turn = true;
                while (turn)
                {
                    Console.Clear();
                    Console.WriteLine(signboard);
                    DrawMap(map, length);
                    Turn(map, playermark, length, target, ref temp, ref first_player_flag, ref in_game, enemymark, ref turn, ref bot_flag);
                    timer++;
                }
            }

            switch (winner)
            {
                case "0":
                    Console.WriteLine("No winner");
                    break;
                case "1":
                    Console.WriteLine("Player 1 wins!!!");
                    break;
                case "2":
                    Console.WriteLine("Player 2 wins!!!");
                    break;
                default:
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
        public static void Turn(string[,] map, string playermark, int length, byte[] target, ref string temp, ref bool first_player_flag, ref bool in_game, string enemymark, ref bool turn, ref bool bot_flag)
        {
            if (bot_flag && !first_player_flag)
            {
                BotPlay(ref turn, length, map, ref in_game, ref first_player_flag, ref enemymark);
            }
            else
            {
                PlayerPlay(map, target, ref temp, length, ref first_player_flag, playermark, enemymark, ref turn);
            }
        }

        public static void BotPlay(ref bool turn, int length, string[,] map, ref bool in_game, ref bool first_player_flag, ref string enemymark)
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
                in_game = false;
            }
            Random rnd = new Random();
            int r = rnd.Next(0, coordinates_for_random.Count);
            map[Convert.ToInt32(Convert.ToString(coordinates_for_random[r][0])), Convert.ToInt32(Convert.ToString(coordinates_for_random[r][1]))] = enemymark;
            first_player_flag = !first_player_flag;
            turn = false;
        }

        public static void PlayerPlay(string[,] map, byte[] target, ref string temp, int length, ref bool first_player_flag, string playermark, string enemymark, ref bool turn)
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
                        map[target[0], target[1]] = first_player_flag ? playermark : enemymark;
                        temp = map[target[0], target[1]];
                        map[target[0], target[1]] = "*";
                        first_player_flag = !first_player_flag;
                        turn = false;
                        break;
                    }
                }
            } while (catcher.Key != ConsoleKey.Escape);
        }
        public static void MakeConstantText(bool bot_flag, bool noughts_flag, byte field_side, out string signboard)
        {
            string s1 = " two players!!! ", s2 = "cross, ", s3 = Convert.ToString(field_side) + "\n Press the Escape(Esc) key to quit: \n\n";
            if (bot_flag)
            {
                s1 = " human and robot!!! ";
            }
            if (noughts_flag)
            {
                s2 = " nought, ";
            }
            signboard = "This is a game between" + s1 + "First player's mark is a" + s2 + " and lenght of a side is: " + s3;
        }

        public static int Points(string[,] map, string mark, int length)
        {
            int playercount = 0;
            int counter = 0;
            for (int i = 1; i < length - 2; i++)
            {
                for (int k = 1; i < length - 2; k++)
                {
                    if (map[i, k] == mark)
                    {
                        for (int l = k; l <= k + 3; l++)
                        {
                            if (map[i, l] == mark)
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

            for (int i = 1; i < length - 2; i++)
            {
                for (int k = 1; i < length - 2; k++)
                {
                    if (map[k, i] == mark)
                    {
                        for (int l = k; l <= k + 3; l++)
                        {
                            if (map[k, i] == mark)
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

            for (int i = 1; i < length - 2; i++)
            {
                for (int k = 1; i < length - 2; k++)
                {
                    if (map[i, k] == mark)
                    {
                        for (int m = i, l = k; l <= k + 3; m++, l++)
                        {
                            if (map[m, l] == mark)
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

            for (int i = 1; i < length - 2; i++)
            {
                for (int k = 3; i < length - 2; k++)
                {
                    if (counter == 3)
                    {
                        counter = 0;
                        playercount++;
                        k += 2;
                    }

                    if (map[i, k] == mark)
                    {
                        for (int m = i, l = k; l <= k + 3; m++, l++)
                        {
                            if (map[m - 1, l + 1] == mark)
                            {
                                counter++;
                            }
                            else
                            {
                                counter = 0;
                                break;
                            }
                        }
                    }
                }
            }

            return playercount;
        }

        public static int PotentialPoints(string[,] map, string mark, int length)
        {
            int playerprobcount = 0;
            int counter = 0;
            for (int i = 1; i < length - 2; i++)
            {
                for (int k = 1; i < length - 2; k++)
                {
                    if (map[i, k] == mark || map[i, k] == " ")
                    {
                        for (int l = k; l <= k + 3; l++)
                        {
                            if (map[i, l] == mark || map[i, l] == " ")
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
                            playerprobcount++;
                            k += 2;
                        }
                    }
                }
            }

            for (int i = 1; i < length - 2; i++)
            {
                for (int k = 1; i < length - 2; k++)
                {
                    if (map[k, i] == mark || map[k, i] == " ")
                    {
                        for (int l = k; l <= k + 3; l++)
                        {
                            if (map[k, i] == mark || map[k, i] == " ")
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
                            playerprobcount++;
                            k += 2;
                        }
                    }
                }
            }

            for (int i = 1; i < length - 2; i++)
            {
                for (int k = 1; i < length - 2; k++)
                {
                    if (map[i, k] == mark || map[i, k] == " ")
                    {
                        for (int m = i, l = k; l <= k + 3; m++, l++)
                        {
                            if (map[m, l] == mark || map[m, l] == " ")
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
                            playerprobcount++;
                            k += 2;
                        }

                    }
                }
            }

            for (int i = 1; i < length - 2; i++)
            {
                for (int k = 3; i < length - 2; k++)
                {
                    if (counter == 3)
                    {
                        counter = 0;
                        playerprobcount++;
                        k += 2;
                    }

                    if (map[i, k] == mark || map[i, k] == " ")
                    {
                        for (int m = i, l = k; l <= k + 3; m++, l++)
                        {
                            if (map[m - 1, l + 1] == mark || map[m - 1, l + 1] == " ")
                            {
                                counter++;
                            }
                            else
                            {
                                counter = 0;
                                break;
                            }
                        }
                    }
                }
            }

            return playerprobcount;

        }

        public static void Wincheck(int playercount, int enemycount, int playerpotential, int enemypotential, ref bool in_game, ref string winner, int maxturns, int timer)
        {
            if (playerpotential < enemycount)
            {
                in_game = false;
                winner = "1";
            }
            if (enemypotential < playercount)
            {
                in_game = false;
                winner = "2";
            }
            if (timer == maxturns)
            {
                in_game = false;
                winner = "0";
            }
        }
    }
}