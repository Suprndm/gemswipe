using System.Threading.Tasks;
using GemSwipe.Data.PlayerLife;
using GemSwipe.Game.Settings;
using GemSwipe.Paladin.Core;
using GemSwipe.Paladin.Navigation;

namespace GemSwipe.Paladin.Layers
{
    public class InterfaceLayer : SkiaView
    {
        private readonly TopBar _topBar;
        private readonly SettingsPanel _settingsPanel;
        private PlayerLifeDisplayer _playerLifeDisplayer;

        public InterfaceLayer()
        {
            _topBar = new TopBar();
            AddChild(_topBar);

            _settingsPanel = new SettingsPanel(Width, 0, 0.8f*Width, 0.9f*Height);
            AddChild(_settingsPanel);

            _topBar.SettingsButtonPressed += () =>
            {
                if (_settingsPanel.IsShowed)
                    _settingsPanel.Hide();
                else
                    _settingsPanel.Show();
            };

            Navigator.NavigationEnded += (arg) =>
            {
                // Delayed show of the topBar when navigated from 
                if (arg.From == PageType.Home)
                {
                    Task.Factory.StartNew(() =>
                    {
                        Task.Delay(1000);
                        _topBar.Show();
                    });
                }
            };
        }

        protected override void Draw()
        {

        }
    }
}
