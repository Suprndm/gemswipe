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
        private SettingDialog _settingDialog;
        private bool _dialogExpanding;

        private float _aY;
        private bool _isDragged;

        public bool IsShowed { get; set; }
        private int _animationMs = 600;

        public SettingsPanel() : base(0,0,SkiaRoot.ScreenHeight*0.9f, SkiaRoot.ScreenWidth)
        {
            _topMargin = Height / 10;
            _blockMargin = Height / 12;
            _buttonMargin = Height / 16;
            _buttonHeight = Height / 40;

            PanelWidth = Width * 0.8f;
            PanelHeight = Height;
            _x = Width + Width - PanelWidth;

            _dialogWidth = PanelWidth - PanelWidth / 4;

            _settingDialog = new SettingDialog(0+(PanelWidth-_dialogWidth)/2,_topMargin+_buttonMargin+2*_buttonHeight,_dialogWidth, PanelHeight / 3, this);
            AddChild(_settingDialog);

            _buttonContainer = new Container();
            AddChild(_buttonContainer);

            _listOfSettingButtons = new List<SettingButton>();

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

            float buttonY = _topMargin - _blockMargin-_buttonMargin;

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

                    settingButton.Activated += () => SettingButton_Tapped(settingButton);
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



        private void SettingButton_Tapped(SettingButton tappedButton)
        {
            HandleButtonTapped(tappedButton);
            foreach (SettingButton settingButton in _listOfSettingButtons.Where(e => e != tappedButton))
            {
                settingButton.Hide(tappedButton.OriginalY);
            }
        }

        private void HandleButtonTapped(SettingButton settingButton)
        {
            switch (settingButton.SettingOption)
            {
                case SettingsEnum.GetLife:
                    PlayerLifeService.Instance.GainLife();
                    break;
                case SettingsEnum.Language:
                    LanguageButton_Tapped(settingButton);
                    break;
                default:
                    LanguageButton_Tapped(settingButton);
                    break;
            }
        }

       

        private async void LanguageButton_Tapped(SettingButton settingButton)
        {
            _dialogExpanding = true;

            await FocusButton(settingButton);
            _settingDialog.Expand(SettingsEnum.Language);
        }

        private Task FocusButton(SettingButton settingButton)
        {
            settingButton.Animate("focusButton", p => settingButton.Y = (float)p, settingButton.Y, _topMargin, 8, (uint)_animationMs,
                Easing.CubicInOut);
            return Task.Delay(_animationMs);
        }

        private async void SettingsPanel_Tapped()
        {
            if (_dialogExpanding)
            {
                await _settingDialog.Shrink();
                _dialogExpanding = false;
                foreach (SettingButton settingButton in _listOfSettingButtons)
                {
                    settingButton.RecoverPosition();
                }
            }
        }

        public Task Show()
        {
            IsShowed = true;
            this.Animate("slideIn", p => _x = (float)p, _x, Width - PanelWidth, 8, (uint)300, Easing.SpringOut);
            return Task.Delay(_animationMs);
        }

        public Task Hide()
        {
            IsShowed = false;
            this.Animate("slideOut", p => _x = (float)p, _x, Width + Width - PanelWidth, 8, (uint)300, Easing.SpringIn);
            return Task.Delay(_animationMs);
        }

        public void MoveToY(float y)
        {
            if (!_dialogExpanding)
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
                SettingsPanel_Tapped();
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
