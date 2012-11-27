using System;
using System.Web;
using Crowbar.Views;
using CsQuery;

namespace Crowbar
{
    public static class BrowserExtensions
    {
        /// <summary>
        /// Submits a form via an AJAX request in the specified partial view.
        /// </summary>
        /// <typeparam name="TViewModel">The type of form payload.</typeparam>
        /// <param name="browser">The <see cref="Browser"/> object used to submit the form.</param>
        /// <param name="partialViewContext">The name of the partial view which contains the form element that should be submitted.</param>
        /// <param name="viewModel">The form payload.</param>
        /// <param name="customize">Customize the request prior to submission.</param>
        /// <param name="modify">Modify the form prior to performing the request.</param>
        /// <returns>A <see cref="BrowserResponse"/> instance of the executed request.</returns>
        public static BrowserResponse AjaxSubmit<TViewModel>(this Browser browser, PartialViewContext partialViewContext, TViewModel viewModel, Action<BrowserContext> customize = null, Action<CQ, TViewModel> modify = null)
            where TViewModel : class
        {
            Action<BrowserContext> context = ctx =>
            {
                ctx.AjaxRequest();

                if (customize != null)
                {
                    customize(ctx);
                }
            };

            return browser.Submit(partialViewContext, viewModel, context, modify);
        }

        /// <summary>
        /// Submits a form in a specified partial view.
        /// </summary>
        /// <typeparam name="TViewModel">The type of form payload.</typeparam>
        /// <param name="browser">The <see cref="Browser"/> object used to submit the form.</param>
        /// <param name="partialViewContext">The name of the partial view which contains the form element that should be submitted.</param>
        /// <param name="viewModel">The form payload.</param>
        /// <param name="customize">Customize the request prior to submission.</param>
        /// <param name="modify">Modify the form prior to performing the request.</param>
        /// <returns>A <see cref="BrowserResponse"/> instance of the executed request.</returns>
        public static BrowserResponse Submit<TViewModel>(this Browser browser, PartialViewContext partialViewContext, TViewModel viewModel, Action<BrowserContext> customize = null, Action<CQ, TViewModel> modify = null)
            where TViewModel : class
        {
            HttpCookieCollection cookies;

            string html = CrowbarController.ToString(partialViewContext, viewModel, out cookies);
            var document = CQ.Create(html);

            var form = document.Is("form") ? document : document.Find("form").First();
            if (form.Length == 0)
            {
                throw new InvalidOperationException("Missing form element in HTML.");
            }

            form.SetPasswordFields(viewModel);

            if (modify != null)
            {
                modify(form, viewModel);
            }

            string method = form.Attr("method");
            if (string.IsNullOrWhiteSpace(method))
            {
                throw new InvalidOperationException("Missing method attribute for form tag in HTML.");
            }

            string action = form.Attr("action");
            if (string.IsNullOrWhiteSpace(action))
            {
                throw new ArgumentException("Missing action attribute for form tag in HTML.");
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

                foreach (var value in form.GetFormValues())
                {
                    ctx.FormValue(value.Key, value.Value);
                }

                if (customize != null)
                {
                    customize(ctx);
                }
            });
        }
    }
}