using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GemSwipe.Paladin.Core;
using SkiaSharp;
using GemSwipe.Game.Models.BoardModel;
using GemSwipe.Game.Models.BoardModel.Gems;
using GemSwipe.Services;

namespace GemSwipe.Game.Models.Entities
{
    public class Board : SkiaView
    {
        public Cell[,] Cells { get; private set; }
        public IList<Cell> CellsList { get; private set; }
        public IList<Gem> Gems { get; private set; }

        public IList<TeleportationGem> TeleportationGems { get; set; }

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


        public Board(BoardSetup boardSetup, float x, float y, float width, float height) : base(x, y, height, width)
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
            _randomizer = new Random();
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

            var gem = CreateGem(cell.X, cell.Y, size.ToString(), size);
            cell.AttachGem(gem);

            await gem.Pop();

            return false;
        }

        private GemType ParseGemType(string rawData)
        {
            int size = 0;
            bool isSize = int.TryParse(rawData, out size);

            if (isSize)
            {
                if (size == 0)
                {
                    return GemType.None;
                }
                else
                {
                    return GemType.Base;
                }
            }
            else
            {
                string gemCode = rawData.Substring(0, 2);
                switch (gemCode)
                {
                    default:
                        return GemType.Base;
                    case "BL":
                        return GemType.Blocking;
                    case "BH":
                        return GemType.Blackhole;
                    case "TP":
                        return GemType.Teleportation;

                }
            }
        }

        private void Setup(BoardSetup boardSetup)
        {
            TeleportationGems = new List<TeleportationGem>();

            Gems = new List<Gem>();
            CellsList = new List<Cell>();
            var boardString = boardSetup.SetupString;

            var rows = boardString.Split('-');
            var nbOfRows = rows.Length;
            var nbOfColumns = rows[0].Split(' ').Length;


            for (int j = 0; j < nbOfRows; j++)
            {
                var rowCells = rows[j].Split(' ');
                for (int i = 0; i < nbOfColumns; i++)
                {
                    Cell cell = CreateCell(i, j, rowCells[i]);
                    CellsList.Add(cell);

                    Gem gem = CreateGem(i, j, rowCells[i]);
                    if (gem != null)
                    {
                        AddChild(gem);
                        cell.AttachGem(gem);
                        Gems.Add(gem);
                    }
                }
            }

            var maxNbOfColumns = 0;
            var maxNbOfRows = 0;
            foreach (var cell in CellsList)
            {
                maxNbOfColumns = Math.Max(maxNbOfColumns, cell.IndexX + 1);
                maxNbOfRows = Math.Max(maxNbOfRows, cell.IndexY + 1);
            }

            NbOfRows = maxNbOfRows;
            NbOfColumns = maxNbOfColumns;
            Cells = new Cell[NbOfColumns, NbOfRows];

            for (int i = 0; i < NbOfColumns; i++)
            {
                for (int j = 0; j < NbOfRows; j++)
                {
                    Cells[i, j] = CellsList.Single(cell => cell.IndexX == i && cell.IndexY == j);
                }
            }

            PopGems();
        }

        private Cell CreateCell(int boardX, int boardY, string rawData)
        {
            return new Cell(boardX, boardY, this, false);
        }

        private Gem CreateGem(int boardX, int boardY, string rawData, int size = 0)
        {
            GemType gemType = ParseGemType(rawData);
            var gemRadius = GetGemSize();

            var gemX = ToGemViewX(boardX) + _cellWidth / 2 - gemRadius;
            var gemY = ToGemViewY(boardY) + _cellWidth / 2 - gemRadius;
            bool isSize = int.TryParse(rawData, out size);

            if (size == 0)
            {
                return null;
            }
            else
            {
                return new Gem(boardX, boardY, size, gemX, gemY, gemRadius, _randomizer, this);
            }

            //Gem gem;

            //switch (gemType)
            //{
            //    case GemType.None:
            //        return null;
            //    case GemType.Base:
            //        bool isSize = int.TryParse(rawData, out size);
            //        return new Gem(boardX, boardY, size, gemX, gemY, gemRadius, _randomizer,this);
            //    case GemType.Blocking:
            //        return null;
            //    case GemType.Blackhole:
            //        return new BlackholeGem(boardX, boardY, size, gemX, gemY, gemRadius, _randomizer,this);
            //    case GemType.Teleportation:
            //        string portalId = rawData.Substring(2);
            //        TeleportationGem teleportationGem = new TeleportationGem(this, portalId, boardX, boardY, size, gemX, gemY, gemRadius, _randomizer);
            //        TeleportationGems.Add(teleportationGem);
            //        teleportationGem.FindExit();
            //        return teleportationGem;
            //    default:
            //        return null;
            //}
        }

