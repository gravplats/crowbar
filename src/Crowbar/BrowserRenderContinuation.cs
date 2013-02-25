using System;
using System.Web;
using CsQuery;

namespace Crowbar
{
    /// <summary>
    /// Represents a continuation in a fluent interface.
    /// </summary>
    public class BrowserRenderContinuation<TViewModel> : IHideObjectMembers
        where TViewModel : class
    {
        private readonly Browser browser;
        private readonly TViewModel viewModel;

        internal BrowserRenderContinuation(Browser browser, TViewModel viewModel, string html, HttpCookieCollection cookies)
        {
            this.browser = browser;
            this.viewModel = viewModel;
            Html = html;
            Cookies = cookies;
        }

        /// <summary>
        /// The cookies that will be submitted.
        /// </summary>
        public HttpCookieCollection Cookies { get; private set; }

        /// <summary>
        /// The HTML that will be submitted.
        /// </summary>
        public string Html { get; private set; }

        /// <summary>
        /// Submits the form via an AJAX request.
        /// </summary>
        /// <param name="customize">Customize the request prior to submission.</param>
        /// <param name="overrides">Modify the form prior to performing the request.</param>
        /// <param name="selector">The first form matching the specified selector will be used for form submission.</param>/// 
        /// <returns>A <see cref="BrowserResponse"/> instance of the executed request.</returns>
        public BrowserResponse AjaxSubmit(Action<BrowserContext> customize = null, Action<CQ, TViewModel> overrides = null, string selector = "form")
        {
            return Submit(As.AjaxRequest.Then(customize), overrides, selector);
        }

        /// <summary>
        /// Submits the form.
        /// </summary>
        /// <param name="customize">Customize the request prior to submission.</param>
        /// <param name="overrides">Modify the form prior to performing the request.</param>
        /// <param name="selector">The first form matching the specified selector will be used for form submission.</param>
        /// <returns>A <see cref="BrowserResponse"/> instance of the executed request.</returns>
        public BrowserResponse Submit(Action<BrowserContext> customize = null, Action<CQ, TViewModel> overrides = null, string selector = "form")
        {
            return browser.Submit(Html, viewModel, customize, overrides, Cookies, selector);
        }
    }
}