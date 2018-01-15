using System;
using System.Threading.Tasks;
using GemSwipe.Game.Effects;
using GemSwipe.Paladin.Core;
using GemSwipe.Paladin.VisualEffects;
using SkiaSharp;
using Xamarin.Forms;
using GemSwipe.Game.Models.BoardModel;
using GemSwipe.Game.Models.BoardModel.Gems;
using GemSwipe.Game.Shards;
using System.Collections.Generic;
using System.Linq;
using GemSwipe.Game.Sprites;
using GemSwipe.Paladin.Sprites;
using GemSwipe.Services;

namespace GemSwipe.Game.Models.Entities
{

    public class Gem : GemBase
    {
        public GemType Type { get; set; }


        public int Size
        {
            get
            {
                return _size;
            }
            private set
            {
                _size = value;
            }
        }
        public int TargetBoardX { get; private set; }
        public int TargetBoardY { get; private set; }
        private bool _hasLeveledUp = false;
        public bool HasLeveledUp
        {
            get
            {
                return _hasLeveledUp;
            }
        }
        private float _fluidX;
        private float _fluidY;
        private float _fluidSize;
        private int _size;
        private float _radius;
        public float Radius
        {
            get
            {
                return _radius;
            }
            set
            {
                _radius = value;
            }
        }

        protected const int MovementAnimationMs = 600;
        private Random _randomizer;
        private readonly Sprite _spriteHalo;
        private float _angle;
        private FloatingParticule _floatingParticule;
        public FloatingParticule FloatingParticule
        {
            get
            {
                return _floatingParticule;
            }
        }

        private SKColor _finalColor;

        public Gem(int indexX, int indexY, int size, float x, float y, float radius, Random randomizer) : base(indexX, indexY, size, x, y, radius, randomizer, null)
        {
            Type = GemType.Base;
            _randomizer = randomizer;
            Size = size;
            _fluidSize = size;
            IndexX = indexX;
            IndexY = indexY;
            _radius = radius;
            _size = size;
            _fluidX = _x;
            _fluidY = _y;
            _opacity = 0;
            _angle = (float)(_randomizer.Next(100) * Math.PI * 2 / 100);
            _finalColor = new SKColor(195, 184, 85);

            _floatingParticule = new FloatingParticule(0, 0, radius / 8, 0.02f, _randomizer);

            _spriteHalo = new Sprite(SpriteConst.SmallWhiteHalo, Width / 2, Height / 2, _size * _size, _size * _size, new SKPaint { Color = new SKColor(255, 255, 255) });
            AddChild(_spriteHalo);
        }

        public Gem(int indexX, int indexY, int size, float x, float y, float radius, Random randomizer, Board board) : base(indexX, indexY, size, x, y, radius, randomizer, board)
        {
            Type = GemType.Base;
            _randomizer = randomizer;
            Size = size;
            _fluidSize = size;
            IndexX = indexX;
            IndexY = indexY;
            _radius = radius;
            _size = size;
            _fluidX = _x;
            _fluidY = _y;
            _opacity = 0;
            _angle = (float)(_randomizer.Next(100) * Math.PI * 2 / 100);
            _finalColor = new SKColor(195, 184, 85);

            _floatingParticule = new FloatingParticule(0, 0, radius / 8, 0.02f, _randomizer);

            _spriteHalo = new Sprite(SpriteConst.SmallWhiteHalo, Width / 2, Height / 2, _size * _size, _size * _size, new SKPaint { Color = new SKColor(255, 255, 255) });
            AddChild(_spriteHalo);

        }

        public override void Reactivate()
        {
            _hasLeveledUp = false;
            base.Reactivate();
        }

        public override bool CanCollide(IGem targetGem)
        {
            if (targetGem is Gem)
            {
                Gem target = (Gem)targetGem;
                return target.Size == Size && !target.HasLeveledUp;
            }
            else
            {
                return false;
            }
        }

