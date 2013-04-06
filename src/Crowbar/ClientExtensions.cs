using System;
using System.IO;
using System.Web;
using CsQuery;

namespace Crowbar
{
    /// <summary>
    /// Provides extra functionality of the <see cref="Client"/> class.
    /// </summary>
    public static class ClientExtensions
    {
        /// <summary>
        /// Loads a rendered form from the server.
        /// </summary>
        /// <param name="client">The <see cref="Client"/> object used to submit the form.</param>
        /// <param name="path">The path that is being requested.</param>
        /// <param name="customize">Customize the request prior to submission.</param>
        /// <returns>A continuation.</returns>
        public static ClientLoadContinuation Load(this Client client, string path, Action<HttpPayload> customize = null)
        {
            var response = client.Get(path, customize);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                string message = string.Format("Could not load resource '{0}' from server: {1}.", path, response.StatusCode);
                throw new InvalidOperationException(message);
            }

            string html = response.ResponseBody;
            return new ClientLoadContinuation(client, html);
        }

        /// <summary>
        /// Renders a form from the server out-of-band.
        /// </summary>
        /// <typeparam name="TViewModel">The type of form payload.</typeparam>
        /// <param name="client">The <see cref="Client"/> object used to submit the form.</param>
        /// <param name="crowbarViewContext">The name of the view which contains the form element that should be submitted.</param>
        /// <param name="viewModel">The form payload.</param>
        /// <returns>A continuation.</returns>
        public static ClientRenderContinuation<TViewModel> Render<TViewModel>(this Client client, CrowbarViewContext crowbarViewContext, TViewModel viewModel)
            where TViewModel : class
        {
            HttpCookieCollection cookies;

            string html = CrowbarController.ToString(crowbarViewContext, viewModel, out cookies);
            return new ClientRenderContinuation<TViewModel>(client, viewModel, html, cookies);
        }

        /// <summary>
        /// Submits a form via an AJAX request in the specified view.
        /// </summary>
        /// <typeparam name="TViewModel">The type of form payload.</typeparam>
        /// <param name="client">The <see cref="Client"/> object used to submit the form.</param>
        /// <param name="html">The HTML that contains the form element that should be submitted.</param>
        /// <param name="viewModel">The form payload (used for overrides).</param>
        /// <param name="customize">Customize the request prior to submission.</param>
        /// <param name="overrides">Modify the form prior to performing the request.</param>
        /// <param name="cookies">Any cookies that should be supplied with the request.</param>
        /// <param name="selector">The first form matching the specified selector will be used for form submission.</param>
        /// <returns>A <see cref="ClientResponse"/> instance of the executed request.</returns>
        public static ClientResponse AjaxSubmit<TViewModel>(this Client client, string html, TViewModel viewModel = null, Action<HttpPayload> customize = null, Action<CQ, TViewModel> overrides = null, HttpCookieCollection cookies = null, string selector = "form")
            where TViewModel : class
        {
            return client.Submit(html, viewModel, As.AjaxRequest.Then(customize), overrides, cookies, selector);
        }

        /// <summary>
        /// Submits a form in a specified view.
        /// </summary>
        /// <typeparam name="TViewModel">The type of form payload.</typeparam>
        /// <param name="client">The <see cref="Client"/> object used to submit the form.</param>
        /// <param name="html">The HTML that contains the form element that should be submitted.</param>
        /// <param name="viewModel">The form payload (used for overrides).</param>
        /// <param name="customize">Customize the request prior to submission.</param>
        /// <param name="overrides">Modify the form prior to performing the request.</param>
        /// <param name="cookies">Any cookies that should be supplied with the request.</param>
        /// <param name="selector">The first form matching the specified selector will be used for form submission.</param>
        /// <returns>A <see cref="ClientResponse"/> instance of the executed request.</returns>
        public static ClientResponse Submit<TViewModel>(this Client client, string html, TViewModel viewModel, Action<HttpPayload> customize = null, Action<CQ, TViewModel> overrides = null, HttpCookieCollection cookies = null, string selector = "form")
            where TViewModel : class
        {
            Ensure.NotNullOrEmpty(html, "html");
            Ensure.NotNullOrEmpty(selector, "selector");

            var document = CQ.Create(html);

            var form = (document.Is(selector) ? document : document.Find(selector)).First();
            if (!form.Is("form") || form.Length == 0)
            {
                using (var writer = new StringWriter())
                {
                    writer.WriteLine("Missing form element in HTML.");
                    writer.WriteLine();
                    writer.WriteLine(html);

                    throw new InvalidOperationException(writer.ToString());
                }
            }

            if (viewModel != null)
            {
                form.SetPasswordFields(viewModel);
                overrides.TryInvoke(form, viewModel);
            }

            string method = form.Attr("method");
            if (string.IsNullOrWhiteSpace(method))
            {
                string message = string.Format("Missing method-attribute for form tag '{0}' in HTML.", form);
                throw new InvalidOperationException(message);
            }

            string action = form.Attr("action");
            if (string.IsNullOrWhiteSpace(action))
            {
                string message = string.Format("Missing action-attribute for form tag '{0}' in HTML.", form);
                throw new InvalidOperationException(message);
            }

            return client.PerformRequest(method, action, payload =>
            {
                if (cookies != null)
                {
                    for (int index = 0; index < cookies.Count; index++)
                    {
                        var cookie = cookies.Get(index);
                        payload.Cookie(cookie);
                    }
                }

                foreach (var formValue in form.GetFormValues())
                {
                    foreach (string value in formValue)
                    {
                        payload.FormValue(formValue.Key, value);
                    }
                }

                customize.TryInvoke(payload);
            });
        }
    }
}