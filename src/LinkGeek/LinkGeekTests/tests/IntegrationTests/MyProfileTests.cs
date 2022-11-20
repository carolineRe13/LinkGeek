using Microsoft.Playwright;
using Xunit;

namespace LinkGeek.tests.Pages.Shared;

public class MyProfileTests
{
    [Fact]
    public async Task EditProfile()
    {
        // Arrange
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false,
        });
        var context = await browser.NewContextAsync();
        var page = await context.NewPageAsync();
        
        // Act
        // create a user
        await page.GotoAsync("https://localhost:7010/");
        await page.GetByRole(AriaRole.Link, new() { NameString = "Register" }).ClickAsync();
        await page.GetByLabel("Email").ClickAsync();
        await page.GetByLabel("Email").FillAsync("linkgeek@test.com");
        await page.GetByLabel("Username").ClickAsync();
        await page.GetByLabel("Username").FillAsync("LinkGeekTest");
        await page.GetByLabel("Confirm password").ClickAsync();
        await page.GetByLabel("Confirm password").FillAsync("AVerySafePassword19*");
        await page.Locator("#Input_Password").ClickAsync();
        await page.Locator("#Input_Password").FillAsync("AVerySafePassword19*");
        await page.Locator("#Input_Password").PressAsync("Enter");
        await page.GetByRole(AriaRole.Link, new() { NameString = "Click here to confirm your account" }).ClickAsync();
        await page.GetByRole(AriaRole.Link, new() { NameString = "Login" }).ClickAsync();
        await page.GetByLabel("Username").ClickAsync();
        await page.GetByLabel("Username").FillAsync("LinkGeekTest");
        await page.GetByLabel("Password").ClickAsync();
        await page.GetByLabel("Password").FillAsync("AVerySafePassword19*");
        await page.GetByRole(AriaRole.Button, new() { NameString = "Log in" }).ClickAsync();
        
        // go to My profile card and change status
        await page.GetByRole(AriaRole.Link, new() { NameString = "My Profile card" }).ClickAsync();
        await page.Locator("id=statusPencil").ClickAsync();
        await page.Locator("#status").ClickAsync();
        await page.Locator("#status").FillAsync("Hey!!");
        await page.GetByRole(AriaRole.Button, new() { NameString = "Save changes" }).ClickAsync();

        // Not ideal, can make tests flaky
        await page.WaitForTimeoutAsync(1000);
        
        // Assert
        var status = await page.Locator("id=userStatus").InnerTextAsync();
        Assert.Equal("Hey!!", status);
    }
    
    
}
