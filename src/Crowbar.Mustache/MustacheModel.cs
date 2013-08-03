using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;

namespace Crowbar.Mustache
{
    /// <summary>
    /// A dynamic mustache model.
    /// </summary>
    public class MustacheModel : DynamicObject
    {
        private readonly Dictionary<string, object> obj = new Dictionary<string, object>();

        /// <summary>
        /// Creates a new instance of <see cref="MustacheModel"/>.
        /// </summary>
        /// <param name="data">Initial data, if any, that should be added to the model.</param>
        public MustacheModel(object data = null)
        {
            if (data == null)
            {
                return;
            }

            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(data))
            {
                obj[property.Name] = property.GetValue(data);
            }
        }

        /// <inheritdoc />
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return obj.TryGetValue(binder.Name, out result);
        }

        /// <inheritdoc />
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            obj[binder.Name] = value;
            return true;
        }

        /// <inheritdoc />
        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return obj.Keys;
        }
    }
}