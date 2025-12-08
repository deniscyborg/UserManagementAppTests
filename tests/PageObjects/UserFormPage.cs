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
        {
            await Page.Locator("input[name='name']").WaitForAsync(
                new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 5000 }
            );
            await Page.FillAsync("input[name='name']", name);
        }

        public async Task FillSurnameAsync(string surname)
        {
            await Page.Locator("input[name='surname']").WaitForAsync(
                new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 5000 }
            );
            await Page.FillAsync("input[name='surname']", surname);
        }

        public async Task FillEmailAsync(string email)
        {
            await Page.Locator("input[name='email']").WaitForAsync(
                new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 5000 }
            );
            await Page.FillAsync("input[name='email']", email);
        }

        public async Task SubmitAsync()
        {
            var submitButton = Page.Locator("button[type='submit']");
            
            // Ждем видимости кнопки Submit
            await submitButton.WaitForAsync(
                new LocatorWaitForOptions 
                { 
                    State = WaitForSelectorState.Visible,
                    Timeout = 5000
                }
            );
            
            // Убеждаемся что кнопка не disabled
            await submitButton.WaitForAsync(
                new LocatorWaitForOptions 
                { 
                    State = WaitForSelectorState.Enabled,
                    Timeout = 5000
                }
            );
            
            await submitButton.ClickAsync();
        }

        public async Task ClickCancelAsync()
        {
            var cancelButton = Page.Locator("button:has-text('Отмена')");
            
            await cancelButton.WaitForAsync(
                new LocatorWaitForOptions 
                { 
                    State = WaitForSelectorState.Visible,
                    Timeout = 5000
                }
            );
            
            await cancelButton.ClickAsync();
        }

        // Метод для проверки сообщения об ошибке
        public async Task<bool> HasErrorMessageAsync(string message)
        {
            var errorMessage = Page.Locator($"text=\"{message}\"");
            try
            {
                await errorMessage.WaitForAsync(
                    new LocatorWaitForOptions 
                    { 
                        State = WaitForSelectorState.Visible,
                        Timeout = 2000
                    }
                );
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}