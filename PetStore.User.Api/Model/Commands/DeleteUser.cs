using PetStore.User.Api.Infrastructure;

namespace PetStore.User.Api.Model.Commands;

public interface IDeleteUser
{
    void Execute(string username);
}
    
public class DeleteUser : IDeleteUser
{
    private readonly IConnectionFactory _connectionFactory;
        
    public DeleteUser(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public void Execute(string username)
    {
        var sql = @" /* PetStore.User.Api */
update users.user set
    Deleted = current_timestamp,
    DeletedBy = 'PetStore.User.Api',
    IsDelete = true
where username = @username";

        using var connection = _connectionFactory.Get();
        connection.Execute(sql, new {username});
    }
}