using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Paladin.Core;
using SkiaSharp;
using Xamarin.Forms;

namespace GemSwipe.Game.Settings
{
    public class SettingDialog : SkiaView
    {
        private int _animationMs = 600;
        private SettingsPanel _settingsPanel;
        private float _clipHeight;

        public SettingDialog(float x, float y, float width, float height, SettingsPanel settingsPanel) : base(x, y, 0, width)
        {
            _clipHeight = height;
            _settingsPanel = settingsPanel;
        }

        public void Expand(SettingsEnum settingEnum)
        {
            switch (settingEnum)
            {
                case SettingsEnum.Language:
                    Expand_LanguageSettings();
                    break;
                default:
                    Expand_LanguageSettings();
                    break;
            }
        }

        private Task Expand_LanguageSettings()
        {
            return ExpandDialog();
        }

        private Task ExpandDialog()
        {
            this.Animate("slideOut", p => Height = (float)p, Height, _clipHeight, 8, (uint)_animationMs,
                Easing.CubicInOut);
            return Task.Delay(_animationMs);
        }

        public Task Shrink()
        {
            this.Animate("shrink", p => Height = (float)p, Height, 0, 8, (uint)_animationMs,
                Easing.CubicInOut);
            return Task.Delay(_animationMs);
        }

        protected override void Draw()
        {
            DrawHitbox();
            //using (var paint = new SKPaint())
            //{
            //    using (new SKAutoCanvasRestore(Canvas))
            //    {
            //        //Canvas.ClipRect(SKRect.Create(X, Y, Width, _clipHeight), antialias: true);

            //        paint.IsAntialias = true;
            //        paint.Color = CreateColor(168, 0, 240);

            //        Canvas.DrawRect(SKRect.Create(X, Y, Width, Height), paint);
            //    }
            //}
        }
    }
}
