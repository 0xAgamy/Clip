import { Component, OnInit, OnDestroy, ChangeDetectionStrategy } from '@angular/core';
import { ModalService } from '../../services/modal.service';

@Component({
    selector: 'app-auth',
    templateUrl: './auth.component.html',
    styleUrls: ['./auth.component.css'],
    changeDetection: ChangeDetectionStrategy.Eager,
    standalone: false
})
export class AuthComponent implements OnInit ,OnDestroy{

  constructor(public modal:ModalService) {
    
    } 

    ngOnInit() {
        this.modal.register('auth');
    }


    ngOnDestroy() {
        
      this.modal.unRegister('auth');
    }
    
}
