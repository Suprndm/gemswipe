using System;
using System.Threading.Tasks;

namespace GemSwipe.Game.Navigation.Pages
{
    public interface IPage : IDisposable
    {
        PageType Type { get; set; }

        void Initialize();

        Task Show();

        Task Hide();
    }
}
