using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LittleStar;

namespace GemSwipe.Generator
{
    public class ConsoleLogger : ILogger
    {
        public ConsoleLogger()
        {
            IsEnabled = true;
        }

        public bool IsEnabled { get; set; }
        public void Log(string message)
        {
            if (IsEnabled)
                Console.WriteLine(message);
        }
    }
}
