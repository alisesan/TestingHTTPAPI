using System;
using System.Reflection;
using System.Threading.Tasks;
using FooApi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Respawn;
using Xunit.Sdk;

namespace FuncionalTests.Seedwork
{
    public class HostFixture: IDisposable
    {
        static Checkpoint _checkpoint = new Checkpoint();

        public TestServer Server { get; private set; }

        public HostFixture()
        {
            var hostBuilder = new WebHostBuilder()
                .UseStartup<TestStartup>();

            Server = new TestServer(hostBuilder);

            Server.Host.MigrateDbContext<FooContext>(context =>
            {
                context.Foo.Add(new Foo() {Bar = "samplebar"});
                context.SaveChanges();
            });

            _checkpoint.TablesToIgnore = new string[]{ "__EFMigrationsHistory" };
            _checkpoint.SchemasToInclude = new string[]{"dbo"};
        }

        public void Dispose()
        {
            if (Server != null)
            {
                Server.Dispose();
            }
        }

        public async Task ExecuteScopeAsync(Func<IServiceProvider, Task> action)
        {
            using (var scope = Server.Host.Services.GetService<IServiceScopeFactory>().CreateScope())
            {
                await action(scope.ServiceProvider);
            }
        }

        public Task ExecuteDbContextAsync(Func<FooContext, Task> action)
        {
            return ExecuteScopeAsync(sp => action(ServiceProviderServiceExtensions.GetService<FooContext>(sp)));
        }

        public static void ResetDatabase()
        {
            _checkpoint.Reset("Server=.;Initial Catalog=Meetup1Test;Integrated Security=true").Wait();
        }
    }

    public class ResetDatabase : BeforeAfterTestAttribute
    {
        public override void Before(MethodInfo methodUnderTest)
        {
            HostFixture.ResetDatabase();
        }
    }

    public static class RequestBuilderExtensions
    {
        public static async Task<TEntity> GetTo<TEntity>(this RequestBuilder builder)
        {
            var response = await builder.GetAsync();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TEntity>(json);
        }
    }

}
