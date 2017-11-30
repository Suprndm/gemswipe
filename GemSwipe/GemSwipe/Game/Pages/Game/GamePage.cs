using System;
using System.Collections.Generic;
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
using GemSwipe.Game.Shards;
using GemSwipe.Game.Toolbox;
using GemSwipe.Paladin.Containers;
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

        public int CurrentLevel
        {
            get
            {
                return _currentLevelId;
            }
        }

        private LevelData _levelData;
        private EventBar _eventBar;
        private Random _randomizer;
        private Container _shardContainer;
        private Container _boardContainer;
        private IList<Shard> _shards;

        private ToolboxBar _toolboxBar;


        private ObjectivesView _objectivesView;

        public GamePage()
        {
            Type = PageType.Game;

            _levelDataRepository = new LevelDataRepository();
            _randomizer = new Random();

            _boardContainer = new Container();
            AddChild(_boardContainer);

            _shardContainer = new Container();
            AddChild(_shardContainer);
            _shards = new List<Shard>();

            var toolboxBar = new ToolboxBar(0, SkiaRoot.ScreenHeight * 0.85f, SkiaRoot.ScreenHeight * 0.15f, SkiaRoot.ScreenWidth);
            AddChild(toolboxBar);
        }

        public void BackgroundNextBoard()
        {

        }

        public async void Start(int levelId)
        {
            _currentLevelId = levelId;

            try
            {
                PlayerLifeService.Instance.LoseLife();

                LevelData levelData = _levelDataRepository.Get(levelId);
                _objectivesView = new ObjectivesView(levelData.Objectives, true, Width / 2, 0.1f * Height, 0.1f * Height);
                AddChild(_objectivesView);

                _levelData = levelData;
                var boardMarginTop = Height * 0.2f;
                _board = new Board(new BoardSetup(levelData), 0, 0 + boardMarginTop, Width, Height);

                _board.Swippe += Swipe;

                _boardContainer.AddContent(_board);

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
                //await _board.Swipe(direction);
                var swipeResult = _board.Swipe(direction);
                _isBusy = true;

                if (swipeResult.DeadGems.Count == 0
                    && swipeResult.FusedGems.Count == 0
                    && swipeResult.MovedGems.Count == 0)
                {
                    // Invalid swipe
                    _isBusy = false;
                    return;
                }

                if (EvalWinStatus())
                {
                    await Task.Delay(1000);
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
                    // Shards
                    //HandleShards(swipeResult);


                    // Events
                    await Task.Delay(500);
                    var eventSucceeded = await _eventBar.ActivateNextEventEvent(_board);
                    UpdateObjectivesView();

                    // LOSE
                    if (_eventBar.GetEventsCount() == 0 || !eventSucceeded)
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
                    else
                    {
                        _isBusy = false;
                    }
                }
            }
        }

        private async Task HandleShards(SwipeResult swipeResult)
        {
            await Task.Delay(500);
            if (swipeResult.FusedGems.Count >= 2)
            {
                for (int i = 0; i < _randomizer.Next(4) + 2; i++)
                {
                    GenerateShard();
                    await Task.Delay(200);
                }
            }
        }

        private void GenerateShard()
        {
            var x = _board.X + 0.2f * _board.Width + _randomizer.Next((int)(_board.Width * 0.8f));
            var y = _board.Y + 0.2f * _board.Height + _randomizer.Next((int)(_board.Height * 0.8f));
            var shard = new Shard(x, y, Width / 7, Width / 7);
            _shardContainer.AddContent(shard);
            _shards.Add(shard);
            shard.Down += () =>
            {
                shard.Die();
                if (_shards.Contains(shard))
                {
                    shard.Die();
                    _shards.Remove(shard);
                }
            };
        }

        private void UpdateObjectivesView()
        {
            var gems = _board.Gems;
            foreach (var objective in _levelData.Objectives)
            {
                var count = gems.Count(g => ((Gem)g).Size == objective.Key);

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
                var count = gems.Count(g => ((Gem)g).Size == objective.Key);

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
        }

        protected override void OnDeactivated()
        {
            _board.Swippe -= Swipe;
            _board.Dispose();
            _objectivesView.Dispose();

            foreach (var shard in _shards.ToList())
            {
                shard.Die();
                _shards.Remove(shard);
            }

            _eventBar.Dispose();
        }
    }
}
