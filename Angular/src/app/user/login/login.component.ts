import { Component, Output, ChangeDetectionStrategy } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ILogin } from '../userModels/login';
import { UserServices } from '../UserServices/user-services.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  changeDetection: ChangeDetectionStrategy.Eager,
  standalone: false,
})
export class LoginComponent {
  loginForm: FormGroup;
  Authenticatd = false;
  showAlert = false;
  alertMsg = ' You will be in, in a second';
  @Output() alertColor: string = 'blue';
  constructor(private UserServices: UserServices) {
    this.loginForm = new FormGroup({
      email: new FormControl('', [Validators.required]),
      password: new FormControl('', [Validators.required]),
    });
  }

  get Email() {
    return this.loginForm.get('email');
  }
  get Password() {
    return this.loginForm.get('password');
  }

  login() {
    const loginmodal: ILogin = this.loginForm.value;

    if (this.loginForm.valid) {
      this.UserServices.Login(loginmodal).subscribe(() => {});
    }

    this.showAlert = true;
  }
}
