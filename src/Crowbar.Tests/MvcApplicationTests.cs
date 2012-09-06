using System;
using Crowbar.Mvc;
using NUnit.Framework;
using Raven.Client;

namespace Crowbar.Tests
{
    public class MvcApplicationTests
    {
        public class NonSerializableDocumentStoreBuilder : IDocumentStoreBuilder
        {
            public IDocumentStore Build()
            {
                return new DefaultDocumentStoreBuilder().Build();
            }
        }

        [Serializable]
        public class SerializableDocumentStoreBuilder : IDocumentStoreBuilder
        {
            public IDocumentStore Build()
            {
                return new DefaultDocumentStoreBuilder().Build();
            }
        }

        [Test]
        public void Should_throw_if_custom_document_store_builder_does_not_implement_serializable()
        {
            Assert.Throws<InvalidOperationException>(() => MvcApplication.Create("Crowbar.Web", "Web.Custom.config", new NonSerializableDocumentStoreBuilder()));
        }

        [Test]
        public void Should_not_throw_if_custom_document_store_builder_implements_serializable()
        {
            Assert.DoesNotThrow(() => MvcApplication.Create("Crowbar.Web", "Web.Custom.config", new SerializableDocumentStoreBuilder()));
        }
    }
}