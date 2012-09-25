using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.Hosting;

namespace Crowbar
{
    internal static class MvcApplicationProxyFactory
    {
        public static TProxy Create<TProxy>(string mvcProjectName)
        {
            var physicalPath = GetPhysicalPath(mvcProjectName);
            CopyDllFiles(physicalPath);

            return (TProxy)ApplicationHost.CreateApplicationHost(typeof(TProxy), "/", physicalPath);
        }

        private static string GetPhysicalPath(string mvcProjectName)
        {
            var searchedLocations = new List<string>();

            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            while (baseDirectory.Contains("\\"))
            {
                string mvcPath = Path.Combine(baseDirectory, mvcProjectName);
                if (Directory.Exists(mvcPath))
                {
                    return mvcPath;
                }

                searchedLocations.Add(mvcPath);
                baseDirectory = baseDirectory.Substring(0, baseDirectory.LastIndexOf("\\"));
            }

            var locations = new StringBuilder();
            foreach (var searchedLocation in searchedLocations)
            {
                locations.AppendLine();
                locations.Append(searchedLocation);
            }

            throw new ArgumentException(string.Format("The MVC Project '{0}' was not found by Crowbar. The following locations were searched: {1}", mvcProjectName, locations));
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