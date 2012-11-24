using System.Web;

namespace Crowbar.Views
{
    internal class HttpRequestStub : HttpRequestWrapper
    {
        private readonly ViewSettings settings;

        public HttpRequestStub(HttpRequest httpRequest, ViewSettings settings)
            : base(httpRequest)
        {
            this.settings = settings;
        }

        // If we don't override something breaks in ASP.NET MVC 4 due to display modes.
        public override HttpBrowserCapabilitiesBase Browser
        {
            get
            {
                var capabilities = new HttpBrowserCapabilitiesStub(settings);
                return new HttpBrowserCapabilitiesWrapper(capabilities);
            }
        }
    }
}