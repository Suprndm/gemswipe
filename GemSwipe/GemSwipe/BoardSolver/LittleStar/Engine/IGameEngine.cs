using System.Collections.Generic;

namespace GemSwipe.BoardSolver.LittleStar.Engine
{
    public interface IGameEngine<TGameState, TMove> where TGameState:IGameState
                                                    where TMove:IMove
    {
        IList<TMove> GetPossibleMovesForState(TGameState gameState);
        TGameState PlayMove(TGameState gameState, TMove move);
        TGameState GetInitialState();
        bool GameStateIsFinal(TGameState gameState);
        double EvalEuristic(TGameState gameState);
        TGameState Duplicate(TGameState gameState);
        bool AreEqual(TGameState gameState1, TGameState gameState2);
    }
}
