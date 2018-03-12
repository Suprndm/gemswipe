using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GemSwipe.Data.LevelData;
using GemSwipe.Data.PlayerData;
using GemSwipe.Data.PlayerLife;
using GemSwipe.Game.Events;
using GemSwipe.Game.Models;
using GemSwipe.Game.Models.Entities;
using GemSwipe.Game.Popups;
using GemSwipe.Game.Shards;
using GemSwipe.Game.Toolbox;
using GemSwipe.Paladin.Containers;
using GemSwipe.Paladin.Navigation;
using GemSwipe.Paladin.Navigation.Pages;
using GemSwipe.Paladin.UIElements.Popups;
using GemSwipe.Services;

namespace GemSwipe.Game.Pages.Game
{
    public class GamePage : PageBase
    {
        private Board _board;
        private bool _isBlocked;
        private bool _isBusy;
        private LevelDataRepository _levelDataRepository;
        private int _currentLevelId;
        private WorldDataRepository _worldDataRepository;

        public int CurrentLevel
        {
            get
            {
                return _currentLevelId;
            }
        }

        private LevelData _levelData;
        private Random _randomizer;
        private Container _shardContainer;
        private Container _boardContainer;

        public GamePage()
        {
            Type = PageType.Game;

            _levelDataRepository = new LevelDataRepository();
            _randomizer = new Random();

            _worldDataRepository = new WorldDataRepository();

            _boardContainer = new Container();
            AddChild(_boardContainer);

            _shardContainer = new Container();
            AddChild(_shardContainer);
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

                var boardMarginTop = Height * 0.2f;
                _board = new Board(new BoardSetup(levelData), 0, 0 + boardMarginTop, Width, Height);

                _board.Swippe += Swipe;

                _boardContainer.AddContent(_board);

                BackgroundNextBoard();
                _isBusy = false;

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

            if (_board != null&&!_board.IsBusy)
            {
                _isBusy = true;

                var swipeResult = await _board.Swipe(direction);

                if (swipeResult.DeadGems.Count == 0
                    && swipeResult.FusedGems.Count == 0
                    && swipeResult.MovedGems.Count == 0)
                {
                    // Invalid swipe
                    Logger.Log("invalid swipe");
                    _isBusy = false;
                    return;
                }


                if (EvalWinStatus())
                {
                    Logger.Log("evalwin");
                    await Task.Delay(1000);
                    // WIN

                    PlayerDataService.Instance.UpdateLevelProgress(_currentLevelId, LevelProgressStatus.Completed);
                    PlayerLifeService.Instance.GainLife();
                    await Task.Delay(1000);

                    var dialogPopup = new WinDialogPopup(5);
                    PopupService.Instance.ShowPopup(dialogPopup);
                    dialogPopup.NextCommand = () =>
                    {
                        var worldId = _worldDataRepository.GetWorldIdByLevelId(_currentLevelId);

                        Navigator.Instance.GoTo(PageType.Map, worldId);
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

                    // LOSE
                    // TO Update move count logic
                    if (false)
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

        private bool EvalWinStatus()
        {
            var gems = _board.Gems;
            return gems.Count == 1;
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
        }
    }
}
