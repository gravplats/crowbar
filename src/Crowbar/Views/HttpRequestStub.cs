using System.Web;

namespace Crowbar.Views
{
    internal class HttpRequestStub : HttpRequestWrapper
    {
        private readonly PartialViewContext partialViewContext;

        public HttpRequestStub(HttpRequest httpRequest, PartialViewContext partialViewContext)
            : base(httpRequest)
        {
            this.partialViewContext = partialViewContext;
        }

        // If we don't override something breaks in ASP.NET MVC 4 due to display modes.
        public override HttpBrowserCapabilitiesBase Browser
        {
            get
            {
                var capabilities = new HttpBrowserCapabilitiesStub(partialViewContext);
                return new HttpBrowserCapabilitiesWrapper(capabilities);
            }
        }
    }
}