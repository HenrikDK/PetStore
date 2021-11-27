using PetStore.Pet.Api.Infrastructure;

namespace PetStore.Pet.Api.Model.Queries;

public interface IGetPetByStatus
{
    IList<Pet> Execute(PetStatus status);
}
    
public class GetPetByStatus : IGetPetByStatus
{
    private readonly IConnectionFactory _connectionFactory;
        
    public GetPetByStatus(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public IList<Pet> Execute(PetStatus status)
    {
        var sql = @" /* PetStore.Pet.Api */
select p.id, p.Name, p.Category, p.Status, p.Tags 
from pets.pet p
where p.IsDelete = false
and p.status = @status";

        using var connection = _connectionFactory.Get();
        return connection.Query<Pet>(sql, new {status}).ToList();
    }
}