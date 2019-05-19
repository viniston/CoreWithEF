using System.Data.SqlClient;
using BusinessDataAccess.EF;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Common;

namespace ReviewAPIMicroService.Configuration
{
    public static class EntityFrameworkConfiguration
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            var sqlConnectionStringBuilder = GetSqlConnectionStringBuilder(configuration, hostingEnvironment, "ReviewAPIConnection", "ReviewDatabase");
            services.AddDbContext<ReviewDbContext>(options => options.UseSqlServer(sqlConnectionStringBuilder.ConnectionString));
        }

        private static SqlConnectionStringBuilder GetSqlConnectionStringBuilder(IConfiguration configuration, IHostingEnvironment hostingEnvironment, string connectionString, string database)
        {
            var sqlConnectionStringBuilder = new SqlConnectionStringBuilder(configuration.GetConnectionString(connectionString));

            if (hostingEnvironment.IsLocalhost()) return sqlConnectionStringBuilder;

            var dbconfig = configuration.GetSection(database).Get<DatabaseConfig>();
            sqlConnectionStringBuilder.DataSource = dbconfig.DataSource;
            sqlConnectionStringBuilder.Password = dbconfig.Password;
            sqlConnectionStringBuilder.UserID = dbconfig.UserId;

            return sqlConnectionStringBuilder;
        }
    }
}
