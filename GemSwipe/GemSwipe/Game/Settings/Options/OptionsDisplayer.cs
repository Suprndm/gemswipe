using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Paladin.Core;

namespace GemSwipe.Game.Settings.Options
{
    public abstract class OptionsDisplayer : SkiaView
    {
        public SettingsEnum SettingName { get; set; }

        public OptionsDisplayer(float x, float y, float width, float height) : base(x,y,height,width)
        {
            
        }

        public abstract Task Display();

        public abstract Task Hide();

        protected override void Draw()
        {
            
        }
    }
}
