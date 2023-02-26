using System.Data;
using System.Data.SqlClient;

namespace Demo.Dapper.Bulk.Models.Context
{
    public class DefaultContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public DefaultContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultSqlConnection");
        }
        public IDbConnection CreateConnection()
            => new SqlConnection(_connectionString);
    }
}
