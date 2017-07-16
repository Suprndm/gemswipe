using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemSwipe.Models
{
    public interface IGemState
    {
        int X { get; }
        int Y { get; }
        int Size { get;}
        Guid Id{ get; }
        bool IsDead();

    }
}
