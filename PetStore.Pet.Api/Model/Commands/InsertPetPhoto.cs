using PetStore.Pet.Api.Infrastructure;

namespace PetStore.Pet.Api.Model.Commands;

public interface IInsertPetPhoto
{
    void Execute(Guid photoId, int petId, string metaData, string url);
}
    
public class InsertPetPhoto : IInsertPetPhoto
{
    private readonly IConnectionFactory _connectionFactory;
        
    public InsertPetPhoto(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public void Execute(Guid photoId, int petId, string metaData, string url)
    {
        var sql = @" /* PetStore.Pet.Api */
insert into pets.photo (id, petid, url, metadata, created, createdby)
values (@id, @petid, @url, @metaData, current_timestamp, 'PetStore.Pet.Api')";

        using (var connection = _connectionFactory.Get())
        {
            connection.Execute(sql, new {id = photoId, petId, metaData, url});
        }
    }
}