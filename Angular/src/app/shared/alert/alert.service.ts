import { Injectable, signal } from '@angular/core';

export type AlertColor = 'info' | 'success' | 'error';

/**
 * Holds the message currently shown by `<app-form-alert />`.
 *
 * Scoping rule: components that render a form alert must list `AlertService` in their
 * own `providers`, so each page/panel gets a private instance. The root instance is only a
 * fallback — using it directly would make a message from one page appear on every other page
 * (the alert is state, not an event).
 */
@Injectable({
  providedIn: 'root',
})
export class AlertService {
  readonly visible = signal(false);
  readonly message = signal('');
  readonly color = signal<AlertColor>('info');

  show(message: string, color: AlertColor = 'info'): void {
    this.message.set(message);
    this.color.set(color);
    this.visible.set(true);
  }

  info(message: string): void {
    this.show(message, 'info');
  }

  success(message: string): void {
    this.show(message, 'success');
  }

  error(message: string): void {
    this.show(message, 'error');
  }

  dismiss(): void {
    this.visible.set(false);
  }
}
