using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BusinessDataAccess.EF
{

    public class ReviewDbContextDesignTimeContextFactory : IDesignTimeDbContextFactory<ReviewDbContext>
    {
        public ReviewDbContext CreateDbContext(string[] args)
        {
            Console.WriteLine(args.Length);
            const string dbConnectionString =
                "Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = Review_Db; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False";
            var builder = new DbContextOptionsBuilder<ReviewDbContext>();
            builder.UseSqlServer(dbConnectionString);
            var context = new ReviewDbContext(builder.Options);
            return context;
        }
    }
}
