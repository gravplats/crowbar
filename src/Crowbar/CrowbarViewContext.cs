using System;
using System.Configuration;
using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace Crowbar
{
    /// <summary>
    /// Represents the context of a view (partial or non-partial) that should be rendered for form submission.
    /// </summary>
    public class CrowbarViewContext
    {
        /// <summary>
        /// Creates a new instance of <see cref="CrowbarViewContext"/>.
        /// </summary>
        /// <param name="viewName">The name of the view that should be rendered.</param>
        public CrowbarViewContext(string viewName)
        {
            ViewName = Ensure.NotNullOrEmpty(viewName, "viewName");

            ClientValidationEnabled = GetBoolFromAppSettings("ClientValidationEnabled", true);
            UnobtrusiveJavaScriptEnabled = GetBoolFromAppSettings("UnobtrusiveJavaScriptEnabled", true);
        }

        /// <summary>
        /// The name of the area.
        /// </summary>
        public string AreaName { get; set; }

        /// <summary>
        /// Gets or sets whether client-side validation should be enabled when rendering the view.
        /// </summary>
        public bool ClientValidationEnabled { get; set; }

        /// <summary>
        /// The name of the controller.
        /// </summary>
        public string ControllerName { get; set; }

        /// <summary>
        /// Gets or sets whether unobtrusive JavaScript should be enabled when rendering the view.
        /// </summary>
        public bool UnobtrusiveJavaScriptEnabled { get; set; }

        /// <summary>
        /// Gets or sets the security principal in which the view should be rendered.
        /// </summary>
        public IPrincipal User { get; set; }

        /// <summary>
        /// Gets the name of the view that should be rendered.
        /// </summary>
        public string ViewName { get; private set; }

        /// <summary>
        /// Gets the route data that will be used by the request context.
        /// </summary>
        /// <returns>The route data that will be used by the request context.</returns>
        public virtual RouteData GetRouteData()
        {
            var routeData = new RouteData();
            routeData.Values["area"] = AreaName;
            // The 'controller' route data value is required by VirtualPathProviderViewEngine.
            routeData.Values["controller"] = (ControllerName ?? typeof(CrowbarController).Name).Replace("Controller", string.Empty);

            return routeData;
        }

        /// <summary>
        /// Finds a view based on the specified view name.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <returns>The view engine result.</returns>
        public virtual ViewEngineResult FindViewEngineResult(ControllerContext controllerContext)
        {
            Ensure.NotNull(controllerContext, "controllerContext");

            string name = ViewName.StartsWith("~")
                ? ViewName.Substring(ViewName.LastIndexOf("/") + 1)
                : ViewName;

            bool isPartialView = name.StartsWith("_");

            var viewEngineResult = isPartialView
                ? ViewEngines.Engines.FindPartialView(controllerContext, ViewName)
                : ViewEngines.Engines.FindView(controllerContext, ViewName, string.Empty);

            if (viewEngineResult == null)
            {
                string message = "The view was not found.";
                throw new ArgumentException(message, ViewName);
            }

            return viewEngineResult;            
        }

        /// <summary>
        /// Sets the security principal to an anonymous user (no username), using a generic identity.
        /// </summary>
        public CrowbarViewContext SetAnonymousPrincipal()
        {
            User = new GenericPrincipal(new GenericIdentity(""), new string[0]);

            return this;
        }

        /// <summary>
        /// Sets the security principal, using forms identity, in which the view should be rendered.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="timeout">The time, in minutes, for which the forms authentication cookie is valid.</param>
        public CrowbarViewContext SetFormsAuthPrincipal(string username, int timeout = 30)
        {
            var ticket = new FormsAuthenticationTicket(username, false, timeout);
            User = new GenericPrincipal(new FormsIdentity(ticket), new string[0]);

            return this;
        }

        /// <summary>
        /// Converts a string to a <see cref="CrowbarViewContext"/> object.
        /// </summary>
        /// <param name="viewName">The name of the view that should be rendered.</param>
        /// <returns></returns>
        public static implicit operator CrowbarViewContext(string viewName)
        {
            return new CrowbarViewContext(viewName);
        }

        private static bool GetBoolFromAppSettings(string name, bool defaultValue)
        {
            string value = ConfigurationManager.AppSettings[name];

            bool result;
            if (string.IsNullOrWhiteSpace(value) || !bool.TryParse(value, out result))
            {
                return defaultValue;
            }

            return result;
        }
    }
}