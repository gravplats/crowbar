using Crowbar.Views;

namespace Crowbar
{
    /// <summary>
    /// Represents the context of a partial view that should be rendered for form submission.
    /// </summary>
    public class PartialViewContext
    {
        public PartialViewContext(string viewName)
        {
            ViewName = viewName;
            ViewSettings = new ViewSettings();
        }

        /// <summary>
        /// Gets the name of the partial view that should be rendered.
        /// </summary>
        public string ViewName { get; private set; }

        /// <summary>
        /// Gets the settings that should be used when rendering the view.
        /// </summary>
        public ViewSettings ViewSettings { get; private set; }

        public static implicit operator PartialViewContext(string viewName)
        {
            return new PartialViewContext(viewName);
        }
    }
}