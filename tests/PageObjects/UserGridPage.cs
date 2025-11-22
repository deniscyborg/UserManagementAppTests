using Microsoft.Playwright;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq; // для метода Contains

namespace PageObjects
{
    public class UserGridPage : BasePage
    {
        public UserGridPage(IPage page) : base(page) { }

        public async Task GoToAsync(string baseUrl)
            => await Page.GotoAsync(baseUrl + "/users");

        // Для тестов, которым нужен GetAllUserNamesAsync
        public async Task<IReadOnlyList<string>> GetAllUserNamesAsync()
        {
            // Берём логины, как уникальные "имена" в твоей таблице
            var elements = await Page.QuerySelectorAllAsync("table.user-table tbody tr td:nth-child(1)");
            var names = new List<string>();
            foreach (var el in elements)
                names.Add(await el.InnerTextAsync());
            return names;
        }

        // Оставляю GetAllUserLoginsAsync, если вдруг тоже используется где-то
        public async Task<IReadOnlyList<string>> GetAllUserLoginsAsync()
            => await GetAllUserNamesAsync();

        // Редактировать пользователя по логину
        public async Task ClickEditUserAsync(string login)
        {
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
            var names = await GetAllUserNamesAsync();
            return names.Contains(login);
        }
    }
}
