using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GemSwipe.Paladin.Core;
using SkiaSharp;
using GemSwipe.Game.Models.BoardModel;
using GemSwipe.Game.Models.BoardModel.Gems;
using GemSwipe.Paladin.Physics;
using GemSwipe.Services;

namespace GemSwipe.Game.Models.Entities
{
    public class Board : SkiaView
    {
        public Cell[,] Cells { get; private set; }
        public IList<Cell> CellsList { get; private set; }
        public IList<Gem> Gems { get; private set; }
        public IList<TeleportationGem> TeleportationGems { get; private set; }

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
        private SKRect _boardLimits;
        private SKRect _playgroundLimits;

        private float _cellDiameter;


        public Board(BoardSetup boardSetup)
        {
            _boardSetup = boardSetup;

            _boardLimits = SKRect.Create(0.05f * Width, 0.1f * Height, 0.9f * Width, 0.8f * Height);

            _randomizer = new Random();

            NbOfRows = boardSetup.Rows;
            NbOfColumns = boardSetup.Columns;
            Setup(boardSetup);

            DeclareTappable(this);

            PhysicsService.Instance.Hit += Instance_Hit;
            PhysicsService.Instance.HitWall += Instance_HitWall;
        }

        private void Instance_HitWall(SkiaView obj)
        {
        }

        private void Instance_Hit(SkiaView arg1, SkiaView arg2)
        {
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
            Gems = new List<Gem>();
            TeleportationGems = new List<TeleportationGem>();
            var boardString = boardSetup.SetupString;

            var rows = boardString.Split('-');
            var nbOfRows = rows.Length;
            var nbOfColumns = rows[0].Split(' ').Length;
            var boardCells = new List<Cell>();

            var maxCellWidth = _boardLimits.Width / nbOfColumns;
            var maxCellHeight = _boardLimits.Height / nbOfRows;

            _cellDiameter = Math.Min(maxCellHeight, maxCellWidth);
            var topPadding = (_boardLimits.Height - nbOfRows * _cellDiameter) / 2;
            var leftPadding = (_boardLimits.Width - nbOfColumns * _cellDiameter) / 2;

            _playgroundLimits = SKRect.Create(_boardLimits.Left+leftPadding, _boardLimits.Top + topPadding, nbOfColumns * _cellDiameter, nbOfRows * _cellDiameter);
            PhysicsService.Instance.DeclareWall(_playgroundLimits);
            for (int j = 0; j < nbOfRows; j++)
            {
                var rowCells = rows[j].Split(' ');
                for (int i = 0; i < nbOfColumns; i++)
                {
                    Gem gem = CreateGem(_playgroundLimits.Left +  (i+0.5f)*_cellDiameter , _playgroundLimits.Top + (j + 0.5f) * _cellDiameter, rowCells[i]);
                    Gems.Add(gem);
                    if (gem != null)
                    {
                        AddChild(gem);
                    }
                }
            }

            PopGems();
        }

        private Gem CreateGem(float x, float y, string rawData, int size = 0)
        {
            GemType gemType = ParseGemType(rawData);
            var gemRadius = GetGemSize();

            var gemX = x;
            var gemY = y;

            switch (gemType)
            {
                case GemType.None:
                    return null;
                case GemType.Base:
                    bool isSize = int.TryParse(rawData, out size);
                    return new Gem( size, gemX, gemY, gemRadius, _randomizer);
                case GemType.Blocking:
                    return null;
                default:
                    return null;
            }
            
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

            foreach (var gem in Gems)
            {
                switch (direction)
                {
                    case Direction.Left:
                        break;
                    case Direction.Bottom:
                        break;
                    case Direction.Right:
                        break;
                    case Direction.Top:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
                }
            }

            return new SwipeResult();
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
            return (float)2 / 3 * _cellDiameter / 2;
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

            foreach (var teleporterGem in swipeResult.TeleporterGems)
            {
                var gemView = (TeleportationGem)teleporterGem;
                var exitGem = gemView.ExitGem;
                var gemToTeleport = gemView.GemToTeleport;
                gemView.TeleportToThenFrom(ToGemViewX(teleporterGem.BoardX) + (_cellWidth - teleporterGem.Width) / 2, ToGemViewY(teleporterGem.BoardY) + (_cellWidth - teleporterGem.Width) / 2,
                    ToGemViewX(exitGem.BoardX) + (_cellWidth - exitGem.Width) / 2, ToGemViewY(exitGem.BoardY) + (_cellWidth - exitGem.Width) / 2,
                    ToGemViewX(gemToTeleport.BoardX) + (_cellWidth - gemToTeleport.Width) / 2, ToGemViewY(gemToTeleport.BoardY) + (_cellWidth - gemToTeleport.Width) / 2);
                //_gemToTeleport.MoveTo(ToGemViewX(teleporterGem.BoardX) + (_cellWidth - teleporterGem.Width) / 2, ToGemViewY(teleporterGem.BoardY) + (_cellWidth - teleporterGem.Width) / 2);
                //_gemToTeleport.MoveTo(ToGemViewX(ExitGem.BoardX) + (_cellWidth - ExitGem.Width) / 2, ToGemViewY(ExitGem.BoardY) + (_cellWidth - ExitGem.Width) / 2);
                //_gemToTeleport.MoveTo(ToGemViewX(gem.BoardX) + (_cellWidth - gem.Width) / 2, ToGemViewY(gem.BoardY) + (_cellWidth - gem.Width) / 2);

                //teleporterGem.Teleport(ToGemViewX(teleporterGem.BoardX) + (_cellWidth - teleporterGem.Width) / 2, ToGemViewY(teleporterGem.BoardY) + (_cellWidth - teleporterGem.Width) / 2);
            }
        }

        protected override void Draw()
        {
            using (var paint = new SKPaint())
            {
                paint.IsAntialias = true;
                paint.Style = SKPaintStyle.Fill;
                paint.StrokeWidth = 2;
                paint.Color = CreateColor(255, 255, 255, 50);
                Canvas.DrawRect(_boardLimits, paint);
                Canvas.DrawRect(_playgroundLimits, paint);
            }

            //DrawCells(Canvas);
        }

     

        private void DrawCells(SKCanvas canvas)
        {
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

        private float ToGemX(int gemIndex, Gem gem)
        {
            return ToGemViewX(gemIndex) + (_cellWidth - gem.Width) / 2;
        }

        private float ToGemY(int gemIndex, Gem gem)
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