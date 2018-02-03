using System;
using System.Collections.Generic;
using System.Linq;
using GemSwipe.Views;
using Xamarin.Forms;

namespace GemSwipe
{
	public partial class App : Application
	{
		public App ()
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

        protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
