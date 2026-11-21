import { chromium } from 'playwright';

const base = process.argv[2] ?? 'http://localhost:4200';
const browser = await chromium.launch({ headless: true });
const context = await browser.newContext({ ignoreHTTPSErrors: true });
const page = await context.newPage();
const errors = [];
page.on('pageerror', (e) => errors.push(`PAGEERROR: ${e.message.slice(0, 160)}`));

await page.goto(base + '/', { waitUntil: 'networkidle', timeout: 45000 });
await page.waitForTimeout(2000);
console.log('home cards:', await page.locator('app-clips-list a[href*="clip"]').count());

await page.click('text=Login / Register');
await page.waitForTimeout(500);
console.log('modal opens:', !(await page.locator('app-modal #modal').first().evaluate((el) => el.className)).includes('hidden'));

// wrong credentials -> API error alert
await page.fill('app-modal input[type=email]', 'nobody@example.com');
await page.fill('app-modal input[type=password]', 'WrongPass123');
await page.locator('app-modal button[type=submit]').first().click();
await page.waitForTimeout(2500);
let alertText = await page.locator('app-modal app-form-alert').first().innerText().catch(() => '');
console.log('bad-login alert:', JSON.stringify(alertText.trim()));

// dismiss
const dismiss = page.locator('app-modal app-form-alert button[aria-label=Dismiss]').first();
if (await dismiss.count()) {
  await dismiss.click();
  await page.waitForTimeout(300);
  console.log('alert dismissed:', (await page.locator('app-modal app-form-alert').count()) === 0);
}

// register tab still renders its own form-alert
await page.locator('app-modal a', { hasText: 'Register' }).first().click().catch(() => {});
await page.waitForTimeout(500);
console.log('register form-alert present:', (await page.locator('app-modal app-form-alert').count()) >= 1);
console.log('page errors:', errors.length ? errors : 'none');
await browser.close();
