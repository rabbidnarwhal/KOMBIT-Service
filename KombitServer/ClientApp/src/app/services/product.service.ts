import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  listSolution = [];
  listUser = [];
  currency = '';
  productPosterId = 0;
  constructor(private apiService: ApiService, private authService: AuthService) {
    this.fetchListSolution().then((res) => {
      this.listSolution = res;
    });
    this.fetchListUser().then((res) => {
      this.listUser = res;
    });
  }

  getListProduct() {
    return this.apiService.get('/product');
  }

  postProduct(data) {
    return this.apiService.post('/product', data);
  }

  editProduct(data, productId) {
    return this.apiService.post('/product/' + productId, data);
  }

  fetchListSolution() {
    return this.apiService.get('/category');
  }

  fetchListUser() {
    return this.apiService.get('/users/list/' + this.authService.getUserId());
  }

  fetchCurrency() {
    return this.apiService.get('/config');
  }

  fetchEditableProductData(id) {
    return this.apiService.get('/product/' + id + '/edit');
  }

  getListSolution(): Promise<Array<any>> {
    return new Promise((resolve, reject) => {
      if (!this.listSolution.length) {
        this.fetchListSolution()
          .then((res) => {
            this.listSolution = res;
            resolve(this.listSolution);
          })
          .catch((err) => reject(err));
      } else {
        resolve(this.listSolution);
      }
    });
  }

  getListUser(): Promise<Array<any>> {
    return new Promise((resolve, reject) => {
      if (!this.listSolution.length) {
        this.fetchListUser()
          .then((res) => {
            this.listUser = res;
            resolve(this.listUser);
          })
          .catch((err) => reject(err));
      } else {
        resolve(this.listUser);
      }
    });
  }

  getCurrency(): Promise<string> {
    return new Promise((resolve, reject) => {
      if (!this.currency) {
        this.fetchCurrency()
          .then((res) => {
            res.map((element) => {
              if (element.paramCode === 'DEFAULT_CURRENCY') {
                this.currency = element.paramValue;
              }
            });
            resolve(this.currency);
          })
          .catch((err) => reject(err));
      } else {
        resolve(this.currency);
      }
    });
  }

  getProductPosterId(): number {
    return this.productPosterId;
  }

  setProductPosterId(id = 0): void {
    this.productPosterId = id;
  }
}
