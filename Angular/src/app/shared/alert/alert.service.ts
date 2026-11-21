import { Injectable, signal } from '@angular/core';

export type AlertColor = 'info' | 'success' | 'error';

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
