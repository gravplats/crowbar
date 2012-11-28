namespace Crowbar.Mvc.Common
{
    public class Payload
    {
        public string Text { get; set; }
    }

    public class CheckBoxPayload
    {
        public bool Condition { get; set; }

        public string SanityCheck { get; set; }
    }
}