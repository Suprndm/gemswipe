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
using GemSwipe.Services.Sound;

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
        private readonly Sprite _spriteStar;
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

            var test = new SKColor(255, 255, 255);

            var skPaint = new SKPaint() { Color = test };
            var skPaint2 = new SKPaint();

            skPaint2.Color = test;
            var test2 = skPaint2.Color.Alpha;

            _floatingParticule = new FloatingParticule(0, 0, radius / 8, 0.02f, _randomizer);
            _spriteHalo = new Sprite(SpriteConst.SmallWhiteHalo, Width / 2, Height / 2, _size * _size, _size * _size, new SKPaint { Color = new SKColor(255, 255, 255) });
            AddChild(_spriteHalo);

            _spriteStar = new Sprite(SizeToStarSpriteFilename(size), Width / 2, Height / 2, _size * _size, _size * _size, new SKPaint { Color = new SKColor(255, 255, 255) });
            AddChild(_spriteStar);
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
            //_floatingParticule = new FloatingParticule(0, 0, 0, 0.02f, _randomizer);
            //_floatingParticule = new FloatingParticule(0, 0, 3 * radius, 0.02f, _randomizer);

            var test = new SKColor(255, 255, 255);

            _spriteHalo = new Sprite(SpriteConst.SmallWhiteHalo, Width / 2, Height / 2, _size * _size, _size * _size, new SKPaint { Color = new SKColor(255, 255, 255) });
            AddChild(_spriteHalo);
            _spriteStar = new Sprite(SizeToStarSpriteFilename(size), Width / 2, Height / 2, _size * _size, _size * _size, new SKPaint { Color = new SKColor(255, 255, 255) });
            AddChild(_spriteStar);

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

        public async override Task Collide(IGem targetGem)
        {
            if (targetGem is Gem)
            {
                if (!_board.CurrentSwipeResult.DeadGems.Contains(this))
                {
                    _board.CurrentSwipeResult.DeadGems.Add(this);
                }
                Gem target = (Gem)targetGem;
                NullifyFloatingBehaviour();
                target.NullifyFloatingBehaviour();

                AudioTrack pinDrop = new AudioTrack(AudioTrackConst.PinDropping);

                target.LevelUp();

                await PerformAction(
                    () => Move(target.IndexX, target.IndexY),
                    () => Die(),
                    () => pinDrop.Play(),
                    () => target.Fuse(),
                    () => target.TightenFloatingBehaviour(_radius / 8)
                    );
            }
            else
            {
                await Task.Delay(0);
            }
        }

        public Task TightenFloatingBehaviour(float radius)
        {
            this.Animate("RecenterParticule", p => _floatingParticule.FloatingRadius = (float)p, _floatingParticule.FloatingRadius, radius, 4, 2 * MovementAnimationMs, Easing.CubicInOut);
            return Task.Delay(MovementAnimationMs);
        }

        public Task LooseFloatingBehaviour(float radius)
        {
            this.Animate("RecenterParticule", p => _floatingParticule.FloatingRadius = (float)p, _floatingParticule.FloatingRadius, _randomizer.Next(3, 6) * radius, 4, 2 * MovementAnimationMs, Easing.CubicInOut);
            return Task.Delay(MovementAnimationMs);
        }

        public Task NullifyFloatingBehaviour()
        {
            this.Animate("RecenterParticule", p => _floatingParticule.FloatingRadius = (float)p, _floatingParticule.FloatingRadius, 0, 4, MovementAnimationMs, Easing.CubicInOut);
            return Task.Delay(MovementAnimationMs);
        }

        public override Task Move(int x, int y)
        {
            if (!_board.CurrentSwipeResult.MovedGems.Contains(this))
            {
                _board.CurrentSwipeResult.MovedGems.Add(this);
            }
            return base.Move(x, y);
        }

        protected virtual Task Shine()
        {
            var effect = new GemPopEffect(_floatingParticule.X, _floatingParticule.Y, Height, Width);
            AddChild(effect);
            return effect.Start();
        }

        public async Task FixedPop()
        {
            Shine();
            await Task.Delay(150);
            this.Animate("opacity", p => _opacity = (float)p, 0, 1, 4, 320, Easing.CubicOut);
        }

        public async Task Pop()
        {
            _isPerformingAction = true;
            await NullifyFloatingBehaviour();
            Shine();
            LooseFloatingBehaviour(_radius);
            await Task.Delay(150);
            this.Animate("opacity", p => _opacity = (float)p, 0, 1, 4, 320, Easing.CubicOut);
            await Task.Delay(800);
            await Task.Delay(3 * MovementAnimationMs);
            TightenFloatingBehaviour(_radius / 8);
            _isPerformingAction = false;
        }

        protected override void Draw()
        {
            _floatingParticule.Update();
            var starX = _floatingParticule.X;
            var starY = _floatingParticule.Y;

            var branches = (int)_fluidSize + 1;
            var reduction = 0.5f;
            var starRadius = (float)(_radius * Math.Min(1, reduction + 0.3));

            var colorReduction = reduction * reduction;
            var starRed = (byte)(255 - (255 - _finalColor.Red) * colorReduction);
            var starGreen = (byte)(255 - (255 - _finalColor.Green) * colorReduction);
            var starBlue = (byte)(255 - (255 - _finalColor.Blue) * colorReduction);
            var startColor = CreateColor(starRed, starGreen, starBlue, (byte)(255 * _opacity));

            _angle += 0.001f / (_size * 0.5f);
            var outerRadius = starRadius * 1.0f;
            var innerRadius = outerRadius * .4f;


            _spriteHalo.X = starX;
            _spriteHalo.Y = starY;
            _spriteHalo.Width = starRadius * 8;
            _spriteHalo.Height = starRadius * 8;


            _spriteStar.X = starX;
            _spriteStar.Y = starY;
            _spriteStar.Width = starRadius * 2;
            _spriteStar.Height = starRadius * 2;

        }

        public Task LevelUp()
        {
            _hasLeveledUp = true;
            if (!_board.CurrentSwipeResult.FusedGems.Contains(this))
            {
                _board.CurrentSwipeResult.FusedGems.Add(this);
            }
            return Task.Delay(0);
        }

        public async Task Fuse()
        {
            var oldSize = _size;
            _size++;
            if (Canvas != null)
            {
                //await Task.Delay(MovementAnimationMs / 2);
                Shine();
                this.Animate("size", p => _fluidSize = (float)p, oldSize, _size, 4, MovementAnimationMs,
                   Easing.CubicOut);
                _spriteStar.UpdateSprite(SizeToStarSpriteFilename(_size));
                //await Task.Delay(MovementAnimationMs / 2);
            }
        }

        public override void Clear()
        {
            _board.Gems.Remove(this);
            base.Clear();
        }

        private string SizeToStarSpriteFilename(int size)
        {
            switch (size)
            {
                case 1:
                    return SpriteConst.Star1;
                case 2:
                    return SpriteConst.Star2;
                case 3:
                    return SpriteConst.Star3;
                case 4:
                    return SpriteConst.Star4;
                case 5:
                    return SpriteConst.Star5;

                default: throw new ArgumentException($"Unknown star size mapping : {size}");
            }
        }
    }
}
