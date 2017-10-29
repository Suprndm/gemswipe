using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GemSwipe.Paladin.Core;
using SkiaSharp;
using Xamarin.Forms;

namespace GemSwipe.Game.Effects.BackgroundEffects
{
    public class OceanDepth:SkiaView
    {
        private IList<SKColor> _colors;
        private int _colorIndex;
        private IList<LinearGradientBackground> _linears;
        public OceanDepth( float x, float y, float height, float width) : base( x, y, height, width)
        {
            _colors = new List<SKColor>
            {
                new SKColor(34, 53, 196, 255),
                new SKColor(18, 13, 83, 255),
                new SKColor(119, 0, 253, 255),
                new SKColor(61, 14, 108, 255),
                new SKColor(0, 154, 192, 255),
                new SKColor(0, 41, 51, 255),
                new SKColor(108, 0, 218, 255),

            };

            _colorIndex = 0;
            _linears = new List<LinearGradientBackground>();
            var linearHeight = Height * 2;
            for (int i = 0; i < 2; i++)
            {
                var linear = new LinearGradientBackground( X, Y + linearHeight * i, linearHeight, Width);
                linear.Reset(_colors[i], _colors[i + 1], Y + linearHeight * i);
                _linears.Add(linear);
                _colorIndex++;
                AddChild(linear);
            }
        }

        public async Task ScrollDown()
        {
            var oldY = _y;
            var newY = _y - Height * 0.15;
            this.Animate("moveY", p => _y = (float)p, oldY, newY, 8, (uint)1000, Easing.SinInOut);
            await Task.Delay(1000);
            foreach (var linear in _linears)
            {
                if (linear.Y < -linear.Height)
                {
                    _colorIndex = _colorIndex % _colors.Count;
                    var color1 = _colors[_colorIndex];
                    _colorIndex++;
                    _colorIndex = _colorIndex % _colors.Count;
                    var color2 = _colors[_colorIndex];

                    // Reset linear
                    linear.Reset(color1, color2, _linears.Max(l => l.Y) - Y + linear.Height);
                }
            }
        }


        protected override void Draw()
        {
        }
    }
}
