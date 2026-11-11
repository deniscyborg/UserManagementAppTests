namespace UserManagement.Tests.Infrastructure;

[AllureNUnit]
[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
public abstract class ApiTestBase
{
    protected UsersApiClient Api { get; private set; } = null!;
    protected TestDataCleanup Cleanup { get; private set; } = null!;

    [SetUp]
    public void SetUpApi()
    {
        Api = new UsersApiClient();
        Cleanup = new TestDataCleanup(Api);
    }

    [TearDown]
    public async Task TearDownApi()
    {
        await Cleanup.RunAsync();
        Api.Dispose();
    }

    /// <summary>Creates a user via the API and registers it for cleanup.</summary>
    protected Task<User> GivenUserAsync() => Api.CreateUserAsync(Cleanup.Track(UserFactory.Valid()));
}
