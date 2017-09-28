using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.BoardSolver;
using GemSwipe.BoardSolver.LittleStar;
using GemSwipe.GameEngine;
using GemSwipe.Generator;
using GemSwipe.Models;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace GemSwipe.Tests
{
    [TestFixture()]
    public class GeneratorTests
    {
        private Solver _solver;

        [SetUp]
        public void Setup()
        {
            _solver = new Solver();
        }

        [TestCase("1 0 0 1-0 0 0 0-0 0 0 0-0 0 0 0", 1)]
        [TestCase("1 1 1 1-0 0 0 0-0 0 0 0-0 0 0 0", 2)]
        [TestCase("1 1 1 1-0 0 0 0-1 0 1 0-1 0 1 0", 4)]
        [TestCase("1 1 0 1-0 1 0 1-0 0 0 0-1 0 1 1", 5)]
        [TestCase("1 1 2 1-0 1 2 1-0 0 2 2-1 0 1 1", 5)]
        [TestCase("3 0 0 3-0 0 0 0-0 1 0 1-1 2 2 1", 8)]
        [TestCase("3 0 0 3 2-2 0 0 0 2-0 1 0 2 1-1 0 2 2 1-3 0 2 0 2", 8)]
        [TestCase("0 0 3 0-0 1 1 1-2 0 0 2-1 3 0 0", 6)]
        [TestCase("2 2 0 2-3 0 2 1-0 2 0 0-0 1 0 0", 7)]
        [TestCase("0 2 1 0-1 2 2 1-0 2 2 2-0 0 1 0", 12)]
        [TestCase("0 0 0 0-0 3 0 1-1 2 0 2-0 3 1 1", 14)]
        [TestCase("0 2 2 2-1 1 1 0-1 0 2 2-0 0 1 1", 7)]
        [TestCase("0 0 0 0-0 1 0 3-3 0 0 0-1 3 2 0", 10)]
        [TestCase("2 0 1 0-2 2 3 0-0 1 1 0-0 0 1 2", 8)]
        [TestCase("3 0 0 0-3 3 0 0-1 2 1 0-0 0 0 0", 10)]
        [TestCase("0 0 0 0-3 0 3 1-1 0 2 2-0 2 0 0", 10)]
        [TestCase("3 0 0 1-2 0 0 0-3 0 2 0-2 1 0 0", 10)]
        public void ShouldSolveBoard(string boardString, int movesCount)
        {
            // Given a board
            var game = new GemSwipeEngine(new Board(boardString));

            // When the IA solves the board
            var moves = _solver.Solve(game);

            // It should solve it in <movesCount>
            Assert.AreEqual(movesCount, moves.Count);
        }
    }
}
