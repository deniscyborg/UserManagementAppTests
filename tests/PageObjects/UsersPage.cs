[Test]
[AllureTag("ui", "users", "page-object")]
[AllureSeverity(SeverityLevel.normal)]
public async Task EditUserTest()
{
    var usersPage = new UsersPage(Page);
    await usersPage.FirstEditButton.ClickAsync();
    // ...
}
