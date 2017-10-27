using System.Threading.Tasks;
using GemSwipe.Game.Navigation;
using GemSwipe.Game.Settings;
using GemSwipe.Game.SkiaEngine;

namespace GemSwipe.Game.Layers
{
    public class InterfaceLayer : SkiaView
    {
        private readonly TopBar _topBar;
        private readonly SettingsPanel _settingsPanel;

        public InterfaceLayer()
        {
            _topBar = new TopBar();
            AddChild(_topBar);

            _settingsPanel = new SettingsPanel();
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
