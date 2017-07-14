using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace GemSwipe.Models
{
    public class Board
    {
        private readonly Cell[,] _cells;
        private readonly IList<Gem> _gems;
        private Random _randomizer;

        public Board(int width, int height)
        {
            Height = height;
            Width = width;
            _cells = new Cell[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    _cells[i, j] = new Cell(i, j);
                }
            }

            _gems = new List<Gem>();
        }

        public int Height { get; }
        public int Width { get; }


        public Gem Pop()
        {
            var emptyCells = GetEmptyCells();

            if (emptyCells.Count <= 0) return null;

            var randomCell = emptyCells[_randomizer.Next(emptyCells.Count)];
            var gem = new Gem(randomCell.X, randomCell.Y);
            randomCell.AttachGem(gem);
            _gems.Add(gem);

            return gem;
        }

        public void Swipe(Direction direction)
        {
            var lanes = GetLanes(direction);

            foreach (var lane in lanes)
            {

                if (lane.All(cell => cell.IsEmpty()))
                {
                    // do nothing
                }
                else
                {
                    // First move all gems to the side 
                    for (int i = 0; i < lane.Count; i++)
                    {
                        var currentCell = lane[i];
                        // While we can still move cells
                        var count = i;
                        while (currentCell.IsEmpty() && count < lane.Count - 1)
                        {
                            count++;
                            // Move all the other gems
                            for (int k = i + 1; k < lane.Count; k++)
                            {
                                if (!lane[k].IsEmpty())
                                    Move(lane[k].GetAttachedGem(), lane[k - 1]);
                            }
                        }
                    }

                    // Detect fusions
                    var u = 0;
                    while (!lane[u].IsEmpty() && u < lane.Count - 1)
                    {
                        var currentGem = lane[u].GetAttachedGem();
                        var nextGem = lane[u + 1].GetAttachedGem();
                        if (currentGem.Size == nextGem.Size)
                        {
                            
                        }
                        u++;
                    }
                }

            }
        }

        private void Merge(Gem currentGem, Gem nextGem)
        {
            currentGem.LevelUp();


        }

        public IList<Cell> GetEmptyCells()
        {
            var emptyCells = new List<Cell>();

            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    if (_cells[i, j].IsEmpty())
                        emptyCells.Add(_cells[i, j]);
                }
            }

            return emptyCells;
        }

        private void Move(Gem gem, Direction direction)
        {

        }

        private void Move(IList<Cell> lane,)

        private void Move(Gem gem, Cell newCell)
        {
            var oldCell = GetCellByGem(gem);
            oldCell.DetachGem(gem);
            newCell.AttachGem(gem);
        }

        private Cell GetCellByGem(Gem gem)
        {
            return _cells[gem.X, gem.Y];
        }

        private IList<IList<Cell>> GetLanes(Direction direction)
        {
            var lanes = new List<IList<Cell>>();
            switch (direction)
            {
                case Direction.Left:
                    for (int j = 0; j < Height; j++)
                    {
                        var lane = new List<Cell>();
                        for (int i = 0; i < Width; i++)
                            lane.Add(_cells[i, j]);

                        lanes.Add(lane);
                    }
                    break;
                case Direction.Bottom:
                    for (int i = 0; i < Width; i++)
                    {
                        var lane = new List<Cell>();
                        for (int j = 0; j < Height; j++)
                            lane.Add(_cells[i, Height - j - 1]);

                        lanes.Add(lane);
                    }
                    break;
                case Direction.Right:
                    for (int j = 0; j < Height; j++)
                    {
                        var lane = new List<Cell>();
                        for (int i = 0; i < Width; i++)
                            lane.Add(_cells[Width - i - 1, j]);

                        lanes.Add(lane);
                    }
                    break;
                case Direction.Top:
                    for (int i = 0; i < Width; i++)
                    {
                        var lane = new List<Cell>();
                        for (int j = 0; j < Height; j++)
                            lane.Add(_cells[i, j]);

                        lanes.Add(lane);
                    }
                    break;
            }

            return lanes;
        }

        private Cell NextCell(Cell cell, Direction direction)
        {

        }
    }
}
