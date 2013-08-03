using System.Collections.Generic;
using Crowbar.Mustache;
using NUnit.Framework;

namespace Crowbar.Tests
{
    [TestFixture]
    public class MustacheModelTests
    {
        [Test]
        public void Should_not_fail_if_null_is_passed_to_constructor()
        {
            Assert.DoesNotThrow(() => new MustacheModel(null));
        }

        [Test]
        public void Should_add_properties_from_object_passed_to_constructor()
        {
            dynamic model = new MustacheModel(new { Value = 123 });
            Assert.That(model.Value, Is.EqualTo(123));
        }

        [Test]
        public void Should_add_properties_dynamically()
        {
            dynamic model = new MustacheModel();
            model.Value = 123;

            Assert.That(model.Value, Is.EqualTo(123));
        }

        [Test]
        public void Should_return_dynamic_member_names()
        {
            dynamic model = new MustacheModel();
            model.Value = 123;

            IEnumerable<string> members = model.GetDynamicMemberNames();
            CollectionAssert.AreEquivalent(members, new[] { "Value" });
        }
    }
}