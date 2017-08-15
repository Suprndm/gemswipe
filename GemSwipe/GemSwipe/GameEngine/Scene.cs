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
        public StartingFloor StartingFloor { get; private set; }
        public EndFloor EndFloor { get; private set; }

        private const int MsPerBoardNavigation = 200;
        private readonly double _boardMargin;
        private readonly IList<Floor> _floors;
        private int _currentFloor = 0;
        private readonly Background _background;

        public Scene(GameSetup gameSetup, SKCanvas canvas, float x, float y, float height, float width) : base(canvas, x, y, height, width)
        {
            //_initialMarginX = x;
            //_initialMarginY = y + (height - width) / 2;

            //_x = _initialMarginX;
            //_y = _initialMarginY;

            var floorCount = 6;

            _floors = new List<Floor>();

            var floorHeight = height;
            var floorMargin = height * 1.5f;
            var startingFloor = new StartingFloor(canvas, X, Y, floorHeight, width, 1);
            _floors.Add(startingFloor);
            AddChild(startingFloor);
            StartingFloor = startingFloor;

            for (int i = 0; i < floorCount - 1; i++)
            {
                var boardSetup = gameSetup.BoardSetups[i];

                var floorSetup = new PlayableFloorSetup(boardSetup, i + 1, i == floorCount - 2);

                var board = new PlayableFloor(canvas, X, Y + floorMargin * (i + 1), floorHeight, width, floorSetup);
                AddChild(board);
                _floors.Add(board);
            }

            var endFloor = new EndFloor(canvas, X, Y + floorMargin * floorCount, floorHeight, width);
            EndFloor = endFloor;
            _floors.Add(endFloor);
            AddChild(endFloor);


            _background = new Background(canvas, 0, 0, floorMargin * floorCount, width);
            AddChild(_background, -1);
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

            if (floor is PlayableFloor)
                CurrentBoard = (floor as PlayableFloor).Board;
        }

        public async Task ResetBoard()
        {
            CurrentBoard.Reset();
        }
    }
}
