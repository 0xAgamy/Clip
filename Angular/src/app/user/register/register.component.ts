import { Component, ChangeDetectionStrategy } from '@angular/core';
import { FormGroup,FormControl,Validators } from '@angular/forms';
import { IRegister } from '../userModels/register';
import { UserServices } from '../UserServices/user-services.service';

@Component({
    selector: 'app-register',
    templateUrl: './register.component.html',
    styleUrls: ['./register.component.css'],
    changeDetection: ChangeDetectionStrategy.Eager,
    standalone: false
})
// min Validator for Nubmers  , and minLength validator for strings
export class RegisterComponent {
  showAlert=false;
  alertMsg='Please Wait ! Your Acount is Begin Created'
  alertColor='blue'
  ErorrMesg=[];
  constructor(private userServices:UserServices) {
  
  }
registerForm=new FormGroup({
  name:new FormControl('',[
    Validators.required,
    Validators.minLength(3)
  ]),
  email:new FormControl('',[Validators.email,Validators.required]),
  age:new FormControl('',[
    Validators.required,
    Validators.min(16),
    Validators.max(120),
  ]),
  password:new FormControl('',[
    Validators.required,
    Validators.pattern(/^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$/gm)]),


  confirm_password:new FormControl('',[
    Validators.required,

  ]),
  phoneNumber:new FormControl('',[
    Validators.required,
    Validators.minLength(11),
    Validators.maxLength(11)
  ]),
})

get Age(){
  return this.registerForm.controls.age;
}
get Password(){
  return this.registerForm.controls.password;
}

get confirmPassword(){
  return this.registerForm.controls.confirm_password;
}

get Name(){
  return this.registerForm.controls.name;
}

get Email(){
  return this.registerForm.controls.email;
}
get PhoneNumber(){
  return this.registerForm.controls.phoneNumber;
}
  

Register(){
  this.showAlert=true;
  this. alertMsg='Please Wait ! Your Acount is Begin Created';
  this.alertColor='blue';
  let user = {
    userName:this.registerForm.value.name,
    email:this.registerForm.value.email,
    password:this.registerForm.value.password,
    phoneNumber:this.registerForm.value.phoneNumber,
  }as IRegister;
  let u 

  if(this.registerForm.valid){
  this.userServices.register(user).subscribe({
    next:(response)=>{
      console.log(response)
    },
    error:error=>{
    console.log(error);
    }
  })    
  }

}
}
