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

        private const int MsPerBoardNavigation = 200;
        private readonly double _boardMargin;
        private readonly IList<Floor> _floors;
        private readonly Background _background;
        private float _floorrHeight;
        private float _floorMargin;
        private int _floorCount;
        private int _currentFloor;

        public Scene(SKCanvas canvas, float x, float y, float height, float width) : base(canvas, x, y, height, width)
        {
            _floorCount = 1;
            _currentFloor = 1;
            _floors = new List<Floor>();

            _floorrHeight = height;
            _floorMargin = height * 1.5f;

            var startingFloor = new StartingFloor(canvas, X, Y, _floorrHeight, width, 1);
            _floors.Add(startingFloor);
            AddChild(startingFloor);
            StartingFloor = startingFloor;


            _background = new Background(canvas, 0, 0, _floorMargin * 6, width);
            AddChild(_background, -1);
        }


        public void Reset()
        {
            _currentFloor = 0;
            foreach (var floor in _floors)
            {
                if(floor is PlayableFloor)
                    floor.Dispose();
            }
        }

        public async Task EndGame()
        {
            _floorCount++;
            var endFloor = new EndFloor(Canvas, X, -Y + _floorMargin, _floorrHeight, Width);
            _floors.Add(endFloor);
            AddChild(endFloor);
            await NextFloor();
        }

        public async Task NextBoard(BoardSetup boardSetup)
        {
            _floorCount++;
            var floorSetup = new PlayableFloorSetup(boardSetup, _floorCount - 1, false);
            var board = new PlayableFloor(Canvas, X, - Y + _floorMargin, _floorrHeight, Width, floorSetup);
            AddChild(board);

            _floors.Add(board);
            await NextFloor();
        }

        protected override void Draw()
        {

        }

        private async Task NextFloor()
        {
            _currentFloor++;
            var floor = _floors[_currentFloor-1];

            var oldX = _x;
            var oldY = _y;
            var zoomScaleTarget = 0.5;
            var newX = -(floor.X - X) + _initialMarginX;
            var newY = -(floor.Y - Y) + _initialMarginY;
            int animationTimeScale = MsPerBoardNavigation * 4;

            this.Animate("moveY", p => _y = (float)p, oldY, newY, 8, (uint)1000, Easing.SinInOut);
            await Task.Delay(1000);

            if (floor is PlayableFloor)
                CurrentBoard = (floor as PlayableFloor).Board;
        }

        public async Task ResetBoard()
        {
            CurrentBoard.Reset();
        }
    }
}
