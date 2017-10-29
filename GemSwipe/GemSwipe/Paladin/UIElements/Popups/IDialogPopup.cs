using System;
using System.Threading.Tasks;
using GemSwipe.Paladin.Core;

namespace GemSwipe.Paladin.UIElements.Popups
{
    public interface IDialogPopup : ISkiaView
    {
        Action BackCommand { get; set; }
        Action NextCommand { get; set; }

        Task Show();
    }
}
