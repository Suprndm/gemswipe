using System;
using System.Collections.Generic;
using System.Linq;
using GemSwipe.Game.Models;
using GemSwipe.Game.SkiaEngine;
using SkiaSharp;

namespace GemSwipe.Game.Entities
{
    public class Board : SkiaView
    {
        public Cell[,] Cells { get; private set; }
        public IList<Cell> CellsList { get; private set; }
        public IList<Gem> Gems { get; private set; }

        public int MovesToResolve { get; private set; }
        public int NbOfRows { get; private set; }
        public int NbOfColumns { get; private set; }

        private const double BoardCellMarginPercentage = 0.10;

        private readonly BoardSetup _boardSetup;
        private float _horizontalMarginPerCell;
        private float _verticalMarginPerCell;
        private float _cellWidth;
        private float _cellHeight;
        private float _boardWidth;
        private float _boardHeight;


        public Board(BoardSetup boardSetup, SKCanvas canvas, float x, float y, float height, float width) : base(canvas, x, y, height, width)
        {
            _boardWidth = width;
            _boardHeight = width;
            _boardSetup = boardSetup;
            MovesToResolve = boardSetup.Moves;
            NbOfRows = boardSetup.Rows;
            NbOfColumns = boardSetup.Columns;
            UpdateDimensions();
            Setup(boardSetup);
        }

        public Board(string boardString) : base(null, 0, 0, 0, 0)
        {
            Setup(new BoardSetup(1, 0, 0, boardString, 0));
        }

        public void Reset()
        {
            CellsList.Clear();
            foreach (var gem in Gems)
            {
                gem.Dispose();
            }
            Gems.Clear();
            Setup(_boardSetup);
        }


