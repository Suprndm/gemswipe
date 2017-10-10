using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemSwipe.Utilities.Buttons
{
    public interface IButton
    {
        event Action Tapped;

        //gérer les states, disabled, reactive,
        //gérer les clicks
        //command pattern
    }
}
