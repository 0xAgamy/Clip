import { Injectable } from '@angular/core';
interface IModal {
  id: string;
  visable: boolean;
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
      visable: false,
    });
  }
  unRegister(id: string) {
    this.modals = this.modals.filter((ele) => ele.id !== id);
  }
  isModelOpen(id: string): boolean {
    return !!this.modals.find((ele) => ele.id == id)?.visable;
  }
  toggleModel(id: string) {
    const modal = this.modals.find((ele) => ele.id == id);
    if (modal) {
      modal.visable = !modal.visable;
    }
  }
}
