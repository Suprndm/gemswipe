using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace GemSwipe.Data.Level
{
    public class LevelRepository
    {
        private IList<LevelConfiguration> _levelConfigurations;

        public LevelRepository()
        {
            Initialize();
        }

        public LevelConfiguration GetLevelConfigurationById(int id)
        {
            return _levelConfigurations.Single(l => l.Id == id);
        }

        private void Initialize()
        {
            string jsonString = ResourceLoader.LoadStringAsync("Data/Level/LevelResources.json").Result;
            _levelConfigurations = JsonConvert.DeserializeObject<List<LevelConfiguration>>(jsonString);
        }
    }
}
