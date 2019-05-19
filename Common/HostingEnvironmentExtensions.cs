using Microsoft.AspNetCore.Hosting;

namespace Common
{
    public static class HostingEnvironmentExtensions
    {
        public static bool IsLocalhost(this IHostingEnvironment hostingEnvironment)
        {
            return hostingEnvironment.IsEnvironment("Localhost");
        }
        public static bool IsIntegration(this IHostingEnvironment hostingEnvironment)
        {
            return hostingEnvironment.IsEnvironment("Integration");
        }
    }
}
