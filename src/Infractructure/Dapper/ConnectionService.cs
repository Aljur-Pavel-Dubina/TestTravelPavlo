using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Infractructure.Dapper
{
    public class ConnectionService
    {
        private readonly IConfiguration _configuration;

        public ConnectionService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection CreateConnection()
        {
            var connection = SqlConnection();
            connection.Open();
            return connection;
        }

        private SqlConnection SqlConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }
    }
}