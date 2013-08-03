using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using Nustache.Core;

namespace Crowbar.Mustache
{
    /// <summary>
    /// Provides functionality for reading values from classes which implement <see cref="IDynamicMetaObjectProvider"/>.
    /// </summary>
    public class DynamicMetaObjectProviderValueGetterFactory : ValueGetterFactory
    {
        /// <inheritdoc />
        public override ValueGetter GetValueGetter(object target, string name)
        {
            var provider = target as IDynamicMetaObjectProvider;
            if (provider != null)
            {
                var members = provider.GetMetaObject(Expression.Constant(target)).GetDynamicMemberNames();
                string result = members.FirstOrDefault(x => string.Equals(x, name, DefaultNameComparison));

                if (result != null)
                {
                    return new DynamicMetaObjectProviderValueGetter(provider, result);
                }
            }

            return null;
        }
    }
}