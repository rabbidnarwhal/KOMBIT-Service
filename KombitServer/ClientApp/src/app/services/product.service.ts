import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { AuthService } from './auth.service';
import { APOSTROPHE } from 'ng-zorro-antd';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  listSolution = [];
  listUser = [];
  currency = '';
  productPosterId = 0;
  intervalProduct = { value: 0, type: '' };
  constructor(private apiService: ApiService, private authService: AuthService) {
    if (this.authService.isLoggin && this.authService.getRole() === 'Supplier') {
      this.fetchListSolution().then((res) => {
        this.listSolution = res;
      });
      this.fetchListUser().then((res) => {
        this.listUser = res;
      });
    }
  }

  getListProduct() {
    const header = { 'Cache-Control': 'no-cache' };
    return this.apiService.get('/product', { headers: header });
  }

  getDetailProduct(id) {
    return this.apiService.get('/product/' + id);
  }

  getListAllProduct() {
    return this.apiService.get('/product/all');
  }

  postProduct(data) {
    return this.apiService.post('/product', data);
  }

  editProduct(data, productId) {
    return this.apiService.post('/product/' + productId, data);
  }

  promoteProduct(productId) {
    return this.apiService.post('/product/' + productId + '/promote', {});
  }

  demoteProduct(productId) {
    return this.apiService.post('/product/' + productId + '/demote', {});
  }

  activeProduct(productId) {
    return this.apiService.post('/product/' + productId + '/active', {});
  }

  deactiveProduct(productId) {
    return this.apiService.post('/product/' + productId + '/deactive', {});
  }

  fetchListSolution() {
    return this.apiService.get('/category');
  }

  fetchListUser() {
    return this.apiService.get('/users/list');
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

  getIntervalProduct(): Promise<{ value: number; type: string }> {
    return new Promise((resolve, reject) => {
      if (!this.intervalProduct.value) {
        this.fetchIntervalProduct()
          .then((res) => {
            console.log('paramValue', res);
            const interval = res.paramValue / (60 * 60 * 24);
            console.log('interval', interval);
            this.intervalProduct.value = interval % 30 === 0 ? interval / 30 : interval;
            this.intervalProduct.type = interval % 30 === 0 ? 'month' : 'day';
            resolve(this.intervalProduct);
          })
          .catch((err) => reject(err));
      } else {
        resolve(this.intervalProduct);
      }
    });
  }

  fetchIntervalProduct() {
    return this.apiService.get('/config/DEFAULT_PRODUCT_INTERVAL');
  }

  getProductPosterId(): number {
    return this.productPosterId;
  }

  setProductPosterId(id = 0): void {
    this.productPosterId = id;
  }

  setProductInterval(interval, type) {
    const request = {
      ParamCode: 'DEFAULT_PRODUCT_INTERVAL',
      ParamValue: type === 'day' ? interval * 60 * 60 * 24 : interval * 60 * 60 * 24 * 30
    };
    this.intervalProduct.type = type;
    this.intervalProduct.value = interval;
    return this.apiService.post('/config/product_interval', request);
  }

  deleteProduct(id: number) {
    return this.apiService.delete('/product/' + id);
  }
}
