using System;
using System.Collections.Generic;
using System.Linq;
using GemSwipe.GameEngine;
using GemSwipe.GameEngine.Floors;
using GemSwipe.Models;
using NUnit.Framework;

namespace GemSwipe.Tests
{

    [TestFixture()]
    public class BoardTests
    {
        private Board _board;

        [SetUp]
        public void SetUp()
        {

        }

        [Test]
        [TestCase("1 0 0-0 0 0-0 0 0", "0 0 1-0 0 0-0 0 0", Direction.Right)]
        [TestCase("0 0 1-0 0 0-0 0 0", "1 0 0-0 0 0-0 0 0", Direction.Left)]
        [TestCase("1 0 0-0 0 0-0 0 0", "0 0 0-0 0 0-1 0 0", Direction.Bottom)]
        [TestCase("0 0 0-0 0 0-0 1 0", "0 1 0-0 0 0-0 0 0", Direction.Top)]
        public void ShouldCorrectlyMoveOnSimpleSwipe(string intialBoardString, string finalBoardString, Direction direction)
        {
            // Given an initialBoard
            var actualBoard = BuildBoardFromString(intialBoardString);
            Console.WriteLine("InitialBoard board");
            Console.WriteLine(DrawBoard(actualBoard));

            // When Swiping
            actualBoard.Swipe(direction);

            // Then the board should have the following configuration
            var excpectedBoard = BuildBoardFromString(finalBoardString);
            AssertBoardEquality(excpectedBoard, actualBoard);
        }

        [Test]
        [TestCase("1 0 0-0 0 1-0 1 0", "0 0 1-0 0 1-0 0 1", Direction.Right)]
        [TestCase("0 0 1-1 0 0-0 1 0", "1 0 0-1 0 0-1 0 0", Direction.Left)]
        [TestCase("1 0 0-0 1 0-0 0 1", "0 0 0-0 0 0-1 1 1", Direction.Bottom)]
        [TestCase("1 0 0-0 1 0-0 0 1", "1 1 1-0 0 0-0 0 0", Direction.Top)]
        public void ShouldCorrectlyMoveOnSimpleSwipeOnMultipleGems(string intialBoardString, string finalBoardString, Direction direction)
        {
            // Given an initialBoard
            var actualBoard = BuildBoardFromString(intialBoardString);
            Console.WriteLine("InitialBoard board");
            Console.WriteLine(DrawBoard(actualBoard));

            // When Swiping
            actualBoard.Swipe(direction);

            // Then the board should have the following configuration
            var excpectedBoard = BuildBoardFromString(finalBoardString);
            AssertBoardEquality(excpectedBoard, actualBoard);
        }

        [Test]
        [TestCase("1 1 0-0 0 0-0 0 0", "0 0 2-0 0 0-0 0 0", Direction.Right)]
        [TestCase("0 1 1-0 0 0-0 0 0", "2 0 0-0 0 0-0 0 0", Direction.Left)]
        [TestCase("1 0 0-0 0 0-1 0 0", "0 0 0-0 0 0-2 0 0", Direction.Bottom)]
        [TestCase("0 1 0-0 0 0-0 1 0", "0 2 0-0 0 0-0 0 0", Direction.Top)]
        public void ShouldCorrectlyMergeOnSimpleSwipe(string intialBoardString, string finalBoardString, Direction direction)
        {
            // Given an initialBoard
            var actualBoard = BuildBoardFromString(intialBoardString);
            Console.WriteLine("InitialBoard board");
            Console.WriteLine(DrawBoard(actualBoard));

            // When Swiping
            actualBoard.Swipe(direction);

            // Then the board should have the following configuration
            var excpectedBoard = BuildBoardFromString(finalBoardString);
            AssertBoardEquality(excpectedBoard, actualBoard);
        }

        [Test]
        [TestCase("1 1 1-0 1 0-0 0 0", "0 1 2-0 0 1-0 0 0", Direction.Right)]
        [TestCase("1 1 1-0 0 0-0 1 0", "2 1 0-0 0 0-1 0 0", Direction.Left)]
        [TestCase("1 1 0-1 0 0-1 0 0", "0 0 0-1 0 0-2 1 0", Direction.Bottom)]
        [TestCase("0 1 0-1 1 0-0 1 0", "1 2 0-0 1 0-0 0 0", Direction.Top)]
        public void ShouldCorrectlyMergeAndStaySingleOnSimpleSwipe(string intialBoardString, string finalBoardString, Direction direction)
        {
            // Given an initialBoard
            var actualBoard = BuildBoardFromString(intialBoardString);
            Console.WriteLine("InitialBoard board");
            Console.WriteLine(DrawBoard(actualBoard));

            // When Swiping
            actualBoard.Swipe(direction);

            // Then the board should have the following configuration
            var excpectedBoard = BuildBoardFromString(finalBoardString);
            AssertBoardEquality(excpectedBoard, actualBoard);
        }

        [Test]
        [TestCase("1 1 1 1-0 0 0 0-0 0 0 0", "0 0 2 2-0 0 0 0-0 0 0 0", Direction.Right)]
        public void ShouldCorrectlyMultipleMergeOnSwipe(string intialBoardString, string finalBoardString, Direction direction)
        {
            // Given an initialBoard
            var actualBoard = BuildBoardFromString(intialBoardString);
            Console.WriteLine("InitialBoard board");
            Console.WriteLine(DrawBoard(actualBoard));

            // When Swiping
            actualBoard.Swipe(direction);

            // Then the board should have the following configuration
            var excpectedBoard = BuildBoardFromString(finalBoardString);
            AssertBoardEquality(excpectedBoard, actualBoard);
        }

