using GemSwipe.BoardSolver.LittleStar.Engine;
using GemSwipe.Game.Models;

namespace GemSwipe.BoardSolver.LittleStar
{
    public class GemSwipeMove : IMove
    {
        public Direction Direction { get; set; }
        public override string ToString()
        {
            return Direction.ToString();
        }
    }
}
