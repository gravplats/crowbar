using System.Security.Principal;
using System.Web.Security;

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
            ClientValidationEnabled = true;         // Read from Web.config?
            UnobtrusiveJavaScriptEnabled = true;    // Read from Web.config?
        }

        public bool ClientValidationEnabled { get; set; }
        
        public bool UnobtrusiveJavaScriptEnabled { get; set; }
        
        /// <summary>
        /// Gets or sets the security principal in which the view should be rendered.
        /// </summary>
        public IPrincipal User { get; set; }

        /// <summary>
        /// Gets the name of the partial view that should be rendered.
        /// </summary>
        public string ViewName { get; private set; }

        /// <summary>
        /// Sets the security principal, using forms authentication, in which the view should be rendered.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="timeout">The time, in minutes, for which the forms authentication cookie is valid.</param>
        public void SetFormsAuthPrincipal(string username, int timeout = 30)
        {
            var ticket = new FormsAuthenticationTicket(username, false, timeout);
            User = new GenericPrincipal(new FormsIdentity(ticket), new string[0]);
        }

        public static implicit operator PartialViewContext(string viewName)
        {
            return new PartialViewContext(viewName);
        }
    }
}