        [Test]
        [TestCase("1 0 0 1 2-1 0 1 1 2-0 1 1 2 2-1 2 2 1 0-2 2 1 1 2", "0 0 0 2 2-0 0 1 2 2-0 0 0 2 3-0 0 1 3 1-0 0 3 2 2", Direction.Right)]
        [TestCase("0 0 0 2 2-0 0 1 2 2-0 0 0 2 3-0 0 1 3 1-0 0 3 2 2", "0 0 2 3 3-0 0 3 2 3-0 0 0 3 1-0 0 0 2 2-0 0 0 0 0", Direction.Top)]
        [TestCase("1 0 0 1 2-1 0 1 1 2-0 1 1 2 2-1 2 2 1 0-2 2 1 1 2", "0 0 0 0 0-0 0 0 0 0-1 0 2 2 0-2 1 2 2 3-2 3 1 2 3", Direction.Bottom)]
        [TestCase("0 0 0 0 0-0 0 0 0 0-1 0 2 2 0-2 1 2 2 3-2 3 1 2 3", "0 0 0 0 0-0 0 0 0 0-1 3 0 0 0-2 1 3 3 0-2 3 1 2 3", Direction.Left)]
        public void ShouldCorrectlyMergeOnComplexSwipe(string intialBoardString, string finalBoardString, Direction direction)
        {
            // Given an initialBoard
            var actualBoard = BuildBoardFromString(intialBoardString);
            Console.WriteLine("InitialBoard board");
            Console.WriteLine(DrawBoard(actualBoard));

            // When Swiping
            actualBoard.Swipe(direction);

            // Then the board should have the following configuration
            var excpectedBoard = BuildBoardFromString(finalBoardString);
            AssertBoardEquality(excpectedBoard, actualBoard);
        }


        [Test]
        [TestCase("1 0 0-1 1 1-0 0 0", 3, 3, 4)]
        [TestCase("1 0 0 1 1-1 0 0 1 1-0 0 0 0 0-1 3 0 4 5-1 1 1 1 1", 5, 5, 15)]
        [TestCase("1 0-2 1-2 6-0 0-1 1 ", 2, 5, 7)]
        public void ShouldCorrectlyBuildBoardFromString(string boardString, int nbOfColumns, int nbOfRows, int numberOfGems)
        {
            // Given a string board

            // when building board from string
            var board = BuildBoardFromString(boardString);

            var cellsList = board.CellsList;
            var cellsGrid = board.Cells;
            var gems = board.Gems;

            // Then the board dimensions should be correct
            Assert.AreEqual(nbOfRows, board.NbOfRows);
            Assert.AreEqual(nbOfColumns, board.NbOfColumns);

            // Then the numbers of gems should be correct
            Assert.AreEqual(numberOfGems, gems.Count);

            // Then the numbers of cells should be correct
            Assert.AreEqual(nbOfRows * nbOfColumns, cellsList.Count);

            // Then the numbers empty cells should be correct
            Assert.AreEqual(nbOfRows * nbOfColumns - numberOfGems, board.GetEmptyCells().Count);

            // Then the cells and gems postions should be correct and correctly attached
            for (int i = 0; i < nbOfColumns; i++)
            {
                for (int j = 0; j < nbOfRows; j++)
                {
                    var cell = cellsGrid[i, j];
                    Assert.AreEqual(i, cell.X);
                    Assert.AreEqual(j, cell.Y);

                    var gem = cell.GetAttachedGem();
                    if (gem != null)
                    {
                        Assert.AreEqual(i, gem.BoardX);
                        Assert.AreEqual(j, gem.BoardY);
                    }
                }
            }

            // Then building a board from the same string should produce an identical board
            var anotherBoard = BuildBoardFromString(boardString);
            AssertBoardEquality(board, anotherBoard);
        }

        private void AssertBoardEquality(Board expectedBoard, Board actualBoard)
        {
            Console.WriteLine("Expected board");
            Console.WriteLine(DrawBoard(expectedBoard));

            Console.WriteLine("Actual board");
            Console.WriteLine(DrawBoard(actualBoard));

            var expectedCells = expectedBoard.CellsList;
            var actualCells = actualBoard.CellsList;

            var expectedGems = expectedBoard.Gems;
            var actualGems = actualBoard.Gems;

            Assert.AreEqual(expectedGems.Count, actualGems.Count);

            for (int i = 0; i < expectedCells.Count; i++)
            {
                AssertCellEquality(expectedCells[i], actualCells[i]);
            }
        }

        private void AssertCellEquality(Cell expectedCell, Cell actualCell)
        {
            Assert.AreEqual(expectedCell.X, actualCell.X);
            Assert.AreEqual(expectedCell.Y, actualCell.Y);
            Assert.AreEqual(expectedCell.IsEmpty(), actualCell.IsEmpty());

            var expectedGem = expectedCell.GetAttachedGem();
            var actualGem = actualCell.GetAttachedGem();

            if (expectedGem != null)
            {
                Assert.AreEqual(expectedGem.BoardX, actualGem.BoardX);
                Assert.AreEqual(expectedGem.BoardY, actualGem.BoardY);
                Assert.AreEqual(expectedGem.Size, actualGem.Size);
            }
            else
            {
                Assert.AreEqual(null, actualGem);
            }
        }

        private Board BuildBoardFromString(string boardString)
        {
            return new Board(boardString);
        }

        private string DrawBoard(Board board)
        {
            var cellsGrid = board.Cells;
            string draw = "";
            for (int j = 0; j < board.Height; j++)
            {
                for (int i = 0; i < board.Width; i++)
                {
                    draw += " ";
                    var cell = cellsGrid[i, j];
                    var gem = cell.GetAttachedGem();
                    if (gem == null)
                        draw += "0";
                    else draw += gem.Size;
                    draw += " ";
                }
                draw += "\n";
            }

            return draw;
        }
    }
}
