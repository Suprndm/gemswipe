using System;
using GemSwipe.Game.Models;
using Xamarin.Forms;

namespace GemSwipe.Game.Gestures
{
    public static class GestureEventHandler
    {
        public static event Action<Direction> Swipped;

        public static void Swipe(Direction direction)
        {
            Swipped?.Invoke(direction);
        }

        public static event Action<PanUpdatedEventArgs> Panning;

        public static void Pan(PanUpdatedEventArgs e)
        {
            Panning?.Invoke(e);
        }
    }
}
