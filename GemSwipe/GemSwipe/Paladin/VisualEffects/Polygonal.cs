using System;
using System.Collections.Generic;
using SkiaSharp;

namespace GemSwipe.Paladin.VisualEffects
{
    public static class Polygonal
    {
        public static IList<SKPoint> GetStarPolygon(float innerRadius, float outerRadius, int pointsCount, float angle)
        {
            var origin = new SKPoint(0, 0);
            var points = new List<SKPoint>();
            angle += -(float) (Math.PI / 2 - (Math.PI / pointsCount));
            for (int i = 0; i < pointsCount; i++)
            {
                var innerPoint = new SKPoint(
                    (float)Math.Cos(i * Math.PI * 2 / pointsCount + angle) * innerRadius,
                    (float)Math.Sin(i * Math.PI * 2 / pointsCount + angle) * innerRadius);


                points.Add(innerPoint);

                var outerPoint = new SKPoint(
                    (float)Math.Cos((i + 0.5) * Math.PI * 2 / pointsCount + angle) * outerRadius,
                    (float)Math.Sin((i + 0.5) * Math.PI * 2 / pointsCount + angle) * outerRadius);

                points.Add(outerPoint);
            }

            return points;
        }
    }
}