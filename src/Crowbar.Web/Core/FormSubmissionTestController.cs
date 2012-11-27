using System.Web.Mvc;
using AttributeRouting.Web.Mvc;

namespace Crowbar.Web.Core
{
    public class FormSubmissionTestController : CrowbarControllerBase
    {
        public class Payload
        {
            public string Text { get; set; }
        }

        [POST(CrowbarRoute.Form)]
        public ActionResult FormSubmission_Post(Payload payload)
        {
            return Assert(() => payload.Text == "text");
        }
        
        [POST(CrowbarRoute.FormAntiForgeryRequestToken), ValidateAntiForgeryToken]
        public ActionResult FormSubmissionAntiForgeryRequestToken_Post(Payload payload)
        {
            return Assert(() => payload.Text == "text");
        }
    }
}