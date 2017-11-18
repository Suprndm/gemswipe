using GemSwipe.Data.PlayerLife;
using GemSwipe.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemSwipe.Game.SettingsBar.SettingOptions
{
    public class GetLifeOptionButton : OptionButton
    {
        public GetLifeOptionButton(float x, float y, float radius): base(x,y,radius)
        {
            SettingName = "GetLife";
        }

        public override void OnActivated()
        {
            base.OnActivated();
            Logger.Log(PlayerLifeService.Instance.GetLifeCount().ToString());
            PlayerLifeService.Instance.GainLife();
            Logger.Log(PlayerLifeService.Instance.GetLifeCount().ToString());
        }

    }
}
