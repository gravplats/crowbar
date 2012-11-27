using System.Web;

namespace Crowbar.Views
{
    internal class HttpBrowserCapabilitiesStub : HttpBrowserCapabilities
    {
        private readonly PartialViewContext partialViewContext;

        public HttpBrowserCapabilitiesStub(PartialViewContext partialViewContext)
        {
            this.partialViewContext = partialViewContext;
        }

        public override bool IsMobileDevice
        {
            get { return partialViewContext.IsMobileDevice; }
        }
    }
}