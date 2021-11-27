using PetStore.Pet.Api.Infrastructure;

namespace PetStore.Pet.Api.Model.Commands;

public interface IDeletePet
{
    void Execute(int petId);
}
    
public class DeletePet : IDeletePet
{
    private readonly IConnectionFactory _connectionFactory;
        
    public DeletePet(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public void Execute(int petId)
    {
        var sql = @" /* PetStore.Pet.Api */
update pets.pet set
Deleted = current_timestamp,
DeletedBy = 'PetStore.Pet.Api',
IsDelete = true
where Id = @Id";

        using var connection = _connectionFactory.Get();
        connection.Execute(sql, new { id = petId });
    }
}