using System.Collections.Generic;
using System.Dynamic;
using Crowbar.Mustache;
using NUnit.Framework;
using Nustache.Core;

namespace Crowbar.Tests
{
    [TestFixture]
    public class DynamicMetaObjectProviderValueGetterTests
    {
        public class ReadWriteDynamicObject : DynamicObject
        {
            private readonly Dictionary<string, object> obj = new Dictionary<string, object>();

            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                return obj.TryGetValue(binder.Name, out result);
            }

            public override bool TrySetMember(SetMemberBinder binder, object value)
            {
                obj[binder.Name] = value;
                return true;
            }

            public override IEnumerable<string> GetDynamicMemberNames()
            {
                return obj.Keys;
            }
        }

        [TestFixtureSetUp]
        public void Context()
        {
            ValueGetterFactories.Factories.Add(new DynamicMetaObjectProviderValueGetterFactory());
        }

        [Test]
        public void It_gets_dynamic_members()
        {
            dynamic target = new ReadWriteDynamicObject();
            target.DynamicMember = 123;

            Assert.AreEqual(123, ValueGetter.GetValue(target, "DynamicMember"));
        }

        [Test]
        public void It_gets_case_insensitive_dynamic_members()
        {
            dynamic target = new ReadWriteDynamicObject();
            target.DynamicMember = 123;

            Assert.AreEqual(123, ValueGetter.GetValue(target, "dynamicMember"));
        }
    }
}