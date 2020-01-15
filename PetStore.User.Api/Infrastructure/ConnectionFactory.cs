using System;
using System.Data;

namespace PetStore.User.Api.Infrastructure
{
    public interface IConnectionFactory
    {
        IDbConnection Get();
    }
    
    public class ConnectionFactory : IConnectionFactory
    {
        private Lazy<string> _connectionString;
        
        public ConnectionFactory()
        {
            //Connection string normally fetched from configuration service
            _connectionString = new Lazy<string>(() => "Server=localhost;Port=5432;Database=henrik;User Id=postgres;Password=postgres;");
        }

        public IDbConnection Get()
        {
            var connection = new Npgsql.NpgsqlConnection(_connectionString.Value);
            connection.Open();
            return connection;
        }
    }
}
