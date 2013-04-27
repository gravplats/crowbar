using System;
using System.IO;
using System.Web.Hosting;
using LinFu.DynamicProxy;

namespace Crowbar
{
    internal class CrowbarHttpWorkerInterceptor : IDisposable, IInterceptor
    {
        private readonly string testBaseDirectory;
        private readonly string method;
        private readonly string path;

        private readonly SimpleWorkerRequest worker;

        private readonly TextWriter writer;

        public CrowbarHttpWorkerInterceptor(string testBaseDirectory, string method, string path, SimpleWorkerRequest worker)
        {
            this.testBaseDirectory = testBaseDirectory;
            this.method = method;
            this.path = path;
            this.worker = worker;

            writer = new StringWriter();
        }

        public void Dispose()
        {
            string trace = writer.ToString();

            string directory = Path.Combine(testBaseDirectory, "trace");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string fullPath = Path.Combine(directory, method + "_" + path.Replace("/", "_") + ".txt");

            using (var stream = File.Open(fullPath, FileMode.Create))
            using (var swriter = new StreamWriter(stream))
            {
                swriter.WriteLine(trace);
            }

            writer.Dispose();
        }

        public object Intercept(InvocationInfo info)
        {
            object result = info.TargetMethod.Invoke(worker, info.Arguments);
            writer.WriteLine(info.TargetMethod.Name + "(" + string.Join(", ", info.Arguments) + ")" + (result != null ? " -> " + result : ""));

            return result;
        }
    }
}