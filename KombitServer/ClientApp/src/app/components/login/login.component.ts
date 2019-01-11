import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { LoginRequest } from 'src/app/models/login-request';
import { AuthService } from 'src/app/services/auth.service';
import { NzMessageService } from 'ng-zorro-antd';

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

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private route: Router,
    private messageService: NzMessageService
  ) {}

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

  async submitForm() {
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
      try {
        await this.authService.login(this.loginRequest);
        this.isLoading = false;
      } catch (error) {
        this.messageService.error(error);
        this.isLoading = false;
      }
    }
  }
}
