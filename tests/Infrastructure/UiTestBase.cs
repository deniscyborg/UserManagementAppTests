using Microsoft.Playwright.NUnit;
using NUnit.Framework.Interfaces;

namespace UserManagement.Tests.Infrastructure;

/// <summary>
/// Base for UI tests: a fresh browser context per test, Playwright tracing
/// and Allure attachments (screenshot + trace.zip) on failure.
/// </summary>
[AllureNUnit]
public abstract class UiTestBase : PageTest
{
    protected UsersApiClient Api { get; private set; } = null!;
    protected TestDataCleanup Cleanup { get; private set; } = null!;

    public override BrowserNewContextOptions ContextOptions() => new()
    {
        BaseURL = TestSettings.BaseUrl,
        Locale = "en-GB",
        ViewportSize = new ViewportSize { Width = 1280, Height = 800 },
    };

    [SetUp]
    public async Task SetUpUi()
    {
        Api = new UsersApiClient();
        Cleanup = new TestDataCleanup(Api);
        await Context.Tracing.StartAsync(new TracingStartOptions
        {
            Screenshots = true,
            Snapshots = true,
            Sources = true,
        });
    }

    [TearDown]
    public async Task TearDownUi()
    {
        var failed = TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed;
        try
        {
            if (failed)
            {
                var screenshot = await Page.ScreenshotAsync(new PageScreenshotOptions { FullPage = true });
                AllureApi.AddAttachment("Screenshot", "image/png", screenshot, "png");

                var tracePath = Path.Combine(Path.GetTempPath(), $"trace-{Guid.NewGuid():N}.zip");
                await Context.Tracing.StopAsync(new TracingStopOptions { Path = tracePath });
                AllureApi.AddAttachment("Playwright trace", "application/zip", tracePath);
            }
            else
            {
                await Context.Tracing.StopAsync();
            }
        }
        finally
        {
            await Cleanup.RunAsync();
            Api.Dispose();
        }
    }
}
