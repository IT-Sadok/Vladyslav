using System.Data;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Healthcare.Infrastructure.Persistance
{
    public class SqlConnectionFactory
    {
        private readonly IConfiguration _configuration;

        public SqlConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection CreateDbConnection()
        {
            var connectionString = _configuration.GetConnectionString("HealthcareDb");
            
            return new SqlConnection(connectionString);
        }
    }
}