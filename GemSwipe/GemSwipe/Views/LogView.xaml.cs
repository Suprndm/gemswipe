using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemSwipe.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GemSwipe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LogView : ContentView
    {
        private readonly IList<string> _logs;
        private const int MaxLines = 20;
        public LogView()
        {
            InitializeComponent();

            _logs = new List<string>();

            if (AppSettings.LogEnabled)
            {
                Logger.OnLogged += Logger_OnLogged;

                Device.StartTimer(new TimeSpan(0,0,0,5),()=> {Logger.Log("");
                    return true;
                });
            }
        }

        private void Logger_OnLogged(string obj)
        {
            _logs.Add(obj);
            if(_logs.Count>20)
                _logs.RemoveAt(0);

            Draw();

        }

        private void Draw()
        {
            var text = "";
            foreach (var log in _logs)
            {
                text += $"\n{log}";
            }

            LogLabel.Text = text;
        }
    }
}