using System;
using System.Collections.Generic;
using System.Linq;
using GemSwipe.BoardSolver.LittleStar.Engine;
using GemSwipe.GameEngine;
using GemSwipe.Models;

namespace GemSwipe.BoardSolver.LittleStar
{
    public class GemSwipeEngine : IGameEngine<GemSwipeState, GemSwipeMove>
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
            return new GemSwipeState { Board = _board };
        }

        public bool GameStateIsInvalid(GemSwipeState gameState)
        {
            return gameState.Board.Gems.Count == 0;
        }

        public bool GameStateIsFinal(GemSwipeState gameState)
        {
            return gameState.Board.Gems.Count == 1;
        }

        public double EvalEuristic(GemSwipeState gameState)
        {
            var horizontalEcart = GetEcartTypeForDirection(gameState.Board, Direction.Left);
            var verticalEcart = GetEcartTypeForDirection(gameState.Board, Direction.Top);

            var horizontalPairs = GetPairs(gameState.Board, Direction.Left);
            var verticalPairs = GetPairs(gameState.Board, Direction.Top);

            var gemCountScore = (double) 1 / (gameState.Board.Gems.Count * 0.75);
            var ecartScore = -Math.Min(horizontalEcart, verticalEcart) * 0.08;
            var pairsScore = Math.Max(horizontalPairs, verticalPairs) * 0.28;
            return gemCountScore + ecartScore + pairsScore;
        }

        private int GetPairs(Board board, Direction direction)
        {
            var cellsLanes = board.GetCellsLanes(direction);
            int pairs = 0;
            int malus = 0;


            foreach (var horizontalLane in cellsLanes)
            {
                int lanePairs = 0;
                int? previous = null;
                int? previousPair = null;
                int? beforePair = null;
                var gems = horizontalLane.Where(c => !c.IsEmpty()).Select(c => c.GetAttachedGem()).ToList();
                foreach (var gem in gems)
                {
                    if (previous == null)
                    {

                        previous = gem.Size;

                        if (beforePair!=null && (previousPair == gem.Size))
                            malus++;

                        previousPair = null;
                        beforePair = gem.Size;
                    }
                    else if (previous == gem.Size)
                    {
                        lanePairs++;
                        previous = null;
                        previousPair = gem.Size;
                    }
                    else
                    {
                        previous = gem.Size;
                        beforePair = gem.Size;
                    }
                }

                if (lanePairs > 1) lanePairs++;
                pairs += lanePairs;
            }

            return pairs - malus;
        }

        private double GetEcartTypeForDirection(Board board, Direction direction)
        {
            var cellsLanes = board.GetCellsLanes(direction);
            double diff = 0;
            List<double> diffs = new List<double>();

            foreach (var horizontalLane in cellsLanes)
            {
                var gems = horizontalLane.Where(c => !c.IsEmpty()).Select(c => c.GetAttachedGem()).ToList();
                if (gems.Count > 0)
                {
                    double average = (double)gems.Sum(g => g.Size) / gems.Count;
                    foreach (var gem in gems)
                    {
                        diff += Math.Abs(average - gem.Size);
                    }

                    diffs.Add(diff);
                }
            }

            return diffs.Average();
        }



        public GemSwipeState Duplicate(GemSwipeState gameState)
        {
            return new GemSwipeState() { Board = new Board(gameState.Board.ToString()) };
        }

        public bool AreEqual(GemSwipeState gameState1, GemSwipeState gameState2)
        {
            return gameState1.Board.ToString() == gameState2.Board.ToString();
        }
    }
}
