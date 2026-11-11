namespace UserManagement.Tests.Pages;

/// <summary>Page Object for the main page with the user list.</summary>
public class UsersPage(IPage page)
{
    public ILocator Heading => page.GetByRole(AriaRole.Heading, new() { Name = "Users", Level = 1 });
    public ILocator AddUserButton => page.GetByRole(AriaRole.Button, new() { Name = "Add user" });
    public ILocator Search => page.GetByRole(AriaRole.Searchbox, new() { Name = "Search by login" });
    public ILocator Rows => page.Locator("table.user-table tbody tr");
    public ILocator SuccessToast => page.Locator("#successMsg");
    public ILocator ErrorToast => page.Locator("#errorMsg");
    public ILocator EmptySearchState => page.GetByText("No users match");

    public ILocator Row(string login) => Rows.Filter(new() { Has = page.Locator(".login", new() { HasTextString = login }) });

    public Task OpenAsync() => AllureApi.Step("Open the users page", async () =>
    {
        await page.GotoAsync("/");
        await Assertions.Expect(Heading).ToBeVisibleAsync();
    });

    public Task<UserDialog> OpenAddDialogAsync() => AllureApi.Step("Open the add form", async () =>
    {
        await AddUserButton.ClickAsync();
        return new UserDialog(page);
    });

    public Task<UserDialog> OpenEditDialogAsync(string login) => AllureApi.Step($"Open editing for {login}", async () =>
    {
        await Row(login).GetByRole(AriaRole.Button, new() { Name = "Edit" }).ClickAsync();
        return new UserDialog(page);
    });

    public Task DeleteAsync(string login) => AllureApi.Step($"Delete user {login}", () =>
        Row(login).GetByRole(AriaRole.Button, new() { Name = "Delete" }).ClickAsync());

    public Task SearchAsync(string text) => AllureApi.Step($"Type “{text}” into search", () => Search.FillAsync(text));
}
