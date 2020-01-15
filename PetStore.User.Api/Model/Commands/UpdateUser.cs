using Dapper;
using PetStore.User.Api.Infrastructure;

namespace PetStore.User.Api.Model.Commands
{
    public interface IUpdateUser
    {
        void Execute(User user);
    }
    
    public class UpdateUser : IUpdateUser
    {
        private readonly IConnectionFactory _connectionFactory;
        
        public UpdateUser(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public void Execute(User user)
        {
            var sql = @" /* PetStore.User.Api */
update users.user set
FirstName = @FirstName,
LastName = @LastName,
Email = @Email,
PasswordHash = @PasswordHash,
Salt = @Salt,
Phone = @Phone,                      
Status = @Status,
Modified = current_timestamp,
ModifiedBy = 'PetStore.User.Api'
where UserName = @UserName";

            using (var connection = _connectionFactory.Get())
            {
                connection.Execute(sql, user);
            }
        }
    }
}
