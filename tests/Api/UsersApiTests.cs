using System.Net;
using System.Net.Http.Json;

namespace UserManagement.Tests.Api;

[AllureParentSuite("API")]
[AllureSuite("Users")]
[AllureFeature("Users REST API")]
[AllureTag("api")]
public class UsersApiTests : ApiTestBase
{
    private const int MissingId = int.MaxValue;

    [Test]
    [AllureStory("Get list")]
    [AllureSeverity(SeverityLevel.critical)]
    public async Task GetAll_ReturnsCreatedUser()
    {
        var user = await GivenUserAsync();

        using var response = await Api.GetAllAsync();

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var users = await response.Content.ReadFromJsonAsync<List<User>>();
        Assert.That(users, Does.Contain(user));
    }

    [Test]
    [AllureStory("Create")]
    [AllureSeverity(SeverityLevel.blocker)]
    public async Task Create_ValidUser_Returns201WithLocationAndBody()
    {
        var request = Cleanup.Track(UserFactory.Valid());

        using var response = await Api.CreateAsync(request);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        var created = await response.Content.ReadFromJsonAsync<User>();
        Assert.Multiple(() =>
        {
            Assert.That(created!.Id, Is.Positive);
            Assert.That(created.Name, Is.EqualTo(request.Name));
            Assert.That(created.Surname, Is.EqualTo(request.Surname));
            Assert.That(created.Email, Is.EqualTo(request.Email));
            Assert.That(created.Login, Is.EqualTo(UserFactory.LoginOf(request.Email)), "Login = part of the email before @");
            Assert.That(response.Headers.Location?.AbsolutePath, Does.EndWith($"/api/users/{created.Id}"));
        });
    }

    [Test]
    [AllureStory("Create")]
    [AllureSeverity(SeverityLevel.normal)]
    public async Task Create_TrimsWhitespace()
    {
        var email = Cleanup.Track(UserFactory.UniqueEmail());

        using var response = await Api.CreateAsync(new UserRequest("  John  ", " Smith ", $" {email} "));

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        var created = await response.Content.ReadFromJsonAsync<User>();
        Assert.That((created!.Name, created.Surname, created.Email), Is.EqualTo(("John", "Smith", email)));
    }

    private static IEnumerable<TestCaseData> InvalidRequests()
    {
        var email = "valid@test.ru";
        yield return new TestCaseData(new { surname = "Smith", email }).SetName("Create_MissingName_Returns400");
        yield return new TestCaseData(new UserRequest("Jo", "Smith", email)).SetName("Create_ShortName_Returns400");
        yield return new TestCaseData(new UserRequest("John", "", email)).SetName("Create_EmptySurname_Returns400");
        yield return new TestCaseData(new UserRequest("John", "Sm", email)).SetName("Create_ShortSurname_Returns400");
        yield return new TestCaseData(new UserRequest("John", "Smith", "")).SetName("Create_EmptyEmail_Returns400");
        yield return new TestCaseData(new UserRequest("John", "Smith", "not-an-email")).SetName("Create_InvalidEmail_Returns400");
    }

    [TestCaseSource(nameof(InvalidRequests))]
    [AllureStory("Validation")]
    [AllureSeverity(SeverityLevel.critical)]
    public async Task Create_InvalidRequest_Returns400(object request)
    {
        using var response = await Api.CreateAsync(request);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        Assert.That(response.Content.Headers.ContentType?.MediaType, Is.EqualTo("application/problem+json"));
    }

    [Test]
    [AllureStory("Validation")]
    [AllureSeverity(SeverityLevel.critical)]
    public async Task Create_DuplicateEmailIgnoringCase_Returns409()
    {
        var existing = await GivenUserAsync();

        using var response = await Api.CreateAsync(UserFactory.Valid() with { Email = existing.Email.ToUpperInvariant() });

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
    }

    [Test]
    [AllureStory("Get by id")]
    [AllureSeverity(SeverityLevel.critical)]
    public async Task GetById_Existing_Returns200()
    {
        var user = await GivenUserAsync();

        using var response = await Api.GetByIdAsync(user.Id);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(await response.Content.ReadFromJsonAsync<User>(), Is.EqualTo(user));
    }

    [Test]
    [AllureStory("Get by id")]
    [AllureSeverity(SeverityLevel.normal)]
    public async Task GetById_Missing_Returns404()
    {
        using var response = await Api.GetByIdAsync(MissingId);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    [AllureStory("Update")]
    [AllureSeverity(SeverityLevel.critical)]
    public async Task Update_Existing_PersistsChangesAndRecalculatesLogin()
    {
        var user = await GivenUserAsync();
        var changes = Cleanup.Track(UserFactory.Valid());

        using var response = await Api.UpdateAsync(user.Id, changes);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var expected = new User(user.Id, UserFactory.LoginOf(changes.Email), changes.Name, changes.Surname, changes.Email);
        Assert.That(await response.Content.ReadFromJsonAsync<User>(), Is.EqualTo(expected));

        using var stored = await Api.GetByIdAsync(user.Id);
        Assert.That(await stored.Content.ReadFromJsonAsync<User>(), Is.EqualTo(expected), "Changes are stored on the server");
    }

    [Test]
    [AllureStory("Update")]
    [AllureSeverity(SeverityLevel.normal)]
    public async Task Update_KeepingOwnEmail_Returns200()
    {
        var user = await GivenUserAsync();

        using var response = await Api.UpdateAsync(user.Id, new UserRequest("Updated", "Name", user.Email));

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "The user's own email does not count as taken");
    }

    [Test]
    [AllureStory("Update")]
    [AllureSeverity(SeverityLevel.normal)]
    public async Task Update_ToEmailOfAnotherUser_Returns409()
    {
        var first = await GivenUserAsync();
        var second = await GivenUserAsync();

        using var response = await Api.UpdateAsync(second.Id, new UserRequest(second.Name, second.Surname, first.Email));

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
    }

    [Test]
    [AllureStory("Update")]
    [AllureSeverity(SeverityLevel.normal)]
    public async Task Update_Missing_Returns404()
    {
        using var response = await Api.UpdateAsync(MissingId, UserFactory.Valid());

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    [AllureStory("Update")]
    [AllureSeverity(SeverityLevel.normal)]
    public async Task Update_InvalidRequest_Returns400()
    {
        var user = await GivenUserAsync();

        using var response = await Api.UpdateAsync(user.Id, new UserRequest("John", "Smith", "broken"));

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    [AllureStory("Delete")]
    [AllureSeverity(SeverityLevel.critical)]
    public async Task Delete_Existing_Returns204AndUserIsGone()
    {
        var user = await GivenUserAsync();

        using var response = await Api.DeleteAsync(user.Id);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        using var lookup = await Api.GetByIdAsync(user.Id);
        Assert.That(lookup.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    [AllureStory("Delete")]
    [AllureSeverity(SeverityLevel.normal)]
    public async Task Delete_Missing_Returns404()
    {
        using var response = await Api.DeleteAsync(MissingId);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
}
