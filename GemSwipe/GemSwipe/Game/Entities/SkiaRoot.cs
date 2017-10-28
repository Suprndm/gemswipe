using System.Linq;
using System.Threading.Tasks;
using GemSwipe.Data.PlayerData;
using GemSwipe.Game.Effects;
using GemSwipe.Game.Effects.BackgroundEffects;
using GemSwipe.Game.Gestures;
using GemSwipe.Game.Layers;
using GemSwipe.Game.Navigation;
using GemSwipe.Game.SkiaEngine;
using GemSwipe.Utilities.Sprites;
using Xamarin.Forms;

namespace GemSwipe.Game.Entities
{
    public class SkiaRoot : SkiaView
    {

        private Background _background;

        public static float ScreenHeight { get; private set; }
        public static float ScreenWidth { get; private set; }
        private TextBlock _fpsText;

        public SkiaRoot(float x, float y, float height, float width) : base(x, y, height, width)
        {
            ScreenHeight = height;
            ScreenWidth = width;
        }

        public async void Initialize()
        {
            SetupLayers();
            await LoadResources();


            Gesture.Down += Gesture_Down;
            Gesture.Up += Gesture_Up;
            Gesture.Pan += Gesture_Pan;
        }

        private void Gesture_Pan(Point p)
        {
            ClearTappables();
            SkiaGestureService.Instance.HandlePan(p);
        }

        private void Gesture_Down(Point p)
        {
            ClearTappables();
            SkiaGestureService.Instance.HandleDown(Tappables, p);
        }

        private void Gesture_Up(Point p)
        {
            ClearTappables();
            SkiaGestureService.Instance.HandleUp(Tappables, p);
        }

        private void ClearTappables()
        {
            foreach (var child in Tappables.Where(child => child.ToDispose).ToList())
            {
                Tappables.Remove(child);
            }
        }
  

        public virtual async Task LoadResources()
        {
            SpriteSheet.Instance.Setup("Resources/Graphics/atlas.png", "Resources/Graphics/atlas.txt");
            await SpriteSheet.Instance.LoadAsync();
        }

        public virtual void SetupLayers()
        {
            AddChild(new BackgroundLayer());
            AddChild(new NavigationLayer());
            AddChild(new InterfaceLayer());
            AddChild(new PopupLayer());
            AddChild(new LoadingLayer());

            Navigator.Instance.GoToInitialPage(PageType.Home);

            _fpsText = new TextBlock(Width / 2, Width / 40, "0", Width / 40, CreateColor(255, 255, 255));
            AddChild(_fpsText);
        }


        public void UpdateFps(long fps)
        {
            _fpsText.Text = fps.ToString();
        }

        protected override void Draw()
        {

        }

        public override void Dispose()
        {
        }
    }
}