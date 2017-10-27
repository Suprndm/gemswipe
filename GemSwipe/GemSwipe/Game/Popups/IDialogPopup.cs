using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GemSwipe.Game.SkiaEngine;

namespace GemSwipe.Game.Popups
{
    public interface IDialogPopup : ISkiaView
    {
        Action BackCommand { get; set; }
        Action NextCommand { get; set; }

        Task Show();
    }
}
