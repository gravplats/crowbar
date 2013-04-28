namespace Crowbar.Tests.Mvc.Common
{
    public static class CrowbarRoute
    {
        public const string AjaxRequest                     = "/ajax-request";

        public const string Cookie                          = "/cookie";
        public const string CustomConfig                    = "/custom-config";

        public const string FormsAuth                       = "/forms-auth";

        public const string ExceptionNonSerializable        = "/exception-non-serializable";
        public const string ExceptionSerializable           = "/exception-serializable";

        public const string Header                          = "/header";
        public const string HttpsRequest                    = "/https-request";

        public const string JsonBody                        = "/json-body";

        public const string MultipleForms                   = "/form-selector";
        
        public const string QueryString                     = "/query-string";

        public const string RedirectPermanent               = "/redirect-permanently";
        public const string RedirectTarget                  = "/redirect-target";
        public const string RedirectTemporary               = "/redirect-temporarily";

        public const string ShouldBeHtml                    = "/should-be-html";
        public const string ShouldBeJson                    = "/should-be-json";
        public const string ShouldBeXml                     = "/should-be-xml";

        public const string SubmitAntiForgeryRequestToken   = "/submit-antiforgeryrequesttoken";
        public const string SubmitCheckBox                  = "/submit-checkbox";
        public const string SubmitDropDown                  = "/submit-dropdown";
        public const string SubmitTextArea                  = "/submit-textarea";
        public const string SubmitTextBox                   = "/submit-textbox";

        public const string XmlBody                         = "/xml-body";

        public static string AsOutbound(this string route, object values = null)
        {
            return new OutboundUrl(route, values);
        }
    }
}