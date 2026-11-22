import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ModalComponent } from './modal/modal.component';
import { TabsContainerComponent } from './tabs-container/tabs-container.component';
import { TabComponent } from './tab/tab.component';
import { AlertComponent } from './alert/alert.component';
import { FormAlertComponent } from './alert/form-alert.component';
import { EventBlockerDirective } from './directives/event-blocker.directive';

@NgModule({
  declarations: [
    ModalComponent,
    TabsContainerComponent,
    TabComponent,
    AlertComponent,
    FormAlertComponent,
    EventBlockerDirective,
  ],
  imports: [CommonModule],
  exports: [
    ModalComponent,
    TabsContainerComponent,
    TabComponent,
    AlertComponent,
    FormAlertComponent,
    EventBlockerDirective,
  ],
})
export class SharedModule {}
