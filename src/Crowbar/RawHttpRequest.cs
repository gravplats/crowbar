using System.Collections.Generic;
using System.IO;

namespace Crowbar
{
    internal class RawHttpRequest
    {
        private readonly string method;
        private readonly string protocol;

        private readonly Dictionary<string, string> headers = new Dictionary<string, string>();

        private string body;
        private string path;

        public RawHttpRequest(string method, string protocol)
        {
            this.method = method;
            this.protocol = protocol;
        }

        public void AddHeader(string name, string value)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            // Could be called multiple times with the same name during a simulated HTTP request.
            if (headers.ContainsKey(name))
            {
                return;
            }

            headers.Add(name, value);
        }

        public void SetBody(string body)
        {
            this.body = body;
        }

        public void SetPath(string path)
        {
            this.path = path;
        }

        public override string ToString()
        {
            using (var writer = new StringWriter())
            {
                writer.WriteLine("{0} {1}://localhost{2}", method.ToUpper(), protocol, path);
                foreach (var header in headers)
                {
                    writer.WriteLine("{0}: {1}", header.Key, header.Value);
                }

                if (!string.IsNullOrWhiteSpace(body))
                {
                    writer.WriteLine();
                    writer.WriteLine(body);
                }

                return writer.ToString();
            }
        }
    }
}