using Microsoft.Playwright;
using System.Threading.Tasks;

namespace PageObjects
{
    public abstract class BasePage
    {
        protected IPage Page;
        public BasePage(IPage page) => Page = page;

        public async Task ClickAsync(string selector) => await Page.ClickAsync(selector);
        public async Task FillAsync(string selector, string value) => await Page.FillAsync(selector, value);
    }
}
