namespace Crowbar.Views
{
    public class ViewSettings
    {
        public ViewSettings()
        {
            ClientValidationEnabled = true;         // Read from Web.config?
            IsMobileDevice = false;
            UnobtrusiveJavaScriptEnabled = true;    // Read from Web.config?
        }

        public bool ClientValidationEnabled { get; set; }
        public bool IsMobileDevice { get; set; }
        public bool UnobtrusiveJavaScriptEnabled { get; set; }
    }
}