using System.Linq;
using Dapper;
using PetStore.User.Api.Infrastructure;

namespace PetStore.User.Api.Model.Queries
{
    public interface IGetUser
    {
        User Execute(string username);
    }
    
    public class GetUser : IGetUser
    {
        private readonly IConnectionFactory _connectionFactory;
        
        public GetUser(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public User Execute(string username)
        {
            var sql = @" /* PetStore.Store.Api */
select u.id, u.username, u.status, u.firstname, u.lastname, u.email, u.phone, u.PasswordHash, u.salt
from users.user u
where u.IsDelete = false
and u.UserName = @UserName";

            using (var connection = _connectionFactory.Get())
            {
               return connection.Query<User>(sql, new {username}).FirstOrDefault();
            }
        }
    }
}
