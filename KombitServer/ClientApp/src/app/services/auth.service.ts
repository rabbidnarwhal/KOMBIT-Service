import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { LoginRequest } from '../models/login-request';
import { environment } from 'src/environments/environment';
import { Router } from '@angular/router';
import { LoginResponse } from '../models/login-response';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  loggedIn = false;
  isLoggin = new BehaviorSubject<boolean>(false);
  private user: LoginResponse = new LoginResponse();

  constructor(private apiService: ApiService, private route: Router) {}

  isAuthenticated(): Promise<boolean> {
    return new Promise((resolve) => {
      if (!this.loggedIn) {
        this.reAuth()
          .then(() => {
            this.isLoggin.next(true);
            resolve(true);
          })
          .catch(() => {
            resolve(false);
          });
      } else {
        resolve(this.getToken() ? true : false);
      }
    });
  }

  reAuth() {
    return new Promise((resolve, reject) => {
      if (this.getToken()) {
        const token = this.getToken();
        const id = atob(token[0]);
        const idNumber = atob(token[1]);
        this.apiService
          .get('/users/' + id + '/' + idNumber)
          .then((res: LoginResponse) => {
            this.setSession(res);
            resolve();
          })
          .catch((err) => {
            reject(err);
          });
      }
    });
  }

  login(loginRequest: LoginRequest): Promise<any> {
    return new Promise((resolve, reject) => {
      this.apiService
        .post('/login', loginRequest)
        .then((res: LoginResponse) => {
          if (res.role === 'Administrator' || res.role === 'Supplier') {
            this.setSession(res);
            this.isLoggin.next(true);
            if (res.role === 'Administrator') {
              this.route.navigate([ '/dashboard' ]);
            } else {
              this.route.navigate([ '' ]);
            }
            resolve();
          } else {
            reject('Unauthorize');
          }
        })
        .catch((err) => reject(err));
    });
  }

  logout() {
    this.clearSession();
    this.loggedIn = false;
    this.isLoggin.next(false);
    this.route.navigate([ '' ]);
  }

  setSession(response: LoginResponse) {
    const idNumber = btoa(response.idNumber);
    const role = btoa(response.role);
    const id = btoa(response.id + '');
    const date = new Date().getTime().toString();
    const token = btoa(id + '|' + idNumber + '|' + role + '|' + date);
    this.loggedIn = true;
    this.user = response;

    const expiresAt = new Date().setSeconds(86400).toString();
    localStorage.setItem('expires_at', JSON.stringify(expiresAt.valueOf()));
    localStorage.setItem(environment.AUTHENTICATION.TOKENNAME, token);
  }

  getToken() {
    const token = localStorage.getItem(environment.AUTHENTICATION.TOKENNAME);
    try {
      const splittedToken = atob(token).split('|');
      if (token) {
        const expired = +this.getExpired();
        if ((expired - +splittedToken[3]) / 1000 > 86400) {
          this.clearSession();
          return null;
        }
      }
      return token ? splittedToken : null;
    } catch (error) {
      this.clearSession();
      return null;
    }
  }

  getExpired() {
    const expired = localStorage.getItem('expires_at');
    return expired ? expired : null;
  }

  getRole() {
    return this.user.role || null;
  }

  getPrincipal() {
    return this.user;
  }

  getUserId() {
    return this.user.id;
  }

  getUserName() {
    return this.user.name;
  }

  clearSession() {
    localStorage.removeItem(environment.AUTHENTICATION.TOKENNAME);
    localStorage.removeItem('expires_at');
    this.loggedIn = false;
    this.user = null;
  }
}
