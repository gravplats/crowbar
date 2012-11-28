using System.Web.Mvc;
using AttributeRouting.Web.Mvc;
using Crowbar.Mvc.Common;

namespace Crowbar.Web.Core
{
    public class SubmitController : CrowbarControllerBase
    {
        [POST(CrowbarRoute.SubmitAntiForgeryRequestToken), ValidateAntiForgeryToken]
        public ActionResult SubmitAntiForgeryRequestToken_Post(TextBoxPayload payload)
        {
            return Assert(() => payload.Text == "text");
        }

        [POST(CrowbarRoute.SubmitCheckBox)]
        public ActionResult SubmitCheckBox_Post(CheckBoxPayload payload)
        {
            return Assert(() => payload.Condition.ToString() == payload.SanityCheck);
        }
        
        [POST(CrowbarRoute.SubmitDropDown)]
        public ActionResult SubmitDropDown_Post(DropDownPayload payload)
        {
            return Assert(() => payload.Value == "2");
        }

        [POST(CrowbarRoute.SubmitTextBox)]
        public ActionResult SubmitTextBox_Post(TextBoxPayload payload)
        {
            return Assert(() => payload.Text == "text");
        }
    }
}