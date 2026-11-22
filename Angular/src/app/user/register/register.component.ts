import { Component, ChangeDetectionStrategy } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { AlertService } from '../../shared/alert/alert.service';
import { IRegister } from '../models/register';
import { UserServices } from '../services/user-services.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
  changeDetection: ChangeDetectionStrategy.Eager,
  standalone: false,
  // Per-component alert state so a message never leaks into another page.
  providers: [AlertService],
})
export class RegisterComponent {
  constructor(
    private userServices: UserServices,
    private alert: AlertService,
  ) {}

  registerForm = new FormGroup({
    name: new FormControl('', [Validators.required, Validators.minLength(3)]),
    email: new FormControl('', [Validators.email, Validators.required]),
    age: new FormControl('', [Validators.required, Validators.min(16), Validators.max(120)]),
    password: new FormControl('', [
      Validators.required,
      Validators.pattern(/^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$/gm),
    ]),

    confirm_password: new FormControl('', [Validators.required]),
    phoneNumber: new FormControl('', [
      Validators.required,
      Validators.minLength(11),
      Validators.maxLength(11),
    ]),
  });

  get Age() {
    return this.registerForm.controls.age;
  }
  get Password() {
    return this.registerForm.controls.password;
  }

  get confirmPassword() {
    return this.registerForm.controls.confirm_password;
  }

  get Name() {
    return this.registerForm.controls.name;
  }

  get Email() {
    return this.registerForm.controls.email;
  }
  get PhoneNumber() {
    return this.registerForm.controls.phoneNumber;
  }

  Register() {
    if (this.registerForm.invalid) {
      this.alert.error('Please fill in all required fields.');
      return;
    }

    const user = {
      userName: this.registerForm.value.name,
      email: this.registerForm.value.email,
      password: this.registerForm.value.password,
      phoneNumber: this.registerForm.value.phoneNumber,
    } as IRegister;

    this.alert.info('Please wait, your account is being created...');
    this.userServices.register(user).subscribe({
      next: () => {
        this.alert.success('Account created! You can log in now.');
        this.registerForm.reset();
      },
      error: () => this.alert.error('Could not create the account. Please try again.'),
    });
  }
}
