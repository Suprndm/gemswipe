using System.Linq;
using System.Threading.Tasks;
using GemSwipe.Data.PlayerData;
using GemSwipe.Game.Effects.BackgroundEffects;
using GemSwipe.Game.Gestures;
using GemSwipe.Game.Layers;
using GemSwipe.Game.SkiaEngine;
using GemSwipe.Utilities.Sprites;
using Xamarin.Forms;

namespace GemSwipe.Game.Entities
{
    public class SkiaRoot : SkiaView
    {

        private Background _background;
        private PlayerData _playerData;

        public SkiaRoot(float x, float y, float height, float width) : base(x, y, height, width)
        {
            Initialize();
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
            AddChild(new BackgroundLayer(Height, Width));
            AddChild(new NavigationLayer(Height, Width));
            AddChild(new InterfaceLayer(Height, Width));
            AddChild(new PopupLayer(Height, Width));
        }

        protected override void Draw()
        {

        }

        public override void Dispose()
        {
        }
    }
}