import { Component, OnInit, OnDestroy } from '@angular/core';
import { ProductService } from 'src/app/services/product.service';
import { AuthService } from 'src/app/services/auth.service';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import { NzMessageService } from 'ng-zorro-antd';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: [ './product-list.component.scss' ]
})
export class ProductListComponent implements OnInit, OnDestroy {
  originProduct = [];
  products = [];
  skeletons: any = [ 0, 0, 0, 0, 0 ];
  uid = 0;
  productSubscription: any;
  isFiltered = false;
  isLoading = true;
  constructor(
    private productService: ProductService,
    private authService: AuthService,
    private route: Router,
    private activatedRoute: ActivatedRoute,
    private message: NzMessageService
  ) {
    this.products = this.skeletons;
    this.productSubscription = this.activatedRoute.queryParams.subscribe((res) => {
      if (res.hasOwnProperty('myPost') && res.myPost) {
        this.isFiltered = true;
        this.products = this.originProduct.filter((x) => x.posterId === this.uid);
      } else {
        this.isFiltered = false;
        this.products = this.originProduct;
      }
    });
  }

  async ngOnInit() {
    try {
      this.checkLogin();
      this.productService.setProductPosterId();
      this.originProduct = await this.productService.getListProduct();
      this.products = this.originProduct;
      this.isLoading = false;
    } catch (error) {
      this.message.error(error, { nzDuration: 5000, nzPauseOnHover: true, nzAnimate: true });
    }
  }

  ngOnDestroy() {
    this.productSubscription.unsubscribe();
  }

  checkLogin() {
    this.authService.isLoggin.subscribe((isLogin) => {
      if (isLogin) {
        this.uid = this.authService.getUserId();
      } else {
        this.uid = 0;
      }
    });
  }

  openGooglePlay() {
    alert('to google play web page');
    // window.location.href = 'http://google.com/';
  }

  editProduct(id, posterId) {
    this.productService.setProductPosterId(posterId);
    this.route.navigate([ '/product/edit/', id ]);
  }

  async deleteProduct(id) {
    try {
      const deleteSuccess = await this.productService.deleteProduct(id);
      this.message.success(deleteSuccess.msg, { nzDuration: 5000, nzPauseOnHover: true, nzAnimate: true });
      this.isLoading = true;
      this.products = this.skeletons;
      this.originProduct = await this.productService.getListProduct();
      this.products = this.isFiltered ? this.originProduct.filter((x) => x.posterId === this.uid) : this.originProduct;
      this.isLoading = false;
    } catch (error) {
      this.message.error(error.toString(), { nzDuration: 5000, nzPauseOnHover: true, nzAnimate: true });
    }
  }
}
