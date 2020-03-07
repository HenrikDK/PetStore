using System;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace PetStore.Store.Api.Infrastructure
{
    public interface IConnectionFactory
    {
        IDbConnection Get();
    }
    
    public class ConnectionFactory : IConnectionFactory
    {
        private readonly IConfiguration _configuration;
        private Lazy<string> _connectionString;
        
        public ConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
            
            _connectionString = new Lazy<string>(GetConnectionStringFromConfiguration);
        }

        public IDbConnection Get()
        {
            var connection = new Npgsql.NpgsqlConnection(_connectionString.Value);
            connection.Open();
            return connection;
        }

        private string GetConnectionStringFromConfiguration()
        {
            var connectionString = Environment.GetEnvironmentVariable("postgres");
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = _configuration.GetConnectionString("postgres");
            }

            return connectionString;
        }
    }
}