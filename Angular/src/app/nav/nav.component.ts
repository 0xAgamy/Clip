import { Component, ChangeDetectionStrategy } from '@angular/core';
import { ModalService } from '../services/modal.service';
import { UserServicesService } from '../user/UserServices/user-services.service';

@Component({
    selector: 'app-nav',
    templateUrl: './nav.component.html',
    styleUrls: ['./nav.component.css'],
    changeDetection: ChangeDetectionStrategy.Eager,
    standalone: false
})
export class NavComponent {
 name?:string=''; 
constructor(public modal:ModalService,public user:UserServicesService) {
      this.name=this.user.userName.toUpperCase();    
      console.log(this.name)
    }
openModal($event:Event){
      $event.preventDefault()
      this.modal.toggleModel('auth');
    }

    Logout(){
        this.user.Logout()
      
    }
}
