using SkiaSharp;

namespace GemSwipe.Paladin.Sprites
{
    public class SpriteModel
    {
        private SpriteData _data;
        public SpriteModel(SpriteData spriteData)
        {
            _data = spriteData;
            Visible = true;
            SourceBounds = SKRect.Create(0, 0, _data.Bounds.Width, _data.Bounds.Height);
            Bitmap = _data.Bitmap;
        }

        public SKRect SourceBounds { get; }

        public SKBitmap Bitmap { get; set; }

        public bool Visible { get; set; }


        public void Draw(SKCanvas canvas, float x, float y, float width, float height, float angle = 0, SKPaint paint = null)
        {
            var canvasWidth = width * 2;
            var canvasHeight = height * 2;

            if (Visible)
            {
               

                 using (new SKAutoCanvasRestore(canvas, true))
                 {
                     canvas.RotateRadians(angle, width, height);
                     canvas.DrawBitmap(Bitmap, SourceBounds, SKRect.Create(x - width / 2, y - height / 2, width, height), paint);
                }
            }
        }
    }
}


