using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Game.Models.Entities;
using GemSwipe.Paladin.Core;
using SkiaSharp;
using Xamarin.Forms;

namespace GemSwipe.Game.Events
{
    public abstract class EventBase:SkiaView, IEvent
    {
        protected  SKColor Color { get; set; }
        protected float HaloOpacity { get; set; }

        protected EventBase(float x, float y, float height, float width) : base(x, y, height, width)
        {
            HaloOpacity = 0;
        }

        public virtual async Task<bool> Activate(Board board)
        {
            var effect = new EventActivationEffect(0,0,Height, Width);
            AddChild(effect);
            await effect.Start();
            return true;
        }

        public abstract IList<Point> GetTargets(Board board);

        protected override void Draw()
        {

            var colors = new SKColor[] {
                CreateColor (255, 255, 255,0),
                CreateColor (255, 255,255, (byte)(255*_opacity*HaloOpacity)),
                CreateColor (255, 255, 255,0),
             
            };
            var haloSize = Width/2 * 1.2f;
            var shader = SKShader.CreateRadialGradient(new SKPoint(X , Y), haloSize, colors, new[] { 0.8f, 0.9f,1f }, SKShaderTileMode.Clamp);
            var glowPaint = new SKPaint()
            {
                Shader = shader
            };

            Canvas.DrawCircle(X, Y, haloSize * 1.2f, glowPaint);

            using (var paint = new SKPaint())
            {
                paint.IsAntialias = true;
                paint.Color = CreateColor(Color.Red, Color.Green, Color.Blue, Color.Alpha);
                Canvas.DrawCircle(X, Y, Width/2, paint);
            }
        }

        public async Task Warmup()
        {
            this.Animate("eventHaloOpacity", p => HaloOpacity = (float)p, HaloOpacity, 1f, 4, 500, Easing.Linear);
            await Task.Delay(500);
        }
    }
}
