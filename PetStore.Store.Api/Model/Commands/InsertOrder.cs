using Dapper;
using PetStore.Store.Api.Infrastructure;

namespace PetStore.Store.Api.Model.Commands
{
    public interface IInsertOrder
    {
        int Execute(Order order);
    }
    
    public class InsertOrder : IInsertOrder
    {
        private readonly IConnectionFactory _connectionFactory;
        
        public InsertOrder(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public int Execute(Order order)
        {
            var sql = @" /* PetStore.Store.Api */
insert into orders.order (petid, quantity, shipdate, status, complete, created, createdby) 
values (@petid, @quantity, @shipdate, @status, @complete, current_timestamp, 'PetStore.Store.Api');

select currval('orders.order_id_seq');";

            using (var connection = _connectionFactory.Get())
            {
                return connection.ExecuteScalar<int>(sql, order);
            }
        }
    }
}
