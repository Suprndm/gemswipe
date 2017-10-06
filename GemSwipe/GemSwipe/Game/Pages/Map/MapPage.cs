using GemSwipe.Game.Effects.BackgroundEffects;
using GemSwipe.Game.Navigation;
using GemSwipe.Game.Navigation.Pages;
using GemSwipe.Utilities;
using GemSwipe.Utilities.Buttons;
using GemSwipe.Utilities.Sprites;
using SkiaSharp;

namespace GemSwipe.Game.Pages.Map
{
    public class MapPage : PageBase
    {
        private TextBlock _level1Button;
        private TextBlock _level2Button;
        private TextBlock _level3Button;

        public MapPage(SKCanvas canvas, float x, float y, float height, float width) : base(canvas, x, y, height, width)
        {

            AddChild(new TextBlock(canvas, width / 2, height / 4, "This is the map !", height / 20f,
                new SKColor(255, 255, 255)));


            _level1Button = new TextBlock(canvas, width / 2, 5 * height / 10, "Level 1", height / 40f, new SKColor(255, 255, 255));
            AddChild(_level1Button);
            DeclareTappable(_level1Button);

            _level2Button = new TextBlock(canvas, width / 2, 6 * height / 10, "Level 2", height / 40f, new SKColor(255, 255, 255));
            AddChild(_level2Button);
            DeclareTappable(_level2Button);

            _level3Button = new TextBlock(canvas, width / 2, 7 * height / 10, "Level 3", height / 40f, new SKColor(255, 255, 255));
            AddChild(_level3Button);
            DeclareTappable(_level3Button);

            SimpleButton _level4Button = new SimpleButton(canvas, width / 2, 8 * height / 10, width / 5, height / 40f, new SKColor(255, 255, 255));
            AddChild(_level4Button);
            //_level4Button.Tapped += Level4Button_Tapped;
            _level4Button.OnTapped_Action(() => Level4Button_Tapped(5));

            //TextButton _level5Button = new TextButton(canvas, width / 2, 9 * height/10, width / 5, height / 40f, "Level 5",
            //    new SKColor(255, 255, 255));
            //AddChild(_level5Button);
            //_level5Button.OnTapped_Action(() => Level4Button_Tapped(2));

            SpriteButton spriteButton = new SpriteButton(canvas, "bg_day", width / 2, 9 * height / 10, width / 5, height / 40f);
            AddChild(spriteButton);

            Sprite sprite = new Sprite(canvas, "bg_day", width / 4, 9 * height / 10, width / 5, height / 40f);
            AddChild(sprite);
        }

        private int Level4Button_Tapped(int i)
        {
            Navigator.Instance.GoTo(PageType.Game, i);
            return 3;
        }

        private void Level1Button_Tapped()
        {
            Navigator.Instance.GoTo(PageType.Game, 1);
        }

        private void Level2Button_Tapped()
        {
            Navigator.Instance.GoTo(PageType.Game, 2);
        }

        private void Level3Button_Tapped()
        {
            Navigator.Instance.GoTo(PageType.Game, 3);
        }

        protected override void Draw()
        {
        }

        protected override void OnActivated(object parameter = null)
        {
            _level1Button.Tapped += Level1Button_Tapped;
            _level2Button.Tapped += Level2Button_Tapped;
            _level3Button.Tapped += Level3Button_Tapped;
        }

        protected override void OnDeactivated()
        {
            _level1Button.Tapped -= Level1Button_Tapped;
            _level2Button.Tapped -= Level2Button_Tapped;
            _level3Button.Tapped -= Level3Button_Tapped;
        }
    }
}
