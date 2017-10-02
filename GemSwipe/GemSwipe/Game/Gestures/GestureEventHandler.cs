using System;
using GemSwipe.Game.Models;

namespace GemSwipe.Game.Gestures
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
