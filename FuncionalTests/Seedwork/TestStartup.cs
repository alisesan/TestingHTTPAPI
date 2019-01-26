using Acheve.AspNetCore.TestHost.Security;
using FooApi;
using FooApi.Models;
using FooApiHost;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FuncionalTests.Seedwork
{
    public class TestStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            FooApiConfiguration.ConfigureServices(services)
                .AddAuthentication(Acheve.TestHost.TestServerAuthenticationDefaults.AuthenticationScheme)
                .AddTestServerAuthentication();

            services.AddDbContext<FooContext>(setup =>
                {
                    setup.UseSqlServer("Server=.;Initial Catalog=Meetup1Test;Integrated Security=true",
                        options =>
                        {
                            options.MigrationsAssembly(typeof(Startup).Assembly.FullName);
                        });
                }).AddAuthentication()
                .AddJwtBearer();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
