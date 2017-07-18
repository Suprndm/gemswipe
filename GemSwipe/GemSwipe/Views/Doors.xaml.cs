using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GemSwipe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Doors : ContentView
    {
        public Doors()
        {
            InitializeComponent();
        }

        public void Open()
        {
            LeftDoor.TranslateTo(-200, 0, 500, Easing.CubicIn);
            RightDoor.TranslateTo(200, 0, 500, Easing.CubicIn);
        }

        public void Close()
        {
            LeftDoor.TranslateTo(0, 0, 500, Easing.CubicOut);
            RightDoor.TranslateTo(0, 0, 500, Easing.CubicOut);
        }
    }
}