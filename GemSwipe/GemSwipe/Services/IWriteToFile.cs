using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemSwipe.Services
{
    public interface IWriteToFile
    {
        string Write(string text);
        void SaveText(string filename, string text);
    }
}
