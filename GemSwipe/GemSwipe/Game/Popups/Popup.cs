using System;
using System.Threading.Tasks;
using GemSwipe.Game.Effects.BackgroundEffects;
using GemSwipe.Game.Entities;
using GemSwipe.Game.SkiaEngine;
using GemSwipe.Utilities.Buttons;
using SkiaSharp;
using Xamarin.Forms;

namespace GemSwipe.Game.Popups
{
    public class Popup: SkiaView
    {
        public event Action BackAction;
        public event Action NextAction;

        public float ContentHeight { get; set; }

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                _titleBlock.Text = value;
            }
        }

        public string ActionName
        {
            get { return _actionName; }
            set
            {
                _actionName = value;
                _rightButton.Text = value;
            }
        }

        protected const float HeaderHeightRatio = 0.07f;
        protected const float FooterHeightRatio = 0.10f;
        protected const float WidthRatio = 0.7f;
        protected const float ButtonWidthRatio = 0.3f;

        private float _popupWidth;
        private float _popupHeight;
        private float _headerHeight;
        private float _footerHeight;
        private float _radius;
        private float _popupX;
        private float _popupY;
        private float _secondButtonX;
        private float _secondButtonWidth;

        private TextBlock _titleBlock;
        private PopupRightButton _rightButton;
        private string _actionName;
        private string _title;


        public Popup(ISkiaView contentView) : base()
        {
            _y = -Height;
            ContentHeight = Height * 0.3f;
       
            _popupWidth = WidthRatio * Width;
            _headerHeight = HeaderHeightRatio * Height;
            _footerHeight = FooterHeightRatio * Height;
            _radius = Height / 25;
            _popupX = (Width - _popupWidth) / 2;
            _popupY = (Height - (_headerHeight + _footerHeight + ContentHeight)) / 2;
            _secondButtonX = _popupX + _popupWidth * ButtonWidthRatio;
            _secondButtonWidth = _popupWidth * (1 - ButtonWidthRatio);
            _popupHeight =  _headerHeight + ContentHeight +  _footerHeight;


            _titleBlock = new TextBlock(_popupX+ _popupWidth/2,_popupY + _headerHeight / 2, Title, _headerHeight/2, new SKColor(255,255,255));
            AddChild(_titleBlock);

            var leftButton = new PopupLeftButton(_popupX, _popupY + _headerHeight + ContentHeight,
                _popupWidth * ButtonWidthRatio, _footerHeight, _popupWidth, _popupHeight, _radius);

            _rightButton = new PopupRightButton( _secondButtonX, _popupY + _headerHeight + ContentHeight, _secondButtonWidth, _footerHeight, _popupWidth, _popupHeight, _radius, ActionName);

            Title = "Popup title";
            ActionName = "Action";

            AddChild(leftButton);
            AddChild(_rightButton);

            if (contentView != null)
            {
                contentView.X = _popupX;
                contentView.Y = _popupY + _headerHeight;

                AddChild(contentView);
            }

            leftButton.Activated += () =>
            {
                HideLeft();
                BackAction?.Invoke();
            };

            _rightButton.Activated += () =>
            {
                HideRight();
                NextAction?.Invoke();
            };

            DeclareTappable(this);
        }

        public override SKRect GetHitbox()
        {
            return SKRect.Create(_popupX, _popupY, _popupWidth, _popupHeight);
        }


        public Task Show()
        {
            this.Animate("slideIn", p => _y = (float)p, _y, 0, 8, (uint)500, Easing.SpringOut);
            return Task.Delay(500);
        }

        public async Task HideLeft()
        {
            this.Animate("slideOutLeft", p => _x = (float)p, _x, -Width, 8, (uint)500, Easing.SpringIn);
            await Task.Delay(500);
            Dispose();
        }


        public async Task HideRight()
        {
            this.Animate("slideOutRight", p => _x = (float)p, _x, Width, 8, (uint)500, Easing.SpringIn);
            await Task.Delay(500);
            Dispose();
        }


        protected override void Draw()
        {
            // Header
            using (var paint = new SKPaint())
            {
                paint.IsAntialias = true;
                paint.Color = CreateColor(107, 117, 230);

                Canvas.DrawRoundRect(SKRect.Create(X + _popupX, Y + _popupY, _popupWidth, _popupHeight), _radius, _radius, paint);
            }

            // Body
            using (var paint = new SKPaint())
            {
                paint.IsAntialias = true;
                paint.Color = CreateColor(172, 178, 241);

                Canvas.DrawRect(SKRect.Create(X + _popupX, Y + _popupY + _headerHeight, _popupWidth, ContentHeight), paint);
            }
        }
    }
}
