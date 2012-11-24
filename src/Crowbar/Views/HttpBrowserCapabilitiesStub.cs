using System.Web;

namespace Crowbar.Views
{
    internal class HttpBrowserCapabilitiesStub : HttpBrowserCapabilities
    {
        private readonly ViewSettings settings;

        public HttpBrowserCapabilitiesStub(ViewSettings settings)
        {
            this.settings = settings;
        }

        public override bool IsMobileDevice
        {
            get { return settings.IsMobileDevice; }
        }
    }
}