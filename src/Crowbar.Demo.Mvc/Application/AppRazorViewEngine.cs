using System.Web.Mvc;

namespace Crowbar.Demo.Mvc.Application
{
    public class AppRazorViewEngine : RazorViewEngine
    {
        public AppRazorViewEngine() : this(null) { }

        public AppRazorViewEngine(IViewPageActivator viewPageActivator)
            : base(viewPageActivator)
        {
            ViewLocationFormats = new[] {
                "~/Views/{0}.cshtml",
            };

            MasterLocationFormats = new[] {
                "~/Views/{0}.cshtml",
            };

            PartialViewLocationFormats = new[] {
                "~/Views/{0}.cshtml",
            };

            FileExtensions = new[] {
                "cshtml"
            };
        }
    }
}