        private async Task PopGems()
        {
            await Task.Delay(500);
            var shuffledGems = Gems.OrderBy(g => _randomizer.Next()).Select(g => g).ToList();
            foreach (var gembase in shuffledGems)
            {
                Gem gem = (Gem)gembase;
                await Task.Delay((_randomizer.Next(100) + 10) * 4);
                gem.Pop();
            }
        }

        public SwipeResult Swipe(Direction direction)
        {
            int pascell = 0;
            int pasgem = 0;

            foreach (GemBase gem in Gems)
            {
                gem.Reinitialize();
            }
            foreach (Cell cell in CellsList)
            {
                cell.Reinitialize();
            }

            while (AnyCellCanActivate())
            {
                foreach (Cell cell in CellsList)
                {
                    cell.ResolveSwipe(direction);
                }
                pascell++;
            }

            foreach (Gem gem in Gems)
            {
                gem.RunAnimation();
            }

            Logger.Log("Pascell = " + pascell);
            Logger.Log("Pasgem = " + pasgem);

            SwipeResult swipeResult = new SwipeResult
            {
                MovedGems = new List<Gem>(){
                    new Gem(0,0,0,this) },
                DeadGems = new List<Gem>(),
                FusedGems = new List<Gem>()
            };
            return swipeResult;
        }

        private bool AnyCellCanActivate()
        {
            bool AnyCanActivate = false;
            foreach (Cell cell in CellsList)
            {
                if (cell.CanActivate())
                {
                    AnyCanActivate = true;
                    break;
                }
            }
            return AnyCanActivate;
        }

