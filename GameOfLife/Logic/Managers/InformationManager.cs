using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameOfLife.Logic.Managers
{
    public static class InformationManager
    {
        static InformationManager()
        {
            Console.OutputEncoding = Encoding.UTF8;
            WorldIndexes = new int[9];
            for (int i = 0; i < 9; i++)
                WorldIndexes[i] = -1;
        }

        public static char LifeCell = '■'; //'\u2588'; // Another version
        public static char DeadCell = '\u25ab'; //'▪';

        public static int[] WorldIndexes { get; }

        private static bool isStoped;

        public static void Start()
        {
            Life();
        }

        private static void Life()
        {
            while (true)
            {
                if (!isStoped)
                {
                    Console.Clear();

                    DrawWorlds();

                    Console.SetCursorPosition(0, 27);

                    Console.Write("Games count: " + GameManager.GameCount +
                        " | Running game count: " + GameManager.RunningGamesCount +
                        " | Total liveform count: " + GameManager.TotalLiveformCount +
                        " | Average liveform count: " + GameManager.AverageLiveformCount);

                    Console.SetCursorPosition(0, 29);
                    Console.Write("> ");
                }
                Thread.Sleep(1000);
            }

        }

        public static void Stop()
        {
            isStoped = true;
        }

        public static void Continue()
        {
            isStoped = false;
        }

        public static void ShowCamerasMenu()
        {
            do
            {
                Console.Clear();

                Console.WriteLine("\nCamera: world number");
                for (int i = 0; i < 9; i++)
                {
                    Console.WriteLine("  " + i + ": " + WorldIndexes[i]);
                }

                Console.WriteLine("\nDo you want to change camera view? [y/n]");

                string answer;
                do
                {
                    Console.Write("> ");
                    answer = Console.ReadLine();
                } while (answer != "y" && answer != "Y" && answer != "n" && answer != "N");
                if (answer == "n" || answer == "N") break;

                Console.WriteLine("\nWhich camera you want to change? (0 - 8)");

                bool incorrectAnswer = true;
                int cameraTochange, worldToShow;
                do
                {
                    Console.Write("> ");
                    incorrectAnswer = ! int.TryParse(Console.ReadLine(), out cameraTochange);
                } while (incorrectAnswer || cameraTochange < 0 || cameraTochange > 8);


                Console.WriteLine("\nWhich world you want to show on " + cameraTochange + " camera? (0 - " + (GameManager.GameCount-1) + ")");

                incorrectAnswer = true;
                do
                {
                    Console.Write("> ");
                    incorrectAnswer = !int.TryParse(Console.ReadLine(), out worldToShow);
                } while (incorrectAnswer || worldToShow < 0 || worldToShow > (GameManager.GameCount - 1));

                WorldIndexes[cameraTochange] = worldToShow;

            } while (true);
        }

        private static void DrawWorlds()
        {
            string[] visualWorlds = GameManager.GetVisualWorlds(WorldIndexes);

            int tableWidth = GameManager.WorldWidth + 2,
                tableHeight = GameManager.WorldHeight + 2;

            Console.SetCursorPosition(0, 0);
            for (int h = 0; h < tableHeight * 3; h++)
            {
                for (int w = 0; w < tableWidth * 3; w++)
                {
                    if (h % tableHeight == 0)
                    {
                        if (w % tableWidth == 0)
                            Console.Write('╔');
                        else if (w % tableWidth == tableWidth - 1)
                            Console.Write('╗');
                        else
                            Console.Write('═');
                    }
                    else if (h % tableHeight == tableHeight - 1)
                    {
                        if (w % tableWidth == 0)
                            Console.Write('╚');
                        else if (w % tableWidth == tableWidth - 1)
                            Console.Write('╝');
                        else
                            Console.Write('═');
                    }
                    else if (w % tableWidth == 0 || w % tableWidth == tableWidth - 1)
                    {
                        Console.Write('║');
                    }
                    else
                    {
                        int worldNumber = h / tableHeight * 3 + w / tableWidth;

                        int x = w % tableWidth -1,
                            y = h % tableHeight - 1;

                        if (visualWorlds[worldNumber] != "")
                            Console.Write(visualWorlds[worldNumber][ x + y * GameManager.WorldWidth ]);
                        else
                            Console.Write(' ');
                    }
                }
                Console.Write('\n');
            }
        }

    }
}
