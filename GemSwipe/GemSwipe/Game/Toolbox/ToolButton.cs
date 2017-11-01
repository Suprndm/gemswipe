using GemSwipe.Paladin.UIElements;
using GemSwipe.Paladin.UIElements.Buttons;
using SkiaSharp;
using Xamarin.Forms;

namespace GemSwipe.Game.Toolbox
{
    public class ToolButton : SimpleButton
    {
        public bool IsToggled { get; set; }
        private TextBlock _useTextBlock;
        public ToolButton(float x, float y, float width, float height) : base(x, y, width, height)
        {
            _y = height;
            Show();

            _useTextBlock = new TextBlock(width / 2, 0.65f * height, "Use?", 0.15f * height, new SKColor(255, 255, 255));
            AddChild(_useTextBlock);
        }

        public void Show()
        {
            this.Animate("toolButtonShow", p => _y = (float)p, _y, Height * 0.4f, 4, 350, Easing.SpringOut);
        }

        public void Hide()
        {
            this.Animate("toolButtonShow", p => _y = (float)p, _y, Height, 4, 350, Easing.SpringOut);
        }

        public void Toggle()
        {
            if (IsToggled)
            {
                UnToggle();
            }
            else
            {
                this.Animate("toolButtonToggle", p => _y = (float)p, _y, Height * 0.2f, 4, 350, Easing.SpringOut);
                IsToggled = true;
            }

        }

        public void UnToggle()
        {
            this.Animate("toolButtonUnToggle", p => _y = (float)p, _y, Height * 0.4f, 4, 350, Easing.SpringOut);
            IsToggled = false;
        }

        protected override void Draw()
        {
            using (var paint = new SKPaint())
            {
                paint.IsAntialias = true;
                paint.Color = new SKColor(R, G, B);
                Canvas.DrawRoundRect(SKRect.Create(X, Y, Width, Height), Width / 3,
                    Width / 3, paint);
            }

            var toolWidth = Width * .85f;
            var toolHeight = toolWidth;
            using (var paint = new SKPaint())
            {
                paint.IsAntialias = true;
                paint.Color = new SKColor(80, 80, 80);
                Canvas.DrawRoundRect(SKRect.Create(X + (Width - toolWidth) / 2, Y + (Width - toolWidth) / 2, toolWidth, toolHeight), toolWidth / 3,
                    toolWidth / 3, paint);
            }

            using (var paint = new SKPaint())
            {
                paint.IsAntialias = true;
                paint.Color = new SKColor(255, 255, 255);
                Canvas.DrawCircle(X + Width / 2, Y + (Width - toolWidth) / 2 + toolWidth / 2, toolWidth * 0.3f, paint);
            }
        }


        public override SKRect GetHitbox()
        {
            return SKRect.Create(X, Y, Width, Height); ;
        }
    }
}
