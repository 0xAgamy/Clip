import { chromium } from 'playwright';
const email = 'plan12-1795273055@example.com';
const browser = await chromium.launch({ headless: true });
const ctx = await browser.newContext({ ignoreHTTPSErrors: true });
const page = await ctx.newPage();
const errs = [];
page.on('pageerror', (e) => errs.push(e.message.slice(0, 160)));

await page.goto('http://localhost:4200/', { waitUntil: 'networkidle' });
await page.waitForTimeout(1500);
await page.click('text=Login / Register');
await page.fill('app-modal input[type=email]', email);
await page.fill('app-modal input[type=password]', 'Plan12Pass');
await page.locator('app-modal button[type=submit]').first().click();
await page.waitForTimeout(2500);
console.log('nav shows Manage:', await page.locator('a:has-text("Manage")').count());

await page.click('a:has-text("Manage")');
await page.waitForTimeout(2000);
console.log('url:', page.url());
console.log('delete modal in DOM:', await page.locator('app-modal #modal').count());

const toggled = await page.evaluate(() => {
  const ng = window.ng;
  const nav = ng.getComponent(document.querySelector('app-nav'));
  nav.modal.toggle('delete-video');
  return nav.modal.isOpen('delete-video');
});
await page.waitForTimeout(400);
const visible = await page.locator('app-modal #modal').first().evaluate((el) => !el.className.includes('hidden'));
console.log('service says open:', toggled, '| DOM visible:', visible);
await page.evaluate(() => window.ng.getComponent(document.querySelector('app-nav')).modal.toggle('delete-video'));
await page.waitForTimeout(400);
console.log('closes again:', await page.locator('app-modal #modal').first().evaluate((el) => el.className.includes('hidden')));
console.log('page errors:', errs.length ? errs : 'none');
await browser.close();
