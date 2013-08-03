using System.Web;

namespace Crowbar.Mustache
{
    /// <summary>
    /// Provides functionality for working with mustache templates.
    /// </summary>
    public static class BrowserExtensions
    {
        /// <summary>
        /// Renders a form (mustache template) from the server out-of-band.
        /// </summary>
        /// <typeparam name="TViewModel">The type of form payload.</typeparam>
        /// <param name="browser">The <see cref="Client"/> object used to submit the form.</param>
        /// <param name="crowbarViewContext">The name of the view which contains the form element that should be submitted.</param>
        /// <param name="viewModel">The form payload.</param>
        /// <returns>A continuation.</returns>
        public static BrowserMustacheContinuation<TViewModel> Mustache<TViewModel>(this Client browser, CrowbarViewContext crowbarViewContext, TViewModel viewModel)
            where TViewModel : class
        {
            HttpCookieCollection cookies;

            string html = CrowbarController.ToString(crowbarViewContext, viewModel, out cookies);
            return new BrowserMustacheContinuation<TViewModel>(browser, viewModel, html, cookies);
        }
    }
}