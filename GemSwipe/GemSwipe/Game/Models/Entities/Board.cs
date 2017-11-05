using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GemSwipe.Paladin.Core;
using SkiaSharp;

namespace GemSwipe.Game.Models.Entities
{
    public class Board : SkiaView
    {
        public Cell[,] Cells { get; private set; }
        public IList<Cell> CellsList { get; private set; }
        public IList<Gem> Gems { get; private set; }

        public int MovesToResolve { get; private set; }
        public int NbOfRows { get; private set; }
        public int NbOfColumns { get; private set; }
        private Random _randomizer;

        private const double BoardCellMarginPercentage = 0.10;

        private readonly BoardSetup _boardSetup;
        private float _horizontalMarginPerCell;
        private float _verticalMarginPerCell;
        private float _horizontalBoardMargin;
        private float _verticalBoardMargin;
        private float _cellWidth;
        private float _cellHeight;
        private float _maxBoardWidth;
        private float _maxBoardHeight;


        public Board(BoardSetup boardSetup, float x, float y, float height, float width) : base(x, y, height, width)
        {
            _maxBoardWidth = width;
            _maxBoardHeight = width;
            _boardSetup = boardSetup;

            _randomizer = new Random();

            NbOfRows = boardSetup.Rows;
            NbOfColumns = boardSetup.Columns;
            UpdateDimensions();
            Setup(boardSetup);

            DeclareTappable(this);
        }

        public Board(string boardString) : base(0, 0, 0, 0)
        {
            Setup(new BoardSetup(1, 0, 0, boardString));
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

        public async Task<bool> RefillGems()
        {
            var cells = GetEmptyCells();

            if (cells.Count == 0)
                return true;

            int size = 3;
            var randomNumber = _randomizer.Next(10);
            if (randomNumber < 10)
                size = 1;
            else if (randomNumber < 10)
                size = 2;

            var cell = cells[_randomizer.Next(cells.Count)];

            var gem = CreateGem(cell.X, cell.Y, size);
            cell.AttachGem(gem);

            await gem.Pop();

            return false;
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

                        newCell.AttachGem(CreateGem(i, j, size));
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

            PopGems();
        }

        private Gem CreateGem(int boardX, int boardY, int size)
        {
            var gemRadius = GetGemSize();

            var gemX = ToGemViewX(boardX) + _cellWidth / 2 - gemRadius;
            var gemY = ToGemViewY(boardY) + _cellWidth / 2 - gemRadius;

            var gem = new Gem(boardX, boardY, size, gemX, gemY, gemRadius, _randomizer);
            Gems.Add(gem);

            AddChild(gem);

            return gem;
        }

        private async Task PopGems()
        {
            await Task.Delay(500);
            var shuffledGems = Gems.OrderBy(g => _randomizer.Next()).Select(g => g).ToList();
            foreach (var gem in shuffledGems)
            {
                await Task.Delay((_randomizer.Next(100) + 10) * 4);
                gem.Pop();
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
                            if (gem.BoardX != cell.X || gem.BoardY != cell.Y)
                            {
                                swipeResult.MovedGems.Add(gem);
                                gem.Move(cell.X, cell.Y);
                            }

  
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

            swipeResult.IsBlocked = false;
            swipeResult.IsFull = IsFull();

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
            //DrawCells(Canvas);
        }

        public void UpdateDimensions()
        {
            _horizontalMarginPerCell = (float)(_maxBoardWidth * BoardCellMarginPercentage) / (NbOfColumns + 1);
            _verticalMarginPerCell = (float)(_maxBoardHeight * BoardCellMarginPercentage) / (NbOfRows + 1);

            _cellWidth = (_maxBoardWidth - (NbOfColumns + 1) * _horizontalMarginPerCell) / NbOfColumns;
            _cellHeight = (_maxBoardHeight - (NbOfRows + 1) * _verticalMarginPerCell) / NbOfRows;

            var min = Math.Min(_cellWidth, _cellHeight);
            _cellWidth = min;
            _cellHeight = min;

            var boardHeight = (_cellHeight + _verticalMarginPerCell) * NbOfRows - _verticalMarginPerCell;
            var boardWidth = (_cellWidth + _horizontalMarginPerCell) * NbOfColumns - _horizontalMarginPerCell;

            _horizontalBoardMargin = (_maxBoardWidth - boardWidth) / 2;
            _verticalBoardMargin = (_maxBoardHeight - boardHeight) / 2;

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
                            paint.Color = CreateColor(155, 155, 155, 255);
                            Canvas.DrawCircle(
                                X + (i * (_cellWidth + _horizontalMarginPerCell) + _horizontalBoardMargin +
                                     _cellWidth / 2),
                                Y + (j * (_cellHeight + _verticalMarginPerCell) + _verticalBoardMargin +
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
                            paint.Color = CreateColor(255, 255, 255, 150);
                            Canvas.DrawCircle(X + (i * (_cellWidth + _horizontalMarginPerCell) + _horizontalBoardMargin + _cellWidth / 2), Y + (j * (_cellHeight + _verticalMarginPerCell) + _verticalBoardMargin + _cellHeight / 2), _cellWidth / 2.5f, paint);
                        }
                    }

                }
            }

        }

        private float ToGemViewX(int gemStateX)
        {
            return (gemStateX * (_cellWidth + _horizontalMarginPerCell) + _horizontalBoardMargin);
        }

        private float ToGemViewY(int gemStateY)
        {
            return (gemStateY * (_cellWidth + _verticalMarginPerCell) + _verticalBoardMargin);
        }

        public override void Dispose()
        {
            base.Dispose();
            Gems.Clear();
            CellsList.Clear();
        }
    }
}