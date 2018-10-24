using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife.Helpers
{
    public static class Logger
    {
        public static void Log(string message)
        {
            using (StreamWriter sw = new StreamWriter("logs.txt"))
            {
                sw.WriteLine("[" + DateTime.Now.ToShortDateString() + "|" + DateTime.Now.ToShortTimeString() + "] " + message);
            }
        }

        public static void Log(Exception exception) { Log(exception.Message); }
    }
}
