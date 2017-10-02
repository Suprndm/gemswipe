using System.Threading.Tasks;
using GemSwipe.Game.Effects.BackgroundEffects;
using SkiaSharp;
using Xamarin.Forms;

namespace GemSwipe.Game.Effects.Popped
{
    public class PoppedText : PoppedElement
    {
        private readonly float _size;
        private float _animatedSize;
        private byte _animatedOpacity;
        private readonly TextBlock _textBlock;
        private readonly SKColor _color;


        public PoppedText(SKCanvas canvas, float x, float y, int displayDuration, int appearingDuration, int disappearingDuration, string text, float size, SKColor color) 
            : base(canvas, x, y, size, size, displayDuration, appearingDuration, disappearingDuration)
        {
            _size = size;
            _color = color;
            _animatedSize = 0;
            _animatedOpacity = 0;

            _textBlock = new TextBlock(canvas, 0, 0, text, 0, color);
            AddChild(_textBlock);
        }

        protected override void Draw()
        {
            _textBlock.Color = new SKColor(_color.Red,_color.Green, _color.Blue, _animatedOpacity);
            _textBlock.Size = _animatedSize;
        }

        protected override async Task Appear()
        {
            this.Animate("opacity", p => _animatedOpacity = (byte)p, 0, 255, 4, (uint)AppearingDuration, Easing.CubicOut);
            this.Animate("size", p => _animatedSize = (int)p, 0, _size, 4, (uint)AppearingDuration, Easing.CubicOut);
            await Task.Delay(AppearingDuration);
        }

        protected override async Task Disappear()
        {
            this.Animate("opacity", p => _animatedOpacity = (byte)p, 255, 0, 4, (uint)DisappearingDuration, Easing.CubicOut);
            await Task.Delay(DisappearingDuration);
        }
    }
}
