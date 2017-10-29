using GemSwipe.BoardSolver.LittleStar.Engine;
using GemSwipe.Game.Models.Entities;

namespace GemSwipe.BoardSolver.LittleStar
{
    public class GemSwipeState : IGameState
    {
        public Board Board { get; set; }
    }
}
