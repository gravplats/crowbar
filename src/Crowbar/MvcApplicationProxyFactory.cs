using System;
using System.IO;
using System.Web.Hosting;

namespace Crowbar
{
    internal static class MvcApplicationProxyFactory
    {
        public static TProxy Create<TProxy>(IPathProvider provider)
        {
            var physicalPath = provider.GetPhysicalPath();
            CopyDllFiles(physicalPath);

            return (TProxy)ApplicationHost.CreateApplicationHost(typeof(TProxy), "/", physicalPath);
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