import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { ActiveCustomer, ActiveSupplier, MostPopularProduct } from '../models/dashboard-response';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  activeCustomer: ActiveCustomer;
  activeSupplier: ActiveSupplier;
  mostPopularProduct: MostPopularProduct;

  constructor(private apiService: ApiService) {}

  getActiveCustomer(): Promise<ActiveCustomer> {
    return new Promise((resolve, reject) => {
      if (this.activeCustomer) {
        resolve(this.activeCustomer);
      } else {
        this.apiService
          .get('/users/active/customer')
          .then((res) => {
            this.activeCustomer = res;
            resolve(this.activeCustomer);
          })
          .catch((err) => reject(err));
      }
    });
  }

  getActiveSupplier(): Promise<ActiveSupplier> {
    return new Promise((resolve, reject) => {
      if (this.activeSupplier) {
        resolve(this.activeSupplier);
      } else {
        this.apiService
          .get('/users/active/supplier')
          .then((res) => {
            this.activeSupplier = res;
            resolve(this.activeSupplier);
          })
          .catch((err) => reject(err));
      }
    });
  }

  getMostPopularProduct(): Promise<MostPopularProduct> {
    return new Promise((resolve, reject) => {
      if (this.mostPopularProduct) {
        resolve(this.mostPopularProduct);
      } else {
        this.apiService
          .get('/product/popular')
          .then((res) => {
            this.mostPopularProduct = res;
            resolve(this.mostPopularProduct);
          })
          .catch((err) => reject(err));
      }
    });
  }
}
