using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemSwipe.Game.Effects.BackgroundEffects
{
    //public class OrbitingStars
    //{
    //    private IList<OrbitingStarParticle> _listOfStarParticles;

    //    public OrbitingStars()
    //    {
    //        _listOfStarParticles = new List<OrbitingStarParticle>();
    //    }
    //    public void ActivateOrbitingStars(float screenWidth, float screenHeight)
    //    {

    //        Random randomizer = new Random();
    //        for (int i = 0; i < 30; i++)
    //        {
    //            OrbitingStarParticle star = new OrbitingStarParticle(randomizer.Next((int)screenWidth) + screenWidth / 2,
    //                randomizer.Next((int)screenHeight) + screenHeight / 2,
    //                Radius / 4,
    //                randomizer,
    //                SKColor.FromHsl(21, 78, randomizer.Next(0, 6))
    //                );
    //            //OrbitingStarParticle star = new OrbitingStarParticle(randomizer.Next((int)screenWidth) - screenWidth / 2, randomizer.Next((int)screenHeight) - screenHeight / 2, Radius / 4, randomizer, CreateColor(0, 0, 0));
    //            star.SetTarget(Radius, Radius);
    //            AddChild(star);
    //            _listOfStarParticles.Add(star);
    //            star.SteerToTarget();

    //        }
    //    }
    //}
}
