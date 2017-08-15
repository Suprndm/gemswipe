using System.Collections.Generic;
using System.Linq;

namespace GemSwipe.BoardSolver.LittleStar.Engine
{
    public class LittleStarEngine
    {
        private readonly ILogger _logger;

        public LittleStarEngine(ILogger logger)
        {
            _logger = logger;
        }

        public IList<TMove> Resolve<TGameState, TMove>(IGameEngine<TGameState, TMove> gameEngine, TGameState gameState)
            where TGameState : IGameState where TMove : IMove
        {
            int iterationCount = 0;
            IList<TrackingPoint<TGameState, TMove>> tracker = new List<TrackingPoint<TGameState, TMove>>();
            var initialTrackingPoint = new TrackingPoint<TGameState, TMove>();

            var score = gameEngine.EvalEuristic(gameState);
            initialTrackingPoint.EuristicScore = score;
            initialTrackingPoint.GameState = gameState;
            initialTrackingPoint.Moves = new List<TMove> { };
            tracker.Add(initialTrackingPoint);

            var bestTrackingPoint = initialTrackingPoint;

            do
            {
                iterationCount++;
                _logger.Log($"Iteration: {iterationCount}");

                // Explore
                var previousMoves = bestTrackingPoint.Moves;
                bestTrackingPoint.IsExplored = true;
                var moves = gameEngine.GetPossibleMovesForState(bestTrackingPoint.GameState);

                foreach (var move in moves)
                {
                    var newState = gameEngine.Duplicate(gameState);
                    newState = gameEngine.PlayMove(newState, move);
                    score = gameEngine.EvalEuristic(newState);
                    if (!tracker.Any(t => gameEngine.AreEqual(t.GameState, newState)))
                    {
                        var trackingPoint = new TrackingPoint<TGameState, TMove>();
                        trackingPoint.EuristicScore = score;
                        trackingPoint.GameState = newState;
                        trackingPoint.Moves = new List<TMove>(bestTrackingPoint.Moves);
                        trackingPoint.Moves.Add(move);
                        tracker.Add(trackingPoint);
                        _logger.Log(trackingPoint.ToString());
                    }
                 
                }

                _logger.Log($"Tracker Size: {tracker.Count}");

                if (tracker.All(t => t.IsExplored)) return new List<TMove>();

                // Choose                                           
                bestTrackingPoint = tracker.Where(t=>t.IsExplored == false).Aggregate((i1, i2) => i1.EuristicScore > i2.EuristicScore ? i1 : i2);

                // Backtrack
                gameState = bestTrackingPoint.GameState;

            } while (!gameEngine.GameStateIsFinal(gameState));

            return bestTrackingPoint.Moves;
        }
    }
}
