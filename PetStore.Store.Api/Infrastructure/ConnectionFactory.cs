namespace PetStore.Store.Api.Infrastructure;

public interface IConnectionFactory
{
    IDbConnection Get();
}
    
public class ConnectionFactory : IConnectionFactory
{
    private readonly IConfiguration _configuration;
    private readonly Lazy<string> _connectionString;
        
    public ConnectionFactory(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = new Lazy<string>(() => _configuration.GetValue<string>("postgres"));
    }

    public IDbConnection Get()
    {
        var connection = new Npgsql.NpgsqlConnection(_connectionString.Value);
        connection.Open();
        return connection;
    }
}