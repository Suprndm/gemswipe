using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Paladin.Core;
using SkiaSharp;
using Xamarin.Forms;

namespace GemSwipe.Data.PlayerLife
{
    public class PlayerLifeDisplayer : SkiaView
    {
        private int _maxNumberOfLives = 3;
        private int _numberOfLives;

        private float _targetX;
        private float _targetY;
        private bool _beingUsed;
        private float _beingUsedLifeX;
        private float _beingUsedLifeY;

        public PlayerLifeDisplayer(float x, float y, float height, float width) : base(x, y, height, width)
        {
        }

        public void UpdateLifeCount()
        {
            _numberOfLives = PlayerLifeService.Instance.GetLifeCount();
        }

        public void SetTarget(float x, float y)
        {
            _targetX = x;
            _targetY = y;
        }

        private float DistinguishLastLifeCoordX(int i)
        {
            if (i < _numberOfLives)
            {
                return X + i * Width / (_maxNumberOfLives + 1);
            }
            else
            {
                if (_beingUsed)
                {
                    return _beingUsedLifeX;
                }
                else
                {
                    return X + i * Width / (_maxNumberOfLives + 1);
                }
            }
        }

        private float DistinguishLastLifeCoordY(int i)
        {
            if (i < _numberOfLives)
            {
                return Y;
            }
            else
            {
                if (_beingUsed)
                {
                    return _beingUsedLifeY;
                }
                else
                {
                    return Y;
                }
            }
        }

        protected override void Draw()
        {
            using (var paint = new SKPaint())
            {

                for (int i = 1; i <= _numberOfLives; i++)
                {
                    paint.IsAntialias = true;
                    paint.Color = CreateColor(CreateColor(241, 142, 142));
                    Canvas.DrawCircle(DistinguishLastLifeCoordX(i), DistinguishLastLifeCoordY(i), Width/10, paint);
                }
            }
        }

        public Task SteerToTarget()
        {
            _beingUsedLifeX = DistinguishLastLifeCoordX(_numberOfLives);
            _beingUsedLifeY = DistinguishLastLifeCoordY(_numberOfLives);
            _beingUsed = true;
            this.Animate("moveX", p => _beingUsedLifeX = (float)p, DistinguishLastLifeCoordX(_numberOfLives), _targetX, 8, (uint)1000, Easing.SinInOut,(p,q)=>_beingUsed=false);
            this.Animate("moveY", p => _beingUsedLifeY = (float)p, DistinguishLastLifeCoordY(_numberOfLives), _targetY, 8, (uint)1000, Easing.SinInOut);

            return Task.Delay(1000);
        }

    }
}
