using System.Collections.Generic;
using System.Linq;

namespace GemSwipe.BoardSolver.LittleStar.Engine
{
    public class TrackingPoint<TGameState, TMove> where TGameState : IGameState where TMove : IMove
    {
        public TGameState GameState { get; set; }
        public IList<TMove> Moves { get; set; }
        public double EuristicScore { get; set; }
        public bool IsExplored { get; set; }

        public override string ToString()
        {
           return $"E = {EuristicScore.ToString("#####.##")}\n Moves = {string.Join("+", Moves.ToArray())}\n ";
        }
    }
}
