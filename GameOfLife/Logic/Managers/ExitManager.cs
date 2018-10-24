using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife.Logic.Managers
{
    public static class ExitManager
    {
        static ExitManager()
        {
            _handler += new EventHandler(Handler);
            SetConsoleCtrlHandler(_handler, true);
        }
        
        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);

        private delegate bool EventHandler(CtrlType sig);
        static EventHandler _handler;

        enum CtrlType
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }

        public static event System.EventHandler ApplicationExitEvent;

        private static bool Handler(CtrlType sig)
        {
            ApplicationExitEvent.Invoke(null, new EventArgs());

            //shutdown right away so there are no lingering threads
            Environment.Exit(-1);

            return true;
        }

    }
}
