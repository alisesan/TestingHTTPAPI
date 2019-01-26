using FooApi;
using FooApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FooApiHost
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            FooApiConfiguration.ConfigureServices(services)
                .AddDbContext<FooContext>(setup =>
                {
                    setup.UseSqlServer("Server=.;Initial Catalog=Meetup1;Integrated Security=true",
                        options => { options.MigrationsAssembly(typeof(Startup).Assembly.FullName); });
                }).AddAuthentication()
                .AddJwtBearer();

            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("somepolicy");
            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}
