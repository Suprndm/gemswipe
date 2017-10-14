using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GemSwipe.Data;
using GemSwipe.Data.Level;
using GemSwipe.Game.Entities;
using GemSwipe.Game.Models;
using GemSwipe.Game.Pages.Game.Floors;
using GemSwipe.Game.SkiaEngine;
using SkiaSharp;
using Xamarin.Forms;

namespace GemSwipe.Game.Pages.Game
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
        private float _floorHeight;
        private float _floorMargin;
        private int _floorCount;
        private int _currentFloor;

        public LevelData LevelData;

        public Scene( float x, float y, float height, float width) : base( x, y, height, width)
        {
            _floorCount = 1;
            _currentFloor = 1;
            _floors = new List<Floor>();

            _floorHeight = height;
            _floorMargin = height * 1.5f;

            var startingFloor = new StartingFloor( X, Y, _floorHeight, width, 1);
            _floors.Add(startingFloor);
            AddChild(startingFloor);
            StartingFloor = startingFloor;
        }

        public void Reset()
        {
            _currentFloor = 0;
            foreach (var floor in _floors)
            {
                if (floor is PlayableFloor)
                    floor.Dispose();
            }
        }

        public async Task EndGame()
        {
            _floorCount++;
            var endFloor = new EndFloor( X, -Y + _floorMargin, _floorHeight, Width, LevelData.Id);
            _floors.Add(endFloor);
            AddChild(endFloor);
            await GoToNextFloor();
        }



        public void SetLevelData(LevelData levelData)
        {
            LevelData = levelData;
        }
        private async Task GoToNextFloor()
        {
            var floor = _floors.Last();

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

        //public async Task DisplayBoard(int levelId)
        public async Task NextTransitionFloor(int levelId)
        {
            var floor = new TransitionFloor( X, -Y + _floorMargin, _floorHeight, Width, LevelData);
            AddChild(floor);

            _floors.Add(floor);

            // Recycle Boards
            ClearFloorsStack();

            await GoToNextFloor();
            floor.Tapped += () => { NextBoard(levelId); };
        }

        public async Task EndFloor()
        {
            var floor = new EndFloor( X, -Y + _floorMargin, _floorHeight, Width, LevelData.Id);
            AddChild(floor);

            _floors.Add(floor);

            // Recycle Boards
            ClearFloorsStack();
            await GoToNextFloor();
        }

        private async void NextBoard(int levelId)
        {
            _floorCount++;
            BoardSetup boardSetup = new BoardSetup(LevelData);
            //var floorSetup = new PlayableFloorSetup(SetupBoard, _floorCount - 1, false, levelId.ToString());
            //var floorSetup = new PlayableFloorSetup(boardSetup, _floorCount - 1, false, (_floorCount - 1).ToString());
            //var floor = new PlayableFloor( X, -Y + _floorMargin, _floorHeight, Width, floorSetup);
            var floor = new PlayableFloor( X, -Y + _floorMargin, _floorHeight, Width, LevelData);
            AddChild(floor);

            _floors.Add(floor);

            // Recycle Boards
            ClearFloorsStack();
            await GoToNextFloor();
        }

        public void ClearFloorsStack()
        {
            if (_floors.Count > 3)
            {
                var floorToDispose = _floors[1];
                _floors.RemoveAt(1);
                floorToDispose.Dispose();
            }
        }

        protected override void Draw()
        {
        }

        public async Task ResetBoard()
        {
            CurrentBoard.Reset();
        }
    }
}
