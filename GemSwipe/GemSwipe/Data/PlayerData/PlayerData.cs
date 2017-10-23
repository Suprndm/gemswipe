using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Annotations;
using GemSwipe.Services;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace GemSwipe.Data.PlayerData
{
    public class PlayerData
    {
        public int MaxLevelReached { get; set; }

        public string Nickname { get; set; }

        public Dictionary<int, LevelProgressStatus> PlayerProgress { get; set; }
        
        public PlayerData()
        {
            MaxLevelReached = 1;
            Nickname = string.Empty;
            PlayerProgress = new Dictionary<int, LevelProgressStatus>();
        }

        
    }
}
