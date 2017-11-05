using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemSwipe.Paladin.VisualEffects
{
    public class FloatingParticule:Particule
    {
        public float FloatingRadius { get; set; }
        public float FloatingSpeed { get; set; }

        private float _sinRotator;
        private float _cosRotator;

        private float _sinAngle;
        private float _cosAngle;

        private float _xRotator;
        private float _yRotator;

        private float _xDistance;
        private float _yDistance;

        private float _anchorX;
        private float _anchorY;

        private float _variant1;
        private float _variant2;
        private float _variant3;
        private float _variant4;
        private float _variant5;
        private float _variant6;

        private float _phaseX;
        private float _phaseY;

        private float _speedModificatorX;
        private float _speedModificatorY;
            
        private Random _randomizer;

        public FloatingParticule(float x, float y, float floatingRadius, float floatingSpeed, Random randomizer) : base(x, y)
        {
            FloatingRadius = floatingRadius;
            FloatingSpeed = floatingSpeed;
            _anchorX = x;
            _anchorY = y;
            _randomizer = randomizer;

            _phaseX = (float)((float)_randomizer.Next(100) / 100 * Math.PI);
            _phaseY = (float)((float)_randomizer.Next(100) / 100 * Math.PI);

            _speedModificatorX = (float)_randomizer.Next(30) / 100;
            _speedModificatorY = (float)_randomizer.Next(30) / 100;
        }


        public override void Update()
        {
            _variant1 += FloatingSpeed * _speedModificatorX;
            _variant2 = (float)Math.Cos(_variant1 + _phaseX) * FloatingSpeed;
            _variant3 += _variant2;

            _variant4 += FloatingSpeed * _speedModificatorY;
            _variant5 = (float)Math.Sin(_variant4 + _phaseY) * FloatingSpeed;
            _variant6 += _variant5;

            X = (float)(FloatingRadius * Math.Cos(_variant3 + _phaseX)) + _anchorX;
            Y = (float)(FloatingRadius * Math.Sin(_variant6 + _phaseY)) + _anchorY;

            _cosRotator += _sinRotator;
            base.Update();
        }
    }
}
