using System.Collections.Generic;
using System.Linq;
using Dapper;
using PetStore.Store.Api.Infrastructure;

namespace PetStore.Store.Api.Model.Queries
{
    public interface IGetInventory
    {
        IList<InventoryLine> Execute();
    }
    
    public class GetInventory : IGetInventory
    {
        private readonly IConnectionFactory _connectionFactory;
        
        public GetInventory(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public IList<InventoryLine> Execute()
        {
            var sql = @" /* PetStore.Store.Api */
select p.Status, count(p.Id) from pets.pet p
where p.IsDelete = false
group by p.Status";

            using (var connection = _connectionFactory.Get())
            {
               return connection.Query<InventoryLine>(sql).ToList();
            }
        }
    }
}
