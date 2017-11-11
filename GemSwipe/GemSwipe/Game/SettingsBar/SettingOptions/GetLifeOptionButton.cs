using GemSwipe.Data.PlayerLife;
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
            PlayerLifeService.Instance.GainLife();
        }

    }
}
