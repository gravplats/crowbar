using System.Configuration;
using System.Web.Mvc;
using AttributeRouting.Web.Mvc;
using Crowbar.Mvc.Common;

namespace Crowbar.Web.Core
{
    public class CustomConfigTestController : CrowbarControllerBase
    {
        [GET(CrowbarRoute.CustomConfig)]
        public ActionResult SanityCheck()
        {
            string value = ConfigurationManager.AppSettings["CustomConfig"];
            return Assert(() => !string.IsNullOrWhiteSpace(value));
        }
    }
}