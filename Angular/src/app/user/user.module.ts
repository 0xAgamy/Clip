import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthComponent } from './auth/auth.component';
import { ReactiveFormsModule,FormsModule } from '@angular/forms';
import { SharedModule } from '../shared/shared.module';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { provideHttpClient, } from '@angular/common/http';

@NgModule({ declarations: [
        AuthComponent,
        LoginComponent,
        RegisterComponent
    ],
    exports: [
        AuthComponent,
    ], imports: [CommonModule,
        SharedModule,
        ReactiveFormsModule,
        FormsModule], providers: [provideHttpClient()] })
export class UserModule { }
