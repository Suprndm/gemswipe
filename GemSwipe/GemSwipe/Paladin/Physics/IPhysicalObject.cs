using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemSwipe.Paladin.Physics
{
    public interface IPhysicalObject
    {
        float Angle { get; set; }

        float FrictionRatio { get; set; }

        float V { get; set; }

        float X { get; set; }

        float Y { get; set; }

        float G { get; set; }

        float GravityAngle { get; set; }

        void ApplyForce(float force, float angle);
    }
}
