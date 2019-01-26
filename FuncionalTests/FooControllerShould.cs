using System.Security.Claims;
using System.Threading.Tasks;
using FooApi.Controllers;
using FooApi.Models;
using FuncionalTests.Seedwork;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace FuncionalTests
{
    [Collection("basichost")]
    public class FooControllerShould
    {
        private HostFixture _fixture;
        public FooControllerShould(HostFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        [ResetDatabase]
        public async Task get_foo_if_exist()
        {
            //var httpClient = _fixture.Server.CreateClient();
            //var response = await httpClient.GetAsync("api/foo/2");

            //response.EnsureSuccessStatusCode();


            var foo = new Foo() {Bar = "Demo"};

            await _fixture.ExecuteDbContextAsync(async context =>
            {
                await context.Foo.AddAsync(foo);
                await context.SaveChangesAsync();
            });

            var response = await _fixture.Server
                .CreateHttpApiRequest<FooController>(f => f.Get(foo.Id))
                .WithIdentity(new Claim[]
                {
                    new Claim("customclaim", "the customer claim value")
                }).GetAsync();

            response.EnsureSuccessStatusCode();
        }
    }
}
