using System.Security.Principal;
using System.Web.Security;
using Crowbar.Views;

namespace Crowbar
{
    /// <summary>
    /// Represents the context of a partial view that should be rendered for form submission.
    /// </summary>
    public class PartialViewContext
    {
        public PartialViewContext(string viewName)
        {
            ViewName = viewName;
            ViewSettings = new ViewSettings();
        }

        /// <summary>
        /// Gets the name of the partial view that should be rendered.
        /// </summary>
        public string ViewName { get; private set; }

        /// <summary>
        /// Gets the settings that should be used when rendering the view.
        /// </summary>
        public ViewSettings ViewSettings { get; private set; }

        /// <summary>
        /// Sets the security principal, using forms authentication, in which the view should be rendered.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="timeout">The time, in minutes, for which the forms authentication cookie is valid.</param>
        public void SetFormsAuthPrincipal(string username, int timeout = 30)
        {
            var ticket = new FormsAuthenticationTicket(username, false, timeout);
            ViewSettings.User = new GenericPrincipal(new FormsIdentity(ticket), new string[0]);
        }

        public static implicit operator PartialViewContext(string viewName)
        {
            return new PartialViewContext(viewName);
        }
    }
}