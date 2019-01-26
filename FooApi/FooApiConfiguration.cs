using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace FooApi
{
    public static class FooApiConfiguration
    {
        public static IServiceCollection ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddAuthorization(setup =>
            {
                setup.AddPolicy("mypolicy", policy => { policy.RequireClaim("customclaim"); });
            });

            return services;
        }

        public static void Configure(IApplicationBuilder app)
        {
        }
    }
}
