using System.Threading.Tasks;
using GemSwipe.Data.PlayerLife;
using GemSwipe.Game.Settings;
using GemSwipe.Paladin.Core;
using GemSwipe.Paladin.Navigation;
using GemSwipe.Game.SettingsBar;

namespace GemSwipe.Paladin.Layers
{
    public class InterfaceLayer : SkiaView
    {
        private readonly SettingsBar _settingsBar;
        private PlayerLifeDisplayer _playerLifeDisplayer;

        public InterfaceLayer()
        {
            var settingsBarWidth = Width / 8;
            var settingsBarHeight = Height / 3;
            var settingsTopMargin = Height / 10;

            _settingsBar = new SettingsBar(Width - settingsBarWidth, settingsTopMargin, settingsBarWidth, settingsBarHeight);
            AddChild(_settingsBar);

            Navigator.NavigationEnded += (arg) =>
            {
                // Delayed show of the topBar when navigated from 
                if (arg.From == PageType.Home)
                {
                    Task.Factory.StartNew(() =>
                    {
                        Task.Delay(1000);
                        _settingsBar.Show();
                    });
                }
            };

            Navigator.NavigationStarted += (arg) =>
            {
                _settingsBar.Close();
            };

            Navigator.NavigationEnded += (arg) =>
            {
                if (arg.To == PageType.Game)
                {
                    _settingsBar.SetInGameConfig();
                    _settingsBar.Show();
                }
                else if (arg.To == PageType.Home)
                {
                    _settingsBar.Hide();
                }
                else if (arg.To == PageType.Map)
                {
                    _settingsBar.SetDefaultConfig();
                    _settingsBar.Show();
                }
            };
        }

        protected override void Draw()
        {

        }
    }
}
