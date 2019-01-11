import { Component, OnInit, Input } from '@angular/core';
import { ProductService } from 'src/app/services/product.service';
import { NzMessageService } from 'ng-zorro-antd';
import { EventsService } from 'src/app/services/events.service';

@Component({
  selector: 'app-product-detail',
  templateUrl: './product-detail.component.html',
  styleUrls: [ './product-detail.component.scss' ]
})
export class ProductDetailComponent implements OnInit {
  @Input() postId: number;
  @Input() editable: boolean;

  product: any = {};
  isLoading = true;
  uid = 0;

  constructor(
    private productService: ProductService,
    private message: NzMessageService,
    private eventsService: EventsService
  ) {}

  ngOnInit() {
    this.getDetailProduct(this.postId);
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

  discoveryMore() {
    alert('More details on our apps!');
  }

  editProduct(id: number) {
    // this.productService.setProductPosterId(posterId);
    const content = {
      state: true,
      type: 'updateProduct',
      updatedId: id,
      header: {
        icon: 'assets/images/new-post.png',
        text: 'Update Post'
      }
    };
    this.eventsService.setModalState(content);
  }

  async deleteProduct(id) {
    try {
      const deleteSuccess = await this.productService.deleteProduct(id);
      this.message.success(deleteSuccess.msg, { nzDuration: 5000, nzPauseOnHover: true, nzAnimate: true });
      const content = {
        state: false,
        type: 'deleteProduct'
      };
      this.eventsService.setModalState(content);
      // this.isLoading = true;
      // this.products = this.skeletons;
      // this.originProduct = await this.productService.getListProduct();
      // this.products = this.isFiltered ? this.originProduct.filter((x) => x.posterId === this.uid) : this.originProduct;
      // this.isLoading = false;
    } catch (error) {
      this.message.error(error.toString(), { nzDuration: 5000, nzPauseOnHover: true, nzAnimate: true });
    }
  }
}
