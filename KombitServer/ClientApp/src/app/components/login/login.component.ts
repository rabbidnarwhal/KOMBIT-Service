import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { LoginRequest } from 'src/app/models/login-request';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: [ './login.component.scss' ]
})
export class LoginComponent implements OnInit {
  validateForm: FormGroup;
  loginRequest: LoginRequest;
  errorMessage: string;
  errorDescription: string;

  isLoading = false;
  isError = false;
  errorType = 'error';

  constructor(private fb: FormBuilder, private authService: AuthService, private route: Router) {}

  ngOnInit(): void {
    this.validateForm = this.fb.group({
      userName: [ '', [ Validators.required ] ],
      password: [ '', [ Validators.required ] ],
      remember: [ false ]
    });

    const remember = localStorage.getItem('remember') || null;
    if (remember === 'true') {
      this.validateForm.get('remember').setValue(true);
      this.validateForm
        .get('userName')
        .setValue(localStorage.getItem('username') ? localStorage.getItem('username') : '');
    }
  }

  submitForm(): void {
    this.isError = false;

    // tslint:disable-next-line:forin
    for (const i in this.validateForm.controls) {
      this.validateForm.controls[i].markAsDirty();
      this.validateForm.controls[i].updateValueAndValidity();
    }
    if (this.validateForm.valid) {
      if (this.validateForm.get('remember').value) {
        localStorage.setItem('remember', 'true');
        localStorage.setItem('username', this.validateForm.get('userName').value);
      } else {
        localStorage.clear();
      }
      this.loginRequest = {
        Username: this.validateForm.get('userName').value,
        Password: this.validateForm.get('password').value
      };
      this.isLoading = true;
      this.authService
        .login(this.loginRequest)
        .then(() => {
          this.isLoading = false;
        })
        .catch((err) => {
          this.errorDescription = err;
          this.errorMessage = 'Error Message';
          this.isLoading = false;
          this.isError = true;
        });
    }
  }

  afterErrorClose() {
    this.isError = false;
    this.errorMessage = null;
    this.errorDescription = null;
  }
}
