using SkiaSharp;

namespace GemSwipe.Data.Planet
{
    public class PlanetData
    {
        public int Id { get; set; }
        public double Size { get; set; }
        public string SpriteName { get; set; }
        public SKColor HaloColor { get; set; }
        public int HaloStrenght { get; set; }
    }
}
