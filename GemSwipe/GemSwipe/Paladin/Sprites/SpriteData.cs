using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;

namespace GemSwipe.Paladin.Sprites
{
    public class SpriteData
    {
        public string Name { get; set; }
        public SKSize Bounds { get; set; }
        public SKBitmap Bitmap { get; set; }
    }
}
