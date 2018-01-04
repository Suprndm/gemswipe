using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Paladin.Core;

namespace GemSwipe.Paladin.Physics
{
    public class PhysicalView:SkiaView, IPhysicalObject
    {
        protected PhysicalView()
        {
        }

        protected override void Draw()
        {
        }

        public float Angle { get; set; }
        public float FrictionRatio { get; set; }
        public float V { get; set; }
        public float G { get; set; }
        public float GravityAngle { get; set; }

        public void ApplyForce(float force, float angle)
        {
            var currentVX = Math.Cos(Angle) * V;
            var currentVY = Math.Sin(Angle) * V;

            var forceX = Math.Cos(angle) * force;
            var forceY = Math.Sin(angle) * force;
        }
    }
}
