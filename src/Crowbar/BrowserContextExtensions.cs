using System.Web.Helpers;

namespace Crowbar
{
    /// <summary>
    /// Defines extensions for the <see cref="BrowserContext"/> type.
    /// </summary>
    public static class BrowserContextExtensions
    {
        /// <summary>
        /// Adds a header indicating that this is an AJAX request.
        /// </summary>
        /// <param name="context">The <see cref="BrowserContext"/> that this data should be added to.</param>
        public static void AjaxRequest(this BrowserContext context)
        {
            context.Header("X-Requested-With", "XMLHttpRequest");
        }

        /// <summary>
        /// Adds an application/json request body.
        /// </summary>
        /// <param name="context">The <see cref="BrowserContext"/> that this data should be added to.</param>
        /// <param name="model">The model that should be converted to JSON.</param>
        public static void JsonBody(this BrowserContext context, object model)
        {
            string json = Json.Encode(model);
            context.Body(json, "application/json");
        }
    }
}