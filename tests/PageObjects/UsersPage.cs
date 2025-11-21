using Microsoft.Playwright;

public class UsersPage
{
    private readonly IPage _page;
    public UsersPage(IPage page) => _page = page;

    public ILocator AddUserButton => _page.Locator("button:has-text('Добавить пользователя')");
    public ILocator FilterInput => _page.Locator("input[placeholder='Фильтр по логину']");
    public ILocator FirstEditButton => _page.Locator("button:has-text('Редактировать')").First;
    public ILocator UserRows => _page.Locator("table tbody tr");
}
