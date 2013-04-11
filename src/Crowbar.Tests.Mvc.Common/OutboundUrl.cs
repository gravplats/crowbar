using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Crowbar.Tests.Mvc.Common
{
    public class OutboundUrl
    {
        public class UrlPathCollection : Collection<string>
        {
            private static readonly Regex SafeUrlPattern = new Regex("[^0-9a-z\\-]", RegexOptions.IgnoreCase | RegexOptions.Compiled);

            public UrlPathCollection(string virtualPath)
            {
                if (virtualPath != "/")
                {
                    Items.Add(virtualPath);
                }
            }

            protected override void InsertItem(int index, string item)
            {
                base.InsertItem(index, ToSafeUrl(item));
            }

            public override string ToString()
            {
                return string.Join("/", Items);
            }

            public static implicit operator string(UrlPathCollection collection)
            {
                return collection.ToString();
            }

            private static string ToSafeUrl(string value)
            {
                value = (value ?? string.Empty).Trim().Replace(" ", "-");
                return SafeUrlPattern.Replace(value, string.Empty).ToLower();
            }
        }

        public class QueryString
        {
            private readonly string queryString;

            public QueryString(ICollection<KeyValuePair<string, object>> values)
            {
                if (values.Count == 0)
                {
                    queryString = "";
                }
                else
                {
                    var pairs = values.Select(x => string.Format("{0}={1}", x.Key, HttpUtility.UrlEncode((string)x.Value.ToString())));
                    queryString = "?" + string.Join("&", pairs);
                }
            }

            public override string ToString()
            {
                return queryString;
            }
        }

        private static readonly Regex IsOptionalParameter = new Regex("{(?<key>.*)\\?}", RegexOptions.Compiled);
        private static readonly Regex IsMandatoryParameter = new Regex("{(?<key>.*)}", RegexOptions.Compiled);

        private readonly string route;
        private readonly object values;

        public OutboundUrl(string route, object values = null)
        {
            this.route = route;
            this.values = values ?? new { };
        }

        protected virtual string VirtualPath
        {
            get
            {
                // HttpContext is null in user stories... perhaps those url:s should be generated differently.
                var context = HttpContext.Current;
                if (context == null)
                {
                    return string.Empty;
                }

                return context.Request.ApplicationPath;
            }
        }

        public override string ToString()
        {
            var valueMap = new Dictionary<string, KeyValuePair<string, object>>();
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(values))
            {
                string name = property.Name;
                valueMap[name.ToLower()] = new KeyValuePair<string, object>(name, property.GetValue(values));
            }

            var path = new UrlPathCollection(VirtualPath);

            Action<string> addToPath = key =>
            {
                string value = valueMap[key].Value.ToString();
                path.Add(value);

                valueMap.Remove(key);
            };

            string[] parameters = route.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string parameter in parameters)
            {
                Match match;

                match = IsOptionalParameter.Match(parameter);
                if (match.Success)
                {
                    string key = match.Groups["key"].Value;
                    if (valueMap.ContainsKey(key))
                    {
                        addToPath(key);
                    }

                    continue;
                }

                match = IsMandatoryParameter.Match(parameter);
                if (match.Success)
                {
                    string key = match.Groups["key"].Value;
                    addToPath(key);

                    continue;
                }

                path.Add(parameter);
            }

            var queryString = new QueryString(valueMap.Values);
            return Path.Combine("/", path) + queryString;
        }

        public static implicit operator string(OutboundUrl url)
        {
            return url.ToString();
        }
    }
}