        private void Setup(BoardSetup boardSetup)
        {
            Gems = new List<Gem>();

            var boardString = boardSetup.SetupString;

            var rows = boardString.Split('-');
            var nbOfRows = rows.Length;
            var nbOfColumns = rows[0].Split(' ').Length;
            var boardCells = new List<Cell>();
            for (int j = 0; j < nbOfRows; j++)
            {
                var cells = rows[j].Split(' ');
                for (int i = 0; i < nbOfColumns; i++)
                {
                    var size = int.Parse(cells[i]);

                    bool cellIsBlocked = false;
                    if (size == 9)
                    {
                        cellIsBlocked = true;
                        size = 0;
                    }

                    var newCell = new Cell(i, j, cellIsBlocked);

                    boardCells.Add(newCell);
                    if (size > 0)
                    {
                        var gemRadius = GetGemSize();

                        var gemX = ToGemViewX(i) + _cellWidth / 2 - gemRadius;
                        var gemY = ToGemViewY(j) + _cellWidth / 2 - gemRadius;

                        var gem = new Gem(i, j, size, Canvas, gemX, gemY, gemRadius);
                        Gems.Add(gem);

                        AddChild(gem);
                        newCell.AttachGem(gem);
                    }
                }
            }


            CellsList = boardCells;
            Gems = new List<Gem>();
            var maxNbOfColumns = 0;
            var maxNbOfRows = 0;
            foreach (var cell in CellsList)
            {
                var gem = cell.GetAttachedGem();
                if (gem != null)
                    Gems.Add(gem);

                maxNbOfColumns = Math.Max(maxNbOfColumns, cell.X + 1);
                maxNbOfRows = Math.Max(maxNbOfRows, cell.Y + 1);
            }

            NbOfRows = maxNbOfRows;
            NbOfColumns = maxNbOfColumns;
            Cells = new Cell[NbOfColumns, NbOfRows];

            for (int i = 0; i < NbOfColumns; i++)
            {
                for (int j = 0; j < NbOfRows; j++)
                {
                    Cells[i, j] = boardCells.Single(cell => cell.X == i && cell.Y == j);
                }
            }
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
            var splitCellsLanes = SplitCellsLanesByBlocked(cellsLanes);

            foreach (var cellsLane in splitCellsLanes)
            {
                var gems = cellsLane.Select(cell => cell.GetAttachedGem()).Where(gem => gem != null).ToList();
                foreach (var cell in cellsLane)
                {
                    cell.DetachGem();
                }

                int gemPositionned = 0;
                foreach (Gem gem in gems)
                {
                    foreach (Cell cell in cellsLane.Skip(gemPositionned - 1))
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
                            gem.Move(alreadyAttachedGem.TargetBoardX, alreadyAttachedGem.TargetBoardY);

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

            swipeResult.IsBlocked = false; // TODO
            swipeResult.BoardWon = Gems.Count == 1;

            UpdateGemsPositions(swipeResult);

            return swipeResult;
        }

        public IList<Cell> GetEmptyCells()
        {
            var emptyCells = new List<Cell>();

            for (int i = 0; i < NbOfColumns; i++)
            {
                for (int j = 0; j < NbOfRows; j++)
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

        public IList<IList<Cell>> SplitCellsLanesByBlocked(IList<IList<Cell>> cellsLanes)
        {
            IList<IList<Cell>> splitedCellsLanes = new List<IList<Cell>>();
            IList<Cell> currentLane = null;
            foreach (var lane in cellsLanes)
            {
                foreach (var cell in lane)
                {
                    if (cell.IsBlocked)
                        currentLane = null;
                    else
                    {
                        if (currentLane == null)
                        {
                            currentLane = new List<Cell>() { cell };
                            splitedCellsLanes.Add(currentLane);
                        }
                        else
                        {
                            currentLane.Add(cell);
                        }
                    }
                }

                currentLane = null;
            }

            return splitedCellsLanes;
        }

        public IList<IList<Cell>> GetCellsLanes(Direction direction)
        {
            var cellsLanes = new List<IList<Cell>>();
            switch (direction)
            {
                case Direction.Left:
                    for (int j = 0; j < NbOfRows; j++)
                    {
                        var cellsLane = new List<Cell>();
                        for (int i = 0; i < NbOfColumns; i++)
                            cellsLane.Add(Cells[i, j]);

                        cellsLanes.Add(cellsLane);
                    }
                    break;
                case Direction.Bottom:
                    for (int i = 0; i < NbOfColumns; i++)
                    {
                        var cellsLane = new List<Cell>();
                        for (int j = 0; j < NbOfRows; j++)
                            cellsLane.Add(Cells[i, NbOfRows - j - 1]);

                        cellsLanes.Add(cellsLane);
                    }
                    break;
                case Direction.Right:
                    for (int j = 0; j < NbOfRows; j++)
                    {
                        var cellsLane = new List<Cell>();
                        for (int i = 0; i < NbOfColumns; i++)
                            cellsLane.Add(Cells[NbOfColumns - i - 1, j]);

                        cellsLanes.Add(cellsLane);
                    }
                    break;
                case Direction.Top:
                    for (int i = 0; i < NbOfColumns; i++)
                    {
                        var cellsLane = new List<Cell>();
                        for (int j = 0; j < NbOfRows; j++)
                            cellsLane.Add(Cells[i, j]);

                        cellsLanes.Add(cellsLane);
                    }
                    break;
            }

            return cellsLanes;
        }

        public override string ToString()
        {
            var cellsGrid = Cells;
            string draw = "";
            for (int j = 0; j < NbOfRows; j++)
            {
                for (int i = 0; i < NbOfColumns; i++)
                {
                    draw += "";
                    var cell = cellsGrid[i, j];
                    var gem = cell.GetAttachedGem();
                    if (gem == null)
                    {
                        if (cell.IsBlocked)
                            draw += "9";
                        else
                            draw += "0";
                    }
                    else draw += gem.Size;
                    draw += " ";
                }
                draw = draw.Substring(0, draw.Length - 1);
                draw += "-";
            }
            draw = draw.Substring(0, draw.Length - 1);
            return draw;
        }

        private float GetGemSize()
        {
            return (float)2 / 3 * _cellWidth / 2;
        }

        /// <summary>
        /// There are three kind of updates
        /// </summary>
        /// <param name="swipeResult"></param>
        private void UpdateGemsPositions(SwipeResult swipeResult)
        {
            foreach (var movedGem in swipeResult.MovedGems)
            {
                var gemView = movedGem;
                ;
                gemView.MoveTo(ToGemViewX(movedGem.BoardX) + (_cellWidth - movedGem.Width) / 2, ToGemViewY(movedGem.BoardY) + (_cellWidth - movedGem.Width) / 2);
            }

            foreach (var deadGem in swipeResult.DeadGems)
            {
                var gemView = deadGem;
                gemView.ZIndex = -1;
                gemView.DieTo(ToGemViewX(deadGem.BoardX) + (_cellWidth - deadGem.Width) / 2, ToGemViewY(deadGem.BoardY) + (_cellWidth - deadGem.Width) / 2);
            }

            foreach (var fusedGem in swipeResult.FusedGems)
            {
                var gemView = fusedGem;
                gemView.Fuse();
            }
        }

        protected override void Draw()
        {
            DrawCells(Canvas);
        }

        public void UpdateDimensions()
        {
            _horizontalMarginPerCell = (float)(_boardWidth * BoardCellMarginPercentage) / (NbOfColumns + 1);
            _verticalMarginPerCell = (float)(_boardHeight * BoardCellMarginPercentage) / (NbOfRows + 1);

            _cellWidth = (_boardWidth - (NbOfColumns + 1) * _horizontalMarginPerCell) / NbOfColumns;
            _cellHeight = (_boardHeight - (NbOfRows + 1) * _verticalMarginPerCell) / NbOfRows;
        }

        private void DrawCells(SKCanvas canvas)
        {

            UpdateDimensions();

            for (int i = 0; i < NbOfColumns; i++)
            {
                for (int j = 0; j < NbOfRows; j++)
                {
                    if (Cells[i, j].IsBlocked)
                    {
                        using (var paint = new SKPaint())
                        {
                            paint.IsAntialias = true;
                            paint.Style = SKPaintStyle.StrokeAndFill;
                            paint.StrokeWidth = 2;
                            paint.Color = new SKColor(155, 155, 155, 255);
                            Canvas.DrawCircle(
                                X + (i * (_cellWidth + _horizontalMarginPerCell) + _horizontalMarginPerCell +
                                     _cellWidth / 2),
                                Y + (j * (_cellHeight + _verticalMarginPerCell) + _verticalMarginPerCell +
                                     _cellHeight / 2), _cellWidth / 2.5f, paint);
                        }
                    }
                    else
                    {
                        using (var paint = new SKPaint())
                        {
                            paint.IsAntialias = true;
                            paint.Style = SKPaintStyle.Stroke;
                            paint.StrokeWidth = 2;
                            paint.Color = new SKColor(255, 255, 255, 150);
                            Canvas.DrawCircle(X + (i * (_cellWidth + _horizontalMarginPerCell) + _horizontalMarginPerCell + _cellWidth / 2), Y + (j * (_cellHeight + _verticalMarginPerCell) + _verticalMarginPerCell + _cellHeight / 2), _cellWidth / 2.5f, paint);
                        }
                    }

                }
            }

        }

        private float ToGemViewX(int gemStateX)
        {
            return (gemStateX * (_cellWidth + _horizontalMarginPerCell) + _horizontalMarginPerCell);
        }

        private float ToGemViewY(int gemStateY)
        {
            return (gemStateY * (_cellWidth + _verticalMarginPerCell) + _verticalMarginPerCell);
        }

        public override void Dispose()
        {
            base.Dispose();
            Gems.Clear();
            CellsList.Clear();
        }
    }
}