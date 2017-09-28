using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Models;

namespace GemSwipe.Gestures
{
    public static class GestureEventHandler
    {
        public static event Action<Direction> Swipped;

        public static void Swipe(Direction direction)
        {
            Swipped?.Invoke(direction);
        }
    }
}
