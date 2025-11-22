using Microsoft.Playwright;
using System.Threading.Tasks;

namespace PageObjects
{
    public class UsersPage : BasePage
    {
        public UsersPage(IPage page) : base(page) { }

        // Здесь твои методы для работы со страницей пользователей
        public async Task ClickFirstEditButtonAsync() =>
            await Page.ClickAsync("#editBtn1");
    }
}
