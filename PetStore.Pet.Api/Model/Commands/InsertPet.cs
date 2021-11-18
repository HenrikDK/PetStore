using PetStore.Pet.Api.Infrastructure;

namespace PetStore.Pet.Api.Model.Commands;

public interface IInsertPet
{
    int Execute(Model.Pet pet);
}
    
public class InsertPet : IInsertPet
{
    private readonly IConnectionFactory _connectionFactory;
        
    public InsertPet(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public int Execute(Pet pet)
    {
        var sql = @" /* PetStore.Pet.Api */
insert into pets.pet (name, category, status, tags, created, createdby)
values (@name, @category, @status, @tags, current_timestamp, 'PetStore.Pet.Api');

select currval('pets.pet_id_seq');";

        using (var connection = _connectionFactory.Get())
        {
            return connection.ExecuteScalar<int>(sql, pet);
        }
    }
}