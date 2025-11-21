using Microsoft.Playwright;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace PageObjects
{
    public class UserGridPage : BasePage
    {
        public UserGridPage(IPage page) : base(page) { }

        // Принимаем baseUrl как параметр, чтобы не было жёсткого адреса!
        public async Task GoToAsync(string baseUrl)
            => await Page.GotoAsync(baseUrl + "/users");

        public async Task<List<string>> GetAllUserNamesAsync()
        {
            var elements = await Page.QuerySelectorAllAsync(".user-row .user-name");
            var names = new List<string>();
            foreach (var elem in elements)
                names.Add(await elem.InnerTextAsync());
            return names;
        }

        public async Task ClickEditUserAsync(string userName)
        {
            var selector = $"tr:has(td:text('{userName}')) .edit-btn";
            await Page.ClickAsync(selector);
        }

        public async Task DeleteUserAsync(string userName)
        {
            var selector = $"tr:has(td:text('{userName}')) .delete-btn";
            await Page.ClickAsync(selector);
            // Можно добавить подтверждение модалки
        }

        public async Task<bool> IsUserPresentAsync(string userName)
        {
            var names = await GetAllUserNamesAsync();
            return names.Contains(userName);
        }
    }
}
