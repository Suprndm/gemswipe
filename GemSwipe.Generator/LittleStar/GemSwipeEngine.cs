using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Models;
using LittleStar;

namespace GemSwipe.Generator.LittleStar
{
    public class GemSwipeEngine:IGameEngine<GemSwipeState, GemSwipeMove>
    {
        private readonly Board _board;

        public GemSwipeEngine(Board board)
        {
            _board = board;
        }

        public IList<GemSwipeMove> GetPossibleMovesForState(GemSwipeState gameState)
        {
            return new List<GemSwipeMove>
            {
                new GemSwipeMove() {Direction = Direction.Bottom},
                new GemSwipeMove() {Direction = Direction.Left},
                new GemSwipeMove() {Direction = Direction.Right},
                new GemSwipeMove() {Direction = Direction.Top},
            };
        }

        public GemSwipeState PlayMove(GemSwipeState gameState, GemSwipeMove move)
        {
            gameState.Board.Swipe(move.Direction);
            return gameState;
        }

        public GemSwipeState GetInitialState()
        {
            return new GemSwipeState {Board = _board};
        }

        public bool GameStateIsFinal(GemSwipeState gameState)
        {
            return gameState.Board.Gems.Count == 1;
        }

        public double EvalEuristic(GemSwipeState gameState)
        {
            return (double) 1 / gameState.Board.Gems.Count;
        }

        public GemSwipeState Duplicate(GemSwipeState gameState)
        {
            return new GemSwipeState() {Board = new Board(gameState.Board.ToString())};
        }

        public bool AreEqual(GemSwipeState gameState1, GemSwipeState gameState2)
        {
            return gameState1.Board.ToString() == gameState2.Board.ToString();
        }
    }
}
