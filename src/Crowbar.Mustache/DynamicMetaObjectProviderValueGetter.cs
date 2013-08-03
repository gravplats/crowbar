using System;
using System.Dynamic;
using System.Runtime.CompilerServices;
using Microsoft.CSharp.RuntimeBinder;
using Nustache.Core;

namespace Crowbar.Mustache
{
    /// <summary>
    /// Provides functionality for reading values from classes which implement <see cref="IDynamicMetaObjectProvider"/>.
    /// </summary>
    public class DynamicMetaObjectProviderValueGetter : ValueGetter
    {
        private readonly IDynamicMetaObjectProvider target;
        private readonly string name;

        internal DynamicMetaObjectProviderValueGetter(IDynamicMetaObjectProvider target, string name)
        {
            this.target = target;
            this.name = name;
        }

        /// <inheritdoc />
        public override object GetValue()
        {
            var binder = Binder.GetMember(CSharpBinderFlags.None, name, target.GetType(), new[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) });

            var callsite = CallSite<Func<CallSite, object, object>>.Create(binder);
            return callsite.Target(callsite, target);
        }
    }
}