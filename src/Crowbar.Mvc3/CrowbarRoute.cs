namespace Crowbar.Web
{
    public static class CrowbarRoute
    {
        public const string Ajax                        = "/ajax";
        public const string AntiForgeryToken            = "/token";
        public const string Authentication              = "/authentication";

        public const string CsQuery                     = "/csquery";
        public const string CustomConfig                = "/config";

        public const string DocumentStore               = "/store";

        public const string ExceptionNonSerializable    = "/exception-ns";
        public const string ExceptionSerializable       = "/exception";

        public const string Form                        = "/form";
        public const string FormAntiForgeryRequestToken = "/formantiforgeryrequesttoken";

        public const string JsonResponse                = "/jsonresponse";
        public const string JsonRequest                 = "/jsonrequest";

        public const string Query                       = "/query";

        public const string Redirected                  = "/redirected";
        public const string RedirectPermanent           = "/redirectperm";
        public const string RedirectTemporary           = "/redirecttemp";
        public const string Root                        = "/";

        public const string Secure                      = "/secure";

        public const string XmlResponse                 = "/xmlresponse";
        public const string XmlRequest                  = "/xmlrequest";

        public static string AsOutbound(this string route, object values = null)
        {
            return new OutboundUrl(route, values);
        }
    }
}