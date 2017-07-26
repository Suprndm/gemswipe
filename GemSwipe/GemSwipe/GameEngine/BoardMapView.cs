﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Models;
using SkiaSharp;
using Xamarin.Forms;

namespace GemSwipe.GameEngine
{
    public class BoardFactoryView : SkiaView
    {
        private readonly float _initialMarginX;
        private readonly float _initialMarginY;

        public BoardView BoardView { get; private set; }
        private int _lastI;
        private int _lastJ;
        private const int MsPerBoardNavigation = 200;
        private readonly double _boardMargin;
        private readonly BoardView[,] _boards;
        private readonly BackgroundView _backgroundView;
        public BoardFactoryView(BoardSetup boardSetup, SKCanvas canvas, float x, float y, float height, float width) : base(canvas, x, y, height, width)
        {
            _initialMarginX = x;
            _initialMarginY = y + (height - width) / 2;

            _x = _initialMarginX;
            _y = _initialMarginY;

            _lastI = 0;
            _lastJ = 0;
            int gridWidth = 10;
            int gridHeight = 10;
            _boards = new BoardView[gridWidth, gridHeight];
            _boardMargin = width * 0.3;
            for (int i = 0; i < gridWidth; i++)
            {
                for (int j = 0; j < gridHeight; j++)
                {
                    var board = new BoardView(canvas, (float)((width + _boardMargin) * i), (float)((width + _boardMargin) * j), width, width);
                    board.Setup(boardSetup.Columns, boardSetup.Rows);
                    AddChild(board);
                    _boards[i, j] = board;
                }
            }

            var backgroundOverlay = 0.1f;
            var boardsHeight = (float) ((width + _boardMargin) * gridHeight);
            var boardsWidth = (float)((width + _boardMargin) * gridWidth);
            _backgroundView = new BackgroundView(canvas, x- boardsHeight* backgroundOverlay, y - boardsWidth * backgroundOverlay, boardsHeight*(1+ backgroundOverlay*2), boardsWidth * (1 + backgroundOverlay*2));
            AddChild(_backgroundView, -1);
        }

        protected override void Draw()
        {
        }

        public void MoveTo(int i, int j)
        {
            var targetedBoard = _boards[i, j];

            var oldX = _x;
            var oldY = _y;
            var zoomScaleTarget = 0.5;
            var newX = -(targetedBoard.X - X) + _initialMarginX;
            var newY = -(targetedBoard.Y - Y) + _initialMarginY;
            int animationTimeX = MsPerBoardNavigation * Math.Abs(_lastI - i);
            int animationTimeY = MsPerBoardNavigation * Math.Abs(_lastJ - j);
            int animationTimeScale = MsPerBoardNavigation * 4;

            this.Animate("zoomIn", p => _scale = (float)p, 1, zoomScaleTarget, 4, (uint)animationTimeScale, Easing.SinInOut);
            Task.Run(async () =>
            {
                await Task.Delay(animationTimeScale);
                this.Animate("moveX", p => _x = (float)p, oldX, newX, 4, (uint)animationTimeX, Easing.SinInOut);

                await Task.Run(async () =>
                {
                    await Task.Delay(animationTimeX);
                    this.Animate("moveY", p => _y = (float)p, oldY, newY, 8, (uint)animationTimeY, Easing.SinInOut);

                    await Task.Run(async () =>
                    {
                        await Task.Delay(animationTimeY);
                        this.Animate("zoomOut", p => _scale = (float)p, zoomScaleTarget, 1, 4, (uint)animationTimeScale, Easing.SinInOut);
                        await Task.Delay(animationTimeScale);
                        //AddChild(new ExplosionView(Canvas, targetedBoard.X-X, targetedBoard.Y-Y, Height, Width ));
                    });
                });
            });

            BoardView = targetedBoard;

            _lastJ = j;
            _lastI = i;
        }
    }
}