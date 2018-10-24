using GameOfLife.Logic.Managers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife.Logic
{
    public class Menu
    {
        public void Show()
        {
            string answer;
            bool isBackupWorld;
            do
            {
                Console.Clear();
                isBackupWorld = File.Exists(GameManager.BackupFileName);
                Console.WriteLine("======================= GAME OF LIFE =======================\n");
                Console.WriteLine("  1. Start games");
                if (isBackupWorld)
                    Console.WriteLine("  2. Resume previous games");
                Console.WriteLine("  {0}. Exit", isBackupWorld ? 3 : 2);

                Console.WriteLine("\n============================================================\n");


                Console.Write("Enter: ");
                answer = Console.ReadLine();
            } while (answer != "1" && answer != "2");

            switch (answer)
            {
                case "1":
                    StartGames();
                    break;

                case "2":
                    if (isBackupWorld)
                        ResumePreviousGames();
                    break;

                default:
                    break;
            }
        }

        private void StartGames()
        {
            // TODO rafactor
            int gameCount, worldHeight, worldWidth, startCount;
            Console.Write("\n\n");
            do
            {
                Console.Write("Game count: ");
            } while (!int.TryParse(Console.ReadLine(), out gameCount) && gameCount >= 0);
            do
            {
                Console.Write("World height (3-7): ");
            } while (!int.TryParse(Console.ReadLine(), out worldHeight) && worldHeight >= 3 && worldHeight <= 11);
            do
            {
                Console.Write("World width (3-37): ");
            } while (!int.TryParse(Console.ReadLine(), out worldWidth) && worldWidth >= 3 && worldWidth <= 38);
            do
            {
                Console.Write("Start lifeforms count (3-" + (worldWidth * worldHeight - 1) +  "): ");
            } while (!int.TryParse(Console.ReadLine(), out startCount) && startCount >= 3 && startCount <= (worldWidth * worldHeight - 1));


            GameManager.Start(gameCount, worldHeight, worldWidth, startCount);
            InputManager.Start();
            InformationManager.Start();
        }

        private void ResumePreviousGames()
        {
            GameManager.ResumePreviousGames();
            InputManager.Start();
            InformationManager.Start();
        }
    }
}
