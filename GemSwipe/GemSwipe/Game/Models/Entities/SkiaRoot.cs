using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GemSwipe.Game.Effects.BackgroundEffects;
using GemSwipe.Game.Sprites;
using GemSwipe.Paladin.Core;
using GemSwipe.Paladin.Gestures;
using GemSwipe.Paladin.Layers;
using GemSwipe.Paladin.Navigation;
using GemSwipe.Paladin.Sprites;
using GemSwipe.Paladin.UIElements;
using Xamarin.Forms;
using GemSwipe.Services;
using GemSwipe.Services.Sound;

namespace GemSwipe.Game.Models.Entities
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
            LoadMusic();
            await LoadResources().ConfigureAwait(false);
            SetupLayers();


            Gesture.Down += Gesture_Down;
            Gesture.Up += Gesture_Up;
            Gesture.Pan += Gesture_Pan;
            Gesture.Swipe += Gesture_Swipe;

            Initialized();
        }

        protected virtual void Initialized()
        {

        }

        private void Gesture_Swipe(Point p, Direction direction)
        {
            ClearTappables();
            SkiaGestureService.Instance.HandleSwipe(p, direction);
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
            await SpriteLoader.Instance.Initialize(
                new List<string>
                {
                    SpriteConst.Shard,
                    SpriteConst.WhiteHalo,
                    SpriteConst.BlackHalo,
                    SpriteConst.Star,
                    SpriteConst.Star1,
                    SpriteConst.Star2,
                    SpriteConst.Star3,
                    SpriteConst.Star4,
                    SpriteConst.Star5,
                    SpriteConst.SmallWhiteDot,
                    SpriteConst.SmallWhiteHalo,
                    SpriteConst.SmallStarStroke,
                    SpriteConst.SmallStarBackFilled,
                    SpriteConst.SmallStarFilled,
                    SpriteConst.BelowTheStars,
                    SpriteConst.BigPlanet,
                    SpriteConst.MediumPlanet,
                    SpriteConst.SmallPlanet,
                    SpriteConst.LevelBase,
                    SpriteConst.Planet1
                },
                "Resources/Graphics",
                ScreenWidth,
                ScreenHeight);

        }

        public void LoadMusic()
        {
            AudioTrack introTrack = new AudioTrack(AudioTrackConst.IntroMusic);
            introTrack.Play();
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


        public virtual void UpdateFps(long fps)
        {
            if (_fpsText != null)
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