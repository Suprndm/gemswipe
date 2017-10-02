using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GemSwipe.BoardSolver.LittleStar;
using GemSwipe.Game.Entities;

namespace GemSwipe.BoardSolver
{
    public class BlockedSensor
    {

        public BlockedSensor()
        {
        }
        public async Task<bool> IsBlocked(string boardString)
        {
            var game = new GemSwipeEngine(new Board(boardString));
            var solver = new Solver();
            var moves =  await Task.Factory.StartNew(()=>solver.Solve(game));

            if (moves.Count == 0) return true;

            return false;
        }
    }
}
