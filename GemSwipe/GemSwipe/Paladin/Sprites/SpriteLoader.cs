using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Data;
using GemSwipe.Paladin.Navigation;
using GemSwipe.Paladin.Navigation.Pages;
using SkiaSharp;

namespace GemSwipe.Paladin.Sprites
{
    public class SpriteLoader
    {
        private IDictionary<string, SpriteData> _spriteDatas;
        private bool _isInitialized;
        private SpriteLoader()
        {
        }

        private static SpriteLoader _instance;
        public static SpriteLoader Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SpriteLoader();
                }
                return _instance;
            }
        }

        public async Task Initialize(IList<string> spritesNames, string resourceFolder, float screenWidth, float screenHeight)
        {
            _spriteDatas = new Dictionary<string, SpriteData>();
            var resolutionLevel = GetResolutionLevel(screenWidth, screenHeight);

            foreach (var spritesName in spritesNames)
            {
                var sheetPath = $"{resourceFolder}/{resolutionLevel}/{spritesName}";
                var bitmap = await ResourceLoader.LoadBitmapAsync(sheetPath);
                var info = bitmap?.Info ?? SKImageInfo.Empty;
                if (bitmap == null || info.Width == 0 || info.Height == 0)
                {
                    throw new ArgumentException($"Unable to load sprite sheet bitmap '{sheetPath}'.");
                }

                var spriteData = new SpriteData()
                {
                    Name = spritesName,
                    Bounds = new SKSize(info.Width, info.Height),
                    Bitmap = bitmap
                };

                _spriteDatas.Add(spritesName, spriteData);
            }

            _isInitialized = true;
        }

        public SpriteData GetData(string name)
        {
            if (_isInitialized == false)
                throw new Exception("Sprite Loader not initialized Yet");

            try
            {
                return _spriteDatas.Single(pair => pair.Key == name).Value;
            }
            catch (Exception e)
            {
                throw new KeyNotFoundException(name, e);
            }
        }

        private string GetResolutionLevel(float width, float height)
        {
            if (width <= 540)
                return "Medium";

            return "High";
        }
    }
}
