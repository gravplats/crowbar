using System.Configuration;
using System.Web.Mvc;
using AttributeRouting.Web.Mvc;
using Crowbar.Tests.Mvc.Common;

namespace Crowbar.Web.Core
{
    public class CustomConfigController : CrowbarControllerBase
    {
        [GET(CrowbarRoute.CustomConfig)]
        public ActionResult CustomConfig()
        {
            string value = ConfigurationManager.AppSettings["CustomConfig"];
            return Assert(() => !string.IsNullOrWhiteSpace(value));
        }
    }
}