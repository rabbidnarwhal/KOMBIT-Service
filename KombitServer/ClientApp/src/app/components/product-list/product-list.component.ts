import { Component, OnInit } from '@angular/core';
import { ProductService } from 'src/app/services/product.service';
import { AuthService } from 'src/app/services/auth.service';
import { Router } from '@angular/router';
import { NzMessageService } from 'ng-zorro-antd';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: [ './product-list.component.scss' ]
})
export class ProductListComponent implements OnInit {
  products: any = [ 0, 0, 0, 0, 0 ];
  uid = 0;
  constructor(
    private productService: ProductService,
    private authService: AuthService,
    private route: Router,
    private message: NzMessageService
  ) {}

  ngOnInit() {
    this.productService.setProductPosterId();
    this.productService
      .getListProduct()
      .then((res) => {
        this.products = res;
      })
      .catch((err) => {
        this.message.error(err, { nzDuration: 5000, nzPauseOnHover: true, nzAnimate: true });
      });

    this.checkLogin();
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
}
