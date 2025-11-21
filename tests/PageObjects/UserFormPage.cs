using Microsoft.Playwright;
using System.Threading.Tasks;

namespace PageObjects
{
    public class UserFormPage : BasePage
    {
        public UserFormPage(IPage page) : base(page) { }

        public async Task GoToAsync(string baseUrl)
            => await Page.GotoAsync(baseUrl + "/users/add");

        public async Task FillNameAsync(string name)
            => await Page.FillAsync("input[name='name']", name);

        public async Task FillEmailAsync(string email)
            => await Page.FillAsync("input[name='email']", email);

        public async Task SubmitAsync()
            => await Page.ClickAsync("button[type='submit']");
    }
}
