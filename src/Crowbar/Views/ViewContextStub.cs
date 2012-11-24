using System.IO;
using System.Web.Mvc;

namespace Crowbar.Views
{
    internal class ViewContextStub : ViewContext
    {
        public ViewContextStub(ControllerContext controllerContext, IView view, ViewDataDictionary viewData, TempDataDictionary tempData, TextWriter writer)
            : base(controllerContext, view, viewData, tempData, writer) { }

        public override bool ClientValidationEnabled { get; set; }

        public override bool UnobtrusiveJavaScriptEnabled { get; set; }
    }
}