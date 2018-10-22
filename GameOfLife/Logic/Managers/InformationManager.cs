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
            WorldIndexes = new List<int>();
        }

        public static char LifeCell = '■'; //'\u2588'; // Another version
        public static char DeadCell = '\u25ab'; //'▪';

        public static List<int> WorldIndexes { get; }

        public static void Start()
        {
            Life();
        }

        private static void Life()
        {
            while (true)
            {
                Console.Clear();

                DrawWorlds();

                Console.SetCursorPosition(0, 27);

                Console.Write("Games count: " + GameManager.GameCount +
                    " | Running game count: " + GameManager.RunningGamesCount +
                    " | Total liveform count: " + GameManager.TotalLiveformCount +
                    " | Average liveform count: " + GameManager.AverageLiveformCount);

                Thread.Sleep(1000);
            }

        }

        private static void DrawWorlds()
        {
            List<string> visualWorlds = GameManager.GetVisualWorlds(WorldIndexes);

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
                        if (worldNumber < visualWorlds.Count)
                            Console.Write(visualWorlds[h / tableHeight + w / tableWidth][(w - tableWidth * (w / tableWidth) - 1) * (h - tableHeight * (h / tableHeight) - 1)]);
                        else
                            Console.Write(' ');
                    }
                }
                Console.Write('\n');
            }
        }

    }
}
