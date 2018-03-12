using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GemSwipe.Game.Models.Entities;
using GemSwipe.Paladin.Core;
using SkiaSharp;
using Xamarin.Forms;

namespace GemSwipe.Game.Effects.BackgroundEffects
{
    public class OceanDepth : SkiaView
    {
        private LinearGradientBackground _linearGradient;
        private IList<WorldColorPack> _colorPacks;

        private byte _rTopAnimated;
        private byte _gTopAnimated;
        private byte _bTopAnimated;


        private byte _rBottomAnimated;
        private byte _gBottomAnimated;
        private byte _bBottomAnimated;


        public OceanDepth(float x, float y, float height, float width) : base(x, y, height, width)
        {
            _colorPacks = new List<WorldColorPack>()
            {
                new WorldColorPack
                {
                    WorldId = 0,
                    TopColor = new SKColor(80, 98, 238, 255),
                    BottomColor = new SKColor(33, 35, 67, 255),
                },
                new WorldColorPack
                {
                    WorldId = 1,
                    TopColor = new SKColor(80, 98, 238, 255),
                    BottomColor = new SKColor(33, 35, 67, 255),
                },

                new WorldColorPack
                {
                    WorldId = 2,
                    TopColor = new SKColor(220, 50, 174, 255),
                    BottomColor = new SKColor(63, 29, 72, 255),
                },

                new WorldColorPack
                {
                    WorldId = 3,
                    TopColor = new SKColor(144, 56, 242, 255),
                    BottomColor = new SKColor(35, 19, 76, 255),
                },
                new WorldColorPack
                {
                    WorldId = 4,
                    TopColor = new SKColor(54, 197, 172, 255),
                    BottomColor = new SKColor(16, 46, 41, 255),
                },
            };

            _linearGradient = new LinearGradientBackground(0, 0, SkiaRoot.ScreenHeight, SkiaRoot.ScreenWidth);
            var defaultColorPack = GetColorPackByWorldId(0);
            _linearGradient.Reset(defaultColorPack.TopColor, defaultColorPack.BottomColor);
            AddChild(_linearGradient);

            _rTopAnimated = defaultColorPack.TopColor.Red;
            _gTopAnimated = defaultColorPack.TopColor.Green;
            _bTopAnimated = defaultColorPack.TopColor.Blue;

            _rBottomAnimated = defaultColorPack.BottomColor.Red;
            _gBottomAnimated = defaultColorPack.BottomColor.Green;
            _bBottomAnimated = defaultColorPack.BottomColor.Blue;
        }

        public async Task ScrollDown()
        {

        }

        private WorldColorPack GetColorPackByWorldId(int worldId)
        {
            return _colorPacks.Single(p => p.WorldId == worldId);
        }

        protected override void Draw()
        {
            var topColor = CreateColor(_rTopAnimated, _gTopAnimated, _bTopAnimated);
            var bottomColor = CreateColor(_rBottomAnimated, _gBottomAnimated, _bBottomAnimated);

            _linearGradient.Reset(topColor, bottomColor);
        }

        public void OnWorldChanged(int worldId)
        {
            var currentTopColor = _linearGradient.GetTopColor();
            var currentBottomColor = _linearGradient.GetBottomColor();
            var colorPack = GetColorPackByWorldId(worldId);

            this.Animate("_rTopAnimated", p => _rTopAnimated = (byte)p, currentTopColor.Red, colorPack.TopColor.Red, 8, (uint)1000, Easing.CubicOut);
            this.Animate("_gTopAnimated", p => _gTopAnimated = (byte)p, currentTopColor.Green, colorPack.TopColor.Green, 8, (uint)1000, Easing.CubicOut);
            this.Animate("_bTopAnimated", p => _bTopAnimated = (byte)p, currentTopColor.Blue, colorPack.TopColor.Blue, 8, (uint)1000, Easing.CubicOut);

            this.Animate("_rBottomAnimated", p => _rBottomAnimated = (byte)p, currentBottomColor.Red, colorPack.BottomColor.Red, 8, (uint)1000, Easing.CubicOut);
            this.Animate("_gBottomAnimated", p => _gBottomAnimated = (byte)p, currentBottomColor.Green, colorPack.BottomColor.Green, 8, (uint)1000, Easing.CubicOut);
            this.Animate("_bBottomAnimated", p => _bBottomAnimated = (byte)p, currentBottomColor.Blue, colorPack.BottomColor.Blue, 8, (uint)1000, Easing.CubicOut);
        }
    }
}
