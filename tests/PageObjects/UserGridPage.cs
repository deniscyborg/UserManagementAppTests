using Microsoft.Playwright;
using System.Threading.Tasks;

namespace PageObjects
{
    public class UserGridPage : BasePage
    {
        public UserGridPage(IPage page) : base(page) { }

        public async Task GoToAsync(string baseUrl)
            => await Page.GotoAsync(baseUrl + "/users");

        public async Task<bool> IsUserPresentAsync(string userName)
            => await Page.IsVisibleAsync($"text={userName}");

        public async Task ClickEditUserAsync(string userName)
            => await Page.ClickAsync($"xpath=//tr[td[text()='{userName}']]//button[@class='edit']");

        public async Task DeleteUserAsync(string userName)
            => await Page.ClickAsync($"xpath=//tr[td[text()='{userName}']]//button[@class='delete']");
    }
}
