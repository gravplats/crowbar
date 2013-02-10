using System;
using System.Web;
using CsQuery;

namespace Crowbar
{
    /// <summary>
    /// Represents a continuation in a fluent interface.
    /// </summary>
    public class BrowserLoadContinuation : IHideObjectMembers
    {
        private readonly Browser browser;
        private readonly string html;

        internal BrowserLoadContinuation(Browser browser, string html)
        {
            this.browser = browser;
            this.html = html;
        }

        /// <summary>
        /// Submits the form via an AJAX request.
        /// </summary>
        /// <typeparam name="TViewModel">The type of form payload.</typeparam>
        /// <param name="viewModel">The form payload (used for overrides).</param>
        /// <param name="customize">Customize the request prior to submission.</param>
        /// <param name="overrides">Modify the form prior to performing the request.</param>
        /// <param name="cookies">Any cookies that should be supplied with the request.</param>
        /// <returns>A <see cref="BrowserResponse"/> instance of the executed request.</returns>
        public BrowserResponse AjaxSubmit<TViewModel>(TViewModel viewModel, Action<BrowserContext> customize = null, Action<CQ, TViewModel> overrides = null, HttpCookieCollection cookies = null)
            where TViewModel : class
        {
            Ensure.NotNull(viewModel, "viewModel");
            return browser.AjaxSubmit(html, viewModel, customize, overrides, cookies);
        }

        /// <summary>
        /// Submits the form.
        /// </summary>
        /// <typeparam name="TViewModel">The type of form payload.</typeparam>
        /// <param name="viewModel">The form payload (used for overrides).</param>
        /// <param name="customize">Customize the request prior to submission.</param>
        /// <param name="overrides">Modify the form prior to performing the request.</param>
        /// <param name="cookies">Any cookies that should be supplied with the request.</param>
        /// <returns>A <see cref="BrowserResponse"/> instance of the executed request.</returns>
        public BrowserResponse Submit<TViewModel>(TViewModel viewModel, Action<BrowserContext> customize = null, Action<CQ, TViewModel> overrides = null, HttpCookieCollection cookies = null)
            where TViewModel : class
        {
            Ensure.NotNull(viewModel, "viewModel");
            return browser.Submit(html, viewModel, customize, overrides, cookies);
        }
    }
}