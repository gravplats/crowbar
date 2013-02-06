using System;
using System.Web;
using Crowbar.Views;
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
        private readonly PartialViewContext partialViewContext;
        private readonly TViewModel viewModel;

        internal BrowserRenderContinuation(Browser browser, PartialViewContext partialViewContext, TViewModel viewModel)
        {
            this.browser = browser;
            this.partialViewContext = partialViewContext;
            this.viewModel = viewModel;
        }

        /// <summary>
        /// Submits the form via an AJAX request.
        /// </summary>
        /// <param name="customize">Customize the request prior to submission.</param>
        /// <param name="overrides">Modify the form prior to performing the request.</param>
        /// <returns>A <see cref="BrowserResponse"/> instance of the executed request.</returns>
        public BrowserResponse AjaxSubmit(Action<BrowserContext> customize = null, Action<CQ, TViewModel> overrides = null)
        {
            return Submit(As.AjaxRequest.Then(customize), overrides);
        }

        /// <summary>
        /// Submits the form.
        /// </summary>
        /// <param name="customize">Customize the request prior to submission.</param>
        /// <param name="overrides">Modify the form prior to performing the request.</param>
        /// <returns>A <see cref="BrowserResponse"/> instance of the executed request.</returns>
        public BrowserResponse Submit(Action<BrowserContext> customize = null, Action<CQ, TViewModel> overrides = null)
        {
            HttpCookieCollection cookies;

            string html = CrowbarController.ToString(partialViewContext, viewModel, out cookies);
            return browser.Submit(html, viewModel, customize, overrides, cookies);
        }
    }
}