using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GemSwipe.Data.LevelData;
using GemSwipe.Data.LevelMapPosition;
using GemSwipe.Data.PlayerData;
using GemSwipe.Game.Navigation;
using GemSwipe.Game.Popups;
using GemSwipe.Game.SkiaEngine;
using GemSwipe.Utilities;
using GemSwipe.Utilities.Buttons;
using SkiaSharp;

namespace GemSwipe.Game.Pages.Map
{
    public class Map : SkiaView
    {
        private float _aY;
        private readonly IList<LevelButton> _levelButtons;
        private IList<SKPoint> _curve;
        private IList<SKPoint> _oldCurve;
        private float _screenHeight;
        private PlayerData _playerData;
        private readonly LevelDataRepository _levelDataRepository;

        private TextButton _nicknameButton;

        private bool _isDragged;
        public Map(float x, float y, float height, float width) : base(x, y, height, width)
        {
            _screenHeight = height;
            _levelDataRepository = new LevelDataRepository();
            _playerData = PlayerDataService.Instance.GetData();
            _nicknameButton = new TextButton(width / 2, height / 2, 100, _playerData.Nickname);

            AddChild(_nicknameButton);
            _levelButtons = new List<LevelButton>();
            _aY = 0;
            DeclareTappable(this);
            Build();

            Up += () => Release();
            Pan += (p) => MoveToY((float)p.Y);
        }


        public void UpdateNickname(string nickname)
        {
            _nicknameButton.Text = nickname;

        }

        public void UpdateLevelStatus()
        {
            foreach (LevelButton levelButton in _levelButtons)
            {
                LevelProgressStatus levelProgress = PlayerDataService.Instance.GetLevelProgress(levelButton.Level);
                levelButton.ProgressStatus = levelProgress;
            }
        }

        private void LevelButton_Tapped(int i)
        {
            var levelData = _levelDataRepository.Get(i);
            var dialogPopup = new LevelDialogPopup(i);
            PopupService.Instance.ShowPopup(dialogPopup);
            dialogPopup.NextCommand = () =>
            {
                Navigator.Instance.GoTo(PageType.Game, i);
            };
        }


        private void Build()
        {
            var levelMapPositionRepository = new LevelMapPositionRepository();
            var levelMapPositions = levelMapPositionRepository.GetAll();

            var higherPointY = levelMapPositions.Max(l => l.Position.Y);
            var newHeight = (float)(Height / 3 + higherPointY / 100 * Height);
            foreach (var levelMapPosition in levelMapPositionRepository.GetAll())
            {
                LevelProgressStatus levelProgress = PlayerDataService.Instance.GetLevelProgress(levelMapPosition.Id);
                var levelButton = new LevelButton(
                    (float)levelMapPosition.Position.X / 100 * Width,
                    newHeight - (float)levelMapPosition.Position.Y / 100 * Height,
                    Height / 40,
                    levelMapPosition.Id, levelProgress);

                AddChild(levelButton);
                DeclareTappable(levelButton);

                levelButton.Activated += () => LevelButton_Tapped(levelMapPosition.Id);
                levelButton.Activated += () => levelButton.ActivateOrbitingStars(Width, Height);

                _levelButtons.Add(levelButton);
            }

            _curve = new List<SKPoint>();
            foreach (var levelButton in _levelButtons)
            {
                _curve.Add(new SKPoint(levelButton.X, levelButton.Y));
            }

            _oldCurve = _curve;
            _curve = SmoothCurve(_curve);
            _curve = SmoothCurve(_curve);
            _curve = SmoothCurve(_curve);
            _curve = SmoothCurve(_curve);
            _curve = SmoothCurve(_curve);

            foreach (var levelButton in _levelButtons)
            {
                var closestPoint = _curve.Select(p => new KeyValuePair<SKPoint, float>(p,
                    MathHelper.Distance(p, new SKPoint(levelButton.X, levelButton.Y)))).OrderBy(p => p.Value).First().Key;

                levelButton.X = closestPoint.X;
                levelButton.Y = closestPoint.Y;
            }


            _y = -newHeight + 2 * _screenHeight / 3;
            Height = newHeight;

          

        }

        private IList<SKPoint> SmoothCurve(IList<SKPoint> curve)
        {

            var newCurve = new List<SKPoint>();
            newCurve.Add(curve.First());

            for (int i = 1; i < _curve.Count - 1; i++)
            {
                var p1 = _curve[i - 1];
                var p2 = _curve[i];
                var p3 = _curve[i + 1];

                var angle = MathHelper.Angle(p1, p2, p3);

                if (Math.Abs(angle) < 7 * Math.PI / 8)
                {
                    var angle1 = MathHelper.Angle(p1, p2);

                    var d1 = MathHelper.Distance(p1, p2) * 0.7f;
                    var newP1 = new SKPoint((float)(p1.X + d1 * Math.Cos(angle1)),
                        (float)(p1.Y + d1 * Math.Sin(angle1)));

                    var angle3 = MathHelper.Angle(p3, p2);
                    var d3 = MathHelper.Distance(p2, p3) * 0.7f;
                    var newP3 = new SKPoint((float)(p3.X + d3 * Math.Cos(angle3)),
                        (float)(p3.Y + d3 * Math.Sin(angle3)));

                    newCurve.Add(newP1);
                    newCurve.Add(newP3);
                }
                else
                {
                    newCurve.Add(p2);
                }
            }

            newCurve.Add(curve.Last());

            return newCurve;
        }


        protected override void Draw()
        {
            UpdateScroll();

            var path = new SKPath();
            for (int i = 0; i < _curve.Count; i++)
            {
                var posWithScroll = new SKPoint(_curve[i].X + _x, _curve[i].Y + _y);

                if (i == 0)
                    path.MoveTo(posWithScroll);
                else
                    path.LineTo(posWithScroll);
            }

            var paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = CreateColor(255, 255, 255, 125),
                StrokeWidth = Width / 50,
                IsAntialias = true
            };

            Canvas.DrawPath(path, paint);


            path = new SKPath();
            for (int i = 0; i < _curve.Count; i++)
            {
                var posWithScroll = new SKPoint(_curve[i].X + _x, _curve[i].Y + _y);

                if (i == 0)
                    path.MoveTo(posWithScroll);
                else
                    path.LineTo(posWithScroll);
            }

            paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = CreateColor(255, 255, 255),
                StrokeWidth = Width / 200,
                IsAntialias = true
            };

            Canvas.DrawPath(path, paint);
        }



        public void MoveToY(float y)
        {
            Y += y;
            _aY = y * 1f;
            _isDragged = true;


            if (_y > 0)
            {
                _y = 0;
            }

            if (_y < -Height + 2 * _screenHeight / 3)
            {
                _y = -Height + 2 * _screenHeight / 3;
            }
        }

        public void Release()
        {
            _isDragged = false;
        }

        private void UpdateScroll()
        {
            if (!_isDragged)
            {
                _y += _aY;

                if (_y > 0)
                {
                    _y = 0;
                    _aY = -_aY * .5f;
                }
                if (_y < -Height + 2 * _screenHeight / 3)
                {
                    _y = -Height + 2 * _screenHeight / 3;
                    _aY = -_aY * .5f;
                }

                _aY = _aY * .9f;
            }

        }
    }
}
