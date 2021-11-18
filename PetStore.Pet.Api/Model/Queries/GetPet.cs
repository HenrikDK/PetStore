using PetStore.Pet.Api.Infrastructure;

namespace PetStore.Pet.Api.Model.Queries;

public interface IGetPet
{
    Pet Execute(int petId);
}
    
public class GetPet : IGetPet
{
    private readonly IConnectionFactory _connectionFactory;
        
    public GetPet(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public Pet Execute(int petId)
    {
        var sql = @" /* PetStore.Pet.Api */
select p.Id, p.Name, p.Category, p.Status, p.Tags 
from pets.pet p
where p.Id = @Id
and p.IsDelete = false";

        using (var connection = _connectionFactory.Get())
        {
            return connection.Query<Pet>(sql, new {id = petId}).FirstOrDefault();
        }
    }
}