using System;
using System.IO;

namespace GAEngine.Utils
{
    internal static class ContentPaths
    {
        public static string Resolve(string relativePath)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
            if (!File.Exists(path))
            {
                throw new FileNotFoundException(
                    "Required demo content is missing. Rebuild Sandbox and keep its res and shaders folders beside Sandbox.exe.",
                    path);
            }

            return path;
        }
    }
}
