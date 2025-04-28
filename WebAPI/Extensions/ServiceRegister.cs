using Microsoft.Extensions.Configuration;
using WebAPI.Services;

namespace WebAPI.Extensions
{
    public static class ServiceRegister
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<TokenService>();

            return services;
        }
    }
}
