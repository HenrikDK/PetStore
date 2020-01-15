using Dapper;
using PetStore.Pet.Api.Infrastructure;

namespace PetStore.Pet.Api.Model.Commands
{
    public interface IUpdatePet
    {
        void Execute(Pet pet);
        void Execute(int petId, string name, PetStatus status);
    }
    
    public class UpdatePet : IUpdatePet
    {
        private readonly IConnectionFactory _connectionFactory;
        
        public UpdatePet(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public void Execute(Pet pet)
        {
            var sql = @" /* PetStore.Pet.Api */
update pets.pet set
Name = @Name,
Status = @Status,
Tags = @Tags,
Category = @Category,
Modified = current_timestamp,
ModifiedBy = 'PetStore.Pet.Api'
where Id = @Id";

            using (var connection = _connectionFactory.Get())
            {
                connection.Execute(sql, pet);
            }
        }
        
        public void Execute(int petId, string name, PetStatus status)
        {
            var sql = @" /* PetStore.Pet.Api */
update pets.pet set
Name = @Name,
Status = @Status,
Modified = current_timestamp,
ModifiedBy = 'PetStore.Pet.Api'
where Id = @Id";

            using (var connection = _connectionFactory.Get())
            {
                connection.Execute(sql, new {name, status, id = petId});
            }
        }
    }
}
