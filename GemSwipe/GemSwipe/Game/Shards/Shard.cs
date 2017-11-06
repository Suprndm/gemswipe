using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Game.Sprites;
using GemSwipe.Paladin.Core;
using GemSwipe.Paladin.Sprites;
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
        private Sprite _sprite;
        private IList<Sprite> _sprites;

        public Shard(float x, float y, float width, float height) : base(x, y, width, height)
        {
            _color = new SKColor(255, 200, 36);
            _particules = new List<FloatingParticule>();
            var randomizer = new Random();
            _sprites = new List<Sprite>();
            for (int i = 0; i < 5; i++)
            {
                var sprite = new Sprite(SpriteConst.Shard, 0, 0, 128, 128, new SKPaint { Color = CreateColor(255, 255, 255), BlendMode = SKBlendMode.Plus });
                AddChild(sprite);

                _sprites.Add(sprite);
                var particule = new FloatingParticule(0, 0, width / 4, 0.1f, randomizer);
                _particules.Add(particule);
            }

            Initialize();

            DeclareTappable(this);
        }

        public void Initialize()
        {
            this.Animate("shardRadiusIn", p => _radius = (float)p, _radius, Width / 2, 4, 500, Easing.CubicOut);

            Task.Factory.StartNew(async () =>
            {
                await Task.Delay(10000);
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
            for (int i = 0; i < _particules.Count; i++)
            {
                var particule = _particules[i];
                particule.Update();

                var sprite = _sprites[i];

                sprite.X = particule.X;
                sprite.Y = particule.Y;

                sprite.Height = _radius * 2;
                sprite.Width = _radius * 2;
            }
        }


        public override SKRect GetHitbox()
        {
            return SKRect.Create(X - Width / 2, Y - Height / 2, Width, Height); ;
        }
    }
}
