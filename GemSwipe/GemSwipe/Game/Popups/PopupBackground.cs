using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Utilities.Buttons;
using Xamarin.Forms;

namespace GemSwipe.Game.Popups
{
    public class PopupBackground:SimpleButton
    {
        public PopupBackground(float width, float height) : base(width/2, height/2, width, height)
        {
            NormalColor = CreateColor(0, 0, 0, 40);
            DownColor = CreateColor(0, 0, 0, 40);
            ActivatedColor = CreateColor(0, 0, 0, 40);

            Opacity = 0;
        }

        public Task Show()
        {
            this.Animate("show", p => _opacity = (float)p, _opacity, 1, 8, (uint)500, Easing.SinInOut);
            return Task.Delay(500);
        }

        public async Task Hide()
        {
            this.Animate("hide", p => _opacity = (float)p, _opacity,0, 8, (uint)500, Easing.SinInOut);
            await Task.Delay(500);
            Dispose();
        }
    }
}
