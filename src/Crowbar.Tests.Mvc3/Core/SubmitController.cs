using System.Collections.Generic;
using System.Web.Mvc;
using AttributeRouting.Web.Mvc;
using Crowbar.Tests.Mvc.Common;

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

        [GET(CrowbarRoute.SubmitCheckBox)]
        public ActionResult SubmitCheckBox_Get()
        {
            return PartialView("_FormCheckBox");
        }

        [POST(CrowbarRoute.SubmitDropDown)]
        public ActionResult SubmitDropDown_Post(DropDownPayload payload)
        {
            return Assert(() => payload.Value == "2");
        }

        [GET(CrowbarRoute.SubmitDropDown)]
        public ActionResult SubmitDropDown_Get()
        {
            var model = new DropDownPayload
            {
                Values = new List<DropDownPayload.DropDownItem>
                {
                    new DropDownPayload.DropDownItem
                    {
                        Text = "black",
                        Value = "1"
                    },
                    new DropDownPayload.DropDownItem
                    {
                        Text = "white",
                        Value = "2"
                    },
                }
            };

            return PartialView("_FormDropDown", model);
        }

        [POST(CrowbarRoute.SubmitTextArea)]
        public ActionResult SubmitTextArea_Post(TextBoxPayload payload)
        {
            return Assert(() => payload.Text == "text");
        }

        [GET(CrowbarRoute.SubmitTextArea)]
        public ActionResult SubmitTextArea_Get()
        {
            return PartialView("_FormTextArea");
        }

        [POST(CrowbarRoute.SubmitTextBox)]
        public ActionResult SubmitTextBox_Post(TextBoxPayload payload)
        {
            return Assert(() => payload.Text == "text");
        }

        [GET(CrowbarRoute.SubmitTextBox)]
        public ActionResult SubmitTextBox_Get(TextBoxPayload payload)
        {
            return PartialView("_FormTextBox");
        }

        [POST(CrowbarRoute.MultipleForms)]
        public ActionResult FormSelector_Post(MultipleFormsPayload payload)
        {
            return Assert(() => payload.Form1 == null && payload.Form2 == "form2");
        }

        [GET(CrowbarRoute.MultipleForms)]
        public ActionResult FormSelector_Get()
        {
            return PartialView("_MultipleForms");
        }
    }
}