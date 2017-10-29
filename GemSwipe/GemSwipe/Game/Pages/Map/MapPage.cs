using System.Threading.Tasks;
using GemSwipe.Data.PlayerLife;
using GemSwipe.Paladin.Navigation.Pages;

namespace GemSwipe.Game.Pages.Map
{
    public class MapPage : PageBase
    {
        private Map _map;
        private PlayerLifeDisplayer _playerLifeDisplayer;

        public MapPage()
        {
            _map = new Map(0, 0, Height, Width);
            AddChild(_map);

            _playerLifeDisplayer = new PlayerLifeDisplayer(Width/2- Width / 8, Height- Height / 20, Height, Width/4);
            _map.GetLifeDisplayer(_playerLifeDisplayer);
            AddChild(_playerLifeDisplayer);
        }

        protected override void Draw()
        {
        }

        protected override void OnActivated(object parameter = null)
        {
            // Get latest player informations
            _map.UpdateLevelStatus();

            _playerLifeDisplayer.UpdateLifeCount();
            
            Task.Run(async () =>
            {
                await Task.Delay(1000);
            });
        }
        protected override void OnDeactivated()
        {
        }
    }
}
