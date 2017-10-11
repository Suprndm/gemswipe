using System.Threading.Tasks;
using GemSwipe.Data.Level;
using GemSwipe.Game.Effects.BackgroundEffects;
using GemSwipe.Game.Gestures;
using GemSwipe.Game.Navigation;
using GemSwipe.Game.Pages.Game;
using GemSwipe.Game.Pages.Home;
using GemSwipe.Game.Pages.Map;
using GemSwipe.Game.Settings;
using GemSwipe.Game.SkiaEngine;
using GemSwipe.Utilities.Sprites;
using SkiaSharp;
using Xamarin.Forms;

namespace GemSwipe.Game.Entities
{
    public class SkiaRoot : SkiaView
    {

        private Background _background;
        private LevelRepository _levelRepository;

        public SkiaRoot( float x, float y, float height, float width) : base( x, y, height, width)
        {
            Initialize();
        }
        public async void Initialize()
        {
            await LoadResources();
            _levelRepository = new LevelRepository();
            SetupNavigation();

            Gesture.Up += Gesture_Up;
        }

        private void Gesture_Up(Point p)
        {
            DetectTap(p);
        }

        public async Task LoadResources()
        {
            SpriteSheet.Instance.Setup("Resources/Graphics/atlas.png", "Resources/Graphics/atlas.txt");
            await SpriteSheet.Instance.LoadAsync();
        }

        public void SetupNavigation()
        {
            var homePage = new HomePage( 0, 0, Height, Width);
            AddChild(homePage);

            _background = new Background( 0, 0, Height, Width);
            AddChild(_background, -1);

            var settingsPanel = new SettingsPanel( 0, 0, Height, Width);
            AddChild(settingsPanel, 10);

            var mapPage = new MapPage( 0, 0, Height, Width,_levelRepository.CountOfLevels());
            AddChild(mapPage);
            var gamePage = new GamePage( 0, 0, Height, Width, _levelRepository);
            AddChild(gamePage);

            Navigator.Instance.SetBackground(_background);
            Navigator.Instance.SetSettingsPanel(settingsPanel);
            Navigator.Instance.RegisterPage(PageType.Home, homePage);
            Navigator.Instance.RegisterPage(PageType.Map, mapPage);
            Navigator.Instance.RegisterPage(PageType.Game, gamePage);
            Navigator.Instance.GoToInitialPage(PageType.Home);
        }

        protected override void Draw()
        {

        }

        public override void Dispose()
        {
        }
    }
}