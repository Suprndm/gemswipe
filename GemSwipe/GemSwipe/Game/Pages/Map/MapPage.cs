using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GemSwipe.Data.Level;
using GemSwipe.Game.Effects.BackgroundEffects;
using GemSwipe.Game.Gestures;
using GemSwipe.Game.Models;
using GemSwipe.Game.Navigation;
using GemSwipe.Game.Navigation.Pages;
using GemSwipe.Game.Settings;
using GemSwipe.Utilities;
using GemSwipe.Utilities.Buttons;
using GemSwipe.Utilities.Sprites;
using SkiaSharp;
using Xamarin.Forms;

namespace GemSwipe.Game.Pages.Map
{
    public class MapPage : PageBase
    {
        private float startY;
        private float lastVY;
        private float _verticalMargin;
        private float _maxHeight;
        private float _minHeight;
        private bool _canPan = true;

        private int _playerProgress;
        private IList<IButton> _listOfLevelButtons;
        private TopBar _topBar;

        public MapPage(SKCanvas canvas, float x, float y, float height, float width, int playerProgress) : base(canvas, x, y, height, width)
        {
            _playerProgress = 20;
            _listOfLevelButtons = new List<IButton>();

            _verticalMargin = height / 10;
            _minHeight = 0;

            _maxHeight = Math.Min(height - _playerProgress * height / 10, 0);
            _maxHeight = -height+_verticalMargin +_playerProgress * height / 10+_verticalMargin;

            //ajouter au skiaroot?
            _topBar = new TopBar(canvas, 0, 0, height, width);
            AddChild(_topBar);

            AddChild(new TextBlock(canvas, width / 2, height -_verticalMargin , "This is the map !", height / 20f,
                new SKColor(255, 255, 255)));


            for (int i = 1; i <= _playerProgress; i++)
            {
                LevelButton levelButton = new LevelButton(canvas, width / 2, height - _verticalMargin - i * height / 10, height / 40f, "Level " + i, i, new SKColor(255, 255, 255));
                _listOfLevelButtons.Add(levelButton);
                AddChild(levelButton);
                DeclareTappable(levelButton);
                int j = i;
                levelButton.Tapped += () => LevelButton_Tapped(j);
                levelButton.Tapped += () => levelButton.ActivateOrbitingStars(Width, Height);
            }
        }



        private void LevelButton_Tapped(int i)
        {
            Navigator.Instance.GoTo(PageType.Game, i);
        }

        protected override void Draw()
        {
        }

        private async Task ScrollMap(Direction direction)
        {
            if (direction == Direction.Top || direction == Direction.Bottom)
            {
                float dir = 0;

                if (direction == Direction.Top)
                {
                    dir = 1;
                }
                else if (direction == Direction.Bottom)
                {
                    dir = -1;
                }
                var scroll = 150;
                var oldY = _y;
                var newY = _y + dir * scroll;
                int animationTimeScale = 200 * 4;

                //foreach (IButton levelButton in _listOfLevelButtons)
                //{
                //    levelButton.ScrollUp();
                //}

                this.Animate("moveY", p => _y = (float)p, oldY, newY, 8, (uint)1000, Easing.SinInOut);
                await Task.Delay(1000);

            }
        }

        private void OnPanning(PanUpdatedEventArgs e)
        {
            Pan(e);
        }

        private async void Pan(PanUpdatedEventArgs e)
        {
            await ScrollMap(e);
        }

        private async Task ScrollMap(PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    startY = _y;
                    break;
                case GestureStatus.Running:
                    {
                        float oldY = _y;
                        _y = startY + (float)e.TotalY;

                        if (_y < _minHeight)
                        {
                            _y = _minHeight;
                        }
                        if (_y > _maxHeight)
                        {
                            _y = _maxHeight;
                        }
                        lastVY = oldY - _y;


                    }
                    break;

                case GestureStatus.Completed:
                    //float stoppingScale = 3;
                    //this.Animate("moveY", p => _y = (float)p, _y, _y + lastVY * stoppingScale, 8, (uint)1000, Easing.SinInOut);
                    break;

            }
        }

        protected override async Task TransitionOut()
        {
            this.Animate("fadeOut", p => _opacity = (float)p, _opacity, 0f, 8, (uint)3000, Easing.CubicIn);
            await Task.Delay(5000);
        }

        protected override void OnActivated(object parameter = null)
        {
            GestureEventHandler.Panning += OnPanning;

            Task.Run(async () =>
            {
                await Task.Delay(1000);
                _topBar.Show();
            });
        }
        protected override void OnDeactivated()
        {
            foreach (IButton levelButton in _listOfLevelButtons)
            {
                levelButton.Tapped -= () => LevelButton_Tapped(_listOfLevelButtons.IndexOf(levelButton));
            }
            _topBar.Hide();
        }
    }
}
