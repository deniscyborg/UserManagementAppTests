using System.Text.RegularExpressions;
using UserManagement.Tests.Pages;

namespace UserManagement.Tests.Ui;

[AllureParentSuite("UI")]
[AllureSuite("Users page")]
[AllureFeature("Управление пользователями в браузере")]
[AllureTag("ui", "e2e")]
public class UsersUiTests : UiTestBase
{
    private UsersPage _usersPage = null!;

    [SetUp]
    public void CreatePage() => _usersPage = new UsersPage(Page);

    [Test]
    [AllureStory("Открытие страницы")]
    [AllureSeverity(SeverityLevel.blocker)]
    public async Task Page_Opens_WithTitleAndControls()
    {
        await _usersPage.OpenAsync();

        await Expect(Page).ToHaveTitleAsync("User Management App · Live demo");
        await Expect(_usersPage.AddUserButton).ToBeEnabledAsync();
        await Expect(_usersPage.Search).ToBeVisibleAsync();
    }

    [Test]
    [AllureStory("Добавление")]
    [AllureSeverity(SeverityLevel.blocker)]
    public async Task AddUser_ValidData_AppearsInTableWithSuccessMessage()
    {
        var user = Cleanup.Track(UserFactory.Valid());
        await _usersPage.OpenAsync();

        var dialog = await _usersPage.OpenAddDialogAsync();
        await dialog.FillAsync(user);
        await dialog.SubmitAsync();

        await Expect(dialog.Root).ToBeHiddenAsync();
        await Expect(_usersPage.SuccessToast).ToHaveTextAsync("Пользователь добавлен");
        var row = _usersPage.Row(UserFactory.LoginOf(user.Email));
        await Expect(row).ToContainTextAsync(user.Name);
        await Expect(row).ToContainTextAsync(user.Surname);
        await Expect(row).ToContainTextAsync(user.Email);
    }

    private static IEnumerable<TestCaseData> InvalidInputs()
    {
        yield return new TestCaseData("Ив", "Петров", "ivan@test.ru", "Минимум 3 символа")
            .SetName("AddUser_ShortName_ShowsValidationError");
        yield return new TestCaseData("Иван", "Пе", "ivan@test.ru", "Минимум 3 символа")
            .SetName("AddUser_ShortSurname_ShowsValidationError");
        yield return new TestCaseData("Иван", "Петров", "ivan@test.com", "Формат email должен быть @... и заканчиваться на .ru")
            .SetName("AddUser_NonRuEmail_ShowsValidationError");
    }

    [TestCaseSource(nameof(InvalidInputs))]
    [AllureStory("Валидация формы")]
    [AllureSeverity(SeverityLevel.critical)]
    public async Task AddUser_InvalidInput_ShowsErrorAndKeepsDialogOpen(string name, string surname, string email, string error)
    {
        await _usersPage.OpenAsync();

        var dialog = await _usersPage.OpenAddDialogAsync();
        await dialog.FillAsync(new UserRequest(name, surname, email));
        await dialog.SubmitAsync();

        await Expect(dialog.Error(error)).ToBeVisibleAsync();
        await Expect(dialog.Root).ToBeVisibleAsync();
    }

    [Test]
    [AllureStory("Добавление")]
    [AllureSeverity(SeverityLevel.critical)]
    public async Task AddUser_DuplicateEmail_ShowsServerError()
    {
        var existing = await Api.CreateUserAsync(Cleanup.Track(UserFactory.Valid()));
        await _usersPage.OpenAsync();

        var dialog = await _usersPage.OpenAddDialogAsync();
        await dialog.FillAsync(UserFactory.Valid() with { Email = existing.Email });
        await dialog.SubmitAsync();

        await Expect(_usersPage.ErrorToast).ToHaveTextAsync("Пользователь с таким email уже существует");
        await Expect(dialog.Root).ToBeVisibleAsync();
    }

    [Test]
    [AllureStory("Добавление")]
    [AllureSeverity(SeverityLevel.normal)]
    public async Task AddUser_Cancel_ClosesDialogWithoutCreatingUser()
    {
        var user = Cleanup.Track(UserFactory.Valid());
        await _usersPage.OpenAsync();

        var dialog = await _usersPage.OpenAddDialogAsync();
        await dialog.FillAsync(user);
        await dialog.CancelAsync();

        await Expect(dialog.Root).ToBeHiddenAsync();
        Assert.That(await Api.FindByEmailAsync(user.Email), Is.Null, "Пользователь не должен создаться");
    }

    [Test]
    [AllureStory("Добавление")]
    [AllureSeverity(SeverityLevel.minor)]
    public async Task Dialog_Escape_ClosesDialog()
    {
        await _usersPage.OpenAsync();

        var dialog = await _usersPage.OpenAddDialogAsync();
        await Page.Keyboard.PressAsync("Escape");

        await Expect(dialog.Root).ToBeHiddenAsync();
    }

    [Test]
    [AllureStory("Редактирование")]
    [AllureSeverity(SeverityLevel.critical)]
    public async Task EditUser_ChangesAreShownInTable()
    {
        var user = await Api.CreateUserAsync(Cleanup.Track(UserFactory.Valid()));
        var changes = Cleanup.Track(UserFactory.Valid());
        await _usersPage.OpenAsync();

        var dialog = await _usersPage.OpenEditDialogAsync(user.Login);
        await Expect(dialog.Email).ToHaveValueAsync(user.Email);
        await dialog.FillAsync(changes);
        await dialog.SubmitAsync();

        await Expect(_usersPage.SuccessToast).ToHaveTextAsync("Изменения сохранены");
        await Expect(_usersPage.Row(user.Login)).ToHaveCountAsync(0);
        await Expect(_usersPage.Row(UserFactory.LoginOf(changes.Email))).ToContainTextAsync(changes.Email);
    }

    [Test]
    [AllureStory("Удаление")]
    [AllureSeverity(SeverityLevel.critical)]
    public async Task DeleteUser_RemovesRowAndUser()
    {
        var user = await Api.CreateUserAsync(Cleanup.Track(UserFactory.Valid()));
        await _usersPage.OpenAsync();

        await _usersPage.DeleteAsync(user.Login);

        await Expect(_usersPage.Row(user.Login)).ToHaveCountAsync(0);
        await Expect(_usersPage.SuccessToast).ToHaveTextAsync(new Regex($"{Regex.Escape(user.Login)} удалён"));
        Assert.That(await Api.FindByEmailAsync(user.Email), Is.Null, "Пользователь удалён на сервере");
    }

    [Test]
    [AllureStory("Поиск")]
    [AllureSeverity(SeverityLevel.normal)]
    public async Task Search_ByLogin_ShowsOnlyMatchingRows()
    {
        var target = await Api.CreateUserAsync(Cleanup.Track(UserFactory.Valid()));
        var other = await Api.CreateUserAsync(Cleanup.Track(UserFactory.Valid()));
        await _usersPage.OpenAsync();

        await _usersPage.SearchAsync(target.Login);

        await Expect(_usersPage.Row(target.Login)).ToBeVisibleAsync();
        await Expect(_usersPage.Row(other.Login)).ToHaveCountAsync(0);
    }

    [Test]
    [AllureStory("Поиск")]
    [AllureSeverity(SeverityLevel.minor)]
    public async Task Search_NoMatches_ShowsEmptyState()
    {
        await Api.CreateUserAsync(Cleanup.Track(UserFactory.Valid()));
        await _usersPage.OpenAsync();

        await _usersPage.SearchAsync($"nobody-{Guid.NewGuid():N}");

        await Expect(_usersPage.EmptySearchState).ToBeVisibleAsync();
    }
}
