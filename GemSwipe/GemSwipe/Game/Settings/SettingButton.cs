using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Paladin.UIElements.Buttons;
using SkiaSharp;
using Xamarin.Forms;

namespace GemSwipe.Game.Settings
{
    public class SettingButton : TextButton
    {
        private int _animationMs = 600;
        private SettingsPanel _settingsPanel;
        public SettingsEnum SettingOption { get; set; }
        public float OriginalY { get; set; }

        public SettingButton(float x, float y, float height, SettingsEnum settingOption, SettingsPanel settingsPanel) : base(x, y, height, string.Empty)
        {
            OriginalY = y;
            Text = SetSettingTitle(settingOption);
            SettingOption = settingOption;
            _settingsPanel = settingsPanel;
        }


        private string SetSettingTitle(SettingsEnum settingName)
        {
            switch (settingName)
            {
                case SettingsEnum.GetLife:
                    return "Get Life";
                    break;
                case SettingsEnum.Language:
                    return "Language";
                    break;
                case SettingsEnum.Sound:
                    return "Sound";
                    break;
                case SettingsEnum.Support:
                    return "Support";
                    break;
                case SettingsEnum.RateUs:
                    return "Rate Us !";
                    break;
                case SettingsEnum.ConnectWithFriends:
                    return "Connect with Friends";
                    break;
                case SettingsEnum.OtherGames:
                    return "Other Games";
                    break;
                case SettingsEnum.ResetAccount:
                    return "Reset";
                    break;
                default:
                    return string.Empty;
            }
        }
        protected override void Draw()
        {
            using (new SKAutoCanvasRestore(Canvas))
            {
                Canvas.ClipRect(SKRect.Create(_settingsPanel.X, _settingsPanel.Y, _settingsPanel.PanelWidth, _settingsPanel.PanelHeight), antialias: true);
                base.Draw();
            }
        }

        public Task Hide(float y)
        {
            if (y > OriginalY)
            {
                this.Animate("slideOut", p => _y = (float)p, _y, _y - _settingsPanel.PanelHeight, 8, (uint)_animationMs,
                    Easing.CubicInOut);
            }
            else
            {
                this.Animate("slideOut", p => _y = (float)p, _y, _y + _settingsPanel.PanelHeight, 8, (uint)_animationMs,
                    Easing.CubicInOut);
            }
            return Task.Delay(_animationMs);
        }


        public Task RecoverPosition()
        {
            this.Animate("returnToPos", p => _y = (float)p, _y, OriginalY, 8, (uint)_animationMs, Easing.CubicInOut);
            return Task.Delay(_animationMs);
        }

        public void ShowSettingDialog()
        {


        }
    }
}
