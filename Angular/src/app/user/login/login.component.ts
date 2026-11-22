import { Component, ChangeDetectionStrategy } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { AlertService } from '../../shared/alert/alert.service';
import { ILogin } from '../models/login';
import { UserServices } from '../services/user-services.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  changeDetection: ChangeDetectionStrategy.Eager,
  standalone: false,
  // Per-component alert state so a message never leaks into another page.
  providers: [AlertService],
})
export class LoginComponent {
  loginForm: FormGroup;

  constructor(
    private userServices: UserServices,
    private alert: AlertService,
  ) {
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
    if (this.loginForm.invalid) {
      this.alert.error('Enter your email and password.');
      return;
    }

    const loginmodal: ILogin = this.loginForm.value;
    this.alert.info('You will be in, in a second...');
    this.userServices.Login(loginmodal).subscribe({
      next: () => this.alert.success('You are logged in.'),
      error: () => this.alert.error('Invalid email or password.'),
    });
  }
}
