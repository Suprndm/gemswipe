using GemSwipe.Paladin.Core;
using System.Threading.Tasks;

namespace GemSwipe.Game.Pages.Map
{
    public interface IWorld:ISkiaView
    {
        Task Activate();
        Task Deactivate();

        Task Advance(LevelClearedResult levelClearedResult);
    }
}
