using System.Collections.Specialized;

namespace Crowbar
{
    public class SimulatedResponse
    {
        private readonly NameValueCollection headers;

        public SimulatedResponse()
        {
            headers = new NameValueCollection();
        }

        public HttpStatusCode StatusCode { get; internal set; }

        public string StatusDescription { get; internal set; }

        public NameValueCollection GetHeaders()
        {
            return headers;
        }

        public void AddHeader(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            headers.Add(name, value);
        }
    }
}