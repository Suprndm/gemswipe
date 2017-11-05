using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Game.Settings.Options;
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
        private OptionsDisplayer _currentOptionsDisplayer;
        public bool IsExpanding { get; set; }
        public bool IsShrinking { get; set; }
        public bool IsActive { get; set; }

        private IList<OptionsDisplayer> _listOfOptionsDisplayers;

        public SettingDialog(float x, float y, float width, float height, SettingsPanel settingsPanel) : base(x, y, 0, width)
        {
            _clipHeight = height;
            _settingsPanel = settingsPanel;
            _listOfOptionsDisplayers = new List<OptionsDisplayer>();

            _listOfOptionsDisplayers.Add(new LanguageOptionsDisplayer(0, 0, width, height));
            //_listOfOptions.Add(new SoundOptionsDisplayer(0,0,width, height));

            foreach (OptionsDisplayer optionsDisplayer in _listOfOptionsDisplayers)
            {
                AddChild(optionsDisplayer);
            }

        }

        public Task Expand(SettingsEnum settingEnum)
        {
            if (!IsShrinking)
            {
                IsActive = true;
                this.AbortAnimation("Shrink");
                IsExpanding = true;
                this.Animate("Expand", p => Height = (float) p, Height, _clipHeight, 8, (uint) _animationMs,
                    Easing.CubicInOut, (a, b) => IsExpanding = false);

                _currentOptionsDisplayer = _listOfOptionsDisplayers.FirstOrDefault(p => p.SettingName == settingEnum);
                _currentOptionsDisplayer?.Display();
                //return ExpandDialogBox();

            }
            return Task.Delay(_animationMs);

        }

        private Task ExpandDialogBox()
        {
            this.Animate("Expand", p => Height = (float)p, Height, _clipHeight, 8, (uint)_animationMs,
                Easing.CubicInOut);
            return Task.Delay(_animationMs);
        }

        public Task Shrink()
        {
            this.AbortAnimation("Expand");
            IsShrinking = true;
            this.Animate("Shrink", p => Height = (float)p, Height, 0, 8, (uint)_animationMs,
                Easing.CubicInOut, (a,b)=> Deactivate());

            _currentOptionsDisplayer?.Hide();
            _currentOptionsDisplayer = null;

            return Task.Delay(_animationMs);
        }

        private void Deactivate()
        {
            IsActive = false;
            IsShrinking = false;
        }

        protected override void Draw()
        {
            DrawHitbox();
        }
    }
}
