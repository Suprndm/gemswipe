using System.Threading.Tasks;
using GemSwipe.Data.PlayerLife;
using GemSwipe.Paladin.Navigation.Pages;
using GemSwipe.Paladin.Navigation;
using GemSwipe.Paladin.UIElements;
using GemSwipe.Paladin.Core;
using GemSwipe.Game.Pages.Map.Worlds;
using System.Collections.Generic;
using GemSwipe.Data.PlayerData;

namespace GemSwipe.Game.Pages.Map
{
    public class MapPage : PageBase
    {
        SlidingCollection<IWorld> _worldCollection;

        public MapPage()
        {
            Type = PageType.Map;

            var unlockedLevelId = 11;
            PlayerDataService.Instance.SetMaxLevelReached(unlockedLevelId);
            PlayerDataService.Instance.SaveChanges();


            _worldCollection = new SlidingCollection<IWorld>(0, 0, Width, Height, new List<IWorld>
                {
                new FirstWorld(),
                new SecondWorld(),
                new ThirdWorld(),
                new FourthWorld(),
                });

            _worldCollection.OnNext += _worldCollection_OnNext;
            AddChild(_worldCollection);
        }

        private void _worldCollection_OnNext(IWorld world)
        {
            Navigator.Instance.ChangeWorld(world.Id);
        }

        protected override void Draw()
        {
        }

        protected override void OnActivated(object parameter = null)
        {
            int worldId = 1;

            if (parameter != null)
                worldId = (int)parameter;

            Navigator.Instance.ChangeWorld(1);

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
