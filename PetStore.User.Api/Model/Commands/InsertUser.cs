using PetStore.User.Api.Infrastructure;

namespace PetStore.User.Api.Model.Commands;

public interface IInsertUser
{
    int Execute(User user);
}
    
public class InsertUser : IInsertUser
{
    private readonly IConnectionFactory _connectionFactory;
        
    public InsertUser(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public int Execute(User user)
    {
        var sql = @" /* PetStore.User.Api */
insert into users.user (username, firstname, lastname, email, passwordhash, salt, phone, status, created, createdby) 
values (@username, @firstname, @lastname, @email, @passwordhash, @salt, @phone, @status, current_timestamp, 'PetStore.User.Api');

select currval('users.user_id_seq')";

        using (var connection = _connectionFactory.Get())
        {
            return connection.ExecuteScalar<int>(sql, user);
        }
    }
}