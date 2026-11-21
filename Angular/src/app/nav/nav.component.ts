import { Component, ChangeDetectionStrategy } from '@angular/core';
import { ModalService } from '../services/modal.service';
import { UserServices } from '../user/services/user-services.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css'],
  changeDetection: ChangeDetectionStrategy.Eager,
  standalone: false,
})
export class NavComponent {
  constructor(
    public modal: ModalService,
    public user: UserServices,
  ) {}
  openModal($event: Event) {
    $event.preventDefault();
    this.modal.toggleModel('auth');
  }

  Logout() {
    this.user.Logout();
  }
}
