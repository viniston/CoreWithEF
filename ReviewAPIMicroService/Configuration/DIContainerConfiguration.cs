using BusinessDataAccess.Services;
using BusinessDataAccessDefinition.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ReviewAPIMicroService.Configuration
{
    public class DiContainerConfiguration
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IReviewService, ReviewService>();
        }
    }
}
