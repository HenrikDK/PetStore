using PetStore.Store.Api.Infrastructure;

namespace PetStore.Store.Api.Model.Queries;

public interface IGetOrder
{
    Order Execute(int orderId);
}
    
public class GetOrder : IGetOrder
{
    private readonly IConnectionFactory _connectionFactory;
        
    public GetOrder(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public Order Execute(int orderId)
    {
        var sql = @" /* PetStore.Store.Api */
select o.Id, o.Status, o.PetId, o.Quantity, o.ShipDate, o.Complete 
from orders.order o
where o.IsDelete = false
and o.id = @id";

        using var connection = _connectionFactory.Get();
        return connection.Query<Order>(sql, new {id = orderId}).FirstOrDefault();
    }
}