using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Data.LevelMapPosition;
using GemSwipe.Data.Planet;
using GemSwipe.Game.Effects.BackgroundEffects;
using GemSwipe.Game.SkiaEngine;
using Newtonsoft.Json;
using SkiaSharp;
using Xamarin.Forms;

namespace GemSwipe.Game
{
    public class TestView : SkiaView
    {
        private float _angle = 0;
        private TextBlock _fpsText;
        public TestView( float x, float y, float height, float width) : base( x, y, height, width)
        {

            _fpsText = new TextBlock( width / 2, width / 40, "0", width / 40, CreateColor(255, 255, 255));
            AddChild(_fpsText);
        }

        public void UpdateFps(long fps)
        {
            _fpsText.Text = fps.ToString();
        }

        protected override void Draw()
        {
            _angle += 0.01f;
            int nbOfRows = 7;
            var margin = Width / 20;
            var outerRadius = (Width - nbOfRows * margin) / nbOfRows / 2;
            var innerRadius = outerRadius * .7f;

            for (int i = 0; i < nbOfRows; i++)
            {
                for (int j = 0; j < nbOfRows + 2; j++)
                {

                    // Shadowed

                    float reductionCoef = 0.95f;


                    var points = Polygonal.GetStarPolygon(innerRadius * (1 - i / 7f) * reductionCoef, outerRadius * reductionCoef, j + 2,
                        2 * (float)(_angle + Math.PI / 2 * 1 / (j + 2)) * (1 + (i * j) / (nbOfRows * nbOfRows)));

                    var path = new SKPath();
                    var starX = margin + (outerRadius) + i * (margin + outerRadius * 2);
                    var starY = +margin + (outerRadius) + j * (margin + outerRadius * 2);
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
                        Style = SKPaintStyle.Fill,
                        Color = CreateColor(175, 175, 175),
                        StrokeWidth = 2,
                        IsAntialias = false
                    };

                    Canvas.DrawPath(path, paint);


                     points = Polygonal.GetStarPolygon(innerRadius * (1 - i / 7f), outerRadius, j + 2, 2 * _angle * (1 + (i * j) / (nbOfRows * nbOfRows)));
                     path = new SKPath();
                     starX = margin + (outerRadius) + i * (margin + outerRadius * 2);
                     starY = +margin + (outerRadius) + j * (margin + outerRadius * 2);
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
                        Color = SKColors.White,
                        StrokeWidth = 2,
                        IsAntialias = true
                    };

                    Canvas.DrawPath(path, paint);

                    paint.Style = SKPaintStyle.Stroke;
                    Canvas.DrawCircle(starX, starY, outerRadius * 1.2f, paint);







                }
            }

        }


    }
}
