using GemSwipe.Paladin.Core;
using GemSwipe.Paladin.UIElements;
using SkiaSharp;

namespace GemSwipe.Game.Toolbox
{
    public class ToolboxBar : SkiaView
    {
        private ToolButton _button1;
        private ToolButton _button2;
        private ToolButton _button3;

        private TextBlock _tapToUseTextblock;

        public ToolboxBar(float x, float y, float height, float width) : base(x, y, height, width)
        {
            var toolButtonWidth = 2 * height / 4;
            _button1 = new ToolButton(0.25f * width - toolButtonWidth / 2, 0, toolButtonWidth, height);
            _button2 = new ToolButton(0.5f * width - toolButtonWidth / 2, 0, toolButtonWidth, height);
            _button3 = new ToolButton(0.75f * width - toolButtonWidth / 2, 0, toolButtonWidth, height);

            _tapToUseTextblock = new TextBlock(Width / 2, 0, "Tap to use selected item", height * 0.13f, new SKColor(255, 255, 255));

            AddChild(_button1);
            AddChild(_button2);
            AddChild(_button3);

            AddChild(_tapToUseTextblock);

            _tapToUseTextblock.Opacity = 0;

            _button1.Activated += Button1_Activated;
            _button2.Activated += Button2_Activated;
            _button3.Activated += Button3_Activated;
        }

        private void Button1_Activated()
        {
            _button1.Toggle();
            _button2.UnToggle();
            _button3.UnToggle();

            UpdateTextVisibility();
        }


        private void Button2_Activated()
        {
            _button1.UnToggle();
            _button2.Toggle();
            _button3.UnToggle();

            UpdateTextVisibility();
        }

        private void Button3_Activated()
        {
            _button1.UnToggle();
            _button2.UnToggle();
            _button3.Toggle();

            UpdateTextVisibility();
        }

        private void UpdateTextVisibility()
        {
            if (_button3.IsToggled || _button2.IsToggled || _button1.IsToggled)
            {
                _tapToUseTextblock.Opacity = 1;
            }
            else
            {
                _tapToUseTextblock.Opacity = 0;
            }
        }

        protected override void Draw()
        {
        }
    }
}
