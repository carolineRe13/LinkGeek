using System.Diagnostics;
using LinkGeek.Areas.ProfileCard.Pages;
using LinkGeek.tests.Pages.Shared.SetUp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using Xunit;

namespace LinkGeek.tests.Pages.Shared;

[Collection(PlaywrightFixture.PlaywrightCollection)]
public class MyProfileTests
{
    private readonly PlaywrightFixture playwrightFixture;

    public MyProfileTests(PlaywrightFixture playwrightFixture)
    {
        this.playwrightFixture = playwrightFixture;
    }

    [Fact]
    public async Task EditProfile()
    {
        var config = new ConfigurationBuilder()
            .Add("client-secrets.json")
            .Build();
        
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false,
        });
        var context = await browser.NewContextAsync();

        var page = await context.NewPageAsync();

        await page.GotoAsync("https://localhost:7010/");
        await page.GetByRole(AriaRole.Link, new() { NameString = "Login" }).ClickAsync();
        await page.GetByRole(AriaRole.Button, new() { NameString = "Microsoft" }).ClickAsync();
        await page.GetByRole(AriaRole.Textbox, new() { NameString = "Enter your email, phone, or Skype." }).FillAsync("caroline.reinig@gmail.com");
        await page.GetByRole(AriaRole.Textbox, new() { NameString = "Enter your email, phone, or Skype." }).PressAsync("Enter");
        await page.GetByRole(AriaRole.Button, new() { NameString = "Send code" }).ClickAsync();
        await page.GetByRole(AriaRole.Textbox, new() { NameString = "Enter the code you received" }).FillAsync("6192607");
        await page.GetByRole(AriaRole.Button, new() { NameString = "Sign in" }).ClickAsync();
        await page.GetByRole(AriaRole.Button, new() { NameString = "Yes" }).ClickAsync();
        await page.GetByRole(AriaRole.Link, new() { NameString = "My Profile card" }).ClickAsync();
        await page.GetByRole(AriaRole.Paragraph).Filter(new() { HasTextString = "Hey!" }).GetByRole(AriaRole.Button, new() { NameString = "" }).ClickAsync();
        await page.Locator("#status").ClickAsync();
        await page.Locator("#status").FillAsync("Hey!!");
        await page.GetByRole(AriaRole.Button, new() { NameString = "Save changes" }).ClickAsync();


    }
}
