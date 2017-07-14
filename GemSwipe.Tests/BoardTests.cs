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
        [TestCase(5,5)]
        [TestCase(3, 8)]
        [TestCase(9,4)]
        public void ShouldCorrectlySetupBoard(int width, int height)
        {
            // Given Width and height

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
            Assert.AreEqual(width*height, cellsList.Count);

            // Then there are no gems 
            var gems = _board.GetGems();
            Assert.AreEqual(0,gems.Count);
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
            for (int i = 0; i < width*height; i++)
            {
                _board.Pop();
            }

            var cellsList = _board.GetCellsList();
            var gems = _board.GetGems();

            // Then the board should be full
            Assert.AreEqual(true, _board.IsFull());

            // Then there should be the exact amount of cells as board capacity
            Assert.AreEqual(width*height, gems.Count);

            // Then all  the cells should be all filled
            Assert.AreEqual(true, cellsList.All(cell=>!cell.IsEmpty()));

            // Then each cell should be attached to a gem that match its position
            Assert.AreEqual(true, cellsList.All(cell => cell.GetAttachedGem().X == cell.X && cell.GetAttachedGem().Y == cell.Y));

            // Then each gem should be at size
            Assert.AreEqual(true, gems.All(gem=>gem.Size==1));
        }
    }
}
