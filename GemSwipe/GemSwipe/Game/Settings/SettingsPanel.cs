using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GemSwipe.Data.PlayerLife;
using GemSwipe.Game.Models.Entities;
using GemSwipe.Paladin.Containers;
using GemSwipe.Paladin.Core;
using GemSwipe.Paladin.Navigation;
using GemSwipe.Paladin.UIElements.Buttons;
using SkiaSharp;
using Xamarin.Forms;

namespace GemSwipe.Game.Settings
{
    public class SettingsPanel : SkiaView
    {

        public float PanelHeight { get; set; }
        public float PanelWidth { get; set; }
        private float _topMargin;
        private float _blockMargin;
        private float _buttonMargin;
        private float _buttonHeight;
        private float _menuLength;

        private IList<IList<SettingsEnum>> _listOfSettingOptions;
        private IList<SettingButton> _listOfSettingButtons;
        private Container _buttonContainer;

        private float _dialogWidth;
        private float _dialogHeight;
        private SettingDialog _settingDialog;
        private bool _dialogExpanding;

        private float _aY;
        private bool _isDragged;

        public bool IsShowed { get; set; }
        private int _animationMs = 600;

        public SettingsPanel(float x, float y, float width, float height) : base(x,y,height,width)
        {

            PanelWidth = Width;
            PanelHeight = Height;
            //_x = Width + Width - PanelWidth;

            _topMargin = PanelHeight / 10;
            _blockMargin = PanelHeight / 12;
            _buttonMargin = PanelHeight / 16;
            _buttonHeight = PanelHeight / 40;


            _dialogHeight = PanelHeight - PanelHeight / 4;
            _dialogWidth = PanelWidth - PanelWidth / 4;

            _dialogExpanding = false;

            _settingDialog = new SettingDialog(0 + (PanelWidth - _dialogWidth) / 2, _topMargin + _buttonMargin + 2 * _buttonHeight, _dialogWidth, _dialogHeight, this);
            AddChild(_settingDialog);

            _buttonContainer = new Container();
            AddChild(_buttonContainer);

            _listOfSettingOptions = new List<IList<SettingsEnum>>()
            {
                new List<SettingsEnum>()
                {
                    SettingsEnum.GetLife
                },

                new List<SettingsEnum>()
                {
                   SettingsEnum.Language,
                    SettingsEnum.Sound,
                    SettingsEnum.Support,
                },
                new List<SettingsEnum>()
                {
                    SettingsEnum.RateUs,
                    SettingsEnum.ConnectWithFriends,
                    SettingsEnum.OtherGames,
                },

                new List<SettingsEnum>(){
                    SettingsEnum.ResetAccount,
                },
            };

            _listOfSettingButtons = new List<SettingButton>();
            float buttonY = _topMargin - _blockMargin - _buttonMargin;

            foreach (IList<SettingsEnum> blockOfOptions in _listOfSettingOptions)
            {
                buttonY += _blockMargin;

                foreach (SettingsEnum settingOption in blockOfOptions)
                {
                    buttonY += _buttonMargin;

                    SettingButton settingButton = new SettingButton(PanelWidth / 2, buttonY, _buttonHeight, settingOption, this);
                    _listOfSettingButtons.Add(settingButton);
                    _buttonContainer.AddContent(settingButton);

                    buttonY += _buttonHeight;

                    settingButton.Activated += () => SettingButtonOnTapped(settingButton);
                }
            }

            _menuLength = buttonY + _topMargin;

            DeclareTappable(this);

            Pan += (p) => MoveToY((float)p.Y);
            Up += Release;

            _opacity = 1;

            //recoverButton.Activated += () =>
            //{
            //    PlayerLifeService.Instance.GainLife();
            //};

            //backButton.Activated += () =>
            //{
            //    Navigator.Instance.GoTo(PageType.Map);
            //    Hide();
            //};

        }

        private void SettingsPanelOnTapped()
        {
                _settingDialog.Shrink();
                foreach (SettingButton settingButton in _listOfSettingButtons)
                {
                    settingButton.RecoverPosition();
                }
        }

        private void SettingButtonOnTapped(SettingButton tappedButton)
        {
            ActivateDialog(tappedButton);
            foreach (SettingButton settingButton in _listOfSettingButtons.Where(e => e != tappedButton))
            {
                settingButton.Hide(tappedButton.OriginalY);
            }
        }

        private async void ActivateDialog(SettingButton settingButton)
        {
            await FocusButton(settingButton);
            _settingDialog.Expand(settingButton.SettingName);
        }

        private Task FocusButton(SettingButton settingButton)
        {
            return settingButton.Focus(_topMargin + _y);
            //settingButton.Animate("FocusButton", p => settingButton.Y = (float)p, settingButton.Y, _topMargin, 8, (uint)_animationMs,
            //    Easing.CubicInOut);
            //return Task.Delay(_animationMs);
        }

        public Task Show()
        {
            IsShowed = true;
            this.Animate("slideIn", p => _x = (float)p, _x, SkiaRoot.ScreenWidth-PanelWidth, 8, (uint)300, Easing.SpringOut);
            return Task.Delay(_animationMs);
        }

        public Task Hide()
        {
            IsShowed = false;
            this.Animate("slideOut", p => _x = (float)p, _x, SkiaRoot.ScreenWidth, 8, (uint)300, Easing.SpringIn);
            return Task.Delay(_animationMs);
        }

        public void MoveToY(float y)
        {
            if (!_settingDialog.IsActive)
            {
                _buttonContainer.Y += y;
                _aY = y * 1f;
                _isDragged = true;

                if (_buttonContainer.Y > 0)
                {
                    _buttonContainer.Y = 0;
                }

                if (_buttonContainer.Y < PanelHeight - _menuLength)
                {
                    _buttonContainer.Y = PanelHeight - _menuLength;
                }
            }
        }

        public void Release()
        {
            if (_isDragged)
            {
                _isDragged = false;
            }
            else
            {
                SettingsPanelOnTapped();
            }
        }

        private void UpdateScroll()
        {
            if (!_isDragged)
            {
                _buttonContainer.Y += _aY;

                if (_buttonContainer.Y > 0)
                {
                    _buttonContainer.Y = 0;
                    _aY = -_aY * .5f;
                }
                if (_buttonContainer.Y < PanelHeight - _menuLength)
                {
                    _buttonContainer.Y = PanelHeight - _menuLength;
                    _aY = -_aY * .5f;
                }

                _aY = _aY * .5f;
            }
        }

        protected override void Draw()
        {
            DrawHitbox();
            UpdateScroll();
            using (var paint = new SKPaint())
            {
                paint.IsAntialias = true;
                paint.Color = CreateColor(168, 174, 240);

                Canvas.DrawRect(SKRect.Create(X, Y, PanelWidth, PanelHeight), paint);
            }
        }
    }
}
