using Microsoft.Playwright;
using System.Threading.Tasks;

namespace PageObjects
{
    public class UserGridPage : BasePage
    {
        public UserGridPage(IPage page) : base(page) { }

        public async Task GoToAsync(string baseUrl)
            => await Page.GotoAsync(baseUrl + "/users");

        public async Task AddUserAsync(string name)
            => await Page.FillAsync("input[name='name']", name); // либо перейти на форму, либо кликнуть "Add"

        public async Task DeleteUserAsync(string name)
        {
           // выбери строку по имени пользователя и кликни по кнопке "Delete"
           await Page.ClickAsync($"//tr[td[text()='{name}']]//button[contains(@class,'delete')]");
        }
        // ... и остальные методы: GetAllUserNamesAsync, ClickEditUserAsync и т.п.
    }
}
