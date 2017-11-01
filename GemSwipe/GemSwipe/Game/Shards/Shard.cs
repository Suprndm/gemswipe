using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Paladin.Core;
using GemSwipe.Paladin.VisualEffects;
using SkiaSharp;
using Xamarin.Forms;

namespace GemSwipe.Game.Shards
{
    public class Shard : SkiaView
    {
        private SKColor _color;
        private IList<FloatingParticule> _particules;
        private SKBlendMode _blendMode;
        private int count = 0;
        private float _radius;
        private bool _dying;

        public Shard(float x, float y, float width, float height) : base(x, y, width, height)
        {
            _color = new SKColor(255, 200, 36);
            _particules = new List<FloatingParticule>();
            var randomizer = new Random();
            for (int i = 0; i < 5; i++)
            {
                var particule = new FloatingParticule(x, y, width / 4, 0.1f, randomizer);
                _particules .Add(particule);
            }

            Initialize();

            DeclareTappable(this);
        }

        public void Initialize()
        {
            this.Animate("shardRadiusIn", p => _radius = (float)p, _radius, Width/2, 4, 500, Easing.CubicOut);

            Task.Factory.StartNew(() =>
            {
                Task.Delay(10000).Wait();
                Die();
            });
        }

        public async void Die()
        {
            if (!_dying)
            {
                _dying = true;
                this.AbortAnimation("shardRadiusIn");
                this.Animate("shardRadiusOut", p => _radius = (float)p, _radius, 0, 4, 500, Easing.CubicOut);
                await Task.Delay(500);
                Dispose();
            }

        }



        protected override void Draw()
        {
            foreach (var particule in _particules)
            {
                particule.Update();
                var colors = new SKColor[] {
                    CreateColor (_color.Red, _color.Green, _color.Blue,_color.Alpha),
                    CreateColor (_color.Red, _color.Green, _color.Blue,0),
                };

                using (var paint = new SKPaint())
                {
                    var shader = SKShader.CreateRadialGradient(new SKPoint(particule.X, particule.Y), Width / 2, colors, new[] { 0.0f, 1f }, SKShaderTileMode.Clamp);
                    paint.BlendMode = SKBlendMode.Plus;
                    paint.Shader = shader;
                    paint.IsAntialias = false;
                    paint.Color = CreateColor(_color.Red, _color.Green, _color.Blue, _color.Alpha);
                    Canvas.DrawCircle(particule.X, particule.Y, _radius, paint);
                }
            }
        }


        public override SKRect GetHitbox()
        {
            return SKRect.Create(X - Width / 2, Y - Height / 2, Width, Height); ;
        }
    }
}
