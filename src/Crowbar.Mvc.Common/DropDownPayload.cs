using System.Collections.Generic;

namespace Crowbar.Mvc.Common
{
    public class DropDownPayload
    {
        public class DropDownItem
        {
            public bool Selected { get; set; }
            public string Text { get; set; }
            public string Value { get; set; }
        }

        public string Value { get; set; }
        public List<DropDownItem> Values { get; set; }
    }
}