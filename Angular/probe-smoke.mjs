import { chromium } from 'playwright';

const base = process.argv[2] ?? 'http://localhost:4200';
const browser = await chromium.launch({ headless: true });
const context = await browser.newContext({ ignoreHTTPSErrors: true });
const page = await context.newPage();

const errors = [];
page.on('pageerror', (err) => errors.push(`PAGEERROR: ${err.message.slice(0, 200)}`));
page.on('console', (msg) => {
  if (msg.type() === 'error') errors.push(`CONSOLE: ${msg.text().slice(0, 200)}`);
});

await page.goto(base + '/', { waitUntil: 'networkidle', timeout: 45000 });
await page.waitForTimeout(2500);

const cards = await page.locator('app-clips-list a[href*="clip"]').count();
console.log('home clip cards:', cards);

await page.click('text=Login / Register');
await page.waitForTimeout(700);
const modalClass = await page.locator('app-modal #modal').first().evaluate((el) => el.className);
console.log('modal opens:', !modalClass.includes('hidden'), '|', modalClass);
console.log('login form inputs:', await page.locator('app-modal input[type=email]').count());

// close modal by clicking the X, then open a clip
await page.locator('app-modal .modal-close').first().click();
await page.waitForTimeout(400);
const closed = await page.locator('app-modal #modal').first().evaluate((el) => el.className);
console.log('modal closes:', closed.includes('hidden'));

if (cards > 0) {
  await page.locator('app-clips-list a[href*="clip"]').first().click();
  await page.waitForTimeout(2500);
  console.log('clip url:', page.url());
  console.log('clip <video>:', await page.locator('video').count());
  const src = await page.locator('video source, video').first().getAttribute('src').catch(() => null);
  console.log('video src set:', Boolean(src));
}

console.log('errors:', errors.length ? errors : 'none');
await browser.close();
