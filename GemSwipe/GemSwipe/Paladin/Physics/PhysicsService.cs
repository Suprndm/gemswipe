using System;
using System.Collections.Generic;
using GemSwipe.Paladin.Core;
using SkiaSharp;

namespace GemSwipe.Paladin.Physics
{
    public class PhysicsService
    {
        private SKRect _walls;
        private readonly IList<SkiaView> _elements;
        private readonly IList<SkiaView> _elementsToRemove;
        public event Action<SkiaView, SkiaView> Hit;
        public event Action<SkiaView> HitWall;

        public PhysicsService()
        {
            _elements = new List<SkiaView>();
            _elementsToRemove = new List<SkiaView>();
        }

        private static PhysicsService _instance;
        public static PhysicsService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PhysicsService();
                }
                return _instance;
            }
        }

        public void DeclareElement(SkiaView element)
        {
            if (!_elements.Contains(element))
            {
                _elements.Add(element);
            }
        }

        public void DeclareWall(SKRect wall)
        {
            _walls = wall;
        }

        public void DetectCollisions()
        {
            // Clean
            foreach (var element in _elementsToRemove)
            {
                _elements.Remove(element);
            }

            _elementsToRemove.Clear();

            foreach (var element in _elements)
            {
                if(element.X<_walls.Left ||  element.X>_walls.Right || element.Y <_walls.Top || element.Y > _walls.Bottom)
                    HitWall?.Invoke(element);
            }
        }

        public void RemoveElement(SkiaView element)
        {
            if(!_elementsToRemove.Contains(element))
                _elementsToRemove.Add(element);
        }
    }
}
