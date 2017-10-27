using System.Threading.Tasks;
using GemSwipe.Data.PlayerData;
using GemSwipe.Game.Navigation.Pages;

namespace GemSwipe.Game.Pages.Map
{
    public class MapPage : PageBase
    {
        private Map _map;

        public MapPage()
        {
            _map = new Map(0, 0, Height, Width);

            AddChild(_map);
        }

        protected override void Draw()
        {
        }

        protected override void OnActivated(object parameter = null)
        {
            // Get latest player informations
            PlayerData playerData = PlayerDataService.Instance.GetData();
            _map.UpdateNickname(playerData.Nickname);
            _map.UpdateLevelStatus();
            
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
