using Microsoft.Playwright;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PageObjects
{
    public class UserGridPage : BasePage
    {
        public UserGridPage(IPage page) : base(page) { }

        // ...остальные методы...

        public async Task<IReadOnlyList<string>> GetAllUserNamesAsync()
        {
            // Пример: вытащить все имена пользователей из таблицы с классом user-grid
            var elements = await Page.QuerySelectorAllAsync("table.user-grid tr td.username");
            var names = new List<string>();

            foreach (var el in elements)
            {
                names.Add(await el.InnerTextAsync());
            }
            return names;
        }
    }
}
