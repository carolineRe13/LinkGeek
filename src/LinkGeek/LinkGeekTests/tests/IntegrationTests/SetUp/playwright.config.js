import { devices } from '@playwright/test';

const STORAGE_STATE_FILE_PREFIX = 'state';
const STORAGE_STATE_PATH = './e2e';
const EXPIRES_IN_MINUTES = 60;

const getStorageStateFileName = () => {
    const lastFile = fs.readdirSync(STORAGE_STATE_PATH)
        .filter(name => name.startsWith(STORAGE_STATE_FILE_PREFIX))
        .pop();
    const currentTime = Date.now();

    if (lastFile) {
        const [,lastTimestamp] = lastFile.split('.');
        const dateDiffInMinutes = Math.floor((currentTime - parseInt(lastTimestamp)) / 1000 / 60);

        return dateDiffInMinutes > EXPIRES_IN_MINUTES
            ? currentTime
            : lastTimestamp;
    }

    return currentTime;
};

const config = {
    globalSetup: './e2e/globalSetup',
    use: {
        baseURL: process.env.BASE_URL,
        // generates new filename concatenated with timestamp or returns old one (for example, state.1645720991712.json)
        storageState: `${STORAGE_STATE_PATH}/state.${getStorageStateFileName()}.json`,
    },
    projects: [
        {
            name: 'chromium',
            use: { ...devices['Desktop Chrome'] },
        },
    ],
    testDir: './src',
    testMatch: '**/*.spec.js'
};

export default config;