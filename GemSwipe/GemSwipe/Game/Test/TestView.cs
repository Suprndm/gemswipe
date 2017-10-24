using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Data.LevelMapPosition;
using GemSwipe.Game.Effects.BackgroundEffects;
using GemSwipe.Game.Entities;
using GemSwipe.Game.Gestures;
using GemSwipe.Game.SkiaEngine;
using GemSwipe.Game.Test;
using GemSwipe.Utilities.Buttons;
using Newtonsoft.Json;
using SkiaSharp;
using Xamarin.Forms;

namespace GemSwipe.Game
{
    public class TestView : SkiaRoot
    {
        private float _angle = 0;
        private TextBlock _fpsText;
        public TestView(float x, float y, float height, float width) : base(x, y, height, width)
        {
            _fpsText = new TextBlock(width / 2, width / 40, "0", width / 40, CreateColor(255, 255, 255));
            AddChild(_fpsText);

            AddChild(new PopupTester( Height, Width));
        }


        public override void SetupLayers()
        {
            // do nothing
        }

        public override Task LoadResources()
        {
            // do nothing
            return Task.Delay(0);
        }


        public void UpdateFps(long fps)
        {
            _fpsText.Text = fps.ToString();
        }

        protected override void Draw()
        {

        }


    }
}
