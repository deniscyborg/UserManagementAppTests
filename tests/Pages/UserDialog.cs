namespace UserManagement.Tests.Pages;

/// <summary>Page Object for the add/edit user dialog.</summary>
public class UserDialog(IPage page)
{
    public ILocator Root => page.GetByRole(AriaRole.Dialog);
    public ILocator Title => Root.GetByRole(AriaRole.Heading);
    public ILocator Name => Root.GetByLabel("Name", new() { Exact = true });
    public ILocator Surname => Root.GetByLabel("Surname", new() { Exact = true });
    public ILocator Email => Root.GetByLabel("Email", new() { Exact = true });
    public ILocator SubmitButton => Root.Locator("button[type='submit']");
    public ILocator CancelButton => Root.GetByRole(AriaRole.Button, new() { Name = "Cancel" });

    public ILocator Error(string message) => Root.Locator(".error-message", new() { HasTextString = message });

    public Task FillAsync(UserRequest user) => AllureApi.Step($"Fill in the form: {user.Name} {user.Surname}, {user.Email}", async () =>
    {
        await Name.FillAsync(user.Name);
        await Surname.FillAsync(user.Surname);
        await Email.FillAsync(user.Email);
    });

    public Task SubmitAsync() => AllureApi.Step("Submit the form", () => SubmitButton.ClickAsync());

    public Task CancelAsync() => AllureApi.Step("Click “Cancel”", () => CancelButton.ClickAsync());
}
