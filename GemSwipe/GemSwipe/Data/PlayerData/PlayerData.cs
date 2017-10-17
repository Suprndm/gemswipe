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
    public class PlayerData
    {
        private int _maxLevelReached { get; set; }
        private string _nickName { get; set; }
        private Dictionary<int, string> _playerProgress { get; set; }
        private List<string> _playerAchievementList { get; set; }
        private int _score { get; set; }

        public PlayerData(int maxLevelReached, string nickName)
        {
            _maxLevelReached = maxLevelReached;
            _nickName = nickName;
        }

        public PlayerData GetProfile()
        {
            var fileReader = DependencyService.Get<IFileHandler>();
            var result = fileReader.LoadText("PlayerProfile.txt");
            PlayerData playerdata = JsonConvert.DeserializeObject<PlayerData>(result);
            return playerdata;
        }

        public void SaveData()
        {
            string playerDataSerialized = JsonConvert.SerializeObject(this);
            var fileWriter = DependencyService.Get<IFileHandler>();
            fileWriter.SaveText("PlayerProfile.txt", playerDataSerialized);
        }

        public void SaveBackup()
        {
            var fileHandler = DependencyService.Get<IFileHandler>();
            string currentSave = fileHandler.LoadText("PlayerProfile.txt");
            fileHandler.SaveText("PlayerProfileBackup.txt", currentSave);
        }

        public void SetNickName(string nickName)
        {
            _nickName = nickName;
        }

        public void SetMaxLevelReached(int maxLevel)
        {
            _maxLevelReached = maxLevel;
        }

    }
}
