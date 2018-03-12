using GemSwipe.Data.LevelData;
using GemSwipe.Data.PlayerData;
using GemSwipe.Data.PlayerLife;
using GemSwipe.Game.Popups;
using GemSwipe.Paladin.Core;
using GemSwipe.Paladin.Navigation;
using GemSwipe.Paladin.UIElements.Popups;
using GemSwipe.Services;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GemSwipe.Game.Pages.Map
{
    public abstract class WorldBase : SkiaView, IWorld
    {
        private readonly IList<LevelButton> _levelButtons;

        private readonly LevelDataRepository _levelDataRepository;
        private readonly WorldDataRepository _worldDataRepository;
        private readonly WorldProgress _worldProgress;

        private int _currentUnlockedLevelId;
        private int _id;

        public int Id { get => _id; }

        protected WorldBase(int id)
        {
            _id = id;
            _worldDataRepository = new WorldDataRepository();
            var worldData = _worldDataRepository.Get(id);
            _levelDataRepository = new LevelDataRepository();

            var levelIds = worldData.LevelIds;
            var spaceBetweenLevelButtons = Height * 0.105f;
            var levelButtonSize = Width / 13.5f;

            var heightUsedByLevelButtons = (spaceBetweenLevelButtons) * levelIds.Count;
            var margins = Height - heightUsedByLevelButtons;

            _levelButtons = new List<LevelButton>();
            var count = 0;
            var currentUnlockedLevel = PlayerDataService.Instance.GetLastUnlockedLevel();
            foreach (var levelId in levelIds)
            {
                LevelProgressStatus levelProgress = LevelProgressStatus.Locked;
                if (levelId < currentUnlockedLevel)
                {
                    levelProgress = LevelProgressStatus.Completed;
                }
                else if (levelId == currentUnlockedLevel)
                {
                    levelProgress = LevelProgressStatus.InProgress;
                }

                float overSize = 0;
                float overSpace = 0;
                bool isFinal = false;
                if (count == levelIds.Count - 1)
                {
                    overSize = levelButtonSize * 1f;
                    overSpace = spaceBetweenLevelButtons * 0.5f;
                    isFinal = true;
                }

                var levelButton = new LevelButton(
                Width / 2,
                Height - (float)(margins / 2 + (spaceBetweenLevelButtons) * count + overSpace + levelButtonSize / 2),
                levelButtonSize + overSize,
                levelId,
                levelProgress,
                isFinal);

                AddChild(levelButton);
               // DeclareTappable(levelButton);

                levelButton.Activated += () => LevelButton_Tapped(levelId);

                _levelButtons.Add(levelButton);

                count++;
            }

            var unlockedLevelId = PlayerDataService.Instance.GetLastUnlockedLevel();
            var worldOfUnlockedLevelId = _worldDataRepository.GetWorldIdByLevelId(unlockedLevelId);

            float targetProgressY = 0;
            if (worldOfUnlockedLevelId < _id)
            {
                targetProgressY = _levelButtons.First().Y;
            }
            else if (worldOfUnlockedLevelId > _id)
            {
                targetProgressY = _levelButtons.Last().Y;
            } else
            {
                var unlockedLevelButton = GetLevelButtonByLevelId(unlockedLevelId);
                targetProgressY = unlockedLevelButton.Y;
            }

            _worldProgress = new WorldProgress(_levelButtons.First().Y, targetProgressY);

            AddChild(_worldProgress);
        }


        public void UpdateLevelStatus()
        {
            foreach (LevelButton levelButton in _levelButtons)
            {
                LevelProgressStatus levelProgress = PlayerDataService.Instance.GetLevelProgress(levelButton.LevelId);
                levelButton.ProgressStatus = levelProgress;
            }
        }

        private void GoToLevel(int i)
        {
            Navigator.Instance.GoTo(PageType.Game, i);
        }

        private void ShowLevelPopup(int i)
        {
            var levelData = _levelDataRepository.Get(i);
            var dialogPopup = new LevelDialogPopup(levelData);
            PopupService.Instance.ShowPopup(dialogPopup);
            dialogPopup.NextCommand = () =>
            {
                try
                {

                    GoToLevel(i);
                }
                catch (Exception ex)
                {
                    Logger.Log("LevelButton_Tapped exception caught");
                    Navigator.Instance.GoTo(PageType.Map);
                }
            };
        }

        private void LevelButton_Tapped(int i)
        {

            if (PlayerLifeService.Instance.HasLife())
            {
                ShowLevelPopup(i);
            }
            else
            {
                var dialogPopup = new OutOfLifePopup();
                PopupService.Instance.ShowPopup(dialogPopup);
                dialogPopup.NextCommand = () =>
                {
                    try
                    {

                        PlayerLifeService.Instance.GainLife();
                        PlayerLifeService.Instance.GainLife();
                        PlayerLifeService.Instance.GainLife();

                        ShowLevelPopup(i);
                    }
                    catch (Exception ex)
                    {
                        Logger.Log("Refill exception caught");
                        Navigator.Instance.GoTo(PageType.Map);
                    }
                };
            }
        }


        private void LevelButton_Tapped(object id)
        {
            throw new NotImplementedException();
        }

        protected override void Draw()
        {
            var orderedByLevelIdLevels = _levelButtons.OrderBy(l => l.LevelId).ToList();
            using (var paint = new SKPaint())
            {
                for (int i = 0; i < orderedByLevelIdLevels.Count - 1; i++)
                {
                    var level1 = orderedByLevelIdLevels[i];
                    var level2 = orderedByLevelIdLevels[i + 1];

                    paint.IsAntialias = true;
                    paint.StrokeWidth = Height / 500;
                    paint.Color = CreateColor(255, 255, 255, 100);
                    Canvas.DrawLine(level1.X, level1.Y - level1.Height / 2, level2.X, level2.Y + level2.Height / 2, paint);
                }
            }




        }

        private LevelButton GetLevelButtonByLevelId(int levelId)
        {
            return _levelButtons.FirstOrDefault(l => l.LevelId == levelId);
        }

        public Task Activate()
        {
            // Music play
            return Task.CompletedTask;
        }

        public Task Deactivate()
        {
            // Music stop
            return Task.CompletedTask;
        }

        public async Task Advance(LevelClearedResult levelClearedResult)
        {
            var levelClearedButton = GetLevelButtonByLevelId(levelClearedResult.ClearedLevelId);

            if (levelClearedButton == null) return;

            await levelClearedButton.LevelCleared();

            if (!levelClearedResult.NextLevelId.HasValue) return;

            var unlockedLevelButton = GetLevelButtonByLevelId(levelClearedResult.NextLevelId.Value);
            if (unlockedLevelButton == null) return;
            await _worldProgress.AdvanceTo(unlockedLevelButton.Y);

            await unlockedLevelButton.LevelUnlocked();
        }
    }
}
