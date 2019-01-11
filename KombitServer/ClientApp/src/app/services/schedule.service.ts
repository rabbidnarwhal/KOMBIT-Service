import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class ScheduleService {
  constructor(private apiService: ApiService, private authService: AuthService) {}

  getSchedules(): Promise<Array<any>> {
    const uid = this.authService.getUserId();
    return this.apiService.get('/appointment/user/' + uid);
  }

  getScheduleDetail(id: number) {
    return this.apiService.get('/appointment/' + id);
  }
}
