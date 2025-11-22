using Microsoft.Playwright;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace PageObjects
{
    public class UserGridPage : BasePage
    {
        public UserGridPage(IPage page) : base(page) { }

        public async Task GoToAsync(string baseUrl)
            => await Page.GotoAsync(baseUrl + "/users");

        public async Task<IReadOnlyList<string>> GetAllUserLoginsAsync()
        {
            // Вытаскиваем логины из первой колонки таблицы
            var elements = await Page.QuerySelectorAllAsync("table.user-table tbody tr td:nth-child(1)");
            var logins = new List<string>();
            foreach (var el in elements)
                logins.Add(await el.InnerTextAsync());
            return logins;
        }

        // Редактировать пользователя по логину
        public async Task ClickEditUserAsync(string login)
        {
            // Находим строку пользователя по логину, затем кликаем кнопку "Редактировать"
            var rowSelector = $"table.user-table tbody tr:has(td:text-is(\"{login}\"))";
            await Page.ClickAsync(rowSelector + " button:has-text(\"Редактировать\")");
        }

        // Удалить пользователя по логину
        public async Task DeleteUserAsync(string login)
        {
            var rowSelector = $"table.user-table tbody tr:has(td:text-is(\"{login}\"))";
            await Page.ClickAsync(rowSelector + " button:has-text(\"Удалить\")");
        }

        // Проверить наличие пользователя по логину
        public async Task<bool> IsUserPresentAsync(string login)
        {
            var logins = await GetAllUserLoginsAsync();
            return logins.Contains(login);
        }
    }
}
