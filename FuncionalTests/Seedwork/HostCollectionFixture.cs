using Xunit;

namespace FuncionalTests.Seedwork
{
    [CollectionDefinition("basichost")]
    public class HostCollectionFixture: ICollectionFixture<HostFixture>
    {
    }
}
