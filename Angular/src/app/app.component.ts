import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { UserServicesService } from './user/UserServices/user-services.service';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css'],
    changeDetection: ChangeDetectionStrategy.Eager,
    standalone: false
})
export class AppComponent implements OnInit  {

  constructor(public UserService:UserServicesService) {
    
  }
ngOnInit(): void {
    this.RefreshUser(); 
}
  private RefreshUser(){
    const jwt =this.UserService.getJWT();
    if(jwt){
      this.UserService.RefreshUser(jwt).subscribe({
        next:_ =>{},
        error: _=>{
          this.UserService.Logout();
        },

      })
    }else{
      this.UserService.RefreshUser(null).subscribe();
    }
  }



}
