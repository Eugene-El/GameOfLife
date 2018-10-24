using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
                var k = Console.ReadKey();
                if (k.KeyChar == 's')
                    GameManager.Stop();
            }
        }

    }
}
