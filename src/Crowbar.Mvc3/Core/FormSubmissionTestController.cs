using System.Web.Mvc;
using AttributeRouting.Web.Mvc;
using Crowbar.Mvc.Common;

namespace Crowbar.Web.Core
{
    public class FormSubmissionTestController : CrowbarControllerBase
    {
        [POST(CrowbarRoute.Form)]
        public ActionResult FormSubmission_Post(Payload payload)
        {
            return Assert(() => payload.Text == "text");
        }

        [POST(CrowbarRoute.FormCheckBox)]
        public ActionResult FormSubmissionCheckBox_Post(CheckBoxPayload payload)
        {
            return Assert(() => payload.Condition.ToString() == payload.SanityCheck);
        }

        [POST(CrowbarRoute.FormAntiForgeryRequestToken), ValidateAntiForgeryToken]
        public ActionResult FormSubmissionAntiForgeryRequestToken_Post(Payload payload)
        {
            return Assert(() => payload.Text == "text");
        }
    }
}