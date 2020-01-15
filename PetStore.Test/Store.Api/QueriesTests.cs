using FluentAssertions;
using Lamar;
using NUnit.Framework;
using PetStore.Store.Api.Infrastructure;
using PetStore.Store.Api.Model.Queries;

namespace PetStore.Test.Store.Api
{
    public class QueriesTests
    {
        private Container _container;

        [SetUp]
        public void Setup()
        {
            _container = new Container(new ApiRegistry());
        }

        [Test]
        public void Should_get_inventory()
        {
            var getInventory = _container.GetInstance<IGetInventory>();

            var inventory = getInventory.Execute();

            inventory.Should().NotBeNull();
            inventory.Count.Should().BeGreaterThan(0);
        }
        
        [Test]
        public void Should_get_order()
        {
            var getOrder = _container.GetInstance<IGetOrder>();

            var order = getOrder.Execute(1);

            order.Should().NotBeNull();
        }
    }
}