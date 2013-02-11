using System;
using System.IO;
using System.Web;
using Crowbar.Views;
using CsQuery;

namespace Crowbar
{
    /// <summary>
    /// Provides extra functionality of the <see cref="Browser"/> class.
    /// </summary>
    public static class BrowserExtensions
    {
        /// <summary>
        /// Submits a form via an AJAX request in the specified partial view.
        /// </summary>
        /// <typeparam name="TViewModel">The type of form payload.</typeparam>
        /// <param name="browser">The <see cref="Browser"/> object used to submit the form.</param>
        /// <param name="html">The HTML that contains the form element that should be submitted.</param>
        /// <param name="viewModel">The form payload (used for overrides).</param>
        /// <param name="customize">Customize the request prior to submission.</param>
        /// <param name="overrides">Modify the form prior to performing the request.</param>
        /// <param name="cookies">Any cookies that should be supplied with the request.</param>
        /// <returns>A <see cref="BrowserResponse"/> instance of the executed request.</returns>
        public static BrowserResponse AjaxSubmit<TViewModel>(this Browser browser, string html, TViewModel viewModel = null, Action<BrowserContext> customize = null, Action<CQ, TViewModel> overrides = null, HttpCookieCollection cookies = null)
            where TViewModel : class
        {
            return browser.Submit(html, viewModel, As.AjaxRequest.Then(customize), overrides, cookies);
        }

        /// <summary>
        /// Loads a rendered form from the server.
        /// </summary>
        /// <param name="browser">The <see cref="Browser"/> object used to submit the form.</param>
        /// <param name="path">The path that is being requested.</param>
        /// <param name="customize">Customize the request prior to submission.</param>
        /// <returns>A continuation.</returns>
        public static BrowserLoadContinuation Load(this Browser browser, string path, Action<BrowserContext> customize = null)
        {
            var response = browser.Get(path, customize);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                string message = string.Format("Could not load resource '{0}' from server: {1}.", path, response.StatusCode);
                throw new InvalidOperationException(message);
            }

            string html = response.ResponseBody;
            return new BrowserLoadContinuation(browser, html);
        }

        /// <summary>
        /// Renders a form from the server out-of-band.
        /// </summary>
        /// <typeparam name="TViewModel">The type of form payload.</typeparam>
        /// <param name="browser">The <see cref="Browser"/> object used to submit the form.</param>
        /// <param name="partialViewContext">The name of the partial view which contains the form element that should be submitted.</param>
        /// <param name="viewModel">The form payload.</param>
        /// <returns>A continuation.</returns>
        public static BrowserRenderContinuation<TViewModel> Render<TViewModel>(this Browser browser, PartialViewContext partialViewContext, TViewModel viewModel)
            where TViewModel : class
        {
            HttpCookieCollection cookies;

            string html = CrowbarController.ToString(partialViewContext, viewModel, out cookies);
            return new BrowserRenderContinuation<TViewModel>(browser, viewModel, html, cookies);
        }

        /// <summary>
        /// Submits a form in a specified partial view.
        /// </summary>
        /// <typeparam name="TViewModel">The type of form payload.</typeparam>
        /// <param name="browser">The <see cref="Browser"/> object used to submit the form.</param>
        /// <param name="html">The HTML that contains the form element that should be submitted.</param>
        /// <param name="viewModel">The form payload (used for overrides).</param>
        /// <param name="customize">Customize the request prior to submission.</param>
        /// <param name="overrides">Modify the form prior to performing the request.</param>
        /// <param name="cookies">Any cookies that should be supplied with the request.</param>
        /// <returns>A <see cref="BrowserResponse"/> instance of the executed request.</returns>
        public static BrowserResponse Submit<TViewModel>(this Browser browser, string html, TViewModel viewModel, Action<BrowserContext> customize = null, Action<CQ, TViewModel> overrides = null, HttpCookieCollection cookies = null)
            where TViewModel : class
        {
            if (string.IsNullOrWhiteSpace(html))
            {
                throw new ArgumentException("Cannot be null or empty.", "html");
            }

            var document = CQ.Create(html);

            var form = document.Is("form") ? document : document.Find("form").First();
            if (form.Length == 0)
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

            return browser.PerformRequest(method, action, ctx =>
            {
                if (cookies != null)
                {
                    for (int index = 0; index < cookies.Count; index++)
                    {
                        var cookie = cookies.Get(index);
                        ctx.Cookie(cookie);
                    }
                }

                foreach (var formValue in form.GetFormValues())
                {
                    foreach (string value in formValue)
                    {
                        ctx.FormValue(formValue.Key, value);
                    }
                }

                customize.TryInvoke(ctx);
            });
        }
    }
}