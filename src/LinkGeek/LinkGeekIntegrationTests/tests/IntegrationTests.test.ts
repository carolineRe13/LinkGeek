import { test, expect, chromium } from '@playwright/test';

test('Account creation test', async () => {
    const browser = await chromium.launch({ headless: false, slowMo: 50 });
    const context = await browser.newContext({ ignoreHTTPSErrors: true });
    const page = await context.newPage();

    let id = Math.floor(Math.random() * 1000);
    await page.goto('https://localhost:7010/');
    await page.getByRole('link', { name: 'Login' }).click();
    await page.getByRole('navigation').getByRole('link', { name: 'Register' }).click();
    await page.getByLabel('Email').click();
    await page.getByLabel('Email').fill('test' + id +'@gmail.com');
    await page.getByLabel('Email').press('Tab');
    await page.getByLabel('Username').fill('test' + id);
    await page.getByLabel('Username').press('Tab');
    await page.locator('#Input_Password').fill('Tt0#' + id);
    await page.getByLabel('Confirm password').click();
    await page.getByLabel('Confirm password').fill('Tt0#' + id);
    await page.getByRole('button', { name: 'Register' }).click();
    
    await browser.close();
});