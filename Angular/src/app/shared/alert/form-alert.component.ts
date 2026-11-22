import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { AlertService } from './alert.service';

@Component({
  selector: 'app-form-alert',
  templateUrl: './form-alert.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  standalone: false,
})
export class FormAlertComponent {
  readonly alertService = inject(AlertService);
}
