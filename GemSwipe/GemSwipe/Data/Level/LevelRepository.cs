using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GemSwipe.Data.Level
{
    public class LevelRepository
    {
        private IDictionary<int,LevelConfiguration> _listOfLevelsConfigurations;

        public LevelRepository()
        {
            BuildListOfLevels();
        }

        public void GetLevelConfigurationById(int id)
        {
           
        }

        public IDictionary<int, LevelConfiguration> GetRepository()
        {
            return _listOfLevelsConfigurations;
        }

        public void BuildListOfLevels()
        {
            string jsonString = ResourceLoader.LoadStringAsync("Data/Level/LevelResources.json").Result;
            _listOfLevelsConfigurations = JsonConvert.DeserializeObject<Dictionary<int, LevelConfiguration>>(jsonString);
            //LevelConfiguration levelconfig = _listOfLevelsConfiguration[i];
        }

    }
}