        public override Task Collide(IGem targetGem)
        {
            if (targetGem is Gem)
            {
                Gem target = (Gem)targetGem;
                PerformAction(() => Move(target.IndexX, target.IndexY), () => Die());
                return target.LevelUp();
            }
            else
            {
                return Task.Delay(0);
            }
        }

        protected virtual void Shine()
        {
            var effect = new GemPopEffect(_floatingParticule.X, _floatingParticule.Y, Height, Width);
            AddChild(effect);
            effect.Start();
        }

        public async Task Pop()
        {
            Shine();
            await Task.Delay(150);
            this.Animate("opacity", p => _opacity = (float)p, 0, 1, 4, 320, Easing.CubicOut);
        }

        protected override void Draw()
        {
            _floatingParticule.Update();
            var starX = X + _floatingParticule.X;
            var starY = Y + _floatingParticule.Y;

            var branches = (int)_fluidSize + 1;
            var reduction = 1f / (8f / Math.Min(8, branches));
            var starRadius = (float)(_radius * Math.Min(1, reduction + 0.3));

            var colorReduction = reduction * reduction;
            var starRed = (byte)(255 - (255 - _finalColor.Red) * colorReduction);
            var starGreen = (byte)(255 - (255 - _finalColor.Green) * colorReduction);
            var starBlue = (byte)(255 - (255 - _finalColor.Blue) * colorReduction);
            var startColor = CreateColor(starRed, starGreen, starBlue, (byte)(255 * _opacity));

            _angle += 0.001f / (_size * 0.5f);
            var outerRadius = starRadius * 1.0f;
            var innerRadius = outerRadius * .4f;


            _spriteHalo.X = 0;
            _spriteHalo.Y = 0;
            _spriteHalo.Width = starRadius * 8;
            _spriteHalo.Height = starRadius * 8;

            // Shadowed


            float reductionCoef = 0.95f;


            var points = Polygonal.GetStarPolygon(innerRadius * reductionCoef, outerRadius * reductionCoef, branches,
                2 * (float)(0 + Math.PI / 2 * 1 / branches));

            var path = new SKPath();

            for (int k = 0; k < points.Count; k++)
            {
                var point = points[k];
                var translatedPoint = new SKPoint(point.X + starX, point.Y + starY);
                if (k == 0)
                    path.MoveTo(translatedPoint);
                else
                    path.LineTo(translatedPoint);
            }

            path.Close();

            var paint = new SKPaint
            {
                Style = SKPaintStyle.StrokeAndFill,
                Color = CreateColor(175, 175, 175, (byte)(255 * _opacity)),
                StrokeWidth = 2,
                IsAntialias = false
            };

            Canvas.DrawPath(path, paint);

            points = Polygonal.GetStarPolygon(innerRadius, outerRadius, branches, 0);
            path = new SKPath();
            for (int k = 0; k < points.Count; k++)
            {
                var point = points[k];
                var translatedPoint = new SKPoint(point.X + starX, point.Y + starY);
                if (k == 0)
                    path.MoveTo(translatedPoint);
                else
                    path.LineTo(translatedPoint);
            }

            path.Close();

            paint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = CreateColor(255, 255, 255, (byte)(255 * _opacity)),
                IsAntialias = true
            };

            Canvas.DrawPath(path, paint);

            paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = startColor,
                StrokeWidth = 10 * _radius / 100,
                IsAntialias = true
            };

            Canvas.DrawPath(path, paint);
        }

        public Task LevelUp()
        {
            _hasLeveledUp = true;
            return Fuse();
        }
        public async Task Fuse()
        {
            var oldSize = _size;
            _size++;
            if (Canvas != null)
            {
                await Task.Delay(MovementAnimationMs / 2);
                Shine();
                this.Animate("size", p => _fluidSize = (float)p, oldSize, _size, 4, MovementAnimationMs,
                   Easing.CubicOut);
                await Task.Delay(MovementAnimationMs / 2);
            }
        }
    }
}
