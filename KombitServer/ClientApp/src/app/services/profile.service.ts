import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class ProfileService {
  constructor(private apiService: ApiService, private authService: AuthService) {}

  getProfile() {
    const id = this.authService.getUserId();
    return this.apiService.get('/users/' + id);
  }
}
