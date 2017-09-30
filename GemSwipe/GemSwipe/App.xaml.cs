using GemSwipe.Views;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace GemSwipe
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            SetMainPage();
        }

        public static void SetMainPage()
        {
            if (AppSettings.TestModeEnabled)
            {
                Current.MainPage = new NavigationPage(new TestPage());
            }
            else
            {
                Current.MainPage = new NavigationPage(new GamePage());
            }
        }
    }
}
