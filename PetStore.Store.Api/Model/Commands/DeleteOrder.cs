using PetStore.Store.Api.Infrastructure;

namespace PetStore.Store.Api.Model.Commands;

public interface IDeleteOrder
{
    void Execute(int OrderId);
}
    
public class DeleteOrder : IDeleteOrder
{
    private readonly IConnectionFactory _connectionFactory;
        
    public DeleteOrder(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public void Execute(int orderId)
    {
        var sql = @" /* PetStore.Store.Api */
update orders.order set
    Deleted = current_timestamp,
    DeletedBy = 'PetStore.Store.Api',
    IsDelete = true
where id = @Id";

        using var connection = _connectionFactory.Get();
        connection.Execute(sql, new {id = orderId});
    }
}