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

            _horizontalMarginPerCell = (float)(Width * BoardCellMarginPercentage) / (nbOfColumns + 1);
            _verticalMarginPerCell = (float)(Height * BoardCellMarginPercentage) / (nbOfRows + 1);

            _cellWidth = (Width - (nbOfColumns + 1) * _horizontalMarginPerCell) / nbOfColumns;
            _cellHeight = (Height - (nbOfRows + 1) * _verticalMarginPerCell) / nbOfRows;
        }

        public void Populate(IList<Gem> gems)
        {
            foreach (var gem in gems)
            {
                AddGem(gem);
            }
        }

        private void AddGem(Gem gem)
        {
            var gemSize = (float)2 / 3 * _cellWidth / 2;

            var gemX = ToGemViewX(gem.X);
            var gemY = ToGemViewY(gem.Y);

            var gemView = new GemView(gem.Id, Canvas, gemX, gemY, gemSize, gemSize);
            _gemViews.Add(gemView);

            AddChild(gemView);
        }

        /// <summary>
        /// There are three kind of updates
        /// </summary>
        /// <param name="gameUpdate"></param>
        public void Update(GameUpdate gameUpdate)
        {
            foreach (var movedGem in gameUpdate.MovedGems)
            {
                var gemView = GetViewByModel(movedGem);
                gemView.MoveTo(ToGemViewX(movedGem.X), ToGemViewY(movedGem.Y));
            }

            foreach (var deadGem in gameUpdate.DeadGems)
            {
                var gemView = GetViewByModel(deadGem);
                gemView.DieTo(ToGemViewX(deadGem.X), ToGemViewY(deadGem.Y));
            }

            foreach (var fusedGem in gameUpdate.FusedGems)
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

        private void DrawCells(SKCanvas canvas)
        {
            var cellColor = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Fill,
                Color = SKColor.FromHsl(0, 0, 93)
            };

            for (int i = 0; i < _nbOfColumns; i++)
            {
                for (int j = 0; j < _nbOfRows; j++)
                {
                    canvas.DrawRect(
                        SKRect.Create(
                            X + (i * (_cellWidth + _horizontalMarginPerCell) + _horizontalMarginPerCell),
                            Y + (j * (_cellHeight + _verticalMarginPerCell) + _verticalMarginPerCell),
                            _cellWidth,
                            _cellHeight),
                        cellColor);
                }
            }
        }

        private float ToGemViewX(int gemStateX)
        {
            return X + (gemStateX * (_cellWidth + _horizontalMarginPerCell) + _cellWidth / 2 +
                        _horizontalMarginPerCell);
        }

        private float ToGemViewY(int gemStateY)
        {
            return Y + (gemStateY * (_cellWidth + _verticalMarginPerCell) + _cellHeight / 2 + _verticalMarginPerCell);
        }
    }
}