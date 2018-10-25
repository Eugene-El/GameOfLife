using System;
using System.Threading;

namespace GameOfLife.Logic.Managers
{
    public static class InputManager
    {
        public static void Start()
        {
            new Thread(Life).Start();
        }

        private static void Life()
        {
            while (true)
            {
                var key = Console.ReadKey();

                switch (key.Key)
                {
                    case ConsoleKey.S:
                        GameManager.Stop();
                        InformationManager.Stop();
                        break;

                    case ConsoleKey.C:
                        GameManager.Continue();
                        InformationManager.Continue();
                        break;

                    case ConsoleKey.Enter:
                        InformationManager.Stop();
                        GameManager.Stop();
                        Thread.Sleep(200); // Fix information manager thread
                        InformationManager.ShowCamerasMenu();
                        InformationManager.Continue();
                        GameManager.Continue();
                        break;
                }
            }
        }

    }
}
