using System;
using System.Web;
using CsQuery;

namespace Crowbar
{
    /// <summary>
    /// Represents a continuation in a fluent interface.
    /// </summary>
    public class ClientLoadContinuation : IHideObjectMembers
    {
        private readonly Client client;

        /// <summary>
        /// Creates a new instance of <see cref="ClientLoadContinuation"/>.
        /// </summary>
        /// <param name="client">The <see cref="Client"/> object used to submit the form.</param>
        /// <param name="html">The HTML that should be submitted.</param>
        public ClientLoadContinuation(Client client, string html)
        {
            this.client = Ensure.NotNull(client, "client");
            Html = Ensure.NotNullOrEmpty(html, "html");
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
        /// <returns>A <see cref="ClientResponse"/> instance of the executed request.</returns>
        public ClientResponse AjaxSubmit<TViewModel>(TViewModel viewModel, Action<HttpPayload> customize = null, Action<CQ, TViewModel> overrides = null, HttpCookieCollection cookies = null, string selector = "form")
            where TViewModel : class
        {
            Ensure.NotNull(viewModel, "viewModel");
            return client.AjaxSubmit(Html, viewModel, customize, overrides, cookies, selector);
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
        /// <returns>A <see cref="ClientResponse"/> instance of the executed request.</returns>
        public ClientResponse Submit<TViewModel>(TViewModel viewModel, Action<HttpPayload> customize = null, Action<CQ, TViewModel> overrides = null, HttpCookieCollection cookies = null, string selector = "form")
            where TViewModel : class
        {
            Ensure.NotNull(viewModel, "viewModel");
            return client.Submit(Html, viewModel, customize, overrides, cookies, selector);
        }
    }
}