        private bool IsResolved()
        {
            bool isResolved = true;
            foreach (Cell cell in CellsList)
            {
                if (cell.AttachedGem != null)
                {
                    IGem gem = cell.AttachedGem;
                    if (!gem.IsResolved())
                    {
                        isResolved = false;
                    }
                }
            }
            return isResolved;
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
                    {
                        if (cell.Modifier == CellModifier.Teleporter)
                        {
                            //if (currentLane != null)
                            //{
                            //    currentLane.Add(cell);
                            //}
                            //if (currentLane != null)
                            //{

                            //    currentLane = new List<Cell>() { cell };
                            //    splitedCellsLanes.Add(currentLane);
                            //}
                            currentLane = new List<Cell>() { cell };
                            splitedCellsLanes.Add(currentLane);

                        }
                        else
                        {
                            currentLane = null;
                        }
                        //currentLane = null;
                    }
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
            foreach (var lanex in splitedCellsLanes)
            {
                Logger.Log("lane");
                string lanestr = "";
                foreach (var cellsplit in lanex)
                {
                    lanestr += cellsplit.X.ToString() + " " + cellsplit.Y.ToString() + ", ";
                    //Logger.Log(cellsplit.X.ToString() + " " + cellsplit.Y.ToString());
                }
                Logger.Log(lanestr);
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
        //private async void UpdateGemsPositions(SwipeResult swipeResult)
        //{
        //    foreach (var movedGem in swipeResult.MovedGems)
        //    {
        //        var gemView = movedGem;
        //        gemView.MoveTo(ToGemViewX(movedGem.BoardX) + (_cellWidth - movedGem.Width) / 2, ToGemViewY(movedGem.BoardY) + (_cellWidth - movedGem.Width) / 2);
        //    }

        //    foreach (var deadGem in swipeResult.DeadGems)
        //    {
        //        var gemView = deadGem;
        //        gemView.ZIndex = -1;
        //        gemView.DieTo(ToGemViewX(deadGem.BoardX) + (_cellWidth - deadGem.Width) / 2, ToGemViewY(deadGem.BoardY) + (_cellWidth - deadGem.Width) / 2);
        //    }

        //    foreach (var fusedGem in swipeResult.FusedGems)
        //    {
        //        var gemView = fusedGem;
        //        gemView.Fuse();
        //    }
        //}

        protected override void Draw()
        {
            DrawCells(Canvas);
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

                    //if (Cells[i, j].IsBlocked)
                    if (Cells[i, j].AttachedGem != null)
                    {
                        var cell = Cells[i, j];
                        var gem = cell.AttachedGem;
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

                            //Canvas.DrawText(gem.BoardX + ", " + gem.BoardY, X + (i * (_cellWidth + _horizontalMarginPerCell) + _horizontalBoardMargin),
                            //    Y + (j * (_cellHeight + _verticalMarginPerCell) + _verticalBoardMargin),
                            //    paint);
                        }
                    }
                    //else
                    if (false)
                    {
                        using (var paint = new SKPaint())
                        {
                            paint.IsAntialias = true;
                            paint.Style = SKPaintStyle.Stroke;
                            paint.StrokeWidth = 2;
                            paint.Color = CreateColor(255, 255, 255, 150);
                            Canvas.DrawCircle(X + (i * (_cellWidth + _horizontalMarginPerCell) + _horizontalBoardMargin + _cellWidth / 2), Y + (j * (_cellHeight + _verticalMarginPerCell) + _verticalBoardMargin + _cellHeight / 2), _cellWidth / 2.5f, paint);
                        }

                        var colors = new SKColor[] {
                            CreateColor (0, 0,0, (byte)(30*_opacity)),
                            CreateColor (0, 00, 0,0),
                        };

                        var glowSize = _cellWidth * 0.5f;
                        var cellX = X + (i * (_cellWidth + _horizontalMarginPerCell) + _horizontalBoardMargin +
                                         _cellWidth / 2);
                        var cellY = Y + (j * (_cellHeight + _verticalMarginPerCell) + _verticalBoardMargin +
                                         _cellHeight / 2);

                        var shader = SKShader.CreateRadialGradient(new SKPoint(cellX, cellY), glowSize, colors, new[] { 0.0f, 1f }, SKShaderTileMode.Clamp);
                        var glowPaint = new SKPaint()
                        {
                            Shader = shader,
                            BlendMode = SKBlendMode.Luminosity,
                            IsAntialias = true,
                            FilterQuality = SKFilterQuality.High
                        };

                        Canvas.DrawCircle(X + (i * (_cellWidth + _horizontalMarginPerCell) + _horizontalBoardMargin +
                                               _cellWidth / 2),
                            Y + (j * (_cellHeight + _verticalMarginPerCell) + _verticalBoardMargin +
                                 _cellHeight / 2), glowSize, glowPaint);


                        //using (var secondPaint = new SKPaint())
                        //{
                        //    secondPaint.IsAntialias = true;
                        //    secondPaint.Style = SKPaintStyle.Fill;
                        //    secondPaint.Color = CreateColor(255, 255, 255, (byte)(_opacity * 255));
                        //    Canvas.DrawCircle(cellX, cellY, 4, secondPaint);
                        //}

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

        public float ToGemX(int gemIndex, GemBase gem)
        {
            return ToGemViewX(gemIndex) + (_cellWidth - gem.Width) / 2;
        }

        public float ToGemY(int gemIndex, GemBase gem)
        {
            return ToGemViewY(gemIndex) + (_cellWidth - gem.Width) / 2;
        }

        public override void Dispose()
        {
            base.Dispose();
            Gems.Clear();
            CellsList.Clear();
        }
    }
}