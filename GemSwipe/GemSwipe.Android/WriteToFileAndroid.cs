using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GemSwipe.Droid;
using GemSwipe.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(WriteToFileAndroid))]
namespace GemSwipe.Droid
{
    public class WriteToFileAndroid : IWriteToFile
    {
        public WriteToFileAndroid()
        {

        }

        //[assembly: Dependency(typeof(SaveAndLoad))]
        //namespace WorkingWithFiles
        //{
        //public class SaveAndLoad : ISaveAndLoad
        //{

        public void SaveText(string filename, string text)
        {
            var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            var filePath = System.IO.Path.Combine(documentsPath, filename);
            System.IO.File.Create(filePath);
            System.IO.File.WriteAllText(filePath, text);
        }
        public string LoadText(string filename)
        {
            var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, filename);
            return System.IO.File.ReadAllText(filePath);
        }
        //    }
        //}

        public string Write(string str)
        {
            return "Coucou";
        }
    }
}