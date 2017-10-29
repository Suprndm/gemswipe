using SkiaSharp;

namespace GemSwipe.Paladin.Sprites
{
    public class SpriteModel
    {
        private SKSurface _innerSurface;

        public SpriteModel(SpriteSheet sheet, string name, SKSize size, SKRect bounds)
        {

            Name = name;
            SpriteSheet = sheet;
            Size = size;
            SourceBounds = bounds;
            Visible = true;
        }

        public string Name { get; private set; }

        public SpriteSheet SpriteSheet { get; private set; }

        public SKRect SourceBounds { get; private set; }

        public SKSize Size { get; private set; }

        public SKBitmap Bitmap => SpriteSheet.Bitmap;

        public bool Visible { get; set; }


        public void Draw(SKCanvas canvas, float x, float y, float width, float height, float angle = 0, SKPaint paint = null)
        {
            var canvasWidth = width * 2;
            var canvasHeight = height * 2;

            if (Visible)
            {
                if (_innerSurface == null)
                {
                    _innerSurface = SKSurface.Create((int)canvasWidth, (int)canvasHeight, SKImageInfo.PlatformColorType, SKAlphaType.Premul);
                }

                var innerCanvas = _innerSurface.Canvas;

                innerCanvas.Clear();
                using (new SKAutoCanvasRestore(innerCanvas, true))
                {
                    innerCanvas.RotateRadians(angle, width, Size.Height * 2);
                    innerCanvas.DrawBitmap(Bitmap, SourceBounds, SKRect.Create(canvasWidth / 2 - width / 2, canvasHeight / 2 - height / 2, width, height), paint);
                }

                canvas.DrawSurface(_innerSurface, x - canvasWidth / 2, y - canvasHeight / 2);
            }
        }

    }
}


