using SkiaSharp;

namespace GemSwipe.Paladin.Sprites
{
    public class SpriteModel
    {
        private SKSurface _innerSurface;

        public SpriteModel(SKBitmap bitmap, string name, SKSize size)
        {
            Bitmap = bitmap;
            Name = name;
            Size = size;
            Visible = true;
            SourceBounds = SKRect.Create(0, 0, size.Width, size.Height);
        }

        public string Name { get; private set; }

        public SKRect SourceBounds { get; private set; }

        public SKSize Size { get; private set; }

        public SKBitmap Bitmap { get; set; }

        public bool Visible { get; set; }


        public void Draw(SKCanvas canvas, float x, float y, float width, float height, float angle = 0, SKPaint paint = null)
        {
            var canvasWidth = width * 2;
            var canvasHeight = height * 2;

            if (Visible)
            {
                canvas.DrawBitmap(Bitmap, SourceBounds, SKRect.Create(x - width / 2, y - height / 2, width, height), paint);

                //using (new SKAutoCanvasRestore(canvas, true))
                //{
                //    //canvas.RotateRadians(angle, width, Size.Height * 2);
                //}
            }
        }

    }
}


