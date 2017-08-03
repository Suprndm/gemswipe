using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GemSwipe.GameEngine.Floors;
using GemSwipe.GameEngine.Menu;
using GemSwipe.GameEngine.SkiaEngine;
using GemSwipe.Models;
using SkiaSharp;
using Xamarin.Forms;

namespace GemSwipe.GameEngine
{
    public class Scene : SkiaView
    {
        private readonly float _initialMarginX;
        private readonly float _initialMarginY;

        public Board CurrentBoard { get; private set; }
        private const int MsPerBoardNavigation = 200;
        private readonly double _boardMargin;
        private readonly IList<Floor> _floors;
        private int _currentFloor = 0;
        private readonly BackgroundView _backgroundView;

        public Scene(GameSetup gameSetup, SKCanvas canvas, float x, float y, float height, float width) : base(canvas, x, y, height, width)
        {
            //_initialMarginX = x;
            //_initialMarginY = y + (height - width) / 2;

            //_x = _initialMarginX;
            //_y = _initialMarginY;

            _floors = new List<Floor>();

            var floorHeight = height * 1.5f;
            var startingFloor = new StartingFloor(canvas, X, Y, floorHeight, width);
            _floors.Add(startingFloor);
            AddChild(startingFloor);

            for (int i = 0; i < 5; i++)
            {
                var boardSetup = gameSetup.BoardSetups[i];
                var board = new Board(boardSetup, canvas, X, Y + floorHeight * (i + 1), floorHeight, width);
                AddChild(board);
                _floors.Add(board);
            }

            var endFloor = new StartingFloor(canvas, X, Y + floorHeight * 6, floorHeight, width);

            _floors.Add(endFloor);
            AddChild(endFloor);

            //var backgroundOverlay = 0.1f;
            //var boardsHeight = (float)((width + _boardMargin) * gridHeight);
            //var boardsWidth = (float)((width + _boardMargin) * gridWidth);
            //_backgroundView = new BackgroundView(canvas, x - boardsHeight * backgroundOverlay, y - boardsWidth * backgroundOverlay, boardsHeight * (1 + backgroundOverlay * 2), boardsWidth * (1 + backgroundOverlay * 2));
            //AddChild(_backgroundView, -1);
        }

        protected override void Draw()
        {
        }

        public async Task NextFloor()
        {
            _currentFloor++;
            var floor = _floors[_currentFloor];

            var oldX = _x;
            var oldY = _y;
            var zoomScaleTarget = 0.5;
            var newX = -(floor.X - X) + _initialMarginX;
            var newY = -(floor.Y - Y) + _initialMarginY;
            int animationTimeScale = MsPerBoardNavigation * 4;

            this.Animate("moveY", p => _y = (float)p, oldY, newY, 8, (uint)1000, Easing.SinInOut);
            await Task.Delay(1000);
            //this.Animate("zoomIn", p => _scale = (float)p, 1, zoomScaleTarget, 4, (uint)animationTimeScale, Easing.SinInOut);
            //Task.Run(async () =>
            //{
            //    await Task.Delay(animationTimeScale);
            //    this.Animate("moveX", p => _x = (float)p, oldX, newX, 4, (uint)animationTimeX, Easing.SinInOut);

            //    await Task.Run(async () =>
            //    {
            //        await Task.Delay(animationTimeX);
            //        this.Animate("moveY", p => _y = (float)p, oldY, newY, 8, (uint)animationTimeY, Easing.SinInOut);

            //        await Task.Run(async () =>
            //        {
            //            await Task.Delay(animationTimeY);
            //            this.Animate("zoomOut", p => _scale = (float)p, zoomScaleTarget, 1, 4, (uint)animationTimeScale, Easing.SinInOut);
            //            await Task.Delay(animationTimeScale);
            //            //AddChild(new ExplosionView(Canvas, targetedBoard.X-X, targetedBoard.Y-Y, Height, Width ));
            //        });
            //    });
            //});

            if(floor is Board)
                CurrentBoard = floor as Board;
        }
    }
}
