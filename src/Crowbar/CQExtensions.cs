using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CsQuery;

namespace Crowbar
{
    internal static class CQExtensions
    {
        public static Dictionary<string, string> GetFormValues(this CQ form)
        {
            var values = new Dictionary<string, string>();

            foreach (var input in form.Find("input, textarea").ToLookup(x => x.Name, x => x))
            {
                foreach (var dom in input)
                {
                    // Special handling for checkboxes.
                    if (dom.Type == "checkbox" && !dom.Checked)
                    {
                        continue;
                    }

                    // Possible BUG: <textarea /> elements get initial \r\n, thus we're currenly trimming: perhaps only for textareas.
                    values.Add(input.Key, dom.Value.Trim());
                }
            }

            foreach (var select in form.Find("select"))
            {
                var value = CQ.Create(select).Val();
                values.Add(select.Name, value);
            }

            return values;
        }

        // If we're using Html.PasswordFor<TModel, TProperty>() the value will NOT be set thus we need to fake it.
        public static void SetPasswordFields<TViewModel>(this CQ form, TViewModel viewModel)
        {
            var passwords = form.Find("input[type=password]");
            if (passwords.Length > 0)
            {
                var valueMap = new Dictionary<string, KeyValuePair<string, object>>();
                foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(viewModel))
                {
                    string name = property.Name.ToLower();
                    valueMap[name] = new KeyValuePair<string, object>(name, property.GetValue(viewModel));
                }

                foreach (var password in passwords)
                {
                    string name = password.Name.ToLower();
                    if (valueMap.ContainsKey(name))
                    {
                        object value = valueMap[name].Value;
                        password.Value = value.ToString();
                    }
                }
            }
        }
    }
}