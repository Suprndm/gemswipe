using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using GemSwipe.Models;
using SkiaSharp;

namespace GemSwipe.GameEngine
{
    public class BoardView : SkiaView
    {
        private const double BoardCellMarginPercentage = 0.10;

        private float _horizontalMarginPerCell;
        private float _verticalMarginPerCell;
        private float _cellWidth;
        private float _cellHeight;
        private readonly IList<GemView> _gemViews;
        private int _nbOfRows;
        private int _nbOfColumns;

        public BoardView(SKCanvas canvas, float x, float y, float height, float width) : base(canvas, x, y, height, width)
        {
            _gemViews = new List<GemView>();
        }

        public void Setup(int nbOfColumns, int nbOfRows)
        {
            _nbOfRows = nbOfRows;
            _nbOfColumns = nbOfColumns;

        }

        public void Populate(IList<Gem> gems)
        {
            UpdateDimensions();

            foreach (var gem in gems)
            {
                AddGem(gem);
            }
        }

        private float GetGemSize()
        {
            return (float)2 / 3 * _cellWidth / 2;
        }

        private void AddGem(Gem gem)
        {
            var gemSize = GetGemSize();

            var gemX = ToGemViewX(gem.X);
            var gemY = ToGemViewY(gem.Y);

            var gemView = new GemView(gem.Id, Canvas, gemX, gemY, gemSize, gemSize);
            _gemViews.Add(gemView);

            AddChild(gemView);
        }

        /// <summary>
        /// There are three kind of updates
        /// </summary>
        /// <param name="swipeResult"></param>
        public void Update(SwipeResult swipeResult)
        {
            foreach (var movedGem in swipeResult.MovedGems)
            {
                var gemView = GetViewByModel(movedGem);
                gemView.MoveTo(ToGemViewX(movedGem.X), ToGemViewY(movedGem.Y));
            }

            foreach (var deadGem in swipeResult.DeadGems)
            {
                var gemView = GetViewByModel(deadGem);
                gemView.ZIndex = -1;
                gemView.DieTo(ToGemViewX(deadGem.X), ToGemViewY(deadGem.Y));
            }

            foreach (var fusedGem in swipeResult.FusedGems)
            {
                var gemView = GetViewByModel(fusedGem);
                gemView.Fuse();
            }
        }

        private GemView GetViewByModel(Gem gem)
        {
            return _gemViews.Single(g => g.Id == gem.Id);
        }

        protected override void Draw()
        {
            DrawCells(Canvas);
        }

        public void UpdateDimensions()
        {
            _horizontalMarginPerCell = (float)(Width * BoardCellMarginPercentage) / (_nbOfColumns + 1);
            _verticalMarginPerCell = (float)(Height * BoardCellMarginPercentage) / (_nbOfRows + 1);

            _cellWidth = (Width - (_nbOfColumns + 1) * _horizontalMarginPerCell) / _nbOfColumns;
            _cellHeight = (Height - (_nbOfRows + 1) * _verticalMarginPerCell) / _nbOfRows;
        }

        private void DrawCells(SKCanvas canvas)
        {

            UpdateDimensions();

                var cellColor = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Fill,
                Color = SKColor.FromHsl(0, 0, 30)
            };

            SKPath path = new SKPath
            {
                FillType = SKPathFillType.EvenOdd
            };


            path.AddRect(SKRect.Create(
                X,
                Y,
                _nbOfColumns * (_cellWidth + _horizontalMarginPerCell),
                _nbOfRows * (_cellHeight + _verticalMarginPerCell)));


            SKPath pathCircles = new SKPath
            {
                FillType = SKPathFillType.EvenOdd
            };

            var gemSize = GetGemSize();
            for (int i = 0; i < _nbOfColumns; i++)
            {
                for (int j = 0; j < _nbOfRows; j++)
                {
                    pathCircles.AddCircle(
                        X + (i * (_cellWidth + _horizontalMarginPerCell) + _horizontalMarginPerCell + _cellWidth / 2),
                        Y + (j * (_cellHeight + _verticalMarginPerCell) + _verticalMarginPerCell + _cellWidth / 2),
                        gemSize);

                    //pathCircles.AddRect(SKRect.Create(
                    //    X + (i * (_cellWidth + _horizontalMarginPerCell) + _horizontalMarginPerCell + _cellWidth),
                    //    Y + (j * (_cellHeight + _verticalMarginPerCell) + _verticalMarginPerCell + _cellWidth / 2),
                    //    _cellWidth-gemSize,
                    //    (_cellWidth - gemSize)/2));


                    //canvas.DrawRect(
                    //    SKRect.Create(
                    //        X + (i * (_cellWidth + _horizontalMarginPerCell) + _horizontalMarginPerCell),
                    //        Y + (j * (_cellHeight + _verticalMarginPerCell) + _verticalMarginPerCell),
                    //        _cellWidth,
                    //        _cellHeight),
                    //    cellColor);
                }
            }

            path.AddPath(pathCircles);

            canvas.DrawPath(path, cellColor);

        }

        private float ToGemViewX(int gemStateX)
        {
            return (gemStateX * (_cellWidth + _horizontalMarginPerCell) + _cellWidth / 2 +
                        _horizontalMarginPerCell);
        }

        private float ToGemViewY(int gemStateY)
        {
            return (gemStateY * (_cellWidth + _verticalMarginPerCell) + _cellHeight / 2 + _verticalMarginPerCell);
        }
    }
}