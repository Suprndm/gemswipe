using System;
using System.Linq;
using System.Threading.Tasks;
using GemSwipe.Data.LevelData;
using GemSwipe.Data.PlayerData;
using GemSwipe.Data.PlayerLife;
using GemSwipe.Game.Effects.BackgroundEffects;
using GemSwipe.Game.Events;
using GemSwipe.Game.Models;
using GemSwipe.Game.Models.Entities;
using GemSwipe.Game.Popups;
using GemSwipe.Paladin.Gestures;
using GemSwipe.Paladin.Navigation;
using GemSwipe.Paladin.Navigation.Pages;
using GemSwipe.Paladin.UIElements.Popups;
using GemSwipe.Services;
using SkiaSharp;

namespace GemSwipe.Game.Pages.Game
{
    public class GamePage : PageBase
    {
        private Board _board;
        private bool _isBlocked;
        private bool _isBusy;
        private LevelDataRepository _levelDataRepository;
        private int _currentLevelId;
        private LevelData _levelData;
        private EventBar _eventBar;


        private ObjectivesView _objectivesView;
        
        public GamePage()
        {
            _levelDataRepository = new LevelDataRepository();
        }

        public void BackgroundNextBoard()
        {

        }

        public async void Start(int levelId)
        {
            _currentLevelId = levelId;

            try
            {
                LevelData levelData = _levelDataRepository.Get(levelId);
                _objectivesView = new ObjectivesView(levelData.Objectives, true, Width / 2, 0.1f * Height, 0.1f * Height);
                AddChild(_objectivesView);

                _levelData = levelData;
                var boardMarginTop = Height * 0.2f;
                _board = new Board(new BoardSetup(levelData), 0, 0 + boardMarginTop, Width, Width);
                AddChild(_board);

                BackgroundNextBoard();
                _isBusy = false;

                UpdateObjectivesView();

                _eventBar = new EventBar(0, 0, 0.1f * Height, Width);
                AddChild(_eventBar);

                await _eventBar.Initialize(levelData.Events);
            }
            catch (Exception e)
            {
                Logger.Log($"invalid levelId {levelId}");
                Navigator.Instance.GoTo(PageType.Map);
            }
        }

        public async void Swipe(Direction direction)
        {
            if (IsBusy()) return;

            if (_board != null)
            {
                var swipeResult = _board.Swipe(direction);
                _isBusy = true;
                await Task.Delay(1000);
                var eventSucceeded = await _eventBar.ActivateNextEventEvent(_board);
                UpdateObjectivesView();

                if (EvalWinStatus() &&  eventSucceeded)
                {
                    // WIN

                    PlayerDataService.Instance.UpdateLevelProgress(_currentLevelId, LevelProgressStatus.Completed);
                    PlayerLifeService.Instance.GainLife();
                    await Task.Delay(1000);

                    var dialogPopup = new WinDialogPopup();
                    PopupService.Instance.ShowPopup(dialogPopup);
                    dialogPopup.NextCommand = () =>
                    {
                        Navigator.Instance.GoTo(PageType.Map);
                    };
                    dialogPopup.BackCommand = () =>
                    {
                        Navigator.Instance.GoTo(PageType.Game, _currentLevelId);
                    };


                    BackgroundNextBoard();
                }
                else
                {
                    // LOSE
                    if (_eventBar.GetEventsCount() == 0  ||! eventSucceeded)
                    {
                        await Task.Delay(1000);
                        var dialogPopup = new LoseDialogPopup();
                        PopupService.Instance.ShowPopup(dialogPopup);
                        dialogPopup.NextCommand = () =>
                        {
                            Navigator.Instance.GoTo(PageType.Game, _currentLevelId);
                        };
                        dialogPopup.BackCommand = () =>
                        {
                            Navigator.Instance.GoTo(PageType.Map);
                        };
                    }
                }

                _isBusy = false;

            }
        }

        private void UpdateObjectivesView()
        {
            var gems = _board.Gems;
            foreach (var objective in _levelData.Objectives)
            {
                var count = gems.Count(g => g.Size == objective.Key);

                if (count <= objective.Value)
                    _objectivesView.UpdateObjective(objective.Key, count);
                else
                {
                    _objectivesView.UpdateObjective(objective.Key, objective.Value);
                }
            }
        }

        private bool EvalWinStatus()
        {
            var gems = _board.Gems;
            bool isWon = true;
            foreach (var objective in _levelData.Objectives)
            {
                var count = gems.Count(g => g.Size == objective.Key);

                if (count < objective.Value)
                    isWon = false;
            }

            return isWon;
        }

        public bool IsBusy()
        {
            return _isBusy;
        }

        protected override void Draw()
        {
        }

        protected override void OnActivated(object parameter = null)
        {
            var levelId = (int)parameter;

            Start(levelId);
            Gesture.Swipe += OnSwipped;
        }

        private void OnSwipped(Direction direction)
        {
            Swipe(direction);
        }

        protected override void OnDeactivated()
        {
            _board.Dispose();
            _objectivesView.Dispose();
            _eventBar.Dispose();
            Gesture.Swipe -= OnSwipped;
        }
    }
}
