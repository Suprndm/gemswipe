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
    public partial class WinPopup : ContentView
    {
        public event Action Back;
        public event Action Retry;
        public event Action Next;

        public WinPopup()
        {
            InitializeComponent();
        }

        private void BackButton_OnClicked(object sender, EventArgs e)
        {
            Back?.Invoke();
        }

        private void RetryButton_OnClicked(object sender, EventArgs e)
        {
            Retry?.Invoke();
        }

        private void NextButton_OnClicked(object sender, EventArgs e)
        {
            Next?.Invoke();
        }
    }
}