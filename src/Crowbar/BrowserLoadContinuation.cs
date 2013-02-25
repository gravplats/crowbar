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

        internal BrowserLoadContinuation(Browser browser, string html)
        {
            this.browser = browser;
            Html = html;
        }

        /// <summary>
        /// The HTML that will be submitted.
        /// </summary>
        public string Html { get; private set; }

        /// <summary>
        /// Submits the form via an AJAX request.
        /// </summary>
        /// <typeparam name="TViewModel">The type of form payload.</typeparam>
        /// <param name="viewModel">The form payload (used for overrides).</param>
        /// <param name="customize">Customize the request prior to submission.</param>
        /// <param name="overrides">Modify the form prior to performing the request.</param>
        /// <param name="cookies">Any cookies that should be supplied with the request.</param>
        /// <param name="selector">The first form matching the specified selector will be used for form submission.</param>
        /// <returns>A <see cref="BrowserResponse"/> instance of the executed request.</returns>
        public BrowserResponse AjaxSubmit<TViewModel>(TViewModel viewModel, Action<BrowserContext> customize = null, Action<CQ, TViewModel> overrides = null, HttpCookieCollection cookies = null, string selector = "form")
            where TViewModel : class
        {
            Ensure.NotNull(viewModel, "viewModel");
            return browser.AjaxSubmit(Html, viewModel, customize, overrides, cookies, selector);
        }

        /// <summary>
        /// Submits the form.
        /// </summary>
        /// <typeparam name="TViewModel">The type of form payload.</typeparam>
        /// <param name="viewModel">The form payload (used for overrides).</param>
        /// <param name="customize">Customize the request prior to submission.</param>
        /// <param name="overrides">Modify the form prior to performing the request.</param>
        /// <param name="cookies">Any cookies that should be supplied with the request.</param>
        /// <param name="selector">The first form matching the specified selector will be used for form submission.</param>
        /// <returns>A <see cref="BrowserResponse"/> instance of the executed request.</returns>
        public BrowserResponse Submit<TViewModel>(TViewModel viewModel, Action<BrowserContext> customize = null, Action<CQ, TViewModel> overrides = null, HttpCookieCollection cookies = null, string selector = "form")
            where TViewModel : class
        {
            Ensure.NotNull(viewModel, "viewModel");
            return browser.Submit(Html, viewModel, customize, overrides, cookies, selector);
        }
    }
}