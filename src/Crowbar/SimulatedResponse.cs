using System.Collections.Specialized;

namespace Crowbar
{
    internal class SimulatedResponse
    {
        private readonly NameValueCollection headers;

        public SimulatedResponse()
        {
            headers = new NameValueCollection();
        }

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