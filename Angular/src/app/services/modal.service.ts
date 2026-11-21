import { Injectable } from '@angular/core';
interface IModal {
  id: string;
  visible: boolean;
}
@Injectable({
  providedIn: 'root',
})
export class ModalService {
  private modals: IModal[] = [];
  constructor() {}

  register(id: string) {
    this.modals.push({
      id,
      visible: false,
    });
  }
  unRegister(id: string) {
    this.modals = this.modals.filter((ele) => ele.id !== id);
  }
  isOpen(id: string): boolean {
    return !!this.modals.find((ele) => ele.id == id)?.visible;
  }
  toggle(id: string) {
    const modal = this.modals.find((ele) => ele.id == id);
    if (modal) {
      modal.visible = !modal.visible;
    }
  }
}
