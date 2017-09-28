using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemSwipe.GameEngine.Navigation.Pages
{
    public interface IPage : IDisposable
    {
        PageType Type { get; set; }

        void Initialize();

        Task Show();

        Task Hide();
    }
}
