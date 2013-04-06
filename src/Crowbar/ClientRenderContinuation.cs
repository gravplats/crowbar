using System;
using System.Web;
using CsQuery;

namespace Crowbar
{
    /// <summary>
    /// Represents a continuation in a fluent interface.
    /// </summary>
    public class ClientRenderContinuation<TViewModel> : IHideObjectMembers
        where TViewModel : class
    {
        private readonly Client client;
        private readonly TViewModel viewModel;

        /// <summary>
        /// Creates a new instance of <see cref="ClientRenderContinuation{TViewModel}"/>.
        /// </summary>
        /// <param name="client">The <see cref="Client"/> object used to submit the form.</param>
        /// <param name="viewModel">The form payload.</param>
        /// <param name="html">The HTML that should be submitted.</param>
        /// <param name="cookies">The cookie that will be submitted.</param>
        public ClientRenderContinuation(Client client, TViewModel viewModel, string html, HttpCookieCollection cookies)
        {
            this.client = Ensure.NotNull(client, "client");
            this.viewModel = viewModel;
            Html = Ensure.NotNullOrEmpty(html, "html");
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
        /// <returns>A <see cref="ClientResponse"/> instance of the executed request.</returns>
        public ClientResponse AjaxSubmit(Action<HttpPayload> customize = null, Action<CQ, TViewModel> overrides = null, string selector = "form")
        {
            return Submit(As.AjaxRequest.Then(customize), overrides, selector);
        }

        /// <summary>
        /// Submits the form.
        /// </summary>
        /// <param name="customize">Customize the request prior to submission.</param>
        /// <param name="overrides">Modify the form prior to performing the request.</param>
        /// <param name="selector">The first form matching the specified selector will be used for form submission.</param>
        /// <returns>A <see cref="ClientResponse"/> instance of the executed request.</returns>
        public ClientResponse Submit(Action<HttpPayload> customize = null, Action<CQ, TViewModel> overrides = null, string selector = "form")
        {
            return client.Submit(Html, viewModel, customize, overrides, Cookies, selector);
        }
    }
}