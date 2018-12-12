import { Component, OnInit } from '@angular/core';
import { ProductService } from 'src/app/services/product.service';
import { NzMessageService } from 'ng-zorro-antd';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-product-detail',
  templateUrl: './product-detail.component.html',
  styleUrls: [ './product-detail.component.scss' ]
})
export class ProductDetailComponent implements OnInit {
  product: any = {};
  isLoading = true;
  uid = 0;
  constructor(
    private productService: ProductService,
    private message: NzMessageService,
    private activedRoute: ActivatedRoute
  ) {}

  ngOnInit() {
    this.checkRouteParameter();
  }

  checkRouteParameter() {
    this.activedRoute.params.subscribe((params) => {
      const id = +params['productId'];
      if (id) {
        this.getDetailProduct(id);
      }
    });
  }

  getDetailProduct(id) {
    this.productService
      .getDetailProduct(id)
      .then((res) => {
        this.product = res;
        this.isLoading = false;
      })
      .catch((err) => {
        this.message.error(err, { nzDuration: 5000, nzPauseOnHover: true, nzAnimate: true });
        this.isLoading = false;
      });
  }
}
