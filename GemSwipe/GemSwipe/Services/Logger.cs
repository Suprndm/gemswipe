using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemSwipe.Services
{
    public static class Logger
    {
        public static event Action<string> OnLogged;
        public static void Log(string message)
        {
            OnLogged?.Invoke(message);
        }
    }
}
