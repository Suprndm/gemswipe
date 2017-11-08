using GemSwipe.Paladin.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GemSwipe.Game.SettingsBar
{
    public class OptionsContainer : Container
    {
        private int _animationMs = 600;

        public OptionsContainer()
        {
        }

        public OptionsContainer(float x, float y , float width, float height) : base(x, y, width, height)
        {

        }

        public Task MoveToX(float finalX)
        {
            this.Animate("MoveTo", p => _x = (float)p, _x, finalX, 8, (uint)_animationMs,
                      Easing.CubicInOut);
            return Task.Delay(_animationMs);
        }
    }
}
