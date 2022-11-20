
import { chromium } from '@playwright/test';
import { AuthPage } from './AuthPage';
import { promises } from 'fs';

async function isFileExists(path) {
    try {
        await promises.access(path);
        return true;
    } catch {
        return false;
    }
}

async function globalSetup(config) {
    const [project] = config.projects;
    const { storageState, baseURL } = project.use;
    const result = await isFileExists(storageState);

    if (result) {
        return;
    }

    const browser = await chromium.launch();
    const page = await browser.newPage();

    const auth = new AuthPage(page, baseURL);
    await auth.login();

    await page.context().storageState({
        path: storageState,
    });

    await browser.close();
}

export default globalSetup;