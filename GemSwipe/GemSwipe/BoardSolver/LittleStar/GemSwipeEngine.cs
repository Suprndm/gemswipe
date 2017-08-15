using System;
using System.Collections.Generic;
using System.Linq;
using GemSwipe.BoardSolver.LittleStar.Engine;
using GemSwipe.GameEngine;
using GemSwipe.Models;

namespace GemSwipe.BoardSolver.LittleStar
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
            var horizontalEcart = GetEcartTypeForDirection(gameState.Board, Direction.Left);
            var verticalEcart = GetEcartTypeForDirection(gameState.Board, Direction.Top);

            return (double) 1 / gameState.Board.Gems.Count - ((horizontalEcart+ verticalEcart) / 2)*0.1;
        }

        private double GetEcartTypeForDirection(Board board, Direction direction)
        {
            var cellsLanes = board.GetCellsLanes(direction);
            double diff = 0;
            List<int> sums = new List<int>();

            foreach (var horizontalLane in cellsLanes)
            {
                var gems = horizontalLane.Where(c => !c.IsEmpty()).Select(c => c.GetAttachedGem()).ToList();
                if (gems.Count > 0)
                {
                    sums.Add(gems.Sum(g=>g.Size));
                }
            }

            var average = sums.Sum() / sums.Count;

            foreach (var sum in sums)
            {
                diff += Math.Abs(average - sum);
            }

            return diff;
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
