using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Paladin.Core;
using SkiaSharp;
using GemSwipe.Paladin.UIElements.Buttons;

namespace GemSwipe.Game.SettingsBar
{

    public class SettingsBarButton : RoundButton
    {
        private SettingsBar _settingsBar;

        public SettingsBarButton(float x, float y,float radius) : base(x,y,radius)
        {
        }

        //protected override void Draw()
        //{
        //    DrawHitbox();
        //    using (var paint = new SKPaint())
        //    {
        //        paint.IsAntialias = true;
        //        paint.Color = CreateColor(168, 174, 240);

        //        Canvas.DrawRect(SKRect.Create(X, Y, Width, Height), paint);
        //    }
        //}


        public void Expand()
        {

        }

        public void Close()
        {

        }
    }
}
