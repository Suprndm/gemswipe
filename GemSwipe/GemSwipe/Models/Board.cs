using System;
using System.Collections.Generic;
using System.Linq;

namespace GemSwipe.Models
{
    public class Board
    {
        public Cell[,] Cells { get; }
        public IList<Cell> CellsList { get; }
        public IList<Gem> Gems { get; }
        private Random _randomizer;

        public int Height { get; }
        public int Width { get; }

        public Board(IList<Cell> cellsList)
        {
            _randomizer = new Random();

            CellsList = cellsList;
            Gems = new List<Gem>();
            var maxHeight = 0;
            var maxWidth = 0;
            foreach (var cell in CellsList)
            {
                var gem = cell.GetAttachedGem();
                if (gem != null)
                    Gems.Add(gem);

                maxHeight = Math.Max(maxHeight, cell.Y + 1);
                maxWidth = Math.Max(maxWidth, cell.X + 1);
            }

            Height = maxHeight;
            Width = maxWidth;
            Cells = new Cell[Width, Height];

            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    Cells[i, j] = cellsList.Single(cell => cell.X == i && cell.Y == j);
                }
            }
        }

        public Board(int width, int height)
        {
            _randomizer = new Random();

            Height = height;
            Width = width;
            CellsList = new List<Cell>();
            Cells = new Cell[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    var newCell = new Cell(i, j);
                    Cells[i, j] = newCell;
                    CellsList.Add(newCell);
                }
            }

            Gems = new List<Gem>();
        }

        public Gem Pop()
        {
            var emptyCells = GetEmptyCells();

            if (emptyCells.Count <= 0) return null;

            var randomCell = emptyCells[_randomizer.Next(emptyCells.Count)];
            var gem = new Gem(randomCell.X, randomCell.Y);
            randomCell.AttachGem(gem);
            Gems.Add(gem);

            return gem;
        }

        public SwipeResult Swipe(Direction direction)
        {
            SwipeResult swipeResult = new SwipeResult
            {
                MovedGems = new List<Gem>(),
                DeadGems = new List<Gem>(),
                FusedGems = new List<Gem>()
            };

            var cellsLanes = GetCellsLanes(direction);

            foreach (var cellsLane in cellsLanes)
            {
                var gems = cellsLane.Select(cell => cell.GetAttachedGem()).Where(gem => gem != null).ToList();
                foreach (var cell in cellsLane)
                {
                    cell.DetachGem();
                }

                int gemPositionned = 0;
                foreach (Gem gem in gems)
                {
                    foreach (Cell cell in cellsLane.Skip(gemPositionned-1))
                    {
                        if (cell.IsEmpty())
                        {
                            cell.AttachGem(gem);
                            gem.Move(cell.X, cell.Y);

                            swipeResult.MovedGems.Add(gem);
                            gemPositionned++;
                            break;
                        }

                        var alreadyAttachedGem = cell.GetAttachedGem();
                        if (alreadyAttachedGem.CanMerge() && gem.CanMerge() && alreadyAttachedGem.Size == gem.Size)
                        {
                            // Its a Fuse
                            alreadyAttachedGem.LevelUp();
                            gem.Die();
                            gem.Move(alreadyAttachedGem.TargetX, alreadyAttachedGem.TargetY);

                            swipeResult.DeadGems.Add(gem);
                            swipeResult.FusedGems.Add(alreadyAttachedGem);
                            break;
                        }
                    }
                }
            }

            foreach (var gem in Gems)
            {
                gem.Resolve();
            }
            var deadGems = Gems.Where(gem => gem.IsDead()).ToList();

            foreach (var deadGem in deadGems)
            {
                Gems.Remove(deadGem);
            }

            return swipeResult;
        }

        public IList<Cell> GetEmptyCells()
        {
            var emptyCells = new List<Cell>();

            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    if (Cells[i, j].IsEmpty())
                        emptyCells.Add(Cells[i, j]);
                }
            }

            return emptyCells;
        }

        public bool IsFull()
        {
            return CellsList.All(cell => !cell.IsEmpty());
        }

        private IList<IList<Cell>> GetCellsLanes(Direction direction)
        {
            var cellsLanes = new List<IList<Cell>>();
            switch (direction)
            {
                case Direction.Left:
                    for (int j = 0; j < Height; j++)
                    {
                        var cellsLane = new List<Cell>();
                        for (int i = 0; i < Width; i++)
                            cellsLane.Add(Cells[i, j]);

                        cellsLanes.Add(cellsLane);
                    }
                    break;
                case Direction.Bottom:
                    for (int i = 0; i < Width; i++)
                    {
                        var cellsLane = new List<Cell>();
                        for (int j = 0; j < Height; j++)
                            cellsLane.Add(Cells[i, Height - j - 1]);

                        cellsLanes.Add(cellsLane);
                    }
                    break;
                case Direction.Right:
                    for (int j = 0; j < Height; j++)
                    {
                        var cellsLane = new List<Cell>();
                        for (int i = 0; i < Width; i++)
                            cellsLane.Add(Cells[Width - i - 1, j]);

                        cellsLanes.Add(cellsLane);
                    }
                    break;
                case Direction.Top:
                    for (int i = 0; i < Width; i++)
                    {
                        var cellsLane = new List<Cell>();
                        for (int j = 0; j < Height; j++)
                            cellsLane.Add(Cells[i, j]);

                        cellsLanes.Add(cellsLane);
                    }
                    break;
            }

            return cellsLanes;
        }
    }
}
