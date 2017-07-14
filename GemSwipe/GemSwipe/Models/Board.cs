﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace GemSwipe.Models
{
    public class Board
    {
        private readonly Cell[,] _cells;
        private readonly IList<Cell> _cellsList;
        private readonly IList<Gem> _gems;
        private Random _randomizer;

        public Board(IList<Cell> cellsList)
        {
            _cellsList = cellsList;
        }

        public Board(int width, int height)
        {
            _randomizer =new Random();

            Height = height;
            Width = width;
            _cellsList = new List<Cell>();
            _cells = new Cell[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    var newCell = new Cell(i, j);
                    _cells[i, j] = newCell;
                    _cellsList.Add(newCell);
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

        public Cell[,] GetCells()
        {
            return _cells;
        }

        public IList<Cell> GetCellsList()
        {
            return _cellsList;
        }

        public IList<Gem> GetGems()
        {
            return _gems;
        }

        public void Swipe(Direction direction)
        {
            var cellsLanes = GetCellsLanes(direction);

            foreach (var cellsLane in cellsLanes)
            {
                var gems = cellsLane.Select(cell => cell.GetAttachedGem()).Where(gem => gem != null).ToList();
                foreach (var cell in cellsLane)
                {
                    cell.DetachGem();
                }

                foreach (Gem gem in gems)
                {
                    foreach (Cell cell in cellsLane)
                    {
                        if (cell.IsEmpty())
                        {
                            cell.AttachGem(gem);
                        }
                        else
                        {
                            var alreadyAttachedGem = cell.GetAttachedGem();
                            if (alreadyAttachedGem.CanMerge() && gem.CanMerge() && alreadyAttachedGem.Size == gem.Size)
                            {
                                Merge(alreadyAttachedGem, gem);
                                break;
                            }
                        }
                    }
                }
            }
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

        public bool IsFull()
        {
            return _cellsList.All(cell => !cell.IsEmpty());
        }

        private void Move(Gem gem, Cell newCell)
        {
            newCell.AttachGem(gem);
        }

        private void Merge(Gem upgradedGem, Gem deadGem)
        {
            upgradedGem.LevelUp();
            deadGem.Die();
            deadGem.Move(upgradedGem.TargetX, upgradedGem.TargetY);
        }

        private Cell GetCellByGem(Gem gem)
        {
            return _cells[gem.X, gem.Y];
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
                            cellsLane.Add(_cells[i, j]);

                        cellsLanes.Add(cellsLane);
                    }
                    break;
                case Direction.Bottom:
                    for (int i = 0; i < Width; i++)
                    {
                        var cellsLane = new List<Cell>();
                        for (int j = 0; j < Height; j++)
                            cellsLane.Add(_cells[i, Height - j - 1]);

                        cellsLanes.Add(cellsLane);
                    }
                    break;
                case Direction.Right:
                    for (int j = 0; j < Height; j++)
                    {
                        var cellsLane = new List<Cell>();
                        for (int i = 0; i < Width; i++)
                            cellsLane.Add(_cells[Width - i - 1, j]);

                        cellsLanes.Add(cellsLane);
                    }
                    break;
                case Direction.Top:
                    for (int i = 0; i < Width; i++)
                    {
                        var cellsLane = new List<Cell>();
                        for (int j = 0; j < Height; j++)
                            cellsLane.Add(_cells[i, j]);

                        cellsLanes.Add(cellsLane);
                    }
                    break;
            }

            return cellsLanes;
        }
    }
}
