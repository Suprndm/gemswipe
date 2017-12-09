using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemSwipe
{
    public static class AppSettings
    {
        public static bool TestModeEnabled => false;
        public static bool LogEnabled => true;
        public static string PlayerPersonalDataFileName => "PlayerData37.txt";
        public static string PlayerLifePersonalDataFileName => "PlayerLifeData13.txt";
        public static int DefaultAnimationMs => 600;
    }
}
