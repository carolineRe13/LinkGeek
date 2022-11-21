import { test, expect } from '@playwright/test';

test('Account creation and update status', async ({ page}) => {
    test.setTimeout(120000);
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
    
    await page.getByRole('link', { name: 'Click here to confirm your account' }).click();
    await page.getByRole('link', { name: 'Login' }).click();
    await page.getByLabel("Username").click();
    await page.getByLabel("Username").fill('test' + id);
    await page.getByLabel('Password').press('Tab');
    await page.getByLabel('Password').fill('Tt0#' + id);
    await page.getByRole('button', { name: 'Log in' }).click();

    // go to My profile card and change status
    await page.getByRole('navigation').getByRole('link', { name: 'My Profile card' }).click();
    // not very clean but else the test behaves flaky
    await page.waitForTimeout(1000);
    await page.locator("id=statusPencil").click();
    await page.locator("#status").click();
    await page.locator("#status").fill("Hey!!");
    await page.getByRole('button', { name: 'Save changes' }).click();

    // Assert
    await expect(page.locator('id=userStatus')).toHaveText('Hey!!');
});