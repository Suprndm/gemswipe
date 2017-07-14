using System.Collections.Generic;
using System.Linq;
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
        [TestCase(5, 5)]
        [TestCase(3, 8)]
        [TestCase(9, 4)]
        public void ShouldCorrectlySetupBoard(int width, int height)
        {
            // Given width and height

            // When board is built
            _board = new Board(width, height);

            // Then all cells are empty
            var cells = _board.GetCells();

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Assert.AreEqual(true, cells[i, j].IsEmpty());
                }
            }

            // Then cells number is correct;
            var cellsList = _board.GetCellsList();
            Assert.AreEqual(width * height, cellsList.Count);

            // Then there are no gems 
            var gems = _board.GetGems();
            Assert.AreEqual(0, gems.Count);
        }

        [Test]
        [TestCase(6, 6)]
        [TestCase(5, 12)]
        [TestCase(5, 4)]
        public void ShouldCorrectlyPopTillMaxCapacity(int width, int height)
        {
            // Given a Width and height board
            _board = new Board(width, height);

            // When the board is popped till its max capacity
            for (int i = 0; i < width * height; i++)
            {
                _board.Pop();
            }

            var cellsList = _board.GetCellsList();
            var gems = _board.GetGems();

            // Then the board should be full
            Assert.AreEqual(true, _board.IsFull());

            // Then there should be the exact amount of cells as board capacity
            Assert.AreEqual(width * height, gems.Count);

            // Then all  the cells should be all filled
            Assert.AreEqual(true, cellsList.All(cell => !cell.IsEmpty()));

            // Then each cell should be attached to a gem that match its position
            Assert.AreEqual(true, cellsList.All(cell => cell.GetAttachedGem().X == cell.X && cell.GetAttachedGem().Y == cell.Y));

            // Then each gem should be at size
            Assert.AreEqual(true, gems.All(gem => gem.Size == 1));
        }

        [Test]
        [TestCase("1 0 0-1 1 1-0 0 0", 3, 3, 4)]
        [TestCase("1 0 0 1 1-1 0 0 1 1-0 0 0 0 0-1 3 0 4 5-1 1 1 1 1", 5, 5, 15)]
        [TestCase("1 0-2 1-2 6-0 0-1 1 ", 2, 5, 7)]
        public void ShouldCorrectlyBuildBoardFromString(string boardString, int width, int height, int numberOfGems)
        {
            // Given a string board

            // when building board from string
            var board = BuildBoardFromString(boardString);

            var cellsList = board.GetCellsList();
            var cellsGrid = board.GetCells();
            var gems = board.GetGems();

            // Then the board dimensions should be correct
            Assert.AreEqual(height, board.Height);
            Assert.AreEqual(width, board.Width);

            // Then the numbers of gems should be correct
            Assert.AreEqual(numberOfGems, gems.Count);

            // Then the numbers of cells should be correct
            Assert.AreEqual(height * width, cellsList.Count);

            // Then the numbers empty cells should be correct
            Assert.AreEqual(height * width - numberOfGems, board.GetEmptyCells().Count);

            // Then the cells and gems postions should be correct and correctly attached
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    var cell = cellsGrid[i, j];
                    Assert.AreEqual(i, cell.X);
                    Assert.AreEqual(j, cell.Y);

                    var gem = cell.GetAttachedGem();
                    if (gem != null)
                    {
                        Assert.AreEqual(i, gem.X);
                        Assert.AreEqual(j, gem.Y);
                    }
                }
            }

            // Then building a board from the same string should produce an identical board
            var anotherBoard = BuildBoardFromString(boardString);
            AssertBoardEquality(board, anotherBoard);
        }

        private void AssertBoardEquality(Board expectedBoard, Board actualBoard)
        {
            var expectedCells = expectedBoard.GetCellsList();
            var actualCells = actualBoard.GetCellsList();

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
                Assert.AreEqual(expectedGem.X, actualGem.X);
                Assert.AreEqual(expectedGem.Y, actualGem.Y);
                Assert.AreEqual(expectedGem.Size, actualGem.Size);
            }
            else
            {
                Assert.AreEqual(null, actualGem);
            }
        }

        private Board BuildBoardFromString(string boardString)
        {
            var rows = boardString.Split('-');
            var height = rows.Length;
            var width = rows[0].Split(' ').Length;
            var boardCells = new List<Cell>();
            for (int j = 0; j < height; j++)
            {
                var cells = rows[j].Split(' ');
                for (int i = 0; i < width; i++)
                {
                    var gemSize = int.Parse(cells[i]);
                    var newCell = new Cell(i, j);
                    boardCells.Add(newCell);
                    if (gemSize > 0)
                    {
                        var newGem = new Gem(i, j);
                        newGem.SetSize(gemSize);
                        newCell.AttachGem(newGem);
                    }
                }
            }

            return new Board(boardCells);
        }
    }
}
