using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace GemSwipe.Data.Level
{

    public class LevelFileReader
    {
        public string TestString;

        public LevelFileReader()
        {
            //#region How to load an Json file embedded resource
            var assembly = typeof(LevelFileReader).GetTypeInfo().Assembly;
            
            Stream stream = assembly.GetManifestResourceStream(@"d:\movie.json");
            //Stream stream = assembly.GetManifestResourceStream("Data/Level/LevelResources.json");

            //Earthquake[] earthquakes;

            // use for debugging, not in released app code!
            foreach (var res in assembly.GetManifestResourceNames())
            {
                System.Diagnostics.Debug.WriteLine("found resource: " + res);
            }

            using (var reader = new System.IO.StreamReader(stream))
            {

                var json = reader.ReadToEnd();
                var rootobject = JsonConvert.DeserializeObject<LevelConfiguration>(json);

                TestString = rootobject.Title;
            }
            

            // NOTE: use for debugging, not in released app code!
            //foreach (var res in assembly.GetManifestResourceNames()) 
            //	System.Diagnostics.Debug.WriteLine("found resource: " + res);
        }
    }
}