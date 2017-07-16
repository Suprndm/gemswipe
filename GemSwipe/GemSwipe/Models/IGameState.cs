using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemSwipe.Models
{
    public interface IGameState
    {
        IBoardState GetBoardState();
    }
}
