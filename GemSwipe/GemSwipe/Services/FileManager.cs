using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GemSwipe.Services
{
    public static class FileManager
    {
        public static string GetResourceName(string[] resources, string path)
        {
            // first try exact, then replace slashes
            return resources.FirstOrDefault(r =>
                r.EndsWith(path, StringComparison.OrdinalIgnoreCase) ||
                r.EndsWith(path.Replace('/', '.').Replace('\\', '.'), StringComparison.OrdinalIgnoreCase));
        }

        public static Stream LoadStream(Assembly assembly, string[] resources, string path)
        {
            var name = FileManager.GetResourceName(resources, path);
            if (name == null)
            {
                throw new ArgumentException($"Unable to find resource for '{path}'.", nameof(path));
            }

            return assembly.GetManifestResourceStream(name);
        }
    }
}
