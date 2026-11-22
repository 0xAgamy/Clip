import { Injectable, computed, signal } from '@angular/core';

interface IModal {
  id: string;
  visible: boolean;
}

@Injectable({
  providedIn: 'root',
})
export class ModalService {
  private readonly modals = signal<IModal[]>([]);
  readonly openIds = computed(() =>
    this.modals()
      .filter((modal) => modal.visible)
      .map((modal) => modal.id),
  );

  register(id: string): void {
    this.modals.update((list) =>
      list.some((modal) => modal.id === id) ? list : [...list, { id, visible: false }],
    );
  }

  unRegister(id: string): void {
    this.modals.update((list) => list.filter((modal) => modal.id !== id));
  }

  isOpen(id: string): boolean {
    return this.modals().some((modal) => modal.id === id && modal.visible);
  }

  toggle(id: string): void {
    this.modals.update((list) =>
      list.map((modal) => (modal.id === id ? { ...modal, visible: !modal.visible } : modal)),
    );
  }
}
