using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemSwipe.Models
{
    public interface IBoardState
    {
        int Width { get; }
        int Height { get; }
        IList<IGemState> GetGemStates();
    }
}
