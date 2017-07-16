using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using GemSwipe.Models;
using SkiaSharp;

namespace GemSwipe.GameEngine
{
    public class BoardGLView : GLViewBase
    {
        private const double BoardCellMarginPercentage = 0.10;
        private readonly IBoardState _board;
        private readonly IList<GemGLView> _gemViews;
        private readonly IList<GemGLView> _gemsToRemove;
        private readonly float _horizontalMarginPerCell;
        private readonly float _verticalMarginPerCell;
        private readonly float _cellWidth;
        private readonly float _cellHeight;

        public BoardGLView(IBoardState board, float x, float y, float height, float width) : base(x, y, height, width)
        {
            _board = board;

            // Init
            _horizontalMarginPerCell = (float)(Width * BoardCellMarginPercentage) / (_board.Width + 1);
            _verticalMarginPerCell = (float)(Height * BoardCellMarginPercentage) / (_board.Height + 1);

            _cellWidth = (Width - (_board.Width + 1) * _horizontalMarginPerCell) / _board.Width;
            _cellHeight = (Height - (_board.Height + 1) * _verticalMarginPerCell) / _board.Height;

            _gemsToRemove = new List<GemGLView>();
            _gemViews = new List<GemGLView>();
            foreach (var gemState in board.GetGemStates())
            {
                AddNewCell(gemState);
            }
        }



        private void AddNewCell(IGemState gemState)
        {
            var gemSize = (float)2 / 3 * _cellWidth / 2;

            var gemX = ToGemGLViewX(gemState.X);
            var gemY = ToGemGLViewY(gemState.Y);

            var gemView = new GemGLView(gemState, gemX, gemY, gemSize, gemSize);

            _gemViews.Add(gemView);

            gemView.Disposed += GemView_Disposed;
        }

        private void GemView_Disposed(GemGLView gemView)
        {
            _gemsToRemove.Add(gemView);
            gemView.Disposed -= GemView_Disposed;
        }

        public override void UpdateState()
        {
            foreach (var gemGlView in _gemsToRemove.ToList())
            {
                _gemViews.Remove(gemGlView);
            }

            _gemsToRemove.Clear();

            foreach (var gemGlView in _gemViews)
            {
                gemGlView.UpdateState();
                gemGlView.MoveTo(ToGemGLViewX(gemGlView.GemState.X), ToGemGLViewY(gemGlView.GemState.Y));
            }
        }

        public override void Draw(SKCanvas canvas)
        {
            DrawCells(canvas);

            foreach (var gemGlView in _gemViews.OrderByDescending(gemView=>gemView.IsDying()))
            {
                gemGlView.Draw(canvas);
            }
        }

        private void DrawCells(SKCanvas canvas)
        {
            var cellColor = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Fill,
                Color = SKColor.FromHsl(0, 0, 93)
            };

            for (int i = 0; i < _board.Width; i++)
            {
                for (int j = 0; j < _board.Height; j++)
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

        private float ToGemGLViewX(int gemStateX)
        {
            return X + (gemStateX * (_cellWidth + _horizontalMarginPerCell) + _cellWidth / 2 +
                        _horizontalMarginPerCell);
        }

        private float ToGemGLViewY(int gemStateY)
        {
            return Y + (gemStateY * (_cellWidth + _verticalMarginPerCell) + _cellHeight / 2 + _verticalMarginPerCell);
        }

        public override void MoveTo(float x, float y)
        {
        }

        public override void Dispose()
        {
        }
    }
}