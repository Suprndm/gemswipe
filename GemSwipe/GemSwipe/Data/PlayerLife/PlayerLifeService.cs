using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using GemSwipe.Services;
using Newtonsoft.Json;

namespace GemSwipe.Data.PlayerLife
{
    public class PlayerLifeService 
    {

        private string _fileName = AppSettings.PlayerLifePersonalDataFileName;
        private PlayerLife _playerLife;
        private static PlayerLifeService _instance;

        private PlayerLifeService()
        {
            if (DependencyService.Get<IFileHandler>().CheckExistenceOf(_fileName))
            {
                Load();
            }
            else
            {
                Initialize();
            }
        }

        public static PlayerLifeService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PlayerLifeService();
                }
                return _instance;
            }
        }

        public int GetLifeCount()
        {
            return _playerLife.Count;
        }

        public bool HasLife()
        {
            return _playerLife.HasLife();
        }

        public void GainLife()
        {
            _playerLife.Increment();
            SaveChanges();
        }

        public void LoseLife()
        {
            _playerLife.Decrement();
            SaveChanges();
        }

        public PlayerLife GetData()
        {
            return _playerLife;
        }

        public void SaveChanges()
        {
            string playerDataSerialized = JsonConvert.SerializeObject(_playerLife);
            var fileWriter = DependencyService.Get<IFileHandler>();
            fileWriter.SaveText(_fileName, playerDataSerialized);
        }

        public void SaveBackup()
        {
            var fileHandler = DependencyService.Get<IFileHandler>();
            string currentSave = fileHandler.LoadText(_fileName);
            fileHandler.SaveText("backup_" + _fileName, currentSave);
        }

        public void Load()
        {
            var fileReader = DependencyService.Get<IFileHandler>();
            var result = fileReader.LoadText(_fileName);
            _playerLife = JsonConvert.DeserializeObject<PlayerLife>(result);
        }

        protected void Initialize()
        {
            _playerLife = new PlayerLife();
            SaveChanges();
        }


        public void Update(PlayerLife playerData)
        {
            _playerLife = playerData;
            //_playerLife.Nickname = playerData.Nickname;
            //_playerLife.MaxLevelReached = playerData.MaxLevelReached;
            //_playerLife.PlayerProgress = playerData.PlayerProgress;
        }

        

    }
}
