using System;
using System.Linq;
using System.Web;
using CsQuery;
using Nustache.Core;

namespace Crowbar.Mustache
{
    /// <summary>
    /// Represents a continuation in a fluent interface.
    /// </summary>
    public class BrowserMustacheContinuation<TViewModel>
        where TViewModel : class
    {
        private readonly Client browser;
        private readonly TViewModel viewModel;

        internal BrowserMustacheContinuation(Client browser, TViewModel viewModel, string template, HttpCookieCollection cookies)
        {
            this.browser = browser;
            this.viewModel = viewModel;
            Template = template;
            Cookies = cookies;
        }

        /// <summary>
        /// The cookies that will be submitted.
        /// </summary>
        public HttpCookieCollection Cookies { get; private set; }

        /// <summary>
        /// The mustache template.
        /// </summary>
        public string Template { get; private set; }

        /// <summary>
        /// Submits the form via an AJAX request.
        /// </summary>
        /// <param name="customize">Customize the request prior to submission.</param>
        /// <param name="overrides">Modify the form prior to performing the request.</param>
        /// <param name="selector">The first form matching the specified selector will be used for form submission.</param>
        /// <returns>A <see cref="HttpPayload"/> instance of the executed request.</returns>
        public ClientResponse AjaxSubmit(Action<HttpPayload> customize = null, Action<CQ, TViewModel> overrides = null, string selector = "form")
        {
            var context = As.AjaxRequest.Then(customize);
            return Submit(context, overrides, selector);
        }

        /// <summary>
        /// Submits the form.
        /// </summary>
        /// <param name="customize">Customize the request prior to submission.</param>
        /// <param name="overrides">Modify the form prior to performing the request.</param>
        /// <param name="selector">The first form matching the specified selector will be used for form submission.</param>
        /// <returns>A <see cref="HttpPayload"/> instance of the executed request.</returns>
        public ClientResponse Submit(Action<HttpPayload> customize = null, Action<CQ, TViewModel> overrides = null, string selector = "form")
        {
            string html = GetHtml();
            return browser.Submit(html, viewModel, customize, overrides, Cookies, selector);
        }

        /// <summary>
        /// Converts the template into HTML.
        /// </summary>
        /// <returns>The HTML.</returns>
        protected virtual string GetHtml()
        {
            if (!ValueGetterFactories.Factories.OfType<DynamicMetaObjectProviderValueGetterFactory>().Any())
            {
                ValueGetterFactories.Factories.Add(new DynamicMetaObjectProviderValueGetterFactory());
            }

            return Render.StringToString(Template, viewModel);
        }
    }
}