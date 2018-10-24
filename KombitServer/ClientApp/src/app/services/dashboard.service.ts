import { Injectable } from '@angular/core';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  constructor(private apiService: ApiService) {}

  getActiveCustomer(): Promise<any> {
    return this.apiService.get('/users/active/customer');
  }

  getActiveSupplier(): Promise<any> {
    return this.apiService.get('/users/active/supplier');
  }

  getMostPopularProduct(): Promise<any> {
    return this.apiService.get('/product/popular');
  }
}
