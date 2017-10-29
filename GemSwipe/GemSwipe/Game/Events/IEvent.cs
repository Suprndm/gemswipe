using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Game.Models.Entities;
using GemSwipe.Paladin.Core;
using Xamarin.Forms;

namespace GemSwipe.Game.Events
{
    public interface IEvent:ISkiaView
    {
        Task<bool> Activate(Board board);
        IList<Point> GetTargets(Board board);
        Task Warmup();
    }
}
