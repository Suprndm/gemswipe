using System.Collections.Generic;
using System.Threading.Tasks;
using GemSwipe.Game.Effects;
using GemSwipe.Game.Models.Entities;
using GemSwipe.Paladin.Containers;
using GemSwipe.Paladin.UIElements.Popups;
using GemSwipe.Paladin.VisualEffects;

namespace GemSwipe.Game.Popups
{
    public class WinDialogPopup : DialogPopup
    {
        private IList<StarEffect> _starEffects;
        public WinDialogPopup(int movesLeft) : base(SkiaRoot.ScreenWidth * 0.7f, SkiaRoot.ScreenHeight * 0.4f, false)
        {
            Popup.Title = $"Congratulation !";
            Popup.ActionName = "Next level";

            _starEffects = new List<StarEffect>();
            var container = new Container();
            for (int i = 0; i < 3; i++)
            {
                var starEffect = new StarEffect((i - 1) * Width * 0.2f, SkiaRoot.ScreenHeight * 0.1f);
                container.AddContent(starEffect);
                _starEffects.Add(starEffect);
            }

            Popup.AddContent(container);

            var scoreDetails = new ScoreDetails(container.X, container.Y, Width, ContentHeight * 0.5f);
            Popup.AddContent(scoreDetails);


            Task.Run(async () =>
            {
                var scoreMovesLeft = movesLeft * 100;
                var scoreLevelCleared = 50;
                await Task.Delay(1000);
                await scoreDetails.Push($"Level Cleared : {scoreLevelCleared}");

                await Task.Delay(500);
                await scoreDetails.Push($"Moves left : {scoreMovesLeft}");


                await Task.Delay(500);

                var scoreText = new IncrementalText(0, scoreMovesLeft+ scoreLevelCleared, 0, 0, SkiaRoot.ScreenHeight * 0.05f, CreateColor(255, 255, 255));
                Popup.AddContent(scoreText);
                await scoreText.Start();

                await Task.Delay(200);
                for (int i = 0; i < 3; i++)
                {
                    _starEffects[i].Start();
                    await Task.Delay(300);
                }
            });
        }
    }
}
