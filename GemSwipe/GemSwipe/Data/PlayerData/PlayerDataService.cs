using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Services;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace GemSwipe.Data.PlayerData
{
    public class PlayerDataService
    {
        private string _fileName = AppSettings.PlayerPersonalDataFileName;
        private PlayerData _playerData;
        private static PlayerDataService _instance;

        private PlayerDataService()
        {
            if (DependencyService.Get<IFileHandler>().CheckExistenceOf(_fileName))
            {
                Load();
            }
            else
            {
                InitializeProfile();
            }
        }

        public static PlayerDataService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PlayerDataService();
                }
                return _instance;
            }
        }

        public PlayerData GetData()
        {
            return _playerData;
        }

        public void SaveChanges()
        {
            string playerDataSerialized = JsonConvert.SerializeObject(_playerData);
            var fileWriter = DependencyService.Get<IFileHandler>();
            fileWriter.SaveText(_fileName, playerDataSerialized);
        }

        public void SaveBackup()
        {
            var fileHandler = DependencyService.Get<IFileHandler>();
            string currentSave = fileHandler.LoadText(_fileName);
            fileHandler.SaveText("backup_"+_fileName, currentSave);
        }

        public void Load()
        {
            var fileReader = DependencyService.Get<IFileHandler>();
            var result = fileReader.LoadText(_fileName);
            _playerData = JsonConvert.DeserializeObject<PlayerData>(result);
        }

        public void Update(PlayerData playerData)
        {
            _playerData = playerData;
            _playerData.Nickname = playerData.Nickname;
            _playerData.MaxLevelReached = playerData.MaxLevelReached;
            _playerData.PlayerProgress = playerData.PlayerProgress;
        }

        private void InitializeProfile()
        {
            _playerData = new PlayerData();
            SaveChanges();
        }


        public void UpdateLevelProgress(int levelId, LevelProgressStatus levelStatus)
        {
            if ((levelId == _playerData.MaxLevelReached) && (levelStatus == LevelProgressStatus.Completed))
            {
                _playerData.MaxLevelReached++;
            }
            _playerData.PlayerProgress[levelId] = levelStatus;
            SaveChanges();
        }

        public LevelProgressStatus GetLevelProgress(int levelId)
        {
            LevelProgressStatus progressStatus;
            if (_playerData.PlayerProgress.TryGetValue(levelId, out progressStatus))
            {
                return progressStatus;
            }
            else if (levelId == _playerData.MaxLevelReached)
            {
                return LevelProgressStatus.InProgress;
            }
            else
            {
                return LevelProgressStatus.Locked;
            }
        }

        public void SetMaxLevelReached(int maxLevel)
        {
            _playerData.MaxLevelReached = maxLevel;
            SaveChanges();
        }

        public void ResetProfile()
        {

        }
    }
}
