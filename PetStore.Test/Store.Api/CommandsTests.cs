using PetStore.Store.Api.Infrastructure;
using PetStore.Store.Api.Model;
using PetStore.Store.Api.Model.Commands;

namespace PetStore.Test.Store.Api;

public class CommandsTests
{
    private Container _container;

    [SetUp]
    public void Setup()
    {
        _container = new Container(new ApiRegistry());
    }

    [Test]
    public void Should_delete_order()
    {
        var deleteOrder = _container.GetInstance<IDeleteOrder>();

        deleteOrder.Execute(1);
    }
        
    [Test]
    public void Should_insert_order()
    {
        var insertOrder = _container.GetInstance<IInsertOrder>();

        var order = new Order
        {
            Complete = true,
            Quantity = 1,
            Status = OrderStatus.Shipped,
            ShipDate = DateTime.Now.AddDays(-19),
            PetId = 123
        };
            
        var petId = insertOrder.Execute(order);

        petId.Should().NotBe(0);
    }
}