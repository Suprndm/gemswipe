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
    public partial class MenuPage : ContentPage
    {
        public MenuPage()
        {
            InitializeComponent();
        }

  

        protected override void OnAppearing()
        {
            Task.Run(() => { Task.Delay(1000); Doors.Open(); });
        }

        private async void StartButton_OnClicked(object sender, EventArgs e)
        {
           Doors.Close(); 
            await Task.Delay(1000);
            await Navigation.PushAsync(new GamePage());
        }
    }
}