namespace UserManagement.Tests.Pages;

/// <summary>Page Object главной страницы со списком пользователей.</summary>
public class UsersPage(IPage page)
{
    public ILocator Heading => page.GetByRole(AriaRole.Heading, new() { Name = "Пользователи", Level = 1 });
    public ILocator AddUserButton => page.GetByRole(AriaRole.Button, new() { Name = "Добавить пользователя" });
    public ILocator Search => page.GetByRole(AriaRole.Searchbox, new() { Name = "Поиск по логину" });
    public ILocator Rows => page.Locator("table.user-table tbody tr");
    public ILocator SuccessToast => page.Locator("#successMsg");
    public ILocator ErrorToast => page.Locator("#errorMsg");
    public ILocator EmptySearchState => page.GetByText("Никого не найдено");

    public ILocator Row(string login) => Rows.Filter(new() { Has = page.Locator(".login", new() { HasTextString = login }) });

    public Task OpenAsync() => AllureApi.Step("Открыть страницу пользователей", async () =>
    {
        await page.GotoAsync("/");
        await Assertions.Expect(Heading).ToBeVisibleAsync();
    });

    public Task<UserDialog> OpenAddDialogAsync() => AllureApi.Step("Открыть форму добавления", async () =>
    {
        await AddUserButton.ClickAsync();
        return new UserDialog(page);
    });

    public Task<UserDialog> OpenEditDialogAsync(string login) => AllureApi.Step($"Открыть редактирование {login}", async () =>
    {
        await Row(login).GetByRole(AriaRole.Button, new() { Name = "Изменить" }).ClickAsync();
        return new UserDialog(page);
    });

    public Task DeleteAsync(string login) => AllureApi.Step($"Удалить пользователя {login}", () =>
        Row(login).GetByRole(AriaRole.Button, new() { Name = "Удалить" }).ClickAsync());

    public Task SearchAsync(string text) => AllureApi.Step($"Ввести в поиск «{text}»", () => Search.FillAsync(text));
}
