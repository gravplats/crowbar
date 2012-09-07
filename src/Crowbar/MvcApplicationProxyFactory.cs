using System;
using System.IO;
using System.Web.Hosting;

namespace Crowbar
{
    internal static class MvcApplicationProxyFactory
    {
        public static TProxy Create<TProxy>(string name)
        {
            var physicalPath = GetPhysicalPath(name);
            if (physicalPath == null)
            {
                throw new ArgumentException(string.Format("Mvc Project {0} not found", name));
            }

            CopyDllFiles(physicalPath);

            return (TProxy)ApplicationHost.CreateApplicationHost(typeof(TProxy), "/", physicalPath);
        }

        private static string GetPhysicalPath(string mvcProjectName)
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            while (baseDirectory.Contains("\\"))
            {
                string mvcPath = Path.Combine(baseDirectory, mvcProjectName);
                if (Directory.Exists(mvcPath))
                {
                    return mvcPath;
                }

                baseDirectory = baseDirectory.Substring(0, baseDirectory.LastIndexOf("\\"));
            }

            return null;
        }

        private static void CopyDllFiles(string mvcProjectPath)
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            foreach (var file in Directory.GetFiles(baseDirectory, "*.dll"))
            {
                var destFile = Path.Combine(mvcProjectPath, "bin", Path.GetFileName(file));
                if (!File.Exists(destFile) || File.GetCreationTimeUtc(destFile) != File.GetCreationTimeUtc(file))
                {
                    File.Copy(file, destFile, true);
                }
            }
        }
    }
}