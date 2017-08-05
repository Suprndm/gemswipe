using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.GameEngine;
using GemSwipe.GameEngine.Floors;
using GemSwipe.Generator;
using GemSwipe.Generator.LittleStar;
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
        [TestCase("3 0 0 3-0 0 0 0-0 1 0 1-1 2 2 1", 9)]
        [TestCase("3 0 0 3 2-2 0 0 0 2-0 1 0 2 1-1 0 2 2 1-3 0 2 0 2", 10)]
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
