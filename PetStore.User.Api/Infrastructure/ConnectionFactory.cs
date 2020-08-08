using System;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace PetStore.User.Api.Infrastructure
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
            
            _connectionString = new Lazy<string>(() => _configuration.GetValue<string>("postgres"));
        }

        public IDbConnection Get()
        {
            var connection = new Npgsql.NpgsqlConnection(_connectionString.Value);
            connection.Open();
            return connection;
        }
